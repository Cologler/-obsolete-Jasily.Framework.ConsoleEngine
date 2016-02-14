# Jasily.Framework.ConsoleEngine

[![Join the chat at https://gitter.im/Cologler/Jasily.Framework.ConsoleEngine](https://badges.gitter.im/Cologler/Jasily.Framework.ConsoleEngine.svg)](https://gitter.im/Cologler/Jasily.Framework.ConsoleEngine?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

a engine/frame for console application

# how to use

write a simple console application enter:

``` cs
namespace xxx
{
    class Program
    {
        static void Main(string[] args)
        {
            var engine = new JasilyConsoleEngine();
            engine.MapperManager.RegistAssembly(Assembly.GetExecutingAssembly());
            using (var session = engine.StartSession())
            {
                while (true)
                {
                    var line = Console.ReadLine() ?? string.Empty;
                    if (line.ToLower() == "exit") return;
                    session.Execute(line);
                }
            }
        }
    }
}
```

then write a command class:

``` cs
namespace xxx
{
    [Command("test")]
    public class Command : ICommand
    {
        public void Execute(Session session, CommandLine line)
        {
            session.WriteLine("hello world!");
        }
    }
}
```

or a command method:

``` cs
namespace xxx
{
    public class Command // command method don't need interface ICommand
    {
        [Command("test")]
        public void Execute(Session session, CommandLine line)
        {
            session.WriteLine("hello world!");
        }
    }
}
```

you also can see my demos:

* [Inoreader-Shell](https://github.com/Cologler/Inoreader-Shell/)

# detail

## command

### class command

class command must implement interface `ICommand`, it is the command enter point.

class command parameter should be perperty with `[PropertyParameter]`.

### method command

method command parameter was method parameter.

if method command parameter without `[MethodParameter]`, it is name will auto fill as parameter name.

you can add or not add build-in parameter `Session` & `CommandLine`:

``` cs
public void Test(Session session, CommandLine line) { .. } // work fine
public void Test(CommandLine line) { .. } // work fine
public void Test(Session session) { .. } // work fine
public void Test() } // work fine
```

it will be auto fill on execute if type match.

#### sub command

**only method can be sub command.**

it like this:

```cs
[Command("list")] // we use class [Command] to index it.
public class ListCommand // be not need ICommand if class is not a Command
{
    [Command("feed")]
    [SubCommand]
    public void Feed(Session session, CommandLine line)
    {
        
    }
}
```

so we can input `list feed -xxxx` to call it.

## parameter

### parameter parse

there are a `CommandParameterParser` in `namespace Jasily.Framework.ConsoleEngine.Parameters`, it is default parameter parser.

you can write your own parameter parser implement `ICommandParameterParser` and set to `engine.CustomMembers.CommandParameterParser`.

build-in class `CommandParameterParser` was supported this parameter pattern:

start with:

* `--`
* `-`
* `/`
* `\`

and split parameter key & parameter value using:

* `:`
* `=`
* ` ` 

you can set `CommandParameterParser.Style` and `CommandParameterParser.SpliterStyle` to change it.

### parameter convert

all parameter input was string, it can convert to another type using converter.

build-in converter was:

* `int`, `double`, decimal
* bool
* enum
* nullable

you can implement you own `IConverter<T>` and regist it like:

``` cs
engine.MapperManager.RegistConverter(new BooleanConverter());
```

* method command will ignore Type `Session` and `CommandLine`, they will auto fill.

### parameter grouping

**only class property parameter can group**

e.g. for auth, user & pwd was required (as group 0), or token was required (as group 1):

``` cs
[Command("auth")]
public sealed class AuthCommand : IGroupingCommand
{
    [PropertyParameter("user")]
    [MethodParameterGrouping(0)]
    public string UserName { get; set; }

    [PropertyParameter("pwd")]
    [MethodParameterGrouping(0)]
    public string Password { get; set; }

    [PropertyParameter("token")]
    [MethodParameterGrouping(1)]
    public string Token { get; set; }

    public void Execute(Session session, CommandLine line, int[] workedGroupId)
    {
        // ...
    }

    public void Execute(Session session, CommandLine line)
    {
        // ...
    }
}
```

when (`user` & `pwd`) was seted, or (`token`) was seted, command will be call.

if class was implement interface `IGroupingCommand : ICommand`, then enter point will become to `IGroupingCommand.Execute(Session, CommandLine, int[])`, so engine will call `IGroupingCommand.Execute(Session, CommandLine, int[])` only.

if property don't contain `[MethodParameterGrouping]`, it will auto put into group 0.

*`[MethodParameterGrouping]` was allow multiple.*

## alias & desciption

all alias will work for command and help.

desciption will work for help.

you can add alias & desciption for command or parameter, like:

``` cs
// class command
[Command("test")][Alias("t")][Desciption("docs")]
public class Command : ICommand { ... }

// class parameter
[PropertyParameter("user")][Alias("u")][Desciption("docs")]
public string UserName { get; set;

// method command
[Command("test")][Alias("t")][Desciption("docs")]
public void Feed(
    [MethodParameter("id")][Alias("i")][Desciption("id of subscription")] // method parameter
    int id, Session session, CommandLine line)
{
    Console.WriteLine(subscriptionId);
}
```
