using System.Collections.Generic;

namespace Jasily.Framework.ConsoleEngine
{
    public interface ICommandBlockParser
    {
        IEnumerable<CommandBlock> Parse(string command);
    }
}