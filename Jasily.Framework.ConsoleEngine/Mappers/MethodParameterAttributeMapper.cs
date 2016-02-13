using Jasily.Framework.ConsoleEngine.Attributes;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Jasily.Framework.ConsoleEngine.Mappers
{
    public sealed class MethodParameterAttributeMapper : ParameterAttributeMapper<MethodParameterAttribute>
    {
        private readonly ParameterInfo member;

        public MethodParameterAttributeMapper(ParameterInfo member)
        {
            this.member = member;
        }

        protected override T GetCustomAttribute<T>()
            => this.member.GetCustomAttribute<T>();

        protected override IEnumerable<T> GetCustomAttributes<T>()
            => this.member.GetCustomAttributes<T>();

        protected override bool TryMapName()
        {
            // default parameter don't need name attribute.
            if (JasilyConsoleEngine.IsDefaultParameters(this.member.ParameterType))
                return true;

            this.NameAttribute = this.GetCustomAttribute<MethodParameterAttribute>() ??
                new MethodParameterAttribute(this.member.Name.ToLower());

            if (string.IsNullOrWhiteSpace(this.NameAttribute.Name))
            {
                if (Debugger.IsAttached) Debugger.Break();
                return false;
            }

            return true;
        }
    }
}