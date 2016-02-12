using Jasily.Framework.ConsoleEngine.Commands;
using Jasily.Framework.ConsoleEngine.Mappers;
using System.Collections.Generic;
using System.Linq;

namespace Jasily.Framework.ConsoleEngine.Executors
{
    public sealed class ClassCommandExecutor : CommandExecutor<PropertyParameterMapper>
    {
        internal ClassCommandExecutor(object obj, IEnumerable<PropertyParameterMapper> mappers)
            : base(obj, mappers)
        {
        }

        public override void Execute(Session session, CommandLine line)
        {
            if (this.IsAllVaild())
            {
                foreach (var task in this.Tasks)
                {
                    task.Mapper.Setter(this.Obj, task.Value);
                }

                ((ICommand)this.Obj).Execute(session, line);
            }
        }

        public bool IsAllVaild() => this.Tasks.All(z => z.IsVaild);
    }
}