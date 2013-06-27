//----------------------------------------------------------------------- 
// <copyright file="IListExtensions.cs" company="Copacetic Software"> 
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

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CopaceticSoftware.Common.Extensions
{
    public static class IListExtensions
    {
        [DebuggerNonUserCode]
        public static void AddRange<T>(this IList<T> collection, IEnumerable<T> items)
        {
            if (null == collection || null == items)
                return;

            foreach(var item in items.ToList())
                collection.Add(item);
        }

        [DebuggerNonUserCode]
        public static void RemoveMany<T>(this IList<T> collection, IEnumerable<T> items)
        {
            if (null == collection || null == items)
                return;

            foreach (var item in items)
                collection.Remove(item);
        }
    }
}
