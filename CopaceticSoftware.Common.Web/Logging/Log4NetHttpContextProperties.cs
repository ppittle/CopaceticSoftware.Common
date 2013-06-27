//----------------------------------------------------------------------- 
// <copyright file="Log4NetHttpContextProperties.cs" company="Copacetic Software"> 
// Copyright (c) Copacetic Software.  
// <author>Philip Pittle</author> 
// <date>Friday, June 21, 2013 11:14:35 AM</date> 
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
using System.Web;
using CopaceticSoftware.Common.Exceptions;
using CopaceticSoftware.Common.Web.Extensions;
using log4net;
using log4net.Util;

namespace CopaceticSoftware.Common.Web.Logging
{
    /// <summary>
    /// Adds custom properties to a log4net Context representing <see cref="HttpContext.Current"/>.<see cref="HttpRequest"/>.
    /// Following properties are added:
    ///     properties["url"] = request.Url.AbsoluteUri;
    ///     properties["urlReferrer"] = request.UrlReferrer;
    ///     properties["userAgent"] = request.UserAgent;
    ///     properties["ipAddress"] = request.GetRequestIpAddress();
    ///     properties["isLocal"] = request.IsLocal;
    ///     properties["queryString"] = request.QueryString;
    ///     properties["httpMethod"] = request.HttpMethod;
    ///     properties["machineName"] = Environment.MachineName;
    /// </summary>
    /// <example>
    /// Global.asax:
    /// <code> <![CDATA[
    /// protected void Application_BeginRequest()
    /// {
    ///     //Add HttpContext info to log4net
    ///     new Log4NetHttpContextProperties(Log4NetHttpContextProperties.ContextTarget.Thread);
    /// }
    /// ]]>
    /// </code>
    /// example appender in web.config:
    /// <code> <![CDATA[
    /// <appender name="ADONetAppender" type="CopaceticSoftware.Common.Logging.WebAppAdoNetAppender">
	///     <bufferSize value="1" />
	///     <threshold value="DEBUG" />
	///     <connectionType value="System.Data.SqlClient.SqlConnection, System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
	///     <ConnectionStringToUse value="LBi.CBA.Voorjaarscampagne.Properties.Settings.CBA_Voordeelviadezaak" />
	///     <commandText value="INSERT INTO Log ([Date],[Thread],[Level],[Logger],[Message],[Exception],[Url],[QueryString],[HttpMethod], [UserAgent] ,[IpAddress], [UrlReferrer], [ServerName] )     VALUES (@log_date, @thread, @log_level, @logger, @message, @exception, @url, @queryString, @httpMethod, @userAgent, @ipAddress, @urlReferrer, @serverName)" />
	///     <parameter>	
	///      	<parameterName value="@log_date" />
	///         <dbType value="DateTime" />
    ///				<layout type="log4net.Layout.RawTimeStampLayout" />
    ///			</parameter>
    ///			<parameter>
    ///				<parameterName value="@thread" />
    ///				<dbType value="String" />
    ///				<size value="255" />
    ///				<layout type="log4net.Layout.PatternLayout">
    ///					<conversionPattern value="%thread" />
    ///				</layout>
    ///			</parameter>
    ///			<parameter>
    ///				<parameterName value="@log_level" />
    ///				<dbType value="String" />
    ///				<size value="50" />
    ///				<layout type="log4net.Layout.PatternLayout">
    ///					<conversionPattern value="%level" />
    ///				</layout>
    ///			</parameter>
    ///			<parameter>
    ///				<parameterName value="@logger" />
    ///				<dbType value="String" />
    ///				<size value="255" />
    ///				<layout type="log4net.Layout.PatternLayout">
    ///					<conversionPattern value="%logger" />
    ///				</layout>
	///			</parameter>
	///			<parameter>
	///				<parameterName value="@message" />
	///				<dbType value="String" />
	///				<size value="4000" />
	///				<layout type="log4net.Layout.PatternLayout">
	///					<conversionPattern value="%message" />
	///				</layout>
    ///			</parameter>
    ///			<parameter>
    ///				<parameterName value="@exception" />
    ///				<dbType value="String" />
    ///				<size value="2000" />
    ///				<layout type="log4net.Layout.ExceptionLayout" />
    ///			</parameter>
    ///			<parameter>
    ///				<parameterName value="@url" />
    ///				<dbType value="String" />
    ///				<size value="512" />
    ///				<layout type="log4net.Layout.PatternLayout">
    ///					<conversionPattern value="%property{url}" />
    ///				</layout>
    ///			</parameter>
    ///			<parameter>
    ///				<parameterName value="@queryString" />
    ///				<dbType value="String" />
    ///				<size value="512" />
    ///				<layout type="log4net.Layout.PatternLayout">
    ///					<conversionPattern value="%property{queryString}" />
    ///				</layout>
    ///			</parameter>
    ///			<parameter>
    ///				<parameterName value="@httpMethod" />
	///				<dbType value="String" />
	///				<size value="25" />
	///				<layout type="log4net.Layout.PatternLayout">
	///					<conversionPattern value="%property{httpMethod}" />
	///				</layout>
	///			</parameter>
	///			<parameter>
	///				<parameterName value="@userAgent" />
    ///				<dbType value="String" />
    ///				<size value="512" />
    ///				<layout type="log4net.Layout.PatternLayout">
    ///					<conversionPattern value="%property{userAgent}" />
    ///				</layout>
    ///			</parameter>
    ///			<parameter>
    ///				<parameterName value="@ipAddress" />
    ///				<dbType value="String" />
    ///				<size value="512" />
    ///				<layout type="log4net.Layout.PatternLayout">
    ///					<conversionPattern value="%property{ipAddress}" />
    ///				</layout>
    ///			</parameter>
    ///			<parameter>
    ///				<parameterName value="@urlReferrer" />
    ///				<dbType value="String" />
    ///				<size value="512" />
    ///				<layout type="log4net.Layout.PatternLayout">
    ///					<conversionPattern value="%property{urlReferrer}" />
    ///				</layout>
    ///			</parameter>
    ///			<parameter>
    ///				<parameterName value="@serverName" />
    ///				<dbType value="String" />
	///				<size value="512" />
	///				<layout type="log4net.Layout.PatternLayout">
	///					<conversionPattern value="%property{machineName}" />
	///				</layout>
	///			</parameter>
	///		</appender>
	/// ]]>
	/// </code>
	/// </example>
	public class Log4NetHttpContextProperties
    {
        /// <summary>
        /// Repsents the log4net contexts - http://logging.apache.org/log4net/release/manual/contexts.html
        /// </summary>
        public enum ContextTarget
        {
            /// <summary>
            /// <see cref="log4net.ThreadContext"/> - The thread context is visible only to the current managed thread. 
            /// </summary>
            Thread = 0,
            /// <summary>
            /// <see cref="GlobalContext"/> - The global context is shared by all threads in the current AppDomain. This context is thread safe for use by multiple threads concurrently. 
            /// </summary>
            Global = 1,
            /// <summary>
            /// <see cref="LogicalThreadContext"/> -  	The logical thread context is visible to a logical thread. Logical threads can jump from one managed thread to another. For more details see the .NET API System.Runtime.Remoting.Messaging.CallContext. 
            /// </summary>
            LogicalThread = 3
        }

        /// <summary>
        /// Adds custom properties to the <see cref="target"/> representing <see cref="HttpContext.Current"/>.<see cref="HttpRequest"/>.
        /// See <see cref="Log4NetHttpContextProperties"/> for a list of all properties added.
        /// </summary>
        /// <param name="target">The <see cref="ContextTarget"/> to update.  Default is <see cref="ContextTarget.Global"/></param>
        public Log4NetHttpContextProperties(ContextTarget target = ContextTarget.Global)
        {
            ContextPropertiesBase targetProperties;

            switch (target)
            {
                case ContextTarget.LogicalThread:
                    targetProperties = LogicalThreadContext.Properties;
                    break;
                case ContextTarget.Global:
                    targetProperties = GlobalContext.Properties;
                    break;
                case ContextTarget.Thread:
                    targetProperties = ThreadContext.Properties;
                    break;
                default:
                    throw new UnknownEnumValueException<ContextTarget>(target);
            }

            AddPropertiesToContext(targetProperties);
        }

        private void AddPropertiesToContext(ContextPropertiesBase properties)
        {
            if (null == HttpContext.Current)
                return;

            var request = HttpContext.Current.Request;

            properties["url"] = request.Url.AbsoluteUri;
            properties["urlReferrer"] = request.UrlReferrer;
            properties["userAgent"] = request.UserAgent;
            properties["ipAddress"] = request.GetRequestIpAddress();
            properties["isLocal"] = request.IsLocal;
            properties["queryString"] = request.QueryString;
            properties["httpMethod"] = request.HttpMethod;
            properties["machineName"] = Environment.MachineName;
        }
    }
}
