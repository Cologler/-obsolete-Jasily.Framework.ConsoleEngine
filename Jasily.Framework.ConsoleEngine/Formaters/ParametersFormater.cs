using Jasily.Framework.ConsoleEngine.IO;
using Jasily.Framework.ConsoleEngine.Mappers;
using Jasily.Framework.ConsoleEngine.Parameters;
using System.Collections.Generic;

namespace Jasily.Framework.ConsoleEngine.Formaters
{
    public class ParametersFormater : IParametersFormater
    {
        public IEnumerable<FormatedString> Format(CommandMapper commandMapper, IEnumerable<IParameterMapper> mappers,
            ICommandParameterParser parser)
        {
            foreach (var mapper in mappers)
            {
                yield return $"{parser.GetInputSytle(mapper.Name)}\t\t\t{mapper.Desciption}";
                yield return $"  alias: {string.Join("; ", mapper.GetNames())}";
                yield return "";
            }
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