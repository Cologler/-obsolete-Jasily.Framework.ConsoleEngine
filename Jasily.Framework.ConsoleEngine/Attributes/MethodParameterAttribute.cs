using System;

namespace Jasily.Framework.ConsoleEngine.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class MethodParameterAttribute : ParameterAttribute
    {
        public MethodParameterAttribute(string name)
            : base(name)
        {
        }
    }
}