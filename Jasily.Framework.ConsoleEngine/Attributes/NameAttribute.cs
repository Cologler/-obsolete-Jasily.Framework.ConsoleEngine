using System;

namespace Jasily.Framework.ConsoleEngine.Attributes
{
    public class NameAttribute : Attribute
    {
        public string Name { get; }

        public string[] Alias { get; }

        public NameAttribute(string name, params string[] alias)
        {
            this.Name = name;
            this.Alias = alias;
        }

        public string Desciption { get; set; }

        public bool IgnoreCase { get; set; } = true;
    }
}