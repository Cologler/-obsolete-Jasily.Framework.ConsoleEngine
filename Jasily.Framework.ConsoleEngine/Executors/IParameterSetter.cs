using Jasily.Framework.ConsoleEngine.Mappers;

namespace Jasily.Framework.ConsoleEngine.Executors
{
    public interface IParameterSetter
    {
        IParameterMapper Mapper { get; }
    }
}