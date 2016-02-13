using Jasily.Framework.ConsoleEngine.IO;
using Jasily.Framework.ConsoleEngine.Mappers;
using Jasily.Framework.ConsoleEngine.Parameters;
using System.Collections.Generic;
using System.Linq;

namespace Jasily.Framework.ConsoleEngine.Formaters
{
    public class MissingParametersFormater : IParametersFormater
    {
        public IEnumerable<FormatedString> Format(CommandMapper commandMapper, IEnumerable<IParameterMapper> mappers,
            ICommandParameterParser parser)
        {
            var names = string.Join(", ", mappers.Select(z => z.Name));
            yield return $"missing parameter: {names}.";
        }

        public void Format(IOutput output, CommandMapper commandMapper, IEnumerable<IParameterMapper> mappers,
            ICommandParameterParser parser)
        {
            foreach (var line in this.Format(commandMapper, mappers, parser))
            {
                output.WriteLine(line);
            }
        }
    }
}