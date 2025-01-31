using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CTExample
{
	internal static class Utils
	{
        internal static object ParseValue(string stringValue)
        {
            object value = stringValue;
            if (stringValue.ToLower() == "true")
            {
                value = true;
            }
            else if (stringValue.ToLower() == "false")
            {
                value = false;
            }
            else if (int.TryParse(stringValue, out int result))
            {
                value = result;
            }
            // parse double using dot separator disregarding the current culture separators, since the comma is used as properties separator
            else if (double.TryParse(stringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out double dResult))
            {
                value = dResult;
            }

            return value;
        }

        internal static string WrapText(string message, int maxCharacters)
        {
            var lines = message.Split("\n").ToList();

            for (int j = 0; j < lines.Count; j++)
            {
                string line = lines[j];
                if (line.Length > maxCharacters)
                {
                    var words = line.Split(" ").ToList();
                    for (int k = 0; k < words.Count; k++)
                    {
                        string word = words[k];
                        if (word.Length > maxCharacters)
                        {
                            words[k] = WrapLongWord(word, maxCharacters);
                        }
                    }
                    lines[j] = string.Join(" ", words);
                }
            }

            message = string.Join("\n", lines);
            return message;
        }

        private static string WrapLongWord(string word, int maxCharacters)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < word.Length; i += maxCharacters)
            {
                if (i > 0)
                    builder.Append("\n");
                int length = Math.Min(maxCharacters, word.Length - i);
                builder.Append(word.Substring(i, length));
            }

            return builder.ToString();
        }
    }
}