using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Athame.Utils
{
    public static class StringObjectFormatter
    {
        private static readonly Regex FormatRegex = new Regex(@"(?<!{){([\w\d\.]*)}");

        private static object GetPropertyValueFromPath(string[] propertyPath, object baseObject)
        {
            while (true)
            {
                // Work on the first part of the path
                var objType = baseObject.GetType();
                var baseProperty = objType.GetProperty(propertyPath[0]);

                // If we can't find the property, return null
                if (baseProperty == null)
                {
                    return null;
                }

                // If we only have one element, just return its string value
                if (propertyPath.Length == 1)
                {
                    return baseProperty.GetValue(baseObject);
                }

                // If we have more than one element, get the property's value, shift
                // the array by 1, then recurse
                var propertyValue = baseProperty.GetValue(baseObject);
                if (propertyValue == null)
                {
                    return null;
                }
                var length = propertyPath.Length - 1;
                var shiftedPath = new string[length];
                Array.Copy(propertyPath, 1, shiftedPath, 0, length);
                propertyPath = shiftedPath;
                baseObject = propertyValue;
            }
        }

        public static string Format(string formatString, object value)
        {
            return Format(formatString, value, null);
        }

        /// <summary>
        /// Returns the string representation of an object, or "null" if the object is null.
        /// </summary>
        public static Func<object, string> DefaultFormatter = o => o == null ? "null" : o.ToString();

        public static string Format(string formatString, object value, Func<object, string> stringFormatter)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (formatString == null)
            {
                throw new ArgumentNullException(nameof(formatString));
            }
            if (stringFormatter == null)
            {
                stringFormatter = DefaultFormatter;
            }

            var matches = FormatRegex.Matches(formatString);
            var tokens = from match in matches.Cast<Match>()
                select match.Groups[1].Value;
            var replacements = new Dictionary<string, object>();

            foreach (var token in tokens)
            {
                var path = token.Split('.');
                replacements[token] = GetPropertyValueFromPath(path, value);
            }

            return FormatRegex.Replace(formatString, match =>
            {
                var matchToken = match.Groups[1].Value;
                return !replacements.ContainsKey(matchToken) ? match.Value : stringFormatter(replacements[matchToken]);
            });

        }

    }
}
