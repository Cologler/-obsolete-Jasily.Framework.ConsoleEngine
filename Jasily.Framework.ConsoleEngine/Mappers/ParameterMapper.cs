using Jasily.Framework.ConsoleEngine.Attributes;
using System;

namespace Jasily.Framework.ConsoleEngine.Mappers
{
    public abstract class ParameterMapper : BaseMapper<ParameterAttribute, ParameterAttributeMapper>,
        IParameterMapper
    {
        public Action<object, object> Setter { get; }

        protected ParameterMapper(Type type, Action<object, object> setter)
            : base(type)
        {
            this.Setter = setter;
        }

        public abstract bool IsOptional { get; }
    }
}