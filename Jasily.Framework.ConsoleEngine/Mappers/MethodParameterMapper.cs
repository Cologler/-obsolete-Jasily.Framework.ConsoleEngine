using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Jasily.Framework.ConsoleEngine.Mappers
{
    public sealed class MethodParameterMapper : ParameterMapper
    {
        public MethodParameterMapper(ParameterInfo source)
            : base(source.ParameterType, null)
        {
            this.AttributeMapper = new MethodParameterAttributeMapper(source);
            this.IsOptional = source.HasDefaultValue;
            if (source.HasDefaultValue)
            {
                this.DefaultValue = source.DefaultValue;
            }
        }

        public override bool IsOptional { get; }

        public object DefaultValue { get; }

        public override IEnumerable<string> GetNames()
        {
            return JasilyConsoleEngine.IsDefaultParameters(this.MapedType)
                ? Enumerable.Empty<string>()
                : base.GetNames();
        }
    }
}