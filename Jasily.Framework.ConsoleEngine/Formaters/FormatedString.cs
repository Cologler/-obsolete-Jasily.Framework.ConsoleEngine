namespace Jasily.Framework.ConsoleEngine.Formaters
{
    public struct FormatedString
    {
        public string NonFormatText { get; }

        public FormatedString(string text)
        {
            this.NonFormatText = text;
        }

        public static implicit operator string(FormatedString s) => s.NonFormatText;

        public static implicit operator FormatedString(string s) => new FormatedString(s);
    }
}