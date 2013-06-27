//----------------------------------------------------------------------- 
// <copyright file="ObjectExtensions.cs" company="Copacetic Software"> 
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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using CopaceticSoftware.Common.Infrastructure;

namespace CopaceticSoftware.Common.Extensions
{
    public static class ObjectExtensions
    {
        [DebuggerNonUserCode]
        public static bool TryCast<T>(this object o, Action<T> onSuccessfulCast) where T : class
        {
            Ensure.ArgumentNotNull(onSuccessfulCast, "onSuccessfulCast");

            if (null == o)
                return false;

            if (o is T)
            {
                onSuccessfulCast(o as T);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns a deep copy of <paramref name="objectToCopy"/>
        /// </summary>
        /// <remarks>
        /// <paramref name="objectToCopy"/> must be serializable, otherwise an exception will be thrown.
        /// </remarks>
        /// <remarks>
        /// http://stackoverflow.com/questions/4054075/how-to-make-a-deep-copy-of-an-array-in-c
        /// </remarks>
        /// <remarks>
        /// This is an expensive operation!
        /// </remarks>
        [DebuggerNonUserCode]
        public static T DeepCopy<T>(this T objectToCopy) where T : class
        {
            if (null == objectToCopy)
                return null;

            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, objectToCopy);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return binaryFormatter.Deserialize(memoryStream) as T;
            }
        }

        [DebuggerNonUserCode]
        public static bool IsNull<T>(this T o) where T : class
        {
            return (null == o);
        }
    }
}
