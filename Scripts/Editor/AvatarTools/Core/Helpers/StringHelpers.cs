using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pumkin.Core.Helpers
{
    public static class StringHelpers
    {
        public static string ToTitleCase(string example)
        {
            var fromSnakeCase = example.Replace("_", " ");
            var lowerToUpper = Regex.Replace(fromSnakeCase, @"(\p{Ll})(\p{Lu})", "$1 $2");
            var sentenceCase = Regex.Replace(lowerToUpper, @"(\p{Lu}+)(\p{Lu}\p{Ll})", "$1 $2");
            return new CultureInfo("en-US", false).TextInfo.ToTitleCase(sentenceCase);
        }
    }
}