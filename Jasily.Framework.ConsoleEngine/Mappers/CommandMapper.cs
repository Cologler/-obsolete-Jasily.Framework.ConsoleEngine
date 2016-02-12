using Jasily.Framework.ConsoleEngine.Attributes;
using Jasily.Framework.ConsoleEngine.Commands;
using System;
using System.Reflection;

namespace Jasily.Framework.ConsoleEngine.Mappers
{
    public sealed class CommandMapper : BaseMapper<CommandAttribute, CommandAttributeMapper>
    {
        private CommandMapper(Type type)
            : base(type)
        {
            this.DesciptionCommand = new Interface<IDesciptionCommand>(this);
            this.HelpCommand = new Interface<IHelpCommand>(this);
        }

        public CommandBuilder CommandBuilder { get; private set; }

        public CommandParameterSetterBuilder ParameterSetterBuilder { get; private set; }

        public static CommandMapper TryMap(JasilyConsoleEngine engine, Type type)
        {
            if (!typeof(ICommand).IsAssignableFrom(type)) return null;

            var attr = type.GetCustomAttribute<CommandAttribute>();
            if (attr == null) return null;

            var mapper = new CommandMapper(type);
            if (!mapper.TryMap()) return null;

            mapper.CommandBuilder = CommandBuilder.TryCreate(mapper);
            if (mapper.CommandBuilder == null) return null;

            mapper.ParameterSetterBuilder = CommandParameterSetterBuilder.TryCreate(engine, mapper);
            if (mapper.ParameterSetterBuilder == null) return null;

            return mapper;
        }

        public bool IsStatic => this.AttributeMapper.NameAttribute.IsStatic;

        public CommandType CommandType => this.AttributeMapper.NameAttribute.CommandType;

        public Interface<IHelpCommand> HelpCommand { get; }

        public Interface<IDesciptionCommand> DesciptionCommand { get; }

        public class Interface<T>
        {
            private readonly CommandMapper mapper;

            public Interface(CommandMapper mapper)
            {
                this.mapper = mapper;
                this.IsImplemented = typeof(T).IsAssignableFrom(mapper.MapedType);
            }

            public bool IsImplemented { get; }

            public T GetInstance()
            {
                if (!this.IsImplemented) throw new InvalidOperationException();
                return (T) this.mapper.CommandBuilder.Build();
            }
        }
    }
}