using System;
using Jasily.Framework.ConsoleEngine.Formaters;

namespace Jasily.Framework.ConsoleEngine.Converters
{
    internal sealed class DecimalConverter : DigitConverter<decimal>, IConverter<decimal>
    {
        public FormatedString GetVaildInput(Type to) => "digit";

        protected override bool TryParse(string text, out decimal ret)
            => decimal.TryParse(text, out ret);
    }
}