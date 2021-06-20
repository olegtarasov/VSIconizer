using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace VSIconizer.Core
{
    using WpfColor  = System.Windows.Media.Color;
    using GdiColor  = System.Drawing.Color;
    using ColorDict = IReadOnlyDictionary<string,System.Windows.Media.Color>;

    public static class TabColorsSerialization
    {
        public static string SerializeTabColorsToCsv(ColorDict dict)
        {
            if (dict is null || dict.Count == 0)
            {
                return string.Empty;
            }

            using(StringWriter wtr = new StringWriter())
            {
                IOrderedEnumerable<string> orderedTabNames = dict.Keys.Where(k => !string.IsNullOrWhiteSpace(k)).OrderBy(k => k);
                foreach(string tabName in orderedTabNames)
                {
//                  string colorText = dict[tabName].Item1;
//                  Color  color     = dict[tabName].Item2;
                    WpfColor wpfColor = dict[tabName];

                    WriteCsvString(wtr, tabName);
                    wtr.Write(',');
                    WriteCsvString(wtr, wpfColor.ToString());

                    //wtr.WriteLine();
                    wtr.Write("\r\n");
                }

                string csv = wtr.ToString();
                return csv;
            }
        }

        #region CSV ffs

        private static void WriteCsvString(TextWriter wtr, string value)
        {
            if(value.All(c => Char.IsLetterOrDigit(c)))
            {
                wtr.Write(value);
            }
            else
            {
//              string enquoted = '"' + value.Replace("\"", "\"\"") + '"';
                string quoted   = value.Replace("\"", "\"\"");
                wtr.Write('"');
                wtr.Write(quoted);
                wtr.Write('"');
            }
        }

        private static string ReadCsvString(TextReader rdr, StringBuilder sb) // `sb` is reusable, hence why it's passed in as a param.
        {
            sb.Length = 0; // Reset sb to be safe:

            Int32 nc = rdr.Read();
            if(nc < 0) return string.Empty;

            char c = (char)nc;
            if (c == '"')
            {
                // Quoted string.
                return ReadQuotedCsvString(rdr, sb);
            }
            else
            {
                // Unquoted string. Read until the next `,`, `\r\n` or EOF:
                _ = sb.Append(c);
                return ReadUnquotedCsvString(rdr, sb);
            }
        }

        private static string ReadUnquotedCsvString(TextReader rdr, StringBuilder sb)
        {
            int nc;
            while ((nc = rdr.Read()) >= 0)
            {
                char c = (char)nc;

                if (c == '"')
                {
                    // Unexpected quote in an unquoted string. Abort.
                    return null;
                }
                else if (c == ',')
                {
                    // End of field.
                    return sb.ToString();
                }
                else if (c == '\r' || c == '\n')
                {
                    // End-of-row.
                    return sb.ToString();
                }
                else
                {
                    _ = sb.Append(c);
                }
            }

            // End-of-file:
            return sb.ToString();
        }

        private static string ReadQuotedCsvString(TextReader rdr, StringBuilder sb)
        {
            bool inQuote = false;

            int nc;
            while ((nc = rdr.Read()) >= 0)
            {
                char c = (char)nc;

                if (inQuote)
                {
                    if (c == '"')
                    {
                        // Doubled-up (escaped) quote, i.e. the end of the field.
                        _ = sb.Append('"');
                        inQuote = false;
                    }
                    else if (c == ',' || c == '\r' || c == '\n')
                    {
                        // End of value - or end of row:
                        return sb.ToString();
                    }
                    else
                    {
                        // Unexpected character. Abort.
                        return null;
                    }
                }
                else
                {
                    if(c == '"')
                    {
                        inQuote = true;
                    }
                    else
                    {
                        _ = sb.Append(c);
                    }
                }
            }

            // EOF:
            if (inQuote)
            {
                return sb.ToString();
            }
            else
            {
                // Unexpected EOF inside a quoted value. Abort.
                return null;
            }
        }

        #endregion

        private static readonly char[] _sep = new[] { ',' };

        /// <summary>Expects rows of 2 columns: <br />
        /// NOTE: Values that cannot be parsed will be discarded.</summary>
        public static ColorDict ReadTabColorsCsv(string csv)
        {
            if(string.IsNullOrWhiteSpace(csv))
            {
                return VSIconizerConfiguration.EmptyTabColors;
            }

            //

            Dictionary<string,WpfColor> dict = new Dictionary<string,WpfColor>(StringComparer.OrdinalIgnoreCase); 

            using(StringReader rdr = new StringReader(csv))
            {
                StringBuilder reusable = new StringBuilder( capacity: 100 );

                // HACK: CBA to write a full CSV reader again....
                string line;
                while( (line = rdr.ReadLine()) != null )
                {
                    if(!string.IsNullOrWhiteSpace(line))
                    {
                        using(StringReader lineReader = new StringReader(line))
                        {
                            string tabText = ReadCsvString(lineReader, reusable);
                            if (tabText is null) return VSIconizerConfiguration.EmptyTabColors;

                            string colorText = ReadCsvString(lineReader, reusable);
                            if (colorText is null) return VSIconizerConfiguration.EmptyTabColors;

                            if(!string.IsNullOrWhiteSpace(tabText) && TryParseWpfColor(colorText, out WpfColor wpfColor))
                            {
                                if(!dict.ContainsKey(tabText))
                                {
                                    dict.Add(tabText, wpfColor);
                                }
                            }
                        }
                    }
                }
            }

            return dict;
        }

        public static bool TryParseWpfColor(string text, out WpfColor wpfColor)
        {
            // GDI's ColorTranslater seems more flexible and accepting than WPF's ColorConverter...

            try
            {
                GdiColor gdiColor = System.Drawing.ColorTranslator.FromHtml(text);
                if (!gdiColor.IsEmpty)
                {
                    wpfColor = gdiColor.ToWpfColor();
                    return true;
                }
            }
            catch
            {
            }

            try
            {
                if (ColorConverter.ConvertFromString(text) is WpfColor parsed)
                {
                    wpfColor = parsed;
                    return true;
                }
            }
            catch
            {
            }

            wpfColor = default;
            return false;
        }
    }
}
