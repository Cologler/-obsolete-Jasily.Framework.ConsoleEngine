using Jasily.Framework.ConsoleEngine.Mappers;
using Jasily.Framework.ConsoleEngine.Parameters;
using System.Collections.Generic;

namespace Jasily.Framework.ConsoleEngine.Formaters
{
    public interface ICommandFormater
    {
        IEnumerable<FormatedString> Format(CommandMapper commandMapper);

        IEnumerable<FormatedString> Format(CommandMapper commandMapper, IParametersFormater formater,
            ICommandParameterParser parser);
    }
}