using Jasily.Framework.ConsoleEngine.Mappers;
using System;

namespace Jasily.Framework.ConsoleEngine.Converters
{
    public struct ConverterAgent
    {
        public ConvertersMapper ConvertersMapper { get; }

        public ConverterAgent(ConvertersMapper convertersMapper)
        {
            this.ConvertersMapper = convertersMapper;
        }

        public bool CanConvert(Type to)
        {
            if (to == typeof(string)) return true;
            if (to.IsEnum) return true;
            if (this.ConvertersMapper[to] != null) return true;
            if (to.IsGenericType)
            {
                var genericTypeDef = to.GetGenericTypeDefinition();
                if (genericTypeDef == typeof(Nullable<>))
                {
                    return this.CanConvert(to.GetGenericArguments()[0]);
                }
            }
            return false;
        }

        public bool Convert(Type to, string input, out object output)
        {
            if (to == typeof(string))
            {
                output = input;
                return true;
            }

            var converter = this.ConvertersMapper[to];

            if (converter == null)
            {
                if (to.IsGenericType)
                {
                    var genericTypeDef = to.GetGenericTypeDefinition();
                    if (genericTypeDef == typeof(Nullable<>))
                    {
                        converter = this.ConvertersMapper.NullableConverter;
                    }
                }
            }

            if (converter != null)
            {
                return converter.Convert(to, input, out output);
            }

            return this.ConvertersMapper.EnumConverter.Convert(to, input, out output);
        }

        public string GetVaildInput(Type to)
        {
            var converter = this.ConvertersMapper[to];
            if (converter != null)
            {
                return converter.GetVaildInput(to);
            }
            return this.ConvertersMapper.EnumConverter.GetVaildInput(to);
        }
    }
}