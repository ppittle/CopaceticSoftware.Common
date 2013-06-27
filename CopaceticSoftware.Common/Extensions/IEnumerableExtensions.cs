//----------------------------------------------------------------------- 
// <copyright file="IEnumerableExtensions.cs" company="Copacetic Software"> 
// Copyright (c) Copacetic Software.  
// <author>Philip Pittle</author> 
// <date>Friday, May 31, 2013 1:51:45 PM</date> 
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


using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CopaceticSoftware.Common.Infrastructure;

namespace CopaceticSoftware.Common.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, TSource second)
        {
            Ensure.ArgumentNotNull(first, "first");

            return first.Union(second.Single());
        }

        //http://stackoverflow.com/questions/1019737/favorite-way-to-create-an-new-ienumerablet-sequence-from-a-single-value
        public static IEnumerable<TSource> Single<TSource>(this TSource source)
        {
            yield return source;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return (null == collection || !collection.Any());
        }
    }
}
