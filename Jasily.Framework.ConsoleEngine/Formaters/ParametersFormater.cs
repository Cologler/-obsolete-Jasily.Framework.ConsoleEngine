using Jasily.Framework.ConsoleEngine.IO;
using Jasily.Framework.ConsoleEngine.Mappers;
using Jasily.Framework.ConsoleEngine.Parameters;
using System.Collections.Generic;

namespace Jasily.Framework.ConsoleEngine.Formaters
{
    public class ParametersFormater : IParametersFormater
    {
        public IEnumerable<string> Format(CommandMapper commandMapper, IEnumerable<ParameterMapper> mappers,
            ICommandParameterParser parser)
        {
            foreach (var mapper in mappers)
            {
                var name = string.Join("/", mapper.GetNames());
                yield return $"{name}";
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