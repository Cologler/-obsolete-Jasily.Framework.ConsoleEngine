using Jasily.Framework.ConsoleEngine.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jasily.Framework.ConsoleEngine.Executors
{
    public sealed class MethodCommandExecutor : CommandExecutor<MethodParameterMapper>
    {
        private readonly Action<object, object[]> methodParameterSetter;

        internal MethodCommandExecutor(object obj, IEnumerable<MethodParameterMapper> mappers,
            Action<object, object[]> methodParameterSetter)
            : base(obj, mappers)
        {
            this.methodParameterSetter = methodParameterSetter;
        }

        public override void Execute(Session session, CommandLine line)
        {
            if (this.IsAllVaild())
            {
                foreach (var task in this.Tasks.Where(z => !z.IsVaild))
                {
                    if (task.Mapper.MapedType == typeof(Session)) task.SetValue(session);
                    if (task.Mapper.MapedType == typeof(CommandLine)) task.SetValue(line);
                }

                var args = this.Tasks.Select(z => z.IsSeted ? z.Value : z.Mapper.DefaultValue).ToArray();
                this.methodParameterSetter(this.Obj, args);
            }
        }

        public override IEnumerable<ParameterMapper> GetMissingParameters()
            => this.Tasks.Where(z => z.IsMissing && !JasilyConsoleEngine.IsDefaultParameters(z.Mapper.MapedType))
                   .Select(z => z.Mapper);

        public bool IsAllVaild() => this.Tasks.All(
            z => z.IsVaild || JasilyConsoleEngine.IsDefaultParameters(z.Mapper.MapedType));
    }
}