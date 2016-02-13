using System.Collections.Generic;

namespace Jasily.Framework.ConsoleEngine.Mappers
{
    public interface INameMapper
    {
        string Name { get; }

        string Desciption { get; }

        IEnumerable<string> GetNames();
    }
}