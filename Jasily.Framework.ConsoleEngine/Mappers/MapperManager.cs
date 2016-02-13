using Jasily.Framework.ConsoleEngine.Converters;
using Jasily.Framework.ConsoleEngine.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Jasily.Framework.ConsoleEngine.Mappers
{
    public class MapperManager
    {
        private readonly JasilyConsoleEngine engine;
        private readonly List<Assembly> registedAssemblys = new List<Assembly>();
        private readonly Dictionary<Type, List<CommandMapper>> registedTypes
            = new Dictionary<Type, List<CommandMapper>>();
        private readonly List<CommandMapper> mappedCommands = new List<CommandMapper>();
        private readonly Dictionary<string, List<CommandMapper>> commandMappersMap
            = new Dictionary<string, List<CommandMapper>>();
        private readonly Dictionary<Type, object> registedDefaultValue
            = new Dictionary<Type, object>();

        public ConvertersMapper Converters { get; } = new ConvertersMapper();

        public MapperManager(JasilyConsoleEngine engine)
        {
            this.engine = engine;
            this.RegistConverter(new Int32Converter());
            this.RegistConverter(new DoubleConverter());
            this.RegistConverter(new DecimalConverter());
        }

        public void RegistCommand(Type type)
        {
            if (this.registedAssemblys.Contains(type.Assembly)) return;

            this.ResortChanged(this.Map(type));
        }

        public void RegistConverter(Type type) => this.Converters.Index(type);

        public void RegistConverter<T>(IConverter<T> converter)
        {
            if (converter == null) throw new ArgumentNullException(nameof(converter));

            this.Converters.Index(converter);
        }

        public void RegistDefaultValue<T>(T value)
        {
            if (this.registedDefaultValue.ContainsKey(typeof(T))) throw new InvalidOperationException();

            this.registedDefaultValue.Add(typeof(T), value);
        }

        public void RegistAssembly(Assembly assembly)
        {
            if (this.registedAssemblys.Contains(assembly)) return;
            this.registedAssemblys.Add(assembly);

            var changed = new List<List<CommandMapper>>();

            var types = assembly.GetTypes().ToArray();

            foreach (var type in types)
            {
                this.RegistConverter(type);
            }

            foreach (var type in types)
            {
                changed.AddRange(this.Map(type));
            }

            this.ResortChanged(changed);
        }

        private IEnumerable<List<CommandMapper>> Map(Type type)
        {
            if (this.registedTypes.ContainsKey(type)) return Enumerable.Empty<List<CommandMapper>>();

            var mappers = CommandMapper.TryMap(type, this.engine.MapperManager.GetAgent()).ToList();
            this.registedTypes.Add(type, mappers);
            this.mappedCommands.AddRange(mappers);

            var changed = new List<List<CommandMapper>>();
            foreach (var mapper in mappers)
            {
                foreach (var lower in mapper.GetNames().Select(z => z.ToLower()))
                {
                    var col = this.commandMappersMap.GetOrCreateValue(lower);
                    col.Add(mapper);
                    changed.Add(col);
                }
            }

            return changed;
        }

        private void ResortChanged(IEnumerable<List<CommandMapper>> changed)
        {
            foreach (var list in changed.Distinct())
            {
                list.Sort((x, y) => ((int)x.CommandType).CompareTo((int)y.CommandType));
            }
        }

        public IEnumerable<CommandMapper> GetCommandMappers() => this.mappedCommands;

        internal CommandMapper GetCommand(CommandLine line)
        {
            var command = line.CommandBlock.OriginText;
            var list = this.commandMappersMap.GetValueOrDefault(command.ToLower());
            return list?.FirstOrDefault(mapper => mapper.IsMatch(command));
        }

        public ConverterAgent GetAgent() => new ConverterAgent(this.Converters, this.registedDefaultValue);
    }
}