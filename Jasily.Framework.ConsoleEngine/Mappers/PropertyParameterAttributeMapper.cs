using Jasily.Framework.ConsoleEngine.Attributes;
using System.Collections.Generic;
using System.Reflection;

namespace Jasily.Framework.ConsoleEngine.Mappers
{
    public sealed class PropertyParameterAttributeMapper : ParameterAttributeMapper<PropertyParameterAttribute>
    {
        private readonly PropertyInfo member;

        public PropertyParameterAttributeMapper(PropertyInfo member)
        {
            this.member = member;
        }

        protected override T GetCustomAttribute<T>()
            => this.member.GetCustomAttribute<T>();

        protected override IEnumerable<T> GetCustomAttributes<T>()
            => this.member.GetCustomAttributes<T>();
    }
}