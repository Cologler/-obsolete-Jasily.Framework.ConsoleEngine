using System;
using System.Collections.Generic;

namespace Jasily.Framework.ConsoleEngine.Parameters
{
    public class CommandParameterParser : ICommandParameterParser
    {
        private readonly bool ignoreCase;
        private readonly string startStyle;

        public CommandParameterParser(bool ignoreCase, string startStyle = "-", string keyValueSpliter = ":")
        {
            this.Spliter = keyValueSpliter;
            this.ignoreCase = ignoreCase;
            this.startStyle = startStyle;
        }

        public string Spliter { get; }

        public KeyValuePair<string, string>? TryParse(CommandBlock block)
        {
            var comparison = this.ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            var text = block.OriginText;
            if (text.StartsWith(this.startStyle, comparison))
            {
                var index = text.IndexOf(this.Spliter, this.startStyle.Length, comparison);
                if (index > -1)
                {
                    return new KeyValuePair<string, string>(
                        text.Substring(this.startStyle.Length, index - this.startStyle.Length),
                        text.Substring(index + this.Spliter.Length));
                }
            }
            return null;
        }

        public string GetInputSytle(string key)
            => $"{this.startStyle}{key}{this.Spliter}$(arg)";

        public IEnumerable<KeyValuePair<string, string>> Parse(IEnumerable<CommandBlock> blocks)
        {
            foreach (var block in blocks)
            {
                var kvp = this.TryParse(block);
                if (kvp != null) yield return kvp.Value;
            }
        }
    }
}