using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Jasily.Framework.ConsoleEngine
{
    public sealed class CommandLine
    {
        public static CommandLine Empty { get; } = new CommandLine(string.Empty, new CommandBlock[0]);

        public string OriginCommand { get; }

        internal CommandLine(string originLine, CommandBlock[] blocks)
        {
            this.OriginCommand = originLine;
            this.CommandBlock = blocks.FirstOrDefault();
            this.ParameterBlocks = new ReadOnlyCollection<CommandBlock>(blocks.Skip(1).ToArray());
        }

        public IEnumerable<CommandBlock> ParameterBlocks { get; }

        /// <summary>
        /// possible null.
        /// </summary>
        public CommandBlock CommandBlock { get; }
    }
}