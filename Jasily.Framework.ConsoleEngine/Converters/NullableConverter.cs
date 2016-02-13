using Jasily.Framework.ConsoleEngine.Formaters;
using Jasily.Framework.ConsoleEngine.Mappers;
using System;

namespace Jasily.Framework.ConsoleEngine.Converters
{
    public sealed class NullableConverter : IConverter
    {
        private readonly ConvertersMapper convertersMapper;

        public NullableConverter(ConvertersMapper convertersMapper)
        {
            this.convertersMapper = convertersMapper;
        }

        public bool Convert(Type to, string text, out object value)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                value = null;
                return true;
            }

            var converter = this.convertersMapper[to];
            if (converter != null)
            {
                return converter.Convert(to, text, out value);
            }

            value = null;
            return false;
        }

        public FormatedString GetVaildInput(Type to) => string.Empty;
    }
}