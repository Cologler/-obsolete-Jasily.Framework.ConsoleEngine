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

## parameter

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
