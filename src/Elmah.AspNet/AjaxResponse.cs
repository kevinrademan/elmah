using System;
using System.Linq;

namespace Elmah
{
    public class AjaxResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public object Body { get; set; }
    }
}
