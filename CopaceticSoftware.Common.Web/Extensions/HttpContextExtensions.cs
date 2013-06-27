//----------------------------------------------------------------------- 
// <copyright file="HttpContextExtensions.cs" company="Copacetic Software"> 
// Copyright (c) Copacetic Software.  
// <author>Philip Pittle</author> 
// <date>Friday, June 07, 2013 12:44:51 PM</date> 
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

using System.Linq;
using System.Web;
using CopaceticSoftware.Common.Extensions;
using CopaceticSoftware.Common.Infrastructure;

namespace CopaceticSoftware.Common.Web.Extensions
{
    public static class HttpContextExtensions
    {
        //http://stackoverflow.com/questions/735350/how-to-get-a-users-client-ip-address-in-asp-net
        public static string GetRequestIpAddress(this HttpContext context)
        {
            Ensure.ArgumentNotNull(context, "context");

            return context.Request.GetRequestIpAddress();
        }

        public static string GetRequestIpAddress(this HttpRequest request)
        {
            Ensure.ArgumentNotNull(request, "request");

            var forawrdedForIp = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            return !forawrdedForIp.IsNullOrEmpty()
                ? forawrdedForIp.Split(',').FirstOrDefault()
                : request.ServerVariables["REMOTE_ADDR"];
        }

        public static string TryGetRequestIpAddress(this HttpContext context)
        {
            try
            {
                return GetRequestIpAddress(context);
            }
            catch 
            {
                return "unknown";
            }
        }
    }
}
