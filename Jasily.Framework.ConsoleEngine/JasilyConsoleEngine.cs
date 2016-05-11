using System;
using Jasily.Framework.ConsoleEngine.Commands;
using Jasily.Framework.ConsoleEngine.Formaters;
using Jasily.Framework.ConsoleEngine.IO;
using Jasily.Framework.ConsoleEngine.Mappers;
using Jasily.Framework.ConsoleEngine.Parameters;

namespace Jasily.Framework.ConsoleEngine
{
    public sealed class JasilyConsoleEngine
    {
        private ConsoleParameters userParameters;

        public ConsoleParameters Parameters
        {
            get
            {
                return new ConsoleParameters()
                {
                    CommandParser = this.userParameters.CommandParser ?? Singleton<CommandBlockParser>.Instance,
                    CommandParameterParser = this.userParameters.CommandParameterParser ?? Singleton<CommandParameterParser>.Instance,
                    HelpCommand = this.userParameters.HelpCommand ?? Singleton<HelpCommand>.Instance,
                    NoneInputCommand = this.userParameters.NoneInputCommand ?? Singleton<NoneCommandHandler>.Instance,
                    Input = this.userParameters.Input ?? Singleton<ConsoleInput>.Instance,
                    Output = this.userParameters.Output ?? Singleton<ConsoleOutput>.Instance,
                    MissingParametersFormater = this.userParameters.MissingParametersFormater ?? Singleton<MissingParametersFormater>.Instance,
                    CommandFormater = this.userParameters.CommandFormater ?? Singleton<CommandFormater>.Instance,
                    ParametersFormater = this.userParameters.ParametersFormater ?? Singleton<ParametersFormater>.Instance,
                };
            }
            set { this.userParameters = value; }
        }

        public MapperManager MapperManager { get; }

        public JasilyConsoleEngine()
        {
            this.MapperManager = new MapperManager(this);
        }

        public Session StartSession(string name = null, IApplicationDescription desc = null)
        {
            var s = new Session(this, name)
            {
                Description = desc
            };
            s.ShowDescription();
            return s;
        }

        public static bool IsDefaultParameters(Type type) => type == typeof(Session) || type == typeof(CommandLine);
    }
}
