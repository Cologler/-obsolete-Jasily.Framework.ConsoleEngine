using Jasily.Framework.ConsoleEngine.Converters;
using Jasily.Framework.ConsoleEngine.Entities;
using Jasily.Framework.ConsoleEngine.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Jasily.Framework.ConsoleEngine.Mappers
{
    public class CommandParameterSetter
    {
        private readonly Dictionary<string, ParameterSetterTask> mappers;
        private readonly ParameterSetterTask[] tasks;

        internal CommandParameterSetter(IEnumerable<ParameterMapper> mappers)
        {
            this.tasks = mappers.Select(z => new ParameterSetterTask(z)).ToArray();
            this.mappers = new Dictionary<string, ParameterSetterTask>();
            foreach (var task in this.tasks)
            {
                foreach (var name in task.Mapper.GetNames())
                {
                    this.mappers.Add(name, task);
                }
            }
        }

        public OperationResult SetParameter(string parameterName, object obj, string value, ConverterAgent converters)
        {
            var task = this.mappers.GetValueOrDefault(parameterName);
            if (task == null) return null;
            return task.SetValue(obj, value, converters);
        }

        public bool IsAllVaild() => this.tasks.All(z => z.IsVaild);

        public IEnumerable<ParameterMapper> GetAllParameters()
            => this.tasks.Where(z => z.IsMissing).Select(z => z.Mapper);

        public IEnumerable<ParameterMapper> GetMissingParameters()
            => this.tasks.Where(z => z.IsMissing).Select(z => z.Mapper);

        private class ParameterSetterTask
        {
            public ParameterMapper Mapper { get; }

            public ParameterSetterTask(ParameterMapper mapper)
            {
                this.Mapper = mapper;
            }

            public OperationResult SetValue(object obj, string value, ConverterAgent agent)
            {
                if (this.IsSeted) return $"parameter {this.Mapper.Name} already seted!";
                this.IsSeted = true;
                object x = null;
                if (!agent.Convert(this.Mapper.Type, value, out x))
                {
                    var vaild = agent.GetVaildInput(this.Mapper.Type);
                    return $"{value} cannot convert to parameter {this.Mapper.Name}, vaild input should be {vaild}.";
                }
                this.Mapper.Setter(obj, x);
                return null;
            }

            public bool IsSeted { get; private set; }

            public bool IsVaild => this.Mapper.Attribute.IsOptional || this.IsSeted;

            public bool IsMissing => !this.Mapper.Attribute.IsOptional && !this.IsSeted;
        }
    }
}