using Jasily.Framework.ConsoleEngine.IO;
using Jasily.Framework.ConsoleEngine.Mappers;
using Jasily.Framework.ConsoleEngine.Parameters;
using System.Collections.Generic;
using System.Linq;

namespace Jasily.Framework.ConsoleEngine.Formaters
{
    public class MissingParametersFormater : IParametersFormater
    {
        public IEnumerable<string> Format(CommandMapper commandMapper, IEnumerable<ParameterMapper> mappers,
            ICommandParameterParser parser)
        {
            foreach (var mapper in mappers)
            {
                yield return $"missing parameter: {mapper.GetNames().First()}.";
            }
        }

        public void Format(IOutput output, CommandMapper commandMapper, IEnumerable<ParameterMapper> mappers,
            ICommandParameterParser parser)
        {
            foreach (var line in this.Format(commandMapper, mappers, parser))
            {
                output.WriteLine(line);
            }
        }
    }
}