﻿using Jasily.Framework.ConsoleEngine.Converters;
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
        private readonly Dictionary<Type, CommandMapper> registedTypes = new Dictionary<Type, CommandMapper>();
        private readonly Dictionary<string, List<CommandMapper>> commandMappersMap
            = new Dictionary<string, List<CommandMapper>>();

        public ConvertersMapper Converters { get; } = new ConvertersMapper();

        public MapperManager(JasilyConsoleEngine engine)
        {
            this.engine = engine;
            this.Converters.Index(new Int32Converter());
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
            if (this.registedTypes.ContainsKey(type))
                return Enumerable.Empty<List<CommandMapper>>();

            var changed = new List<List<CommandMapper>>();
            var mapper = CommandMapper.TryMap(this.engine, type);
            if (mapper != null)
            {
                this.registedTypes.Add(type, mapper);
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
                list.Sort((x, y) => ((int)x.Attribute.CommandType).CompareTo((int)y.Attribute.CommandType));
            }
        }

        public IEnumerable<CommandMapper> GetCommandMappers() => this.registedTypes.Values;

        internal CommandMapper GetCommand(CommandLine line)
        {
            var command = line.CommandBlock.OriginText;
            var list = this.commandMappersMap.GetValueOrDefault(command.ToLower());
            return list?.FirstOrDefault(mapper => mapper.IsMatch(command));
        }
    }
}