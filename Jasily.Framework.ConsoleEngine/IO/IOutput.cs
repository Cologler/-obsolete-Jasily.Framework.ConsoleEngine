namespace Jasily.Framework.ConsoleEngine.IO
{
    public interface IOutput
    {
        void Write(string value);

        void WriteLine(string line);
    }
}