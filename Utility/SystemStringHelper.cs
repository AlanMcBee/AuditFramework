using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace CodeCharm.Utility
{
    /// <summary>
    /// Summary description for SystemStringHelper.
    /// </summary>
    public static class SystemStringHelper
    {
#if DOTNET_1
        public static bool IsNullOrEmpty(string s)
        {
            return null == s || 0 == s.Length;
        }
#endif

        /// <summary>
        /// Create a single string filled with integers padded with spaces so that
        /// every integer uses a fixed-length string, concatenated into 
        /// one string.
        /// </summary>
        /// <param name="list">A set of positive integers</param>
        /// <returns>A <seealso cref="System.String"/>.</returns>
        /// <remarks>
        /// An example of where this method can be useful:
        /// When submitting a variable-size collection of integer identifiers (such as primary key values)
        /// to a stored procedure, you have a variety of options from which to select.
        /// A fairly heavy (high-overhead) technique is to use table value parameters.
        /// Another technique with less overhead is to build a delimited string, such as XML (also heavy),
        /// or a comma or tab delimited string (lightweight). While T-SQL does possess some string-parsing
        /// features, they are not powerful or numerous. (There is no Split method, for example).
        /// In contrast, a fixed-length list of numbers is easily split into sub-strings.
        /// It does require an additional paired parameters to the stored procedure (or function) indicating
        /// the length of the fixed field, but this can be easily computed by the caller of this function
        /// by <code>`fixed-length` = `returned-string-length` / `list.Count`</code>.
        /// </remarks>
        public static string FixedLengthList(IEnumerable<int> list)
        {
            int max = list.Max();
            int maxLength = max.ToString().Length;
            StringBuilder builder = new StringBuilder(list.Count() * maxLength);
            foreach (int i in list)
            {
                builder.Append(i.ToString().PadRight(maxLength));
            }
            return builder.ToString();
        }

        public static string SpaceDelimitedList(this StringCollection strings)
        {
            return strings.Cast<string>().Aggregate(string.Empty, (current, schema) => current + " " + schema).Trim();
        }

        public static string StripLeft(this string value, int length)
        {
            return value.Substring(length, value.Length - length);
        }
    }
}
