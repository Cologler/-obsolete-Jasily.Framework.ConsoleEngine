using Jasily.Framework.ConsoleEngine.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Jasily.Framework.ConsoleEngine.Mappers
{
    public class BaseMapper<T, TAttributeMapper> : INameMapper
        where T : NameAttribute
        where TAttributeMapper : BaseAttributeMapper<T>
    {
        protected BaseMapper(Type type)
        {
            this.MapedType = type;
        }

        public Type MapedType { get; }

        public TAttributeMapper AttributeMapper { get; protected set; }

        public string Name
        {
            get
            {
                Debug.Assert(this.AttributeMapper != null);
                return this.AttributeMapper.Name;
            }
        }

        public virtual IEnumerable<string> GetNames()
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

        public bool TryMap()
        {
            Debug.Assert(this.AttributeMapper != null);
            return this.AttributeMapper.TryMap();
        }

        public IEnumerable<string> Alias => this.AttributeMapper.AliasAttribute.Select(z => z.Name);

        public string Desciption => this.AttributeMapper.DesciptionAttribute.Desciption;
    }
}