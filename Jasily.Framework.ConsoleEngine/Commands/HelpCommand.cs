using Jasily.Framework.ConsoleEngine.Attributes;

namespace Jasily.Framework.ConsoleEngine.Commands
{
    [Command("help")]
    public class HelpCommand : ICommand
    {
        public void Execute(Session session, CommandLine line)
        {
            session.WriteLine("usage:");
            session.WriteLine("  <command> [patameters ...]");
            session.WriteLine();

            session.WriteLine("commands:");
            foreach (var mapper in session.Engine.CommandMappers)
            {
                var formater = session.Engine.GetCommandMember(z => z.CommandFormater);
                var formated = formater.Format(mapper);
                foreach (var formatedString in formated)
                {
                    session.WriteLine("  " + formatedString);
                }
            }
        }
    }
}