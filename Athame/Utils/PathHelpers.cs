using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Athame.Utils
{

    /// <summary>
    /// Provides supplementary methods for the <see cref="Path"/> class.
    /// </summary>
    public static class PathHelpers
    {
        /// <summary>
        /// The character invalid path characters are replaced with.
        /// </summary>
        public const string ReplacementChar = "-";

        /// <summary>
        /// Replaces invalid path characters in a filename.
        /// </summary>
        /// <param name="name">The filename to clean.</param>
        /// <returns>A valid filename.</returns>
        public static string CleanFilename(string name)
        {
            var invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            var invalidRegStr = String.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return Regex.Replace(name, invalidRegStr, ReplacementChar);
        }

        /// <summary>
        /// Splits a path by <see cref="Path.DirectorySeparatorChar"/> and cleans each component.
        /// </summary>
        /// <param name="path">The path to clean.</param>
        /// <returns>A valid path.</returns>
        public static string CleanPath(string path)
        {
            var components = path.Split(Path.DirectorySeparatorChar);
            var cleanComponents = new string[components.Length];

            for (var i = 0; i < components.Length; i++)
            {
                cleanComponents[i] = CleanFilename(components[i]);
            }
            return String.Join(Path.DirectorySeparatorChar.ToString(), cleanComponents);
        }
    }
}
