using System.Collections.Generic;
using System.Linq;

namespace Jasily.Framework.ConsoleEngine
{
    public sealed class CommandLine
    {
        public static CommandLine Empty { get; } = new CommandLine(string.Empty)
        {
            blocks = new CommandBlock[0]
        };

        private readonly string originLine;
        private CommandBlock[] blocks;

        public CommandLine(string originLine)
        {
            this.originLine = originLine;
        }

        public IEnumerable<CommandBlock> Blocks => this.blocks;

        public CommandBlock CommandBlock { get; private set; }

        public void Parse(CommandBlockParser blockParser)
        {
            this.blocks = blockParser.Parse(this.originLine).ToArray();
            this.CommandBlock = this.blocks.FirstOrDefault();
            this.blocks = this.blocks.Skip(1).ToArray();
        }
    }
}