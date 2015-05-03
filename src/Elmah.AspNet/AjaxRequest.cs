using System;
using System.Linq;

namespace Elmah
{
    public class AjaxRequest
    {
        public string Command { get; set; }
        public dynamic Body { get; set; }
    }
}
