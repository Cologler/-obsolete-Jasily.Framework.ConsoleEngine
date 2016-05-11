using Jasily.Framework.ConsoleEngine.Commands;
using Jasily.Framework.ConsoleEngine.Formaters;
using Jasily.Framework.ConsoleEngine.IO;
using Jasily.Framework.ConsoleEngine.Parameters;

namespace Jasily.Framework.ConsoleEngine
{
    public struct ConsoleParameters
    {
        public ICommandBlockParser CommandParser { get; set; }

        public ICommandParameterParser CommandParameterParser { get; set; }

        public ICommand Helper { get; set; }

        public ICommand NoneInput { get; set; }

        public IInput Input { get; set; }

        public IOutput Output { get; set; }

        public IParametersFormater MissingParametersFormater { get; set; }

        public ICommandFormater CommandFormater { get; set; }

        public IParametersFormater ParametersFormater { get; set; }
    }
}