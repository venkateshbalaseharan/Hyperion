using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hyperion.Web.Models
{
    public class MainModel
    {

        public ApiCaller GetBaseVersion()
        {
            var utils = new Utilities();
            var apiCaller = new ApiCaller
            {
                BaseUrl = utils.GetSarvatraBaseUrl(),
                ContentType = utils.GetSarvatraContentType()
            };
            return apiCaller;
        }
        public ResponseInfo ValidateVpa(ValidateVpa vpaDtls)
        {
            var resp = new ResponseInfo
            {
                RespCode = 99,
                RespMsg = ""
            };

            var utils = new Utilities();
            var apiCaller = GetBaseVersion();
            apiCaller.Method = utils.GetSarvatraValidateVpa();

            var restConnector = new RestConnector();// GetResponseAsync(apiCaller).Wait();

            var data = new Dictionary<string, string>() {
        { "channel-code", vpaDtls.Channel },
        { "mid", vpaDtls.Mid },
        { "virtual-address", vpaDtls.Vpa }
    };


            apiCaller.Data = utils.GetJsonString(data);
            resp = restConnector.CallWebApi(apiCaller);

            if (resp.RespCode == 1)
            {
                var jsonResp = utils.ParseJsonResponse(resp.RespMsg);


                if (jsonResp != null)
                {

                    var status = (string)jsonResp["success"];
                    if (status == "0")
                    {
                        resp.RespCode = 1;
                        resp.RespMsg = (string)jsonResp["Result"]["customerName"];

                    }
                    else if (status == "37")
                    {
                        resp.RespCode = 2;
                        resp.RespMsg = "Invalid VPA";
                    }


                }

            }


            return resp;
        }

        public ResponseInfo InitiateCollect(TxnDetails txnDtls)
        {
            var resp = new ResponseInfo
            {
                RespCode = 99,
                RespMsg = ""
            };

            var utils = new Utilities();
            var apiCaller = GetBaseVersion();
            apiCaller.Method = utils.GetSarvatraCollect();

            var restConnector = new RestConnector();// GetResponseAsync(apiCaller).Wait();

            var data = new Dictionary<string, string>() {
        { "device-id", txnDtls.MerchantId},
        { "mobile", txnDtls.MerchantId},
                { "mid", txnDtls.MerchantId },
                { "channel-code", txnDtls.Channel },
                  { "payer-va", txnDtls.Payer},
                  { "payee-va", txnDtls.Payee},
                  { "amount", txnDtls.Amount},
                {"note","NA" },
                {"expire-after",txnDtls.Expiry },
        { "mer-txn-id", txnDtls.TxnRefnum },
        { "ref-id", txnDtls.TxnRefnum },
        { "payer-name", txnDtls.PayerName },
        { "ref-url", txnDtls.TxnRefnum },
         { "min-amount", "1" }

            };


            apiCaller.Data = utils.GetJsonString(data);
            resp = restConnector.CallWebApi(apiCaller);

            if (resp.RespCode == 1)
            {
                var jsonResp = utils.ParseJsonResponse(resp.RespMsg);


                if (jsonResp != null)
                {

                    var status = (string)jsonResp["response"];
                    if (status == "92")
                    {
                        resp.RespCode = 1;
                        resp.RespMsg = (string)jsonResp["RRN"];

                    }
                    else if (status == "37")
                    {
                        resp.RespCode = 2;
                        resp.RespMsg = "Invalid VPA";
                    }
                    else if (status == "84")
                    {
                        resp.RespCode = 3;
                        resp.RespMsg = "Invalid Txn Reference Number";
                    }

                }

            }


            return resp;
        }


        public ResponseInfo TransactionStatus(TxnStatus txnStat)
        {
            var resp = new ResponseInfo
            {
                RespCode = 99,
                RespMsg = ""
            };

            var utils = new Utilities();
            var apiCaller = GetBaseVersion();
            apiCaller.Method = utils.GetSarvatraTxnStatus();

            var restConnector = new RestConnector();// GetResponseAsync(apiCaller).Wait();

            var data = new Dictionary<string, string>() {
        { "channel-code", txnStat.Channel },
        { "mid", txnStat.MerchantId },
        { "ori-mer-txn-id", txnStat.TxnRefnum }
    };


            apiCaller.Data = utils.GetJsonString(data);
            resp = restConnector.CallWebApi(apiCaller);

            if (resp.RespCode == 1)
            {
                var jsonResp = utils.ParseJsonResponse(resp.RespMsg);


                if (jsonResp != null)
                {

                    var status = (string)jsonResp["transaction"]["rc"];
                    if (status == "0092")
                    {
                        resp.RespCode = 2;
                        resp.RespMsg = "PENDING";

                    }
                    else if (status == "0")
                    {
                        resp.RespCode = 1;
                        resp.RespMsg = "SUCCESS";
                    }


                }

            }


            return resp;
        }



    }
}