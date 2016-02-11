using Jasily.Framework.ConsoleEngine.Attributes;
using Jasily.Framework.ConsoleEngine.Mappers;

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

            var mapper = session.GetCommandMapper(line);
            if (mapper == null)
            {
                foreach (var m in session.Engine.MapperManager.GetCommandMappers())
                {
                    this.HelpFor(m, session, line);
                }
            }
            else
            {
                this.HelpFor(mapper, session, line);
                var parameterFormater = session.Engine.GetCommandMember(z => z.ParametersFormater);
                var ParameterParser = session.Engine.GetCommandMember(z => z.CommandParameterParser);
                session.WriteLine();
                session.WriteLine("parameters:");
                foreach (var formatedString in parameterFormater.Format(mapper, mapper.ParameterSetterBuilder.Mappers, ParameterParser))
                {
                    session.WriteLine("  " + formatedString);
                }
            }
        }

        private void HelpFor(CommandMapper mapper, Session session, CommandLine line)
        {
            if (typeof(IHelpCommand).IsAssignableFrom(mapper.MapedType))
            {
                ((IHelpCommand)mapper.CommandBuilder.Build()).Help(session, line);
            }
            else
            {
                var commandFormater = session.Engine.GetCommandMember(z => z.CommandFormater);
                foreach (var formatedString in commandFormater.Format(mapper))
                {
                    session.WriteLine("  " + formatedString);
                }
            }
        }
    }
}