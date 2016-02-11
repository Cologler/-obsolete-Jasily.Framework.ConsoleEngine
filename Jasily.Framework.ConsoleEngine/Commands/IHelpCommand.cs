namespace Jasily.Framework.ConsoleEngine.Commands
{
    public interface IHelpCommand : ICommand
    {
        void Help(Session session, CommandLine line);
    }
}