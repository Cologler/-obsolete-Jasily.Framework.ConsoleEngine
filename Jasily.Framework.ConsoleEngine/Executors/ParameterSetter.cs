using Jasily.Framework.ConsoleEngine.Converters;
using Jasily.Framework.ConsoleEngine.Entities;
using Jasily.Framework.ConsoleEngine.Mappers;
using System.Diagnostics;

namespace Jasily.Framework.ConsoleEngine.Executors
{
    public class ParameterSetter<TMapper> : IParameterSetter
        where TMapper : IParameterMapper
    {
        public object Value { get; private set; }

        public TMapper Mapper { get; }

        IParameterMapper IParameterSetter.Mapper => this.Mapper;

        public ParameterSetter(TMapper mapper)
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

        public bool IsMissing => !this.Mapper.IsInternal && !this.Mapper.IsOptional && !this.IsSeted;
    }
}