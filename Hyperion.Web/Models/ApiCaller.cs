using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hyperion.Web.Models
{
    public class ApiCaller
    {
        public string BaseUrl { get; set; }
        public string ContentType { get; set; }
        public string Method { get; set; }
        public string Data { get; set; }
        public ResponseInfo PostResp { get; set; }
    }
}