using System;

namespace Jasily.Framework.ConsoleEngine.Parameters
{
    [Flags]
    public enum ParameterSpliterStyle
    {
        /// <summary>
        /// ':'
        /// </summary>
        Colon = 0,

        /// <summary>
        /// ' '
        /// </summary>
        Spaces = 1,

        /// <summary>
        /// '='
        /// </summary>
        EqualsSign = 2,
    }
}