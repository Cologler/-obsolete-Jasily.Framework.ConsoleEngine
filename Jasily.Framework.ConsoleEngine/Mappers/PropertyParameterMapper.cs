using Jasily.Framework.ConsoleEngine.Attributes;
using System;
using System.Reflection;

namespace Jasily.Framework.ConsoleEngine.Mappers
{
    public sealed class PropertyParameterMapper
        : ParameterMapper<PropertyParameterAttribute, PropertyParameterAttributeMapper>
    {
        public Action<object, object> Setter { get; }

        public PropertyParameterMapper(PropertyInfo source, Type type, Action<object, object> setter)
            : base(type)
        {
            this.AttributeMapper = new PropertyParameterAttributeMapper(source);

            this.Setter = setter;
        }

        public override bool IsOptional => this.AttributeMapper.NameAttribute.IsOptional;
    }
}