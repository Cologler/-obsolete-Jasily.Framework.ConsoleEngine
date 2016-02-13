using System;
using Jasily.Framework.ConsoleEngine.Formaters;

namespace Jasily.Framework.ConsoleEngine.Converters
{
    public sealed class BooleanConverter : IConverter<bool>
    {
        public bool Convert(Type to, string text, out bool value)
        {
            value = true;
            return true;
        }

        public bool Convert(Type to, string text, out object value)
        {
            value = true;
            return true;
        }

        public FormatedString GetVaildInput(Type to) => string.Empty;
    }
}