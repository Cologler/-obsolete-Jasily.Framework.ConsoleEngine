using Jasily.Framework.ConsoleEngine.Commands;
using Jasily.Framework.ConsoleEngine.Converters;
using Jasily.Framework.ConsoleEngine.Extensions;
using Jasily.Framework.ConsoleEngine.Formaters;
using Jasily.Framework.ConsoleEngine.IO;
using Jasily.Framework.ConsoleEngine.Mappers;
using Jasily.Framework.ConsoleEngine.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Jasily.Framework.ConsoleEngine
{
    public sealed class JasilyConsoleEngine
    {
        private readonly List<Assembly> assemblys = new List<Assembly>();
        private readonly Dictionary<string, List<CommandMapper>> commandMappersMap
            = new Dictionary<string, List<CommandMapper>>();
        private readonly List<CommandMapper> commandMappers
            = new List<CommandMapper>();
        private readonly BuiltInMembers defaultMembers = BuiltInMembers.GetDefault();

        public ConverterAgent Converters { get; } = new ConverterAgent();

        public BuiltInMembers CustomMembers { get; } = BuiltInMembers.GetDefault();

        internal T GetCommandMember<T>(Func<BuiltInMembers, T> func) where T : class
            => func(this.CustomMembers) ?? func(this.defaultMembers);

        public JasilyConsoleEngine()
        {
            this.Converters.ConvertersMapper.Index(new Int32Converter());
        }

        public void RegistAssembly(Assembly assembly)
        {
            if (this.assemblys.Contains(assembly)) return;

            this.assemblys.Add(assembly);

            var changed = new List<List<CommandMapper>>();

            var types = assembly.GetTypes().ToArray();

            foreach (var type in types)
            {
                this.Converters.ConvertersMapper.Index(type);
            }

            foreach (var type in types)
            {
                var mapper = CommandMapper.TryMap(this, type);
                if (mapper != null)
                {
                    foreach (var key in mapper.GetNames().Select(z => z.ToLower()))
                    {
                        var col = this.commandMappersMap.GetOrCreateValue(key);
                        col.Add(mapper);
                        changed.Add(col);
                    }
                    this.commandMappers.Add(mapper);
                }
            }

            foreach (var list in changed.Distinct())
            {
                list.Sort((x, y) => (int)x.Attribute.CommandType.CompareTo((int)y.Attribute.CommandType));
            }
        }

        public IEnumerable<CommandMapper> CommandMappers => this.commandMappers;

        public Session StartSession() => new Session(this);

        internal CommandMapper GetFirstMatch(string command)
        {
            var list = this.commandMappersMap.GetValueOrDefault(command);
            return list?.FirstOrDefault(mapper => mapper.MatchCommand(command));
        }

        public class BuiltInMembers
        {
            public CommandBlockParser CommandParser { get; set; }

            public ICommandParameterParser CommandParameterParser { get; set; }

            public ICommand Helper { get; set; }

            public ICommand NoneInput { get; set; }

            public IOutput Output { get; set; }

            public IParametersFormater MissingParametersFormater { get; set; }

            public ICommandFormater CommandFormater { get; set; }

            public IParametersFormater ParametersFormater { get; set; }

            public static BuiltInMembers GetDefault()
            {
                return new BuiltInMembers()
                {
                    CommandParser = Singleton<CommandBlockParser>.Instance,
                    CommandParameterParser = new CommandParameterParser(true),
                    Helper = Singleton<HelpCommand>.Instance,
                    NoneInput = Singleton<DefaultNoneCommand>.Instance,
                    Output = Singleton<ConsoleOutput>.Instance,
                    MissingParametersFormater = Singleton<MissingParametersFormater>.Instance,
                    CommandFormater = Singleton<CommandFormater>.Instance,
                    ParametersFormater = Singleton<ParametersFormater>.Instance,
                };
            }
        }
    }
}
