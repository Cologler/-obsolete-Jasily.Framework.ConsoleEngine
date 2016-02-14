using Jasily.Framework.ConsoleEngine.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Jasily.Framework.ConsoleEngine.Mappers
{
    public sealed class PropertyParameterAttributeMapper : ParameterAttributeMapper<PropertyParameterAttribute>
    {
        private readonly PropertyInfo member;

        public PropertyParameterAttributeMapper(PropertyInfo member)
        {
            this.member = member;

            var groupIds = member.GetCustomAttributes<MethodParameterGroupingAttribute>()
                .Select(z => z.GroupId)
                .ToArray();
            this.GroupIds = groupIds.Length > 0 ? groupIds.Distinct().ToArray() : new[] { 0 };
        }

        protected override T GetCustomAttribute<T>()
            => this.member.GetCustomAttribute<T>();

        protected override IEnumerable<T> GetCustomAttributes<T>()
            => this.member.GetCustomAttributes<T>();

        public int[] GroupIds { get; }
    }
}