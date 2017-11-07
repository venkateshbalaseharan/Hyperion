using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hyperion.Web.Models
{
    public class TxnStatus
    {
        public string Channel { get; set; }
        public string MerchantId { get; set; }
        public string TxnRefnum { get; set; }
    }
}