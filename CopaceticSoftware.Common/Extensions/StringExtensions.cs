//----------------------------------------------------------------------- 
// <copyright file="StringExtensions.cs" company="Copacetic Software"> 
// Copyright (c) Copacetic Software.  
// <author>Philip Pittle</author> 
// <date>Wednesday, May 29, 2013 6:40:54 PM</date> 
// Licensed under the Apache License, Version 2.0,
// you may not use this file except in compliance with one of the Licenses.
//  
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an 'AS IS' BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright> 
//-----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Text;
using CopaceticSoftware.Common.Infrastructure;

namespace CopaceticSoftware.Common.Extensions
{
    public static class StringExtensions
    {
        [DebuggerNonUserCode]
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        #region Ensure
        [DebuggerNonUserCode]
        public static string EnsureStartsWith(this string s, string prefix,
            StringComparison comparisonType = StringComparison.InvariantCulture)
        {
            if (null == s)
                return prefix;

            if (null == prefix)
                return s;

            if (s.StartsWith(prefix, comparisonType))
                return s;

            return prefix + s;
        }
        
        [DebuggerNonUserCode]
        public static string EnsureEndsWith(this string s, string suffix,
            StringComparison comparisonType = StringComparison.InvariantCulture)
        {
            if (null == s)
                return suffix;

            if (null == suffix)
                return s;

            if (s.EndsWith(suffix, comparisonType))
                return s;

            return s + suffix;
        }

        [DebuggerNonUserCode]
        public static string EnsureIsShorterThan(this string s, int length)
        {
            return (s != null && s.Length > length)
                ? s.Substring(0, length)
                : s;
        }
        #endregion

        #region Replace
        [DebuggerNonUserCode]
        public static string ReplaceLastInstanceOf(this string s, 
            string oldString, string newString,
            StringComparison comparisonType = StringComparison.InvariantCulture)
        {
            if (s.IsNullOrEmpty())
                return s;

            if (oldString.IsNullOrEmpty())
                return s;

            Ensure.ArgumentNotNull(newString, "newString");

            var lastIdx = s.LastIndexOf(oldString, comparisonType);

            if (lastIdx < 1) //oldString not found
                return s;

            var firstHalf = s.Substring(0, lastIdx);
            var secondHalf = s.Substring(lastIdx, s.Length - lastIdx);

            return firstHalf + secondHalf.Replace(oldString, newString);
        }

        /// <summary>
        /// Extends the <see cref="string.Replace(string,string)"/> functionality
        /// by allowing a custom <see cref="StringComparison"/>.
        /// </summary>
        /// <remarks>
        /// Based on http://stackoverflow.com/questions/244531/is-there-an-alternative-to-string-replace-that-is-case-insensitive
        /// </remarks>
        public static string Replace(this string s, string oldValue, string newValue, StringComparison comparison)
        {
            StringBuilder sb = new StringBuilder();

            int previousIndex = 0;
            int index = s.IndexOf(oldValue, comparison);
            while (index != -1)
            {
                sb.Append(s.Substring(previousIndex, index - previousIndex));
                sb.Append(newValue);
                index += oldValue.Length;

                previousIndex = index;
                index = s.IndexOf(oldValue, index, comparison);
            }
            sb.Append(s.Substring(previousIndex));

            return sb.ToString();
        }
        #endregion

    }
}
