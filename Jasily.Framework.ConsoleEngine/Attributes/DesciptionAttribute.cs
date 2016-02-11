using System;

namespace Jasily.Framework.ConsoleEngine.Attributes
{
    public sealed class DesciptionAttribute : Attribute
    {
        public string Desciption { get; }

        public DesciptionAttribute(string desciption)
        {
            this.Desciption = desciption;
        }

        public static DesciptionAttribute Empty { get; }
            = new DesciptionAttribute(string.Empty);

        public override string ToString() => this.Desciption ?? string.Empty;
    }
}