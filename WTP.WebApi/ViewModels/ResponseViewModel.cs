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

        public ResponseViewModel()
        { }

        public ResponseViewModel(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public ResponseViewModel(int statusCode, string message, string info)
        {
            StatusCode = statusCode;
            Message = message;
            Info = info;
        }
    }
}
