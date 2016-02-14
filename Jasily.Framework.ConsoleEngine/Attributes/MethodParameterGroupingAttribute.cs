using System;

namespace Jasily.Framework.ConsoleEngine.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class MethodParameterGroupingAttribute : Attribute
    {
        public int GroupId { get; }

        public MethodParameterGroupingAttribute(int groupId)
        {
            this.GroupId = groupId;
        }
    }
}