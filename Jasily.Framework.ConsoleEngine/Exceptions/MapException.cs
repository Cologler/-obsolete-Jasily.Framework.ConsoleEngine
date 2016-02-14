using System;

namespace Jasily.Framework.ConsoleEngine.Exceptions
{
    public class MapException : Exception
    {
        public Type Type { get; }

        public MapException(Type type, string message) : base(message)
        {
            this.Type = type;
        }
    }
}