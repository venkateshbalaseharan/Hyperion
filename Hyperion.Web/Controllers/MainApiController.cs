using Hyperion.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Hyperion.Web.Controllers
{
    [RoutePrefix("api/merchantupi")]
    public class MainApiController : ApiController
    {
        [Route("validate")]
        [HttpPost]
        public HttpResponseMessage ValidateVpa([FromBody]  ValidateVpa vpaDtls)
        {
            var mainMod = new MainModel();
            var vProp = mainMod.ValidateVpa(vpaDtls);
            var jser = new JavaScriptSerializer();
            var jsrespInfo = jser.Serialize(vProp);
            return Request.CreateResponse(HttpStatusCode.OK, jsrespInfo);
        }

        [Route("collect")]
        [HttpPost]
        public HttpResponseMessage InitiateCollect([FromBody]  TxnDetails txnDtls)
        {
            var mainMod = new MainModel();
            var vProp = mainMod.InitiateCollect(txnDtls);
            var jser = new JavaScriptSerializer();
            var jsrespInfo = jser.Serialize(vProp);
            return Request.CreateResponse(HttpStatusCode.OK, jsrespInfo);
        }

        [Route("status")]
        [HttpPost]
        public HttpResponseMessage TransactionStatus([FromBody]  TxnStatus txnStat)
        {
            var mainMod = new MainModel();
            var vProp = mainMod.TransactionStatus(txnStat);
            var jser = new JavaScriptSerializer();
            var jsrespInfo = jser.Serialize(vProp);
            return Request.CreateResponse(HttpStatusCode.OK, jsrespInfo);
        }



    }
}
