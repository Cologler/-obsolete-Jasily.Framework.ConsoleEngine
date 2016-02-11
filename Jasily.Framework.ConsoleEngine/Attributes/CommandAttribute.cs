using System;

namespace Jasily.Framework.ConsoleEngine.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : NameAttribute
    {
        public CommandAttribute(string commandName)
            : base(commandName)
        {
        }

        public bool IsStatic { get; set; }

        public CommandType CommandType { get; set; } = CommandType.Custom;
    }
}