namespace Jasily.Framework.ConsoleEngine.Commands
{
    public class NoneCommandHandler : ICommand
    {
        public void Execute(Session session, CommandLine line)
        {
            session.WriteLine("try use --help");
        }
    }
}