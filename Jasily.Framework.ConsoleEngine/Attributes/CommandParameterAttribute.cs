using System;

namespace Jasily.Framework.ConsoleEngine.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ParameterAttribute : NameAttribute
    {
        public bool IsOptional { get; }

        public ParameterAttribute(bool isOptional, string name, params string[] alias)
            : base(name, alias)
        {
            this.IsOptional = isOptional;
        }
    }
}