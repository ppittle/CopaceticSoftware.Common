//----------------------------------------------------------------------- 
// <copyright file="Ensure.cs" company="Copacetic Software"> 
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using CopaceticSoftware.Common.Extensions;
using JetBrains.Annotations;

namespace CopaceticSoftware.Common.Infrastructure
{
    /// <summary>
    /// Guard class for checking method parameters
    /// </summary>
    public static class Ensure
    {
        [DebuggerNonUserCode]
// ReSharper disable UnusedParameter.Global
        public static void ArgumentNotNull(
            object param,
            [InvokerParameterName]
            string paramName)
// ReSharper restore UnusedParameter.Global
        {
            if (null == param)
                throw new ArgumentNullException(paramName);
        }

        [DebuggerNonUserCode]
        public static void ArgumentIsGreaterThanZero(
            int param,
            [InvokerParameterName]
            string paramName)
        {
            if (param < 0)
                throw new ArgumentException(Strings.ExceptionParameterMustBeGreaterThanZero, paramName);
        }
        
        
        [DebuggerNonUserCode]
        public static void ArgumentNotNullOrEmpty(
            string param,
            [InvokerParameterName]
            string paramName)
        {
            if (param.IsNullOrEmpty())
                throw new ArgumentException(Strings.ExceptionParameterIsNullOrEmpty, paramName);
        }

        [DebuggerNonUserCode]
// ReSharper disable UnusedParameter.Global
// ReSharper disable UnusedMember.Global
        public static void ArgumentNotNullOrEmpty<T>(
            IEnumerable<T> collection,
            [InvokerParameterName]
            string paramName)
// ReSharper restore UnusedMember.Global
// ReSharper restore UnusedParameter.Global
        {
            if (collection.IsNullOrEmpty())
                throw new ArgumentException(Strings.ExceptionParameterIsNullOrEmpty, paramName);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if <paramref name="email"/> is not a valid email address.
        /// Validation is based on http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx
        /// </summary>
        public static void ArgumentIsValidEmailAddress(string email,
                                                       [InvokerParameterName] string paramName)
        {
            ArgumentNotNullOrEmpty(email, paramName);
            
            const string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                                   + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                                   + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            var regex = new Regex(pattern, RegexOptions.IgnoreCase);

            if (!regex.IsMatch(email))
                throw new ArgumentException(
                    string.Format(Strings.InvalidEmailFormatExceptionMessage, email)
                    , paramName);
        }
    }
}
