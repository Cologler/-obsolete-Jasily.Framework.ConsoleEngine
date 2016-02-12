using System;

namespace Jasily.Framework.ConsoleEngine.Attributes
{
    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Property |
        AttributeTargets.Method | AttributeTargets.Parameter,
        AllowMultiple = true)]
    public sealed class AliasAttribute : NameAttribute
    {
        public AliasAttribute(string name)
            : base(name)
        {
        }
    }
}