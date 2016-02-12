using Jasily.Framework.ConsoleEngine.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Jasily.Framework.ConsoleEngine.Mappers
{
    public class BaseMapper<T, TAttributeMapper>
        where T : NameAttribute
        where TAttributeMapper : BaseAttributeMapper<T>, new()
    {
        protected BaseMapper(Type type)
            : this(type, type)
        {
        }
        protected BaseMapper(MemberInfo source, Type type)
        {
            this.MapedSource = source;
            this.MapedType = type;
        }

        public Type MapedType { get; }

        public MemberInfo MapedSource { get; }

        public TAttributeMapper AttributeMapper { get; } = new TAttributeMapper();

        public string Name => this.AttributeMapper.Name;

        public IEnumerable<string> GetNames()
        {
            yield return this.Name;
            foreach (var alias in this.Alias) yield return alias;
        }

        public bool IsMatch(string name)
        {
            return string.Equals(name, this.AttributeMapper.NameAttribute.Name,
                this.AttributeMapper.NameAttribute.IgnoreCase
                    ? StringComparison.OrdinalIgnoreCase
                    : StringComparison.Ordinal) ||
                this.AttributeMapper.AliasAttribute.Any(z => string.Equals(name, z.Name, z.IgnoreCase
                        ? StringComparison.OrdinalIgnoreCase
                        : StringComparison.Ordinal));
        }

        public bool TryMap() => this.AttributeMapper.TryMap(this.MapedSource);

        public IEnumerable<string> Alias => this.AttributeMapper.AliasAttribute.Select(z => z.Name);

        public string Desciption => this.AttributeMapper.DesciptionAttribute.Desciption;
    }
}