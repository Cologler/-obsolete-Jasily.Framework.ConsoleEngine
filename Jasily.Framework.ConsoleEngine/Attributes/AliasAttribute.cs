using System;

namespace Jasily.Framework.ConsoleEngine.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public sealed class AliasAttribute : NameAttribute
    {
        public AliasAttribute(string name)
            : base(name)
        {
        }
    }
}