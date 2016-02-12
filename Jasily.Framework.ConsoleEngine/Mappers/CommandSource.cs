using Jasily.Framework.ConsoleEngine.Attributes;
using System;
using System.Reflection;

namespace Jasily.Framework.ConsoleEngine.Mappers
{
    public abstract class MapSource
    {
        public abstract MemberInfo GetMapObject();
    }

    public abstract class CommandSource : MapSource
    {
        public CommandSource(Type classType)
        {
            this.ClassType = classType;
            this.IsStatic = classType.GetCustomAttribute<StaticAttribute>() != null;
        }

        public Type ClassType { get; }

        public abstract CommandSourceType SourceType { get; }

        public bool IsStatic { get; }
    }

    public sealed class ClassCommandSource : CommandSource
    {
        public ClassCommandSource(Type classType)
            : base(classType)
        {
        }

        public override CommandSourceType SourceType => CommandSourceType.Class;

        public override MemberInfo GetMapObject() => this.ClassType;
    }

    public sealed class MethodCommandSource : CommandSource
    {
        public MethodInfo Method { get; }

        public MethodCommandSource(Type type, MethodInfo method)
            : base(type)
        {
            this.Method = method;
        }

        public override CommandSourceType SourceType => CommandSourceType.Method;

        public override MemberInfo GetMapObject() => this.Method;
    }
}