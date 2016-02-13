using System;
using Jasily.Framework.ConsoleEngine.Formaters;

namespace Jasily.Framework.ConsoleEngine.Converters
{
    internal sealed class DoubleConverter : DigitConverter<double>, IConverter<double>
    {
        public FormatedString GetVaildInput(Type to) => "digit";

        protected override bool TryParse(string text, out double ret)
            => double.TryParse(text, out ret);
    }
}