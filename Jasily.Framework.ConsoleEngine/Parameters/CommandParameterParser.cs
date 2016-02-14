using Jasily.Framework.ConsoleEngine.Executors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jasily.Framework.ConsoleEngine.Parameters
{
    public class CommandParameterParser : ICommandParameterParser
    {
        public ParameterStyle Style { get; set; } = ParameterStyle.DoubleBar | ParameterStyle.Bar |
                                                    ParameterStyle.Backslash | ParameterStyle.Slash;

        public ParameterStyle DisplayStyle { get; set; } = ParameterStyle.Bar;

        public ParameterSpliterStyle SpliterStyle { get; set; }
            = ParameterSpliterStyle.Colon | ParameterSpliterStyle.EqualsSign | ParameterSpliterStyle.Spaces;

        public string GetInputSytle(string key)
        {
            var headers = ParameterHeader.Parse(this.DisplayStyle).ToArray();
            var spliter = ParameterSpliter.Parse(this.SpliterStyle).First().Spliter;
            if (headers.Length > 0) return $"{headers[0].Header}{key}{spliter}<{key}>";
            else return $"none parameter style";
        }

        public IEnumerable<KeyValuePair<string, string>> Parse(
            CommandLine commandLine, IEnumerable<IParameterSetter> parameterSetters)
        {
            var headers = ParameterHeader.Parse(this.Style).ToArray();
            var spliters = ParameterSpliter.Parse(this.SpliterStyle).ToArray();

            var setters = parameterSetters as IParameterSetter[] ?? parameterSetters.ToArray();
            using (var itor = commandLine.Blocks.GetEnumerator())
            {
                while (itor.MoveNext())
                {
                    var block = itor.Current;

                    foreach (var header in headers)
                    {
                        var isParsed = false;

                        if (block.OriginText.StartsWith(header.Header))
                        {
                            foreach (var spliter in spliters)
                            {
                                var index = spliter.Style == ParameterSpliterStyle.Spaces
                                    ? -1
                                    : block.OriginText.IndexOf(spliter.Spliter, header.Header.Length, StringComparison.Ordinal);

                                var parsedKey = index < 0
                                    ? block.OriginText.Substring(header.Header.Length)
                                    : block.OriginText.Substring(header.Header.Length, index - header.Header.Length);

                                var setter = setters.FirstOrDefault(z => z.Mapper.IsMatch(parsedKey));
                                if (setter != null)
                                {
                                    if (spliter.Style != ParameterSpliterStyle.Spaces)
                                    {
                                        if (index < 0)
                                        {
                                            yield return new KeyValuePair<string, string>(
                                                parsedKey,
                                                string.Empty);
                                        }
                                        else
                                        {
                                            yield return new KeyValuePair<string, string>(
                                                parsedKey,
                                                block.OriginText.Substring(index + spliter.Spliter.Length));
                                        }

                                        isParsed = true;
                                        break;
                                    }
                                    else
                                    {
                                        if (itor.MoveNext())
                                        {
                                            yield return new KeyValuePair<string, string>(
                                                parsedKey,
                                                itor.Current.OriginText);

                                            isParsed = true;
                                            break;
                                        }
                                    }
                                }
                            }

                            if (isParsed) break;
                        }
                    }
                }
            }
        }

        private struct ParameterHeader
        {
            public string Header { get; }

            private ParameterHeader(string header)
            {
                this.Header = header;
            }

            public static IEnumerable<ParameterHeader> Parse(ParameterStyle style)
            {
                if ((style & ParameterStyle.DoubleBar) == ParameterStyle.DoubleBar)
                    yield return new ParameterHeader("--");
                if ((style & ParameterStyle.Bar) == ParameterStyle.Bar)
                    yield return new ParameterHeader("-");
                if ((style & ParameterStyle.Slash) == ParameterStyle.Slash)
                    yield return new ParameterHeader("/");
                if ((style & ParameterStyle.Backslash) == ParameterStyle.Backslash)
                    yield return new ParameterHeader("\\");
            }
        }

        private struct ParameterSpliter
        {
            public ParameterSpliterStyle Style { get; }

            public string Spliter { get; }

            private ParameterSpliter(ParameterSpliterStyle style, string spliter)
            {
                this.Style = style;
                this.Spliter = spliter;
            }

            public static IEnumerable<ParameterSpliter> Parse(ParameterSpliterStyle style)
            {
                if ((style & ParameterSpliterStyle.Spaces) == ParameterSpliterStyle.Spaces)
                    yield return new ParameterSpliter(ParameterSpliterStyle.Spaces, " ");
                if ((style & ParameterSpliterStyle.Colon) == ParameterSpliterStyle.Colon)
                    yield return new ParameterSpliter(ParameterSpliterStyle.Colon, ":");
                if ((style & ParameterSpliterStyle.EqualsSign) == ParameterSpliterStyle.EqualsSign)
                    yield return new ParameterSpliter(ParameterSpliterStyle.EqualsSign, "=");
            }
        }
    }
}