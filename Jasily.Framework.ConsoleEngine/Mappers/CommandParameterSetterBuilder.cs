using Jasily.Framework.ConsoleEngine.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Jasily.Framework.ConsoleEngine.Mappers
{
    public class CommandParameterSetterBuilder
    {
        private readonly List<ParameterMapper> parameterMappers = new List<ParameterMapper>();

        private CommandParameterSetterBuilder()
        {
        }

        public CommandParameterSetter CreateSetter() => new CommandParameterSetter(this.parameterMappers);

        internal static CommandParameterSetterBuilder TryCreate(JasilyConsoleEngine engine, CommandMapper mapper)
        {
            var builder = new CommandParameterSetterBuilder();
            var dict = new Dictionary<string, bool>();
            foreach (var property in mapper.MapedType.GetProperties())
            {
                var attr = property.GetCustomAttribute<ParameterAttribute>();

                if (attr != null)
                {
                    if (!property.CanWrite)
                    {
                        Debug.WriteLine("parameter alway should can write!");
                        if (Debugger.IsAttached) Debugger.Break();
                        return null;
                    }
                    if (!engine.Converters.CanConvert(property.PropertyType))
                    {
                        Debug.WriteLine($"parameter type [{property.PropertyType.Name}] missing converter.");
                        if (Debugger.IsAttached) Debugger.Break();
                        return null;
                    }
                    var setter = new Action<object, object>(property.SetValue);
                    var parameterMapper = new ParameterMapper(property, property.PropertyType, setter);
                    if (!parameterMapper.TryMap()) return null;

                    foreach (var parameterName in parameterMapper.GetNames())
                    {
                        if (dict.ContainsKey(parameterName))
                        {
                            Debug.WriteLine($"parameter name {parameterName} was exists");
                            if (Debugger.IsAttached) Debugger.Break();
                            return null;
                        }
                        else
                        {
                            dict.Add(parameterName, false);
                        }
                    }

                    builder.parameterMappers.Add(parameterMapper);
                }
            }
            return builder;
        }

        public IEnumerable<ParameterMapper> Mappers => this.parameterMappers;
    }
}