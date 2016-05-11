using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Jasily.Framework.ConsoleEngine.Commands;
using Jasily.Framework.ConsoleEngine.IO;
using Jasily.Framework.ConsoleEngine.Mappers;

namespace Jasily.Framework.ConsoleEngine
{
    public sealed class Session : IInput, IOutput, IDisposable
    {
        private readonly string name;
        private readonly List<CommandLine> historys;

        internal Session(JasilyConsoleEngine engine, string name)
        {
            Debug.Assert(engine != null);
            this.Engine = engine;
            this.name = name ?? string.Empty;

            this.ConsoleParameters = engine.Parameters;
            this.historys = new List<CommandLine>();
            this.Historys = new ReadOnlyCollection<CommandLine>(this.historys);
        }

        public JasilyConsoleEngine Engine { get; }

        public ConsoleParameters ConsoleParameters { get; }

        public IReadOnlyList<CommandLine> Historys { get; }

        public Dictionary<string, object> State { get; } = new Dictionary<string, object>();

        public IApplicationDescription Description { get; set; }

        public void ShowDescription()
        {
            var desc = this.Description;
            if (desc != null)
            {
                this.WriteLine($"{desc.ApplicationName} {desc.Version}");
                this.WriteLine(desc.Copyright);
                this.WriteLine();
                this.WriteLine(desc.Description);
            }
        }

        public void Help()
            => this.Execute(this.ConsoleParameters.HelpCommand, CommandLine.Empty);

        public void Help(CommandLine commandLine)
            => this.Execute(this.ConsoleParameters.HelpCommand, commandLine);

        public void Execute(string command)
        {
            var commandLine = new CommandLine(command,
                this.ConsoleParameters.CommandParser.Parse(command).ToArray());
            this.historys.Add(commandLine);

            if (commandLine.CommandBlock == null)
            {
                this.Execute(this.ConsoleParameters.NoneInputCommand, commandLine);
                this.Execute(this.ConsoleParameters.HelpCommand, commandLine);
            }
            else
            {
                var mapper = this.GetCommandMapper(commandLine);
                if (mapper != null)
                {
                    this.Execute(mapper, commandLine);
                    return;
                }
                this.Execute(this.ConsoleParameters.HelpCommand, commandLine);
            }
        }

        public CommandMapper GetCommandMapper(CommandLine commandLine)
        {
            if (commandLine.CommandBlock == null) return null;
            return this.Engine.MapperManager.GetCommand(commandLine);
        }

        private void Execute(CommandMapper mapper, CommandLine command)
        {
            var obj = mapper.CommandClassBuilder.Build();
            var executor = mapper.ExecutorBuilder.CreateExecutor(obj);
            var r = executor.SetCommandLine(command,
                this.ConsoleParameters.CommandParameterParser,
                this.Engine.MapperManager.GetAgent());
            if (r.HasError)
            {
                this.WriteLine(r);
                return;
            }

            if (executor.IsVaildCommand())
            {
                executor.Execute(this, command);
            }
            else
            {
                var missing = executor.GetMissingParameters().ToArray();
                if (missing.Length != 0)
                {
                    this.ConsoleParameters.MissingParametersFormater
                        .Format(this.ConsoleParameters.Output, mapper, missing,
                            this.ConsoleParameters.CommandParameterParser);
                    this.Help(command);
                }
            }
        }

        private void Execute(ICommand command, CommandLine commandLine)
        {
            command.Execute(this, commandLine);
        }

        public void Dispose() => this.Shutdown();

        public void Write(string value)
            => this.ConsoleParameters.Output.Write(value);

        public void WriteLine(string line = null)
            => this.ConsoleParameters.Output.WriteLine(line ?? string.Empty);

        public string ReadLine()
            => this.ConsoleParameters.Input.ReadLine();

        public void StartUp()
        {
            if (this.IsShutdowned) throw new InvalidOperationException();

            while (!this.IsShutdowned)
            {
                this.Write(this.name + "> ");
                var line = this.ReadLine() ?? string.Empty;
                this.Execute(line);
            }
        }

        public void Shutdown() => this.IsShutdowned = true;

        public bool IsShutdowned { get; private set; }
    }
}