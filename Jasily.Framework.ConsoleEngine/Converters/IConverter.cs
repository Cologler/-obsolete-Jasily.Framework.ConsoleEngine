using Jasily.Framework.ConsoleEngine.Formaters;
using System;

namespace Jasily.Framework.ConsoleEngine.Converters
{
    public interface IConverter
    {
        bool Convert(Type to, string text, out object value);

        FormatedString GetVaildInput(Type to);
    }

    public interface IConverter<T> : IConverter
    {
        bool Convert(Type to, string text, out T value);
    }
}