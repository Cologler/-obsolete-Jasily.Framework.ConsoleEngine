namespace Jasily.Framework.ConsoleEngine.IO
{
    public sealed class NoneOutput : IOutput
    {
        public static IOutput Instance { get; } = new NoneOutput();

        public void Write(string value)
        {
        }

        public void WriteLine(string line)
        {
        }
    }
}