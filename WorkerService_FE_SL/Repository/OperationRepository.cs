using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WorkerService_FE_SL.Repository.Interfaces;

namespace WorkerService_FE_SL.Repository
{
    public class OperationRepository : IOperationRepository
    {
        public HttpWebResponse PostInfo(string obj, string document, string route)
        {
            throw new NotImplementedException();
        }

        public HttpWebResponse PostLogin()
        {
            try
            {

                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13 | SecurityProtocolType.Tls;


                //string data = "{    \"CompanyDB\": \"" +  ConfigurationManager.AppSettings["CompanyDB"].ToString() + "\",  \"UserName\": \"" + ConfigurationManager.AppSettings["UserName"].ToString() + "\", \"Password\": \"" + ConfigurationManager.AppSettings["Password"].ToString() + "\", \"Language\":\"23\"}";
                string data = "{    \"CompanyDB\": \"" + "LOCALIZACION_RDR" + "\",  \"UserName\": \"" + "manager" + "\", \"Password\": \"" + "1234" + "\", \"Language\":\"23\"}";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(@"https://saphaargendemo:50000/b1s/v2/" + "Login");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                /*
                httpWebRequest.KeepAlive = true;
               
                httpWebRequest.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                httpWebRequest.Headers.Add("B1S-WCFCompatible", "true");
                httpWebRequest.Headers.Add("B1S-MetadataWithoutSession", "true");
                httpWebRequest.Accept = 
                httpWebRequest.ServicePoint.Expect100Continue = false;
                httpWebRequest.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip;
                */

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                { streamWriter.Write(data); }

                return (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public HttpWebResponse UpdInfo(string obj, string document, string route)
        {
            try
            {
                var httpWebGetRequest = (HttpWebRequest)WebRequest.Create(route);
                httpWebGetRequest.ContentType = "application/json";
                httpWebGetRequest.Method = "PATCH";
                httpWebGetRequest.KeepAlive = true;
                httpWebGetRequest.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                httpWebGetRequest.Headers.Add("B1S-WCFCompatible", "true");
                httpWebGetRequest.Headers.Add("B1S-MetadataWithoutSession", "true");
                httpWebGetRequest.Accept = "*/*";
                httpWebGetRequest.ServicePoint.Expect100Continue = false;
                httpWebGetRequest.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                httpWebGetRequest.AutomaticDecompression = DecompressionMethods.GZip;
                CookieContainer cookies = new CookieContainer();
                cookies.Add(new Cookie("B1SESSION", obj.ToString()) { Domain = "saphaargendemo" });
                //cookies.Add(new Cookie("ROUTEID", ".node1") { Domain = _configuration["ServiceLayer:ip_value"].ToString() });
                httpWebGetRequest.CookieContainer = cookies;

                using (var streamWriter = new StreamWriter(httpWebGetRequest.GetRequestStream()))
                { streamWriter.Write(document); }

                return (HttpWebResponse)httpWebGetRequest.GetResponse();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
