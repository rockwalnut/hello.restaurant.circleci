
using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace hello.restaurant.apiExtensions
{
    public static class StringExtensions
    {

        public static string ToMD5(this string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public static string StripIncompatableQuotes(this string s)
        {
            if (!string.IsNullOrEmpty(s))
                return s.Replace('\u2018', '\'').Replace('\u2019', '\'').Replace('\u201c', '\"').Replace('\u201d', '\"');
            else
                return s;
        }

        public static string[] toReplaceArray(this IEnumerable<string> values, string oldValue = ",", string newValue = "")
        {
            var lst = new List<string>();
            foreach (var v in values)
            {
                if (string.IsNullOrEmpty(v))
                    lst.Add(v);
                else
                    lst.Add(v.Replace(oldValue, newValue));
            }

            return lst.ToArray();
        }

        public static string Capitalize(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }


    }
}
