using Jasily.Framework.ConsoleEngine.Converters;
using Jasily.Framework.ConsoleEngine.Entities;
using Jasily.Framework.ConsoleEngine.Extensions;
using Jasily.Framework.ConsoleEngine.Mappers;
using Jasily.Framework.ConsoleEngine.Parameters;
using System.Collections.Generic;
using System.Linq;

namespace Jasily.Framework.ConsoleEngine.Executors
{
    public abstract class CommandExecutor
    {
        public abstract OperationResult SetCommandLine(CommandLine commandLine,
            ICommandParameterParser parameterParser, ConverterAgent converters);

        public abstract IEnumerable<IParameterMapper> GetAllParameters();

        public abstract IEnumerable<IParameterMapper> GetMissingParameters();

        public abstract bool IsVaildCommand();

        public abstract void Execute(Session session, CommandLine line);
    }

    public abstract class CommandExecutor<TMapper> : CommandExecutor
        where TMapper : IParameterMapper
    {
        private readonly Dictionary<string, List<ParameterSetter<TMapper>>> settersMap
            = new Dictionary<string, List<ParameterSetter<TMapper>>>();
        protected readonly ParameterSetter<TMapper>[] Setters;
        protected readonly object Obj;

        protected CommandExecutor(object obj, IEnumerable<TMapper> mappers)
        {
            this.Obj = obj;
            this.Setters = mappers.Select(z => new ParameterSetter<TMapper>(z)).ToArray();
            foreach (var task in this.Setters)
            {
                foreach (var name in task.Mapper.GetNames())
                {
                    this.settersMap.GetOrCreateValue(name).Add(task);
                }
            }
        }

        public override OperationResult SetCommandLine(CommandLine commandLine,
            ICommandParameterParser parameterParser,
            ConverterAgent converters)
        {
            foreach (var kvp in parameterParser.Parse(commandLine, this.Setters))
            {
                var setters = this.settersMap.GetValueOrDefault(kvp.Key);
                var result = setters?
                    .FirstOrDefault(z => z.Mapper.IsMatch(kvp.Key))?
                    .PrevSetValue(kvp.Value, converters);
                if (result?.HasError == true) return result.Value;
            }
            return null;
        }

        public override IEnumerable<IParameterMapper> GetAllParameters()
            => this.Setters.Select(z => (IParameterMapper)z.Mapper);

        public override IEnumerable<IParameterMapper> GetMissingParameters()
            => this.Setters.Where(z => z.IsMissing).Select(z => (IParameterMapper)z.Mapper);


    }
}