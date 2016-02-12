using Jasily.Framework.ConsoleEngine.Attributes;
using Jasily.Framework.ConsoleEngine.Commands;
using Jasily.Framework.ConsoleEngine.Converters;
using Jasily.Framework.ConsoleEngine.Executors;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Jasily.Framework.ConsoleEngine.Mappers
{
    public sealed class CommandMapper : BaseMapper<CommandAttribute, CommandAttributeMapper>
    {
        private CommandMapper(Type type)
            : base(type)
        {
            this.CommandSource = new ClassCommandSource(type);

            this.DesciptionCommand = new Interface<IDesciptionCommand>(this);
            this.HelpCommand = new Interface<IHelpCommand>(this);
            this.AttributeMapper = new CommandAttributeMapper(this.CommandSource);
        }
        private CommandMapper(Type type, MethodInfo method)
            : base(type)
        {
            this.CommandSource = new MethodCommandSource(type, method);

            this.DesciptionCommand = Interface<IDesciptionCommand>.None;
            this.HelpCommand = Interface<IHelpCommand>.None;
            this.AttributeMapper = new CommandAttributeMapper(this.CommandSource);
        }

        public CommandClassBuilder CommandClassBuilder { get; private set; }

        public CommandExecutorBuilder ExecutorBuilder { get; private set; }

        public CommandSource CommandSource { get; }

        public static IEnumerable<CommandMapper> TryMap(Type type, ConverterAgent converter)
        {
            if (type.IsAbstract && type.IsSealed) yield break; // ignore static class

            CommandClassBuilder classBuilder = null;

            if (type.GetCustomAttribute<CommandAttribute>() != null && typeof(ICommand).IsAssignableFrom(type))
            {
                var mapper = new CommandMapper(type);
                if (mapper.TryMap())
                {
                    classBuilder = CommandClassBuilder.TryCreate(mapper.CommandSource);
                    mapper.CommandClassBuilder = classBuilder;
                    mapper.ExecutorBuilder = CommandExecutorBuilder.TryCreate(mapper, converter);
                    if (mapper.CommandClassBuilder != null && mapper.ExecutorBuilder != null)
                    {
                        yield return mapper;
                    }
                }
            }

            foreach (var method in type.GetRuntimeMethods())
            {
                if (method.GetCustomAttribute<CommandAttribute>() != null)
                {
                    var mapper = new CommandMapper(type, method);
                    if (mapper.TryMap())
                    {
                        if (classBuilder == null) classBuilder = CommandClassBuilder.TryCreate(mapper.CommandSource);
                        mapper.CommandClassBuilder = classBuilder;
                        mapper.ExecutorBuilder = CommandExecutorBuilder.TryCreate(mapper, converter);
                        if (mapper.CommandClassBuilder != null && mapper.ExecutorBuilder != null)
                        {
                            yield return mapper;
                        }
                    }
                }
            }
        }

        public bool IsStatic => this.CommandSource.IsStatic;

        public CommandType CommandType => this.AttributeMapper.NameAttribute.CommandType;

        public Interface<IHelpCommand> HelpCommand { get; }

        public Interface<IDesciptionCommand> DesciptionCommand { get; }

        public class Interface<T>
        {
            public static Interface<T> None { get; } = new Interface<T>(null);

            private readonly CommandMapper mapper;

            internal Interface(CommandMapper mapper)
            {
                if (mapper != null)
                {
                    this.mapper = mapper;
                    this.IsImplemented = typeof(T).IsAssignableFrom(mapper.MapedType);
                }
            }

            public bool IsImplemented { get; }

            public T GetInstance()
            {
                if (!this.IsImplemented) throw new InvalidOperationException();
                return (T)this.mapper.CommandClassBuilder.Build();
            }
        }
    }
}