using Jasily.Framework.ConsoleEngine.Attributes;

namespace Jasily.Framework.ConsoleEngine.Commands
{
    [Command("exit")]
    [Static]
    public class ExitCommand : ICommand
    {
        public void Execute(Session session, CommandLine line) => session.Shutdown();
    }
}