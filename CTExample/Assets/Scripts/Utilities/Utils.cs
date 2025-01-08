using System.Globalization;

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
    }
}