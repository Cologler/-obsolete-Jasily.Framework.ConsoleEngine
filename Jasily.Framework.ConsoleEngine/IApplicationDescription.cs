namespace Jasily.Framework.ConsoleEngine
{
    public interface IApplicationDescription
    {
        string ApplicationName { get; }

        string Version { get; }

        string Description { get; }

        string Copyright { get; }
    }
}