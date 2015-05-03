#region License, Terms and Author(s)
//
// ELMAH - Error Logging Modules and Handlers for ASP.NET
// Copyright (c) 2004-9 Atif Aziz. All rights reserved.
//
//  Author(s):
//
//      Atif Aziz, http://www.raboof.com
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

[assembly: Elmah.Scc("$Id: AjaxHandler.cs 923 2011-12-23 22:02:10Z azizatif $")]

namespace Elmah
{
    #region Imports

    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Xml.Linq;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    #endregion

    /// <summary>
    /// Renders an RSS feed that is a daily digest of the most recently 
    /// recorded errors in the error log. The feed spans at most 15
    /// days on which errors occurred.
    /// </summary>

    static class AjaxHandler
    {
        public static void ProcessRequest(HttpContextBase context)
        {
            if (context.Request.HttpMethod != "POST")
            {
                var response = new AjaxResponse();
                response.Message = "Only post requests are allowed";
                response.Status = -1;
                context.Response.Write(JsonConvert.SerializeObject(response));
                return;
            }
            

            var requestBody = (new StreamReader(HttpContext.Current.Request.InputStream).ReadToEnd());

            var ajaxRequest = JsonConvert.DeserializeObject<AjaxRequest>(requestBody);

            context.Response.Write(JsonConvert.SerializeObject(ExecuteCommand(ajaxRequest,context)));
        }

        private static AjaxResponse ExecuteCommand(AjaxRequest request, HttpContextBase context)
        {
            switch(request.Command){
                case "delete-errors":
                    return DeleteErrors(request, context);
                default:
                    var response = new AjaxResponse();
                    response.Message = "Command not found";
                    response.Status = -1;
                    return response;
            }
        }

        private static AjaxResponse DeleteErrors(AjaxRequest request, HttpContextBase context)
        {
            var response = new AjaxResponse();

            string[] ids = request.Body.ToObject<string[]>();
            var log = ErrorLog.GetDefault(context);
            foreach (var id in ids)
            {
                log.Delete(id);
            }
            response.Message = "Errors deleted";
            response.Status = 1;
            return response;
        }
   
    }
}