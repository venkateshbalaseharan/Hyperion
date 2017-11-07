using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hyperion.Web.Models
{
    public class RequestHeader
    {
        public string Api_Version { get; set; }
        public string Sarvatra_Hmac { get; set; }
        public string Sarvatra_Channel { get; set; }

    }
}