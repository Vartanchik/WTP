using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WTP.WebAPI.Helpers
{
    [DataContract]
    public class APIResponse
    {
        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public int StatusCode { get; set; }

        [DataMember]
        public string Message { get; set; }

        public APIResponse(int statusCode, string message = "", string apiVersion = "1.0.0.0")
        {
            this.StatusCode = statusCode;
            this.Message = message;
            this.Version = apiVersion;
        }
    }
}
