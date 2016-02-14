using System;
using System.Linq;

namespace Jasily.Framework.ConsoleEngine.Mappers
{
    public class CommandClassBuilder
    {
        private readonly bool isStatic;
        private readonly Func<object> constructor;
        private object instance;

        private CommandClassBuilder(bool isStatic, Func<object> constructor)
        {
            this.isStatic = isStatic;
            this.constructor = constructor;
        }

        public object Build()
        {
            if (this.isStatic)
            {
                this.instance = this.instance ?? this.constructor();
                return this.instance;
            }
            else
            {
                return this.constructor();
            }
        }

        internal static CommandClassBuilder TryCreate(CommandSource commandSource)
        {
            var type = commandSource.ClassType;
            var constructors = type.GetConstructors().ToArray();
            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters().ToArray();
                if (parameters.Length == 0)
                {
                    return new CommandClassBuilder(commandSource.IsStatic, () => Activator.CreateInstance(type));
                }
            }
            return null;
        }
    }
}