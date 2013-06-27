//----------------------------------------------------------------------- 
// <copyright file="WebAppAdoNetAppender.cs" company="Copacetic Software"> 
// Copyright (c) Copacetic Software.  
// <author>Philip Pittle</author> 
// <date>Thursday, June 20, 2013 2:30:21 PM</date> 
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
using System.Configuration;
using log4net.Appender;

namespace CopaceticSoftware.Common.Logging
{
    /// <summary>
    /// Extends the log4net <see cref="AdoNetAppender"/> to allow the reuse of an existing (named!) connection string 
    /// instead of requiring a new one be specified.
    /// </summary>
    /// <remarks>
    /// Based on http://stackoverflow.com/questions/2441359/can-you-pull-the-connectionstring-for-a-log4net-adonetappender-from-elsewhere-in
    /// </remarks>
    public class WebAppAdoNetAppender : AdoNetAppender
    {
        private string _connectionStringToUse;
        /// <summary>
        /// The name of the existing Connection String to use to connect to the Logging database.
        /// </summary>
        public string ConnectionStringToUse
        {
            get { return _connectionStringToUse; }
            set
            {
                ConnectionStringSettings connString = null;
                for(var i = 0; i < ConfigurationManager.ConnectionStrings.Count; i++)
                {
                    if (ConfigurationManager.ConnectionStrings[i].Name.Equals(
                        value, StringComparison.InvariantCultureIgnoreCase))
                    {
                        connString = ConfigurationManager.ConnectionStrings[i];
                        break;
                    }
                }

                if (null == connString)
                    throw new ArgumentException(string.Format(Strings.ExceptionWebAppAdoNetAppenderCouldNotFindConnectionString, value));

                ConnectionString = connString.ConnectionString;

                _connectionStringToUse = value;
            }
        }
    }
}
