//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Common NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NotLimited.Framework.Common.Helpers
{
	public static class StringExtensions
	{
		public static bool ParseBool(this string input, bool def = false)
		{
			bool boolResult;
			if (bool.TryParse(input, out boolResult))
				return boolResult;

			int intResult;
			if (int.TryParse(input, out intResult))
				return intResult != 0;

			return def;
		}

		public static string Left(this string input, int length)
		{
			return input.Substring(0, length);
		}

		public static string Left(this string input, char ch)
		{
			return input.Substring(0, input.IndexOf(ch));
		}

        public static string Right(this string input, int start)
        {
            return input.Substring(start);
        }

		public static int CountSubstrings(this string source, string substring)
		{
			int pos = -1;
			int cnt = 0;

			while ((pos = source.IndexOf(substring, pos + 1)) != -1)
				cnt++;

			return cnt;
		}

		public static string WithLowerFirstChar(this string source)
		{
			if (char.IsLower(source, 0))
				return source;

			return char.ToLowerInvariant(source[0]) + source.Substring(1);
		}

		public static List<string> ReadLines(this string src)
		{
			var result = new List<string>();
			string line;

			using (var reader = new StringReader(src))
			{
				while ((line = reader.ReadLine()) != null)
					result.Add(line);
			}

			return result;
		}

		public static string Reflow(this string src)
		{
			var sb = new StringBuilder();
			int minWs = int.MaxValue, curWs;
			var lines = src.ReadLines();
			bool hasText = false;

			for (int i = 0; i < lines.Count; i++)
			{
				string line = lines[i];

				if (line.Length == 0 || string.IsNullOrWhiteSpace(line))
				{
					if (!hasText)
					{
						lines.RemoveAt(i);
						i--;
					}
					
					continue;
				}
				for (curWs = 0; curWs < line.Length; curWs++)
				{
					if (!char.IsWhiteSpace(line[curWs]))
					{
						hasText = true;
						if (char.IsLower(line[curWs]) && i > 0)
						{
							lines[i - 1] = lines[i - 1].TrimEnd() + " " + line.Substring(curWs);
							lines.RemoveAt(i);
							i--;
						}
						else
						{
							if (curWs < minWs)
								minWs = curWs;
						}

						break;
					}
				}
			}

			for (int i = 0, cnt = 0; i < lines.Count; i++)
			{
				string line = lines[i];

				if (line.Length == 0 || string.IsNullOrWhiteSpace(line))
					continue;

				if (cnt > 0)
					sb.AppendLine();
				sb.Append(line.Substring(minWs));
				cnt++;
			}

			return sb.ToString();
		}

		public static string AddSlashes(this string input)
		{
			var sb = new StringBuilder();
			var lines = input.ReadLines();

			for (int i = 0; i < lines.Count; i++)
			{
				if (i > 0)
					sb.AppendLine();
				sb.Append("// ").Append(lines[i]);
			}

			return sb.ToString();
		}

		public static bool EqualsIgnoreCase(this string a, string b)
		{
			return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
		}

		public static bool EndsWithIgnoreCase(this string a, string b)
		{
			if (a == null || string.IsNullOrEmpty(b))
				return false;
			return a.EndsWith(b, StringComparison.OrdinalIgnoreCase);
		}

		public static bool EqualsOrdinal(this string a, string b, bool ignoreCase = false)
		{
			return string.Equals(a, b, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
		}

		public static bool StartsWithOrdinal(this string input, string value, bool ignoreCase = false)
		{
			if (string.IsNullOrEmpty(input))
				return false;
			return input.StartsWith(value, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
		}

        public static string TruncateAtWord(this string input, int length)
        {
            if (input == null || input.Length < length)
                return input;
            int iNextSpace = input.LastIndexOf(" ", length, StringComparison.Ordinal);
            return string.Format("{0}...", input.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim());
        }
	}
}