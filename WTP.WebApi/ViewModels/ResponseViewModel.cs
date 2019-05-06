using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WTP.WebAPI.ViewModels
{
    public class ResponseViewModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Info { get; set; }
    }
}
