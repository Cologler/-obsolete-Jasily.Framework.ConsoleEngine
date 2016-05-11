using Jasily.Framework.ConsoleEngine.Attributes;
using Jasily.Framework.ConsoleEngine.Converters;
using Jasily.Framework.ConsoleEngine.Mappers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Jasily.Framework.ConsoleEngine.Executors
{
    public class CommandExecutorBuilder
    {
        private readonly CommandSourceType commandSourceType;
        private Action<object, object[]> methodParameterSetter;

        private readonly List<IParameterMapper> parameterMappers = new List<IParameterMapper>();

        private CommandExecutorBuilder(CommandSourceType commandSourceType)
        {
            this.commandSourceType = commandSourceType;
        }

        public CommandExecutor CreateExecutor(object obj)
        {
            switch (this.commandSourceType)
            {
                case CommandSourceType.Class:
                    return new ClassCommandExecutor(obj,
                        this.parameterMappers.OfType<PropertyParameterMapper>());

                case CommandSourceType.Method:
                    return new MethodCommandExecutor(obj,
                        this.parameterMappers.OfType<MethodParameterMapper>(),
                        this.methodParameterSetter);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal static CommandExecutorBuilder TryCreate(CommandMapper mapper, ConverterAgent agent)
        {
            var builder = new CommandExecutorBuilder(mapper.CommandSource.SourceType);

            var dict = new Dictionary<string, bool>();
            switch (mapper.CommandSource.SourceType)
            {
                case CommandSourceType.Class:
                    var mappers = new List<PropertyParameterMapper>();
                    foreach (var property in mapper.CommandSource.ClassType.GetProperties())
                    {
                        if (property.GetCustomAttribute<PropertyParameterAttribute>() != null)
                        {
                            if (!property.CanWrite)
                            {
                                Debug.WriteLine("parameter alway should can write!");
                                if (Debugger.IsAttached) Debugger.Break();
                                return null;
                            }

                            if (!agent.CanConvert(property.PropertyType))
                            {
                                Debug.WriteLine($"parameter type [{property.PropertyType.Name}] missing converter.");
                                if (Debugger.IsAttached) Debugger.Break();
                                return null;
                            }

                            var setter = new Action<object, object>(property.SetValue);
                            var parameterMapper = new PropertyParameterMapper(property, property.PropertyType, setter);
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
                            mappers.Add(parameterMapper);
                        }
                    }
                    mappers.Sort((x, y) =>
                        x.AttributeMapper.NameAttribute.Order.CompareTo(
                        y.AttributeMapper.NameAttribute.Order));
                    builder.parameterMappers.AddRange(mappers);
                    break;

                case CommandSourceType.Method:
                    var method = ((MethodCommandSource)mapper.CommandSource).Method;
                    foreach (var parameter in method.GetParameters())
                    {
                        if (!JasilyConsoleEngine.IsDefaultParameters(parameter.ParameterType) &&
                            !agent.CanConvert(parameter.ParameterType))
                        {
                            Debug.WriteLine($"parameter type [{parameter.ParameterType.Name}] missing converter.");
                            if (Debugger.IsAttached) Debugger.Break();
                            return null;
                        }

                        var parameterMapper = new MethodParameterMapper(parameter);
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

                        builder.methodParameterSetter = (obj, args) => method.Invoke(obj, args);
                        builder.parameterMappers.Add(parameterMapper);
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return builder;
        }

        public IEnumerable<IParameterMapper> Mappers => this.parameterMappers.Where(z => !z.IsInternal);
    }
}