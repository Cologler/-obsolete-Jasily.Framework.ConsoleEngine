using Jasily.Framework.ConsoleEngine.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Jasily.Framework.ConsoleEngine.Mappers
{
    public class BaseMapper<T> where T : NameAttribute
    {
        protected BaseMapper(Type type, T attr)
            : this(type, type, attr)
        {
        }
        protected BaseMapper(MemberInfo source, Type type, T attr)
        {
            this.MapedSource = source;
            this.MapedType = type;
            this.Attribute = attr;
        }

        public Type MapedType { get; }

        public MemberInfo MapedSource { get; }

        public T Attribute { get; }

        public virtual string Name => this.Attribute.Name;

        public IEnumerable<string> GetNames()
        {
            yield return this.Name;
            foreach (var alia in this.Alias) yield return alia.Name;
        }

        public bool IsMatch(string name)
        {
            var comparison = this.Attribute.IgnoreCase
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.Ordinal;
            return string.Equals(name, this.Name, comparison) ||
                this.Alias.Any(z => string.Equals(name, z.Name, comparison));
        }

        public virtual bool Map()
        {
            this.Alias = this.MapedSource.GetCustomAttributes<AliasAttribute>().ToList();
            foreach (var attr in this.Alias)
            {
                if (string.IsNullOrWhiteSpace(attr.Name))
                {
                    Debug.WriteLine("AliasAttribute.Name cannot be empty.", nameof(attr.Name));
                    return false;
                }
            }

            this.Desciption = this.MapedSource.GetCustomAttribute<DesciptionAttribute>() ?? DesciptionAttribute.Empty;

            return true;
        }

        public IReadOnlyList<AliasAttribute> Alias { get; private set; }

        public DesciptionAttribute Desciption { get; private set; }
    }
}