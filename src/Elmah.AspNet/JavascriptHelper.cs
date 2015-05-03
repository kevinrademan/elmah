#region License, Terms and Author(s)
//
// ELMAH - Error Logging Modules and Handlers for ASP.NET
// Copyright (c) 2004-9 Atif Aziz. All rights reserved.
//
//  Author(s):
//
//      James Driscoll, mailto:jamesdriscoll@btinternet.com
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

namespace Elmah
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using MoreLinq;

    #endregion

    public static class JavascriptHelper
    {
        static string _javascriptHash;
        static readonly ReadOnlyCollection<string> _javascriptResourceNames = Array.AsReadOnly(new[] {"jquery.js", "elmah.js"});

        public static string JavascriptHash
        {
            get { return _javascriptHash ?? (_javascriptHash = CalculateHash()); }
        }

        public static IEnumerable<string> JavascriptResourceNames
        {
            get { return _javascriptResourceNames; }
        }

        private static string CalculateHash()
        {
            var memoryStream = new MemoryStream();
            foreach (var resourceName in _javascriptResourceNames)
                ManifestResourceHelper.WriteResourceToStream(memoryStream, typeof(JavascriptHelper), resourceName);

            return MD5.Create()
                      .ComputeHash(memoryStream)
                      .Select(b => b.ToString("x2"))
                      .ToDelimitedString(string.Empty);
        }
    }
}