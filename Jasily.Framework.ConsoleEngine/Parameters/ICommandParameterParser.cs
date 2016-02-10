using System.Collections.Generic;

namespace Jasily.Framework.ConsoleEngine.Parameters
{
    public interface ICommandParameterParser
    {
        string GetInputSytle(string key);

        IEnumerable<KeyValuePair<string, string>> Parse(IEnumerable<CommandBlock> blocks);
    }
}