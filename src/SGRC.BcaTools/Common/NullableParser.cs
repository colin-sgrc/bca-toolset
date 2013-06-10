using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGRC.BCATools
{
    /// <summary>
    /// stolen from http://stackoverflow.com/questions/45030/how-to-parse-a-string-into-a-nullable-int-in-c-sharp-net-3-5
    /// and http://social.msdn.microsoft.com/Forums/en-US/csharplanguage/thread/26a2d96a-468c-46cc-a0fb-bd2b52b6771e
    /// makes it easy for setting nullable class values from winforms text boxes
    /// </summary>
    public static class NullableParser
    {
        public delegate T ParseDelegate<T>(string input) where T : struct;
        public delegate bool TryParseDelegate<T>(string input, out T outtie) where T : struct;
        private static T Parse<T>(string input, ParseDelegate<T> DelegateTheParse) where T : struct
        {
            if (string.IsNullOrEmpty(input)) return default(T);
            return DelegateTheParse(input);
        }
        private static T? TryParse<T>(string input, TryParseDelegate<T> DelegateTheTryParse) where T : struct
        {
            T x;
            if (DelegateTheTryParse(input, out x)) return x;
            return null;
        }
        public static int ParseInt(string input)
        {
            return Parse<int>(input, new ParseDelegate<int>(int.Parse));
        }
        public static int? TryParseInt(string input)
        {
            return TryParse<int>(input, new TryParseDelegate<int>(int.TryParse));
        }

        public static int? TryParseInt(object input)
        {
            if (input == null)
            {
                return null;
            }
            return TryParse<int>(input.ToString(), new TryParseDelegate<int>(int.TryParse));
        }

        public static bool? TryParseBool(string input)
        {
            return TryParse<bool>(input, new TryParseDelegate<bool>(bool.TryParse));
        }
        public static DateTime? TryParseDateTime(string input)
        {
            return TryParse<DateTime>(input, new TryParseDelegate<DateTime>(DateTime.TryParse));
        }

        public static double ParseDouble(string input)
        {
            return Parse<double>(input, new ParseDelegate<double>(double.Parse));
        }
        public static double? TryParseDouble(string input)
        {
            return TryParse<double>(input, new TryParseDelegate<double>(double.TryParse));
        }

        public static string ParseString(object input)
        {
            if (input == null || string.IsNullOrEmpty(input.ToString().Trim()))
            {
                return null;
            }

            return input.ToString().Trim();
        }

        public static string ParseString(string input)
        {
            if (string.IsNullOrEmpty(input.Trim()))
            {
                return null;
            }

            return input.Trim();
        }
    }

}

