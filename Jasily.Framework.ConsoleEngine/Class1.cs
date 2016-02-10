using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jasily.Framework.ConsoleEngine
{
    public class JasilyConsoleEngine
    {
        private List<Assembly> assemblys = new List<Assembly>();

        public void RegistAssembly(Assembly assembly)
        {
            
        }
    }

    public interface IConsoleCommand
    {
        
    }

    [AttributeUsage(AttributeTargets.Class)]
    class ConsoleCommandAttribute : Attribute
    {
        public string Command { get; }

        public ConsoleCommandAttribute(string command)
        {
            this.Command = command;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    class CommandParameterAttribute : Attribute
    {
        public string Command { get; }

        public CommandParameterAttribute(bool isOption)
        {
            this.Command = command;
        }
    }
}
