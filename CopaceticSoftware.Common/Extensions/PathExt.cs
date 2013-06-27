//----------------------------------------------------------------------- 
// <copyright file="PathExt.cs" company="Copacetic Software"> 
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
using System.IO;

namespace CopaceticSoftware.Common.Extensions
{
    public static class PathExt
    {
        public static bool PathsAreEqual(string path1, string path2)
        {
            if (null == path1 || null == path2)
                return false;

            return path1.Equals(path2, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Converts the <paramref name="relativePath"/> to an absolute path
        /// such that <see cref="Path.IsPathRooted"/>() returns <c>True</c>
        /// </summary>
        /// <param name="absolutePath">An Absolute Path that is used
        /// to resolve the <paramref name="relativePath"/>.  This can be a 
        /// directory or a file.</param>
        /// <param name="relativePath">
        /// A relative file path.  If this is already an absolute path, it is returned
        /// unmodified.
        /// </param>
        /// <remarks>
        /// Throws an Exception if <see cref="Path.IsPathRooted"/>(<paramref name="absolutePath"/>)
        /// is <c>false</c>.
        /// </remarks>
        /// /// <remarks>
        /// Throws an Exception if <see cref="Path.GetDirectoryName"/>(<paramref name="absolutePath"/>)
        /// is <c>null</c>.
        /// </remarks>
        /// <remarks>Based on http://stackoverflow.com/questions/3139467/how-to-get-absolute-file-path-from-base-path-and-relative-containing
        /// </remarks>
        public static string MakePathAbsolute(string absolutePath, string relativePath)
        {
            if (Path.IsPathRooted(relativePath))
                return relativePath;

            if (!Path.IsPathRooted(absolutePath))
                throw new ArgumentException(string.Format("[{0}] is not Absolute", absolutePath),
                    "absolutePath");

            var absoluteDirectoryName = Path.GetDirectoryName(absolutePath);

            if (string.IsNullOrEmpty(absoluteDirectoryName))
                throw new ArgumentException(string.Format(
                    "Failed to resolve DirectoryName from [{0}]", absolutePath), "absolutePath");

            return Path.GetFullPath(Path.Combine(absoluteDirectoryName, relativePath));
        }
    }
}
