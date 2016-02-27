using System;

namespace Jasily.Framework.ConsoleEngine.IO
{
    public sealed class ConsoleInput : IInput
    {
        public string ReadLine() => Console.ReadLine();
    }
}