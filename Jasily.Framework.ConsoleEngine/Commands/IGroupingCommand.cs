namespace Jasily.Framework.ConsoleEngine.Commands
{
    public interface IGroupingCommand : ICommand
    {
        void Execute(Session session, CommandLine line, int[] workedGroupId);
    }
}