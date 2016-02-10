using Jasily.Framework.ConsoleEngine.Formaters;
using System;

namespace Jasily.Framework.ConsoleEngine.Converters
{
    public class Int32Converter : IConverter<int>
    {
        public bool Convert(Type to, string text, out int value) => int.TryParse(text, out value);

        public bool Convert(Type to, string text, out object value)
        {
            int n;
            if (int.TryParse(text, out n))
            {
                value = n;
                return true;
            }
            value = null;
            return false;
        }

        public FormatedString GetVaildInput(Type to) => "digit";
    }
}