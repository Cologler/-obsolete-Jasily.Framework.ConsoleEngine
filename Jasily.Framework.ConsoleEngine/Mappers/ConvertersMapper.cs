using Jasily.Framework.ConsoleEngine.Converters;
using Jasily.Framework.ConsoleEngine.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jasily.Framework.ConsoleEngine.Mappers
{
    public sealed class ConvertersMapper
    {
        private readonly Dictionary<Type, IConverter> converters = new Dictionary<Type, IConverter>();

        public ConvertersMapper()
        {
            this.EnumConverter = new EnumConverter();
            this.NullableConverter = new NullableConverter(this);
        }

        public void Index(Type type)
        {
            foreach (var z in type.GetInterfaces().ToArray())
            {
                if (z.IsGenericType &&
                    z.GetGenericTypeDefinition() == typeof(IConverter<>) &&
                    z.GetConstructor(new Type[0]) != null)
                {
                    var to = z.GetGenericArguments()[0];
                    this.converters[to] = (IConverter)Activator.CreateInstance(type);
                }
            }
        }

        public void Index<T>(IConverter<T> converter)
        {
            if (converter == null) throw new ArgumentNullException(nameof(converter));

            this.converters[typeof(T)] = converter;
        }

        public IConverter this[Type t] => this.converters.GetValueOrDefault(t);

        public IConverter EnumConverter { get; }

        public IConverter NullableConverter { get; }

        public ConverterAgent GetAgent() => new ConverterAgent(this);
    }
}