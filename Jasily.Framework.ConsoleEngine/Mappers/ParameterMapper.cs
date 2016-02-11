using Jasily.Framework.ConsoleEngine.Attributes;
using System;
using System.Reflection;

namespace Jasily.Framework.ConsoleEngine.Mappers
{
    public class ParameterMapper : BaseMapper<ParameterAttribute>
    {
        public Action<object, object> Setter { get; }

        public ParameterMapper(PropertyInfo source, Type type, ParameterAttribute attr, Action<object, object> setter)
            : base(source, type, attr)
        {
            this.Setter = setter;
        }
    }
}