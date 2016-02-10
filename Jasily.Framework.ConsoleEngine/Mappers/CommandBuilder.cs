using Jasily.Framework.ConsoleEngine.Commands;
using System;
using System.Linq;

namespace Jasily.Framework.ConsoleEngine.Mappers
{
    public class CommandBuilder
    {
        private readonly bool isStatic;
        private readonly Func<object> constructor;
        private ICommand instance;

        private CommandBuilder(bool isStatic, Func<object> constructor)
        {
            this.isStatic = isStatic;
            this.constructor = constructor;
        }

        public ICommand Build()
        {
            if (this.isStatic)
            {
                this.instance = this.instance ?? (ICommand)this.constructor();
                return this.instance;
            }
            else
            {
                return (ICommand)this.constructor();
            }
        }

        internal static CommandBuilder TryCreate(CommandMapper mapper)
        {
            var constructors = mapper.Type.GetConstructors().ToArray();
            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters().ToArray();
                if (parameters.Length == 0)
                {
                    return new CommandBuilder(mapper.Attribute.IsStatic, () => Activator.CreateInstance(mapper.Type));
                }
            }
            return null;
        }
    }
}