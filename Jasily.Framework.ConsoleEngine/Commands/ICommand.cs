namespace Jasily.Framework.ConsoleEngine.Commands
{
    public interface ICommand
    {
        void Execute(Session session, CommandLine line);
    }
}