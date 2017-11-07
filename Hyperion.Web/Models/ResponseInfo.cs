using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Hyperion.Web.Models
{
    public class ResponseInfo
    {
        public int RespCode { get; set; }
        public string RespMsg { get; set; }

        public HttpResponseMessage ApiResponse {get;set;}

    }
}