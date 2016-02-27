using Jasily.Framework.ConsoleEngine.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jasily.Framework.ConsoleEngine.Executors
{
    internal sealed class MethodCommandExecutor : CommandExecutor<MethodParameterMapper>
    {
        private readonly Action<object, object[]> methodParameterSetter;

        internal MethodCommandExecutor(object obj, IEnumerable<MethodParameterMapper> mappers,
            Action<object, object[]> methodParameterSetter)
            : base(obj, mappers)
        {
            this.methodParameterSetter = methodParameterSetter;
        }

        protected override void InternalExecute(Session session, CommandLine line)
        {
            var args = this.Setters.Select(z => z.IsSeted ? z.Value : z.Mapper.DefaultValue).ToArray();
            this.methodParameterSetter(this.Obj, args);
        }

        public override IEnumerable<IParameterMapper> GetMissingParameters()
            => this.Setters.Where(z => z.IsMissing && !JasilyConsoleEngine.IsDefaultParameters(z.Mapper.MapedType))
                   .Select(z => z.Mapper);
    }
}