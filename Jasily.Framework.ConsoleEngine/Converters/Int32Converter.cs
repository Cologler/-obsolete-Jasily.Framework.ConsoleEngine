using Jasily.Framework.ConsoleEngine.Formaters;
using System;

namespace Jasily.Framework.ConsoleEngine.Converters
{
    internal sealed class Int32Converter : DigitConverter<int>, IConverter<int>
    {
        public FormatedString GetVaildInput(Type to) => "digit";

        protected override bool TryParse(string text, out int ret)
            => int.TryParse(text, out ret);
    }
}