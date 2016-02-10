using Jasily.Framework.ConsoleEngine.Mappers;
using System;

namespace Jasily.Framework.ConsoleEngine.Converters
{
    public class ConverterAgent
    {
        public ConvertersMapper ConvertersMapper { get; } = new ConvertersMapper();

        public IConverter EnumConverter { get; } = new EnumConverter();

        public bool CanConvert(Type to)
        {
            return to == typeof(string) || this.ConvertersMapper[to] != null || to.IsEnum;
        }

        public bool Convert(Type to, string input, out object output)
        {
            if (to == typeof(string))
            {
                output = input;
                return true;
            }
            var converter = this.ConvertersMapper[to];
            if (converter != null)
            {
                return converter.Convert(to, input, out output);
            }
            return this.EnumConverter.Convert(to, input, out output);
        }

        public string GetVaildInput(Type to)
        {
            var converter = this.ConvertersMapper[to];
            if (converter != null)
            {
                return converter.GetVaildInput(to);
            }
            return this.EnumConverter.GetVaildInput(to);
        }
    }
}