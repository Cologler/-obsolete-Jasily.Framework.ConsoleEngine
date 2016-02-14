using Jasily.Framework.ConsoleEngine.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jasily.Framework.ConsoleEngine.Executors
{
    internal sealed class MethodCommandExecutor : CommandExecutor<MethodParameterMapper>
    {
        private readonly Action<object, object[]> methodParameterSetter;

        internal MethodCommandExecutor(object obj, IEnumerable<MethodParameterMapper> mappers,
            Action<object, object[]> methodParameterSetter)
            : base(obj, mappers)
        {
            this.methodParameterSetter = methodParameterSetter;
        }

        public override bool IsVaildCommand() => this.Setters.All(
            z => z.IsVaild || JasilyConsoleEngine.IsDefaultParameters(z.Mapper.MapedType));

        public override void Execute(Session session, CommandLine line)
        {
            if (this.IsVaildCommand())
            {
                foreach (var task in this.Setters.Where(z => !z.IsVaild))
                {
                    if (task.Mapper.MapedType == typeof(Session)) task.SetValue(session);
                    if (task.Mapper.MapedType == typeof(CommandLine)) task.SetValue(line);
                }

                var args = this.Setters.Select(z => z.IsSeted ? z.Value : z.Mapper.DefaultValue).ToArray();
                this.methodParameterSetter(this.Obj, args);
            }
        }

        public override IEnumerable<IParameterMapper> GetMissingParameters()
            => this.Setters.Where(z => z.IsMissing && !JasilyConsoleEngine.IsDefaultParameters(z.Mapper.MapedType))
                   .Select(z => z.Mapper);
    }
}