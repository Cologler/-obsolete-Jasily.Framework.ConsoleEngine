using Jasily.Framework.ConsoleEngine.IO;
using Jasily.Framework.ConsoleEngine.Mappers;
using Jasily.Framework.ConsoleEngine.Parameters;
using System.Collections.Generic;

namespace Jasily.Framework.ConsoleEngine.Formaters
{
    public interface IParametersFormater
    {
        IEnumerable<string> Format(CommandMapper commandMapper, IEnumerable<ParameterMapper> mappers,
            ICommandParameterParser parser);

        void Format(IOutput output, CommandMapper commandMapper, IEnumerable<ParameterMapper> mappers,
            ICommandParameterParser parser);
    }
}