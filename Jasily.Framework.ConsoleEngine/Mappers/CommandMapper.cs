using Jasily.Framework.ConsoleEngine.Attributes;
using Jasily.Framework.ConsoleEngine.Commands;
using System;
using System.Reflection;

namespace Jasily.Framework.ConsoleEngine.Mappers
{
    public class CommandMapper : NameableMapper<CommandAttribute>
    {
        private readonly CommandAttribute commandAttribute;

        private CommandMapper(Type type, CommandAttribute commandAttribute)
            : base(commandAttribute)
        {
            this.commandAttribute = commandAttribute;
            this.Type = type;
        }

        public Type Type { get; }

        public override string Name => this.Command;

        public string Command { get; private set; }

        public CommandBuilder CommandBuilder { get; private set; }

        public CommandParameterSetterBuilder ParameterSetterBuilder { get; private set; }

        public static CommandMapper TryMap(JasilyConsoleEngine engine, Type type)
        {
            if (!typeof(ICommand).IsAssignableFrom(type)) return null;

            var attr = type.GetCustomAttribute<CommandAttribute>();
            if (attr == null) return null;

            var mapper = new CommandMapper(type, attr);
            var cmd = attr.Name;
            if (string.IsNullOrWhiteSpace(cmd))
            {
                cmd = type.Name;
                if (cmd.EndsWith("Command")) cmd = cmd.Substring(0, cmd.Length - "Command".Length);
            }
            mapper.Command = cmd;

            mapper.CommandBuilder = CommandBuilder.TryCreate(mapper);
            if (mapper.CommandBuilder == null) return null;

            mapper.ParameterSetterBuilder = CommandParameterSetterBuilder.TryCreate(engine, mapper);
            if (mapper.ParameterSetterBuilder == null) return null;

            return mapper;
        }
    }
}