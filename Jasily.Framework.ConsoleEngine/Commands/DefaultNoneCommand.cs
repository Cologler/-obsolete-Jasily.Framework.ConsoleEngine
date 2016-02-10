namespace Jasily.Framework.ConsoleEngine.Commands
{
    public class DefaultNoneCommand : ICommand
    {
        public void Execute(Session session, CommandLine line)
        {
            session.WriteLine("user was input nothing.");
        }
    }
}