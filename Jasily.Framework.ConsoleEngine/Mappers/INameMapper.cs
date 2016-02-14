using System;
using System.Collections.Generic;

namespace Jasily.Framework.ConsoleEngine.Mappers
{
    public interface INameMapper
    {
        Type MapedType { get; }

        string Name { get; }

        string Desciption { get; }

        IEnumerable<string> GetNames();
    }
}