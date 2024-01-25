using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WorkerService_FE_Entities.Response;
using WorkerService_FE_Response.Repository.Interfaces;

namespace WorkerService_FE_Response.Repository
{
    public class ResponseRepository : IResponseRepository
    {
        public ResultReponse GetResult()
        {
            string fileName = Path.GetFileName(@"C:\fe\factura_ejemplo.xml");
            string content1 = System.IO.File.ReadAllText(@"C:\fe\factura_ejemplo.xml");
            string str1 = "voxelcaribetest";
            string str2 = "Voxelcaribe01@";
            string requestUri = "https://fileconnector.voxelgroup.net/inbox/E310000396466.json";// + fileName;
            using (HttpClient httpClient = new HttpClient())
            {
                string base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(str1 + ":" + str2));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64String);
                StringContent content2 = new StringContent(content1, Encoding.UTF8, "application/xml");
                try
                {
                    HttpResponseMessage result1 = httpClient.GetAsync(requestUri).Result;
                    //ParamsOfResult oParamsOfResult = new ParamsOfResult();
                    //string xml = System.IO.File.ReadAllText(oPath);
                    //XmlDocument xmlDocument = new XmlDocument();
                    //xmlDocument.LoadXml(xml);
                    //oParamsOfResult.DocEntry = BusinessOneServices.GetDocEntry(xmlDocument);
                    if (result1.IsSuccessStatusCode)
                    {
                        string result2 = result1.Content.ReadAsStringAsync().Result;
                        //oParamsOfResult.Estado = "DS";
                        //oParamsOfResult.ResultDscrp = "Envío Correcto";
                        //BusinessOneServices.SetResultInvoice(oParamsOfResult);
                        //System.IO.File.Move("C:\\MGS - Facturación Electrónica\\xml\\" + fileName, "C:\\MGS - Facturación Electrónica\\xml\\out\\" + fileName);
                    }
                    else
                    {
                        string result3 = result1.Content.ReadAsStringAsync().Result;
                        //System.IO.File.Move("C:\\MGS - Facturación Electrónica\\xml\\" + fileName, "C:\\MGS - Facturación Electrónica\\xml\\out\\Error\\" + fileName);
                        //oParamsOfResult.Estado = "DE";
                        //oParamsOfResult.ResultDscrp = result3;
                        //BusinessOneServices.SetResultInvoice(oParamsOfResult);
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return null;
        }
    }
}
