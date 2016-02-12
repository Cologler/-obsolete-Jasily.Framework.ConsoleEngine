using Jasily.Framework.ConsoleEngine.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Jasily.Framework.ConsoleEngine.Mappers
{
    public class BaseMapper
    {
        public virtual bool TryMap() => true;
    }

    public class BaseMapper<T, TAttributeMapper> : BaseMapper
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
            foreach (var alia in this.Alias) yield return alia.Name;
        }

        public bool IsMatch(string name)
        {
            return string.Equals(name, this.AttributeMapper.NameAttribute.Name,
                this.AttributeMapper.NameAttribute.IgnoreCase
                    ? StringComparison.OrdinalIgnoreCase
                    : StringComparison.Ordinal) ||
                this.Alias.Any(z => string.Equals(name, z.Name, z.IgnoreCase
                        ? StringComparison.OrdinalIgnoreCase
                        : StringComparison.Ordinal));
        }

        public override bool TryMap()
        {
            if (!this.AttributeMapper.TryMap(this.MapedSource)) return false;

            return true;
        }

        public IReadOnlyList<AliasAttribute> Alias => this.AttributeMapper.AliasAttribute;

        public string Desciption => this.AttributeMapper.DesciptionAttribute.Desciption;
    }
}