using Jasily.Framework.ConsoleEngine.Commands;
using Jasily.Framework.ConsoleEngine.Extensions;
using Jasily.Framework.ConsoleEngine.Mappers;
using System.Collections.Generic;
using System.Linq;

namespace Jasily.Framework.ConsoleEngine.Executors
{
    internal sealed class ClassCommandExecutor : CommandExecutor<PropertyParameterMapper>
    {
        private readonly Dictionary<int, Dictionary<string, List<ParameterSetter<PropertyParameterMapper>>>> settersMap
               = new Dictionary<int, Dictionary<string, List<ParameterSetter<PropertyParameterMapper>>>>();

        internal ClassCommandExecutor(object obj, IEnumerable<PropertyParameterMapper> mappers)
            : base(obj, mappers)
        {
            foreach (var setter in this.Setters)
            {
                foreach (var groupId in setter.Mapper.AttributeMapper.GroupIds)
                {
                    var dict = this.settersMap.GetOrCreateValue(groupId);
                    foreach (var name in setter.Mapper.GetNames())
                    {
                        dict.GetOrCreateValue(name).Add(setter);
                    }
                }
            }
        }

        public override bool IsVaildCommand()
            => this.settersMap.Any(z => z.Value.SelectMany(x => x.Value).All(c => c.IsVaild));

        public override void Execute(Session session, CommandLine line)
        {
            if (this.IsVaildCommand())
            {
                foreach (var task in this.Setters.Where(z => z.IsSeted))
                {
                    task.Mapper.Setter(this.Obj, task.Value);
                }

                ((ICommand)this.Obj).Execute(session, line);
            }
        }
    }
}