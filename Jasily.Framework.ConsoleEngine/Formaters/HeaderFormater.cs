using Jasily.Framework.ConsoleEngine.IO;

namespace Jasily.Framework.ConsoleEngine.Formaters
{
    public class HeaderFormater : IHeaderFormater
    {
        #region Implementation of IHeaderFormater

        public void WriteHeader(IOutput output, string sessionName)
        {
            output.Write(sessionName + "> ");
        }

        #endregion
    }
}