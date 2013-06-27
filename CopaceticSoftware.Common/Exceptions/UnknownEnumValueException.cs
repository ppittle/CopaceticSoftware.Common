//----------------------------------------------------------------------- 
// <copyright file="UnknownEnumValueException.cs" company="Copacetic Software"> 
// Copyright (c) Copacetic Software.  
// <author>Philip Pittle</author> 
// <date>Friday, June 21, 2013 10:42:53 AM</date> 
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

namespace CopaceticSoftware.Common.Exceptions
{
    /// <summary>
    /// Thrown when a method doesn't recognize an Enum value.
    /// </summary>
    /// <example>
    /// <code><![CDATA[
    /// public enum SomeEnum{}
    /// 
    /// public void SomeMethod(SomeEnum enumValue)
    /// {
    ///     switch(enumValue)
    ///     {
    ///         default:
    ///             throw new UnknownEnumValueException<SomeEnum>(enumValue);
    ///     }
    /// }
    /// ]]> </code>
    /// </example>
    /// <typeparam name="TEnum">The type of the <see cref="Enum"/></typeparam>
    public class UnknownEnumValueException<TEnum> : Exception
    {
        public UnknownEnumValueException(TEnum enumValue, Exception innerException = null)
            : base(string.Format(Strings.UnknownEnumValueExceptionMessage, typeof(TEnum).FullName, Enum.GetName(typeof(TEnum), enumValue)), innerException)
        {
            
        }
    }
}
