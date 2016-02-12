using Jasily.Framework.ConsoleEngine.Commands;
using Jasily.Framework.ConsoleEngine.Converters;
using Jasily.Framework.ConsoleEngine.Formaters;
using Jasily.Framework.ConsoleEngine.IO;
using Jasily.Framework.ConsoleEngine.Mappers;
using Jasily.Framework.ConsoleEngine.Parameters;
using System;

namespace Jasily.Framework.ConsoleEngine
{
    public sealed class JasilyConsoleEngine
    {
        private readonly BuiltInMembers defaultMembers = BuiltInMembers.GetDefault();

        public BuiltInMembers CustomMembers { get; } = BuiltInMembers.GetDefault();

        public MapperManager MapperManager { get; }

        internal T GetCommandMember<T>(Func<BuiltInMembers, T> func) where T : class
            => func(this.CustomMembers) ?? func(this.defaultMembers);

        public JasilyConsoleEngine()
        {
            this.MapperManager = new MapperManager(this);
        }

        public ConverterAgent Converters => this.MapperManager.Converters.GetAgent();

        public Session StartSession() => new Session(this);

        public class BuiltInMembers
        {
            public CommandBlockParser CommandParser { get; set; }

            public ICommandParameterParser CommandParameterParser { get; set; }

            public ICommand Helper { get; set; }

            public ICommand NoneInput { get; set; }

            public IOutput Output { get; set; }

            public IParametersFormater MissingParametersFormater { get; set; }

            public ICommandFormater CommandFormater { get; set; }

            public IParametersFormater ParametersFormater { get; set; }

            public static BuiltInMembers GetDefault()
            {
                return new BuiltInMembers()
                {
                    CommandParser = Singleton<CommandBlockParser>.Instance,
                    CommandParameterParser = Singleton<CommandParameterParser>.Instance,
                    Helper = Singleton<HelpCommand>.Instance,
                    NoneInput = Singleton<NoneCommandHandler>.Instance,
                    Output = Singleton<ConsoleOutput>.Instance,
                    MissingParametersFormater = Singleton<MissingParametersFormater>.Instance,
                    CommandFormater = Singleton<CommandFormater>.Instance,
                    ParametersFormater = Singleton<ParametersFormater>.Instance,
                };
            }
        }

        public static bool IsDefaultParameters(Type type) => type == typeof(Session) || type == typeof(CommandLine);
    }
}
