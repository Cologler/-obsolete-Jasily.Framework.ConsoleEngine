using Jasily.Framework.ConsoleEngine.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Jasily.Framework.ConsoleEngine.Mappers
{
    public abstract class BaseAttributeMapper<TNameAttribute> : BaseAttributeMapper
        where TNameAttribute : NameAttribute
    {
        public TNameAttribute NameAttribute { get; protected set; }

        public virtual string Name => this.NameAttribute.Name;

        internal override bool TryMap()
        {
            Debug.Assert(this.NameAttribute == null);

            return base.TryMap() && this.TryMapName();
        }

        protected virtual bool TryMapName()
        {
            this.NameAttribute = this.GetCustomAttribute<TNameAttribute>();
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
        public IReadOnlyList<AliasAttribute> AliasAttribute { get; private set; }

        public IEnumerable<string> Alias => this.AliasAttribute.Select(z => z.Name);

        public DesciptionAttribute DesciptionAttribute { get; private set; }

        internal virtual bool TryMap()
        {
            Debug.Assert(this.AliasAttribute == null);

            this.AliasAttribute = this.GetCustomAttributes<AliasAttribute>().ToList();
            foreach (var attr in this.AliasAttribute)
            {
                if (string.IsNullOrWhiteSpace(attr.Name))
                {
                    if (Debugger.IsAttached) Debugger.Break();
                    return false;
                }
            }

            this.DesciptionAttribute = this.GetCustomAttribute<DesciptionAttribute>() ?? DesciptionAttribute.Empty;

            return true;
        }

        protected abstract T GetCustomAttribute<T>() where T : Attribute;

        protected abstract IEnumerable<T> GetCustomAttributes<T>() where T : Attribute;
    }
}