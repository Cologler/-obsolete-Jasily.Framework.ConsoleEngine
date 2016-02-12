using System;

namespace Jasily.Framework.ConsoleEngine.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SwitchParameterAttribute : NameAttribute
    {
        public SwitchParameterAttribute(bool value, string name)
            : base(name)
        {
            
        }
    }
}