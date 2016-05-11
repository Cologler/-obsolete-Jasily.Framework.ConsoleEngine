using Jasily.Framework.ConsoleEngine.IO;

namespace Jasily.Framework.ConsoleEngine.Formaters
{
    public interface IHeaderFormater
    {
        void WriteHeader(IOutput output, string sessionName);
    }
}