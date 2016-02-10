using System;

namespace Jasily.Framework.ConsoleEngine.IO
{
    public class ConsoleOutput : IOutput
    {
        public void Write(string value) => Console.Write(value ?? string.Empty);

        public void WriteLine(string line) => Console.WriteLine(line ?? string.Empty);
    }
}