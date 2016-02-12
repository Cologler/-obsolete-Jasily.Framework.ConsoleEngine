using System;
using System.Reflection;

namespace Jasily.Framework.ConsoleEngine.Mappers
{
    public sealed class PropertyParameterMapper : ParameterMapper
    {
        public PropertyParameterMapper(PropertyInfo source, Type type, Action<object, object> setter)
            : base(type, setter)
        {
            this.AttributeMapper = new PropertyParameterAttributeMapper(source);
        }

        public override bool IsOptional => this.AttributeMapper.NameAttribute.IsOptional;
    }
}