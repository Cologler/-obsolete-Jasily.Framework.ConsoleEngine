using Jasily.Framework.ConsoleEngine.IO;
using Jasily.Framework.ConsoleEngine.Mappers;
using Jasily.Framework.ConsoleEngine.Parameters;
using System.Collections.Generic;
using System.Linq;

namespace Jasily.Framework.ConsoleEngine.Formaters
{
    public class ParametersFormater : IParametersFormater
    {
        public IEnumerable<FormatedString> Format(CommandMapper commandMapper, IEnumerable<ParameterMapper> mappers,
            ICommandParameterParser parser)
        {
            foreach (var mapper in mappers)
            {
                var names = string.Join("/", mapper.GetNames());
                yield return $"{names} {parser.GetInputSytle(mapper.Name)}\t\t\t{mapper.Desciption}";
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