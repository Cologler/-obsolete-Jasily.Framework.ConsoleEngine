using Jasily.Framework.ConsoleEngine.Attributes;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Jasily.Framework.ConsoleEngine.Mappers
{
    public class BaseAttributeMapper<TNameAttribute> : BaseAttributeMapper
        where TNameAttribute : NameAttribute
    {
        public TNameAttribute NameAttribute { get; private set; }

        public override string Name => this.NameAttribute.Name;

        internal override bool TryMap(MemberInfo source)
        {
            Debug.Assert(this.NameAttribute == null);

            if (!base.TryMap(source)) return false;

            this.NameAttribute = source.GetCustomAttribute<TNameAttribute>();
            if (this.NameAttribute == null) return false;

            if (string.IsNullOrWhiteSpace(this.NameAttribute.Name))
            {
                if (Debugger.IsAttached) Debugger.Break();
                return false;
            }

            return true;
        }
    }

    public abstract class BaseAttributeMapper
    {
        public abstract string Name { get; }

        public IReadOnlyList<AliasAttribute> AliasAttribute { get; private set; }

        public DesciptionAttribute DesciptionAttribute { get; private set; }

        internal virtual bool TryMap(MemberInfo source)
        {
            Debug.Assert(this.AliasAttribute == null);

            this.AliasAttribute = source.GetCustomAttributes<AliasAttribute>().ToList();
            foreach (var attr in this.AliasAttribute)
            {
                if (string.IsNullOrWhiteSpace(attr.Name))
                {
                    if (Debugger.IsAttached) Debugger.Break();
                    return false;
                }
            }

            this.DesciptionAttribute = source.GetCustomAttribute<DesciptionAttribute>() ?? DesciptionAttribute.Empty;

            return true;
        }
    }
}