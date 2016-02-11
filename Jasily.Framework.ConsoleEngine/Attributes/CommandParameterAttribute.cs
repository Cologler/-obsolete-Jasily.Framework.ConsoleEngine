using System;

namespace Jasily.Framework.ConsoleEngine.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ParameterAttribute : NameAttribute
    {
        public bool IsOptional { get; set; }

        public ParameterAttribute(string name)
            : base(name)
        {
        }
    }
}