using System;

namespace Jasily.Framework.ConsoleEngine.Converters
{
    internal abstract class DigitConverter<T>
    {
        public bool Convert(Type to, string text, out T value) => this.TryParse(text, out value);

        public bool Convert(Type to, string text, out object value)
        {
            T n;
            if (this.TryParse(text, out n))
            {
                value = n;
                return true;
            }
            value = null;
            return false;
        }

        protected abstract bool TryParse(string text, out T ret);
    }
}