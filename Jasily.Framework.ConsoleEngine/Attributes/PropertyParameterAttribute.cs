using System;

namespace Jasily.Framework.ConsoleEngine.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class PropertyParameterAttribute : ParameterAttribute
    {
        public PropertyParameterAttribute(string name)
            : base(name)
        {
        }

        public bool IsOptional { get; set; }

        public int Order { get; set; }
    }
}