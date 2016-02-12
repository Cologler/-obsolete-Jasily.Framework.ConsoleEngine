using Jasily.Framework.ConsoleEngine.Attributes;
using System;
using System.Reflection;

namespace Jasily.Framework.ConsoleEngine.Mappers
{
    public class ParameterMapper : BaseMapper<ParameterAttribute, ParameterAttributeMapper>
    {
        public Action<object, object> Setter { get; }

        public ParameterMapper(PropertyInfo source, Type type, Action<object, object> setter)
            : base(source, type)
        {
            this.Setter = setter;
        }

        public bool IsOptional => this.AttributeMapper.NameAttribute.IsOptional;
    }
}