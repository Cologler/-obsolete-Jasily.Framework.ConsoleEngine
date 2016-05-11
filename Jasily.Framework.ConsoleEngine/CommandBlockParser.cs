using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Jasily.Framework.ConsoleEngine
{
    public class CommandBlockParser : ICommandBlockParser
    {
        public IEnumerable<CommandBlock> Parse(string command)
        {
            var sb = new StringBuilder();
            using (var reader = new StringReader(command))
            {
                var inS = false;
                var lwS = false;
                int chi;
                while ((chi = reader.Read()) >= 0)
                {
                    var ch = (char)chi;
                    if (lwS)
                    {
                        sb.Append(ch);
                        lwS = false;
                    }
                    else
                    {
                        switch (ch)
                        {
                            case '"':
                                inS = !inS;
                                break;

                            case '\\':
                                lwS = true;
                                break;

                            case ' ':
                                if (inS)
                                {
                                    sb.Append(ch);
                                }
                                else
                                {
                                    yield return new CommandBlock(sb.ToString());
                                    sb.Clear();
                                }
                                break;

                            default:
                                sb.Append(ch);
                                break;
                        }
                    }
                }
            }
            if (sb.Length > 0)
            {
                yield return new CommandBlock(sb.ToString());
                sb.Clear();
            }
        }
    }
}