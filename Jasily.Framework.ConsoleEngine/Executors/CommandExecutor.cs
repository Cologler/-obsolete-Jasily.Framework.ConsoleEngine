using Jasily.Framework.ConsoleEngine.Converters;
using Jasily.Framework.ConsoleEngine.Entities;
using Jasily.Framework.ConsoleEngine.Extensions;
using Jasily.Framework.ConsoleEngine.Mappers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Jasily.Framework.ConsoleEngine.Executors
{
    public abstract class CommandExecutor
    {
        public abstract OperationResult SetParameter(string parameterName, string value, ConverterAgent converters);

        public abstract IEnumerable<IParameterMapper> GetAllParameters();

        public abstract IEnumerable<IParameterMapper> GetMissingParameters();

        public abstract void Execute(Session session, CommandLine line);
    }

    public abstract class CommandExecutor<TMapper> : CommandExecutor
        where TMapper : ParameterMapper
    {
        private readonly Dictionary<string, ParameterSetterTask> mappers;
        protected readonly ParameterSetterTask[] Tasks;
        protected readonly object Obj;

        protected CommandExecutor(object obj, IEnumerable<TMapper> mappers)
        {
            this.Obj = obj;
            this.Tasks = mappers.Select(z => new ParameterSetterTask(z)).ToArray();
            this.mappers = new Dictionary<string, ParameterSetterTask>();
            foreach (var task in this.Tasks)
            {
                foreach (var name in task.Mapper.GetNames())
                {
                    this.mappers.Add(name, task);
                }
            }
        }

        public override OperationResult SetParameter(string parameterName, string value, ConverterAgent converters)
        {
            var task = this.mappers.GetValueOrDefault(parameterName);
            if (task == null) return null;
            return task.PrevSetValue(value, converters);
        }

        public override IEnumerable<IParameterMapper> GetAllParameters()
            => this.Tasks.Select(z => z.Mapper);

        public override IEnumerable<IParameterMapper> GetMissingParameters()
            => this.Tasks.Where(z => z.IsMissing).Select(z => z.Mapper);

        protected class ParameterSetterTask
        {
            public object Value { get; private set; }

            public TMapper Mapper { get; }

            public ParameterSetterTask(TMapper mapper)
            {
                this.Mapper = mapper;
                this.IsVaild = this.Mapper.IsOptional;
            }

            public OperationResult PrevSetValue(string value, ConverterAgent agent)
            {
                if (this.IsSeted) return $"parameter {this.Mapper.Name} already seted!";
                this.IsSeted = true;
                this.IsVaild = false;
                object x = null;
                if (!agent.Convert(this.Mapper.MapedType, value, out x))
                {
                    var vaild = agent.GetVaildInput(this.Mapper.MapedType);
                    return $"{value} cannot convert to parameter {this.Mapper.Name}, vaild input should be {vaild}.";
                }

                this.IsVaild = true;
                this.Value = x;
                return null;
            }

            internal void SetValue<T>(T session)
            {
                if (this.IsVaild) return;
                Debug.Assert(JasilyConsoleEngine.IsDefaultParameters(typeof(T)));
                Debug.Assert(this.Mapper.MapedType == typeof(T));

                this.IsSeted = true;
                this.IsVaild = true;
                this.Value = session;
            }

            public bool IsSeted { get; private set; }

            public bool IsVaild { get; private set; }

            public bool IsMissing => !this.Mapper.IsOptional && !this.IsSeted;
        }
    }
}