using Jasily.Framework.ConsoleEngine.Attributes;
using System;

namespace Jasily.Framework.ConsoleEngine.Mappers
{
    public class ParameterMapper : NameableMapper<ParameterAttribute>
    {
        public Type Type { get; }

        public Action<object, object> Setter { get; }

        public ParameterMapper(Type type, ParameterAttribute attr, Action<object, object> setter)
            : base(attr)
        {
            this.Type = type;
            this.Setter = setter;
        }
    }
}