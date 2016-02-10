using Jasily.Framework.ConsoleEngine.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jasily.Framework.ConsoleEngine.Mappers
{
    public class NameableMapper<T> where T : NameAttribute
    {
        public NameableMapper(T attr)
        {
            this.Attribute = attr;
        }

        public T Attribute { get; }

        public virtual string Name => this.Attribute.Name;

        public IEnumerable<string> GetNames()
        {
            yield return this.Name;
            foreach (var alia in this.Attribute.Alias)
            {
                yield return alia;
            }
        }

        public bool MatchCommand(string command)
        {
            var comparison = this.Attribute.IgnoreCase
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.Ordinal;
            return string.Equals(command, this.Name, comparison) ||
                this.Attribute.Alias.Any(z => string.Equals(command, z, comparison));
        }
    }
}