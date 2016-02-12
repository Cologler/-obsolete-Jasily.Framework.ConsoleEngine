using Jasily.Framework.ConsoleEngine.Attributes;
using System.Collections.Generic;
using System.Reflection;

namespace Jasily.Framework.ConsoleEngine.Mappers
{
    public sealed class CommandAttributeMapper : BaseAttributeMapper<CommandAttribute>
    {
        private readonly CommandSource commandSource;

        public CommandAttributeMapper(CommandSource commandSource)
        {
            this.commandSource = commandSource;
        }

        protected override T GetCustomAttribute<T>()
            => this.commandSource.GetMapObject().GetCustomAttribute<T>();

        protected override IEnumerable<T> GetCustomAttributes<T>()
            => this.commandSource.GetMapObject().GetCustomAttributes<T>();
    }
}