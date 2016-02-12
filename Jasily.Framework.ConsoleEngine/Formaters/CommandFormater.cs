using Jasily.Framework.ConsoleEngine.Mappers;
using System.Collections.Generic;

namespace Jasily.Framework.ConsoleEngine.Formaters
{
    public class CommandFormater : ICommandFormater
    {
        public IndentMode Indent { get; set; } = IndentMode.Tab;

        public int IndentSize { get; set; } = 4;

        private string GetIndent()
        {
            var size = this.IndentSize;
            return size < 1 ? string.Empty
                : new string(this.Indent == IndentMode.Tab ? '\t' : ' ', size);
        }

        public IEnumerable<FormatedString> Format(CommandMapper commandMapper)
        {
            var command = commandMapper.Name;
            yield return new FormatedString($"{command}{this.GetIndent()}{commandMapper.Desciption}");
        }
    }
}