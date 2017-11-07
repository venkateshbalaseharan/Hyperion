using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;

namespace Hyperion.Web.Models
{
    public class Utilities
    {


        public ResponseInfo GetJsonKeyVal(string JsonResp, string KeyVal)
        {
            var resp = new ResponseInfo
            {
                RespCode = 99,
                RespMsg = ""
            };

            try
            {
                var jsSerializer = new JavaScriptSerializer();
                var result = jsSerializer.DeserializeObject(JsonResp);
                var obj2 = new Dictionary<string, object>();
                obj2 = (Dictionary<string, object>)(result);
                string val = obj2[KeyVal].ToString();
                resp.RespCode = 1;
                resp.RespMsg = val;

            }
            catch (Exception ex) {
                resp.RespCode = 2;
                resp.RespMsg = ex.ToString();

            }
            // if string with JSON data is not empty, deserialize it to class and return its instance 
            return resp;
        }

        
    

        public string GetJsonString(Dictionary<string,string> myDictionary)
        {
           return JsonConvert.SerializeObject(myDictionary);
        }

        public JObject ParseJsonResponse(string response)
        {
            return JObject.Parse(response);
           
        }
        public string MapPath(string path)
        {
            path = string.Concat("~/", path);
            return HttpContext.Current.Server.MapPath(path);
        }



        public string GetServiceParam(string key)
        {
            try
            {
                var output = "";
                var sValues = new string[11];

                if (File.Exists(MapPath("Params/ServiceParams.txt")) != true)
                    return output;
                var objReader = new StreamReader(MapPath("Params/ServiceParams.txt"));
                while (objReader.Peek() != -1)
                {
                    var textLine = objReader.ReadLine();

                    if (textLine != null) sValues = textLine.Split(Convert.ToChar("~"));

                    if (string.Equals(sValues[0].Trim().ToUpper(), key.Trim().ToUpper(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        output = sValues[1];
                    }

                }
                objReader.Close();
                return output;
            }
            catch (Exception ex)
            {
                WriteToLog(ex);
                return "";
            }
        }

        public string GetEnvironment()
        {
            return GetServiceParam("Environment");
        }
        public string GetSarvatraBaseUrl()
        {
           return GetServiceParam("SarvatraBaseUrl");
        }

        public string GetSarvatraContentType()
        {
            return GetServiceParam("ContentType");
        }
        public string GetSarvatraTxnStatus()
        {
            return GetServiceParam("TransactionStatus");
        }

        public string GetSarvatraValidateVpa()
        {
            return GetServiceParam("ValidateVpa");
        }

        public string GetSarvatraCollect()
        {
            return GetServiceParam("InitiateCollect");
        }
        public void WriteToLog(Exception ex)
        {
            try
            {
                var appPath = AppDomain.CurrentDomain.BaseDirectory;
                var filename = DateTime.Now.ToString("dd-MMM-yyyy");
                if (!Directory.Exists(appPath + "\\ErrorLogs\\"))
                {
                    Directory.CreateDirectory(appPath + "\\ErrorLogs\\");

                }
                var fullpath = appPath + "\\ErrorLogs\\" + filename + " - logfile.txt";
                var log = !File.Exists(fullpath) ? new StreamWriter(fullpath) : File.AppendText(fullpath);
                log.WriteLine("Type:" + "Exception");
                log.WriteLine("Data:" + ex);
                log.WriteLine("Data Time:" + DateTime.Now);
                log.WriteLine(Environment.NewLine);
                log.WriteLine("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ");
                log.WriteLine(Environment.NewLine);
                log.Close();
            }
            catch (Exception)
            {
                //ignored
            }

        }

    }
}