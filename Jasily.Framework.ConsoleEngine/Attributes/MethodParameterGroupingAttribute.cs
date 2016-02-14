using System;

namespace Jasily.Framework.ConsoleEngine.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class MethodParameterGroupingAttribute : Attribute
    {
        public static MethodParameterGroupingAttribute Default { get; }
            = new MethodParameterGroupingAttribute(0);

        public int GroupId { get; }

        public MethodParameterGroupingAttribute(int groupId)
        {
            this.GroupId = groupId;
        }
    }
}