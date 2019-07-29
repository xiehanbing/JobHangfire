using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jobs.Core.Models
{
    public class AjaxResult
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public int ErorCode { get; set; }
        public object Data { get; set; }
    }
}
