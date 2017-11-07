using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Hyperion.Web.Models
{
    public class RestConnector
    {
        public ResponseInfo CallWebApi(ApiCaller apiCaller)
        {

            var resp = new ResponseInfo
            {
                RespCode = 99,
                RespMsg = ""
            };

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Concat(apiCaller.BaseUrl,apiCaller.Method));
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(apiCaller.Data);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    resp.RespCode = 1;
                    resp.RespMsg = result;
                }

            }
            catch(Exception ex)
            {
                resp.RespCode = 2;
                resp.RespMsg = ex.ToString();

            }




                return resp;
            }
        }


        //public ResponseInfo CallWebApi(ApiCaller apiCaller)
        // {

        //     var resp = new ResponseInfo
        //     {
        //         RespCode = 99,
        //         RespMsg = ""
        //     };

        //     using (var client = new HttpClient())
        //     {
        //         client.BaseAddress = new Uri(apiCaller.BaseUrl);
        //         client.DefaultRequestHeaders.Accept.Clear();

        //         client.DefaultRequestHeaders.Accept.Add(
        //       new MediaTypeWithQualityHeaderValue("application/json"));



        //        // client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(apiCaller.ContentType));
        //         try
        //         {


        //         var response = client.PostAsJsonAsync(apiCaller.Method, apiCaller.Data).Result;




        //             if (response != null)
        //             {
        //                 resp.ApiResponse = response;
        //             }

        //             if (response.IsSuccessStatusCode)
        //             {
        //                 resp.RespCode = 1;
        //                 resp.RespMsg = "Success";

        //             }
        //             else
        //             {
        //                 resp.RespCode = 2;
        //                 resp.RespMsg = "Error Occurred";
        //             }

        //         }catch (Exception ex)
        //         {
        //             resp.RespCode = 3;
        //             resp.RespMsg = ex.ToString();

        //         }



        //         return resp;
        //     }
        // }




    }
