using Jasily.Framework.ConsoleEngine.Mappers;
using System.Collections.Generic;

namespace Jasily.Framework.ConsoleEngine.Formaters
{
    public interface ICommandFormater
    {
        IEnumerable<FormatedString> Format(CommandMapper commandMapper);
    }
}