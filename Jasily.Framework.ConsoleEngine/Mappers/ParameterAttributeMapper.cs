using Jasily.Framework.ConsoleEngine.Attributes;

namespace Jasily.Framework.ConsoleEngine.Mappers
{
    public abstract class ParameterAttributeMapper : BaseAttributeMapper<ParameterAttribute>
    {

    }

    public abstract class ParameterAttributeMapper<T> : BaseAttributeMapper<T>
        where T : ParameterAttribute
    {

    }
}