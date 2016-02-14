using Jasily.Framework.ConsoleEngine.Executors;
using System.Collections.Generic;

namespace Jasily.Framework.ConsoleEngine.Parameters
{
    public interface ICommandParameterParser
    {
        string GetInputSytle(string key);

        IEnumerable<KeyValuePair<string, string>> Parse(
            CommandLine commandLine, IEnumerable<IParameterSetter> parameterSetters);
    }
}