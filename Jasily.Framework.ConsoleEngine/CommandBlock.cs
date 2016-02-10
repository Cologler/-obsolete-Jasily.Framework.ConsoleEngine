namespace Jasily.Framework.ConsoleEngine
{
    public class CommandBlock
    {
        private string command;

        public string OriginText { get; }

        public string Command
        {
            get
            {
                if (this.command == null)
                {
                    var sp = this.OriginText.Split(':');
                    this.command = sp[0].ToLower();
                }
                return this.command;
            }
        }

        public CommandBlock(string text)
        {
            this.OriginText = text;
        }
    }
}