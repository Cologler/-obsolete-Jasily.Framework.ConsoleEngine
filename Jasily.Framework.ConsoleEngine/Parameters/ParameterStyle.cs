using System;

namespace Jasily.Framework.ConsoleEngine.Parameters
{
    [Flags]
    public enum ParameterStyle
    {
        /// <summary>
        /// -{0}
        /// </summary>
        Bar = 1,

        /// <summary>
        /// --{0}
        /// </summary>
        DoubleBar = 2,

        /// <summary>
        /// /{0}
        /// </summary>
        Slash = 4,

        /// <summary>
        /// \{0}
        /// </summary>
        Backslash = 8
    }
}