using Jasily.Framework.ConsoleEngine.Extensions;
using Jasily.Framework.ConsoleEngine.Formaters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jasily.Framework.ConsoleEngine.Converters
{
    public class EnumConverter : IConverter
    {
        private readonly Dictionary<Type, EnumMapper[]> mappers
            = new Dictionary<Type, EnumMapper[]>();

        public bool Convert(Type to, string text, out object value)
        {
            var enumValues = this.GetEnums(to);

            var val =
                enumValues.FirstOrDefault(z => string.Equals(z.Text, text, StringComparison.Ordinal)) ??
                enumValues.FirstOrDefault(z => string.Equals(z.Text, text, StringComparison.OrdinalIgnoreCase));

            if (val == null)
            {
                int n;
                if (int.TryParse(text, out n))
                {
                    val = enumValues.FirstOrDefault(z => z.Value == n);
                }
            }

            if (val != null)
            {
                value = val.Enum;
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }

        public FormatedString GetVaildInput(Type to)
        {
            var enumValues = this.GetEnums(to);
            return "[" + string.Join("|", enumValues.Select(z => z.Text)) + "]";
        }

        private EnumMapper[] GetEnums(Type to)
        {
            return this.mappers.GetOrSetValue(to, () =>
                Enum.GetValues(to).OfType<object>().Select(z => new EnumMapper(z)).ToArray());
        }

        private class EnumMapper
        {
            public EnumMapper(object @enum)
            {
                this.Enum = @enum;
                this.Value = (int)@enum;
                this.Text = @enum.ToString();
            }

            public object Enum { get; }

            public int Value { get; }

            public string Text { get; }
        }
    }
}