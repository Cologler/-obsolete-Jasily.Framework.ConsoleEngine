using Jasily.Framework.ConsoleEngine.Attributes;
using System;

namespace Jasily.Framework.ConsoleEngine.Mappers
{
    public abstract class ParameterMapper<TParameterAttribute, TParameterAttributeMapper> :
        BaseMapper<TParameterAttribute, TParameterAttributeMapper>, IParameterMapper
        where TParameterAttribute : ParameterAttribute
        where TParameterAttributeMapper : ParameterAttributeMapper<TParameterAttribute>
    {
        protected ParameterMapper(Type type)
            : base(type)
        {
        }

        public abstract bool IsOptional { get; }

        bool IParameterMapper.IsMatch(string commandLine) => this.IsMatch(commandLine);
    }
}