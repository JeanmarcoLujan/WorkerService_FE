using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using WorkerService_FE_Entities.Repository.Interfaces;
using WorkerService_FE_Entities.Request;
using WorkerService_FE_Entities.Response;
using WorkerService_FE_Entities.ServiceLayer;
using WorkerService_FE_Entities.ServiceLayer.Document;
using WorkerService_FE_Response.Repository.Interfaces;
using WorkerService_FE_SL.Repository.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WorkerService_FE_Response.Repository
{
    public class ResponseRepository : IResponseRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IServicioRepository _servicioRepository;
        private readonly ILogRepository _logRepository;
        public ResponseRepository(IConfiguration configuration, IServicioRepository servicioRepository, ILogRepository logRepository, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _servicioRepository = servicioRepository;
            _logRepository = logRepository;
            _httpClientFactory = httpClientFactory;
        }
        public List<NCFResponse> GetDocumentSeguimientoSAP()
        {
            List<NCFResponse> result = new List<NCFResponse>();
           
            string HANAConnectionString = _configuration["Acceso:ConnectionStringsSAP"];
            HanaConnection conn = new HanaConnection(HANAConnectionString);
            try
            {

                var asdasd = conn.State;

                if (conn.State.Equals(ConnectionState.Closed))
                {
                    conn.Open();
                    HanaCommand cmd = new HanaCommand("", conn);
                    cmd.CommandText = "MGS_SP_FE"; 
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@vTipo", HanaDbType.NVarChar, 30).Value = "GET_DOCS_SEGUI";
                    cmd.Parameters.Add("@vParam1", HanaDbType.NVarChar, 50).Value = "";
                    cmd.Parameters.Add("@vParam2", HanaDbType.NVarChar, 50).Value = "";
                    cmd.Parameters.Add("@vParam3", HanaDbType.NVarChar, 50).Value = "";
                    cmd.Parameters.Add("@vParam4", HanaDbType.NVarChar, 50).Value = "";

                    HanaDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        NCFResponse nCF = new NCFResponse();
                        nCF.DocEntry = reader.GetString(reader.GetOrdinal("DocEntry")).ToString();
                        nCF.NCF = reader.GetString(reader.GetOrdinal("NCF")).ToString();
                        nCF.DocSubType = reader.GetString(reader.GetOrdinal("DocSubType")).ToString();
                        nCF.ObjType = reader.GetString(reader.GetOrdinal("ObjType")).ToString();
                        result.Add(nCF);
                    }


                    reader.Close();



                    conn.Close();
                }

            }
            catch (Exception ex)
            {

                if (conn.State.Equals(ConnectionState.Open))
                    conn.Close();

                result = null;

            }

            return result;
        }

        public async Task<ResultReponse> GetResult(NCFResponse nCFResponse, string token)
        {
            //string fileName = Path.GetFileName(@"C:\fe\factura_ejemplo.xml");
            string content1 = ""; 
            string str1 = _configuration["Voxel:User"].ToString();
            string str2 = _configuration["Voxel:Pass"].ToString();
            string requestUri = _configuration["Voxel:Url"] +"inbox/" + nCFResponse.NCF +".json";

            ResultReponse resultReponse = new ResultReponse();
            InfoRequest infoRequest = new InfoRequest();

            string ruta = _configuration["Report:Ruta"].ToString()+"\\files";
            if (Directory.Exists(ruta))
            {
                Directory.CreateDirectory(ruta);
            }

            using (HttpClient httpClient = new HttpClient())
            {
                string base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(str1 + ":" + str2));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64String);
                StringContent content2 = new StringContent(content1, Encoding.UTF8, "application/xml");

                string documentName = "";
                switch (nCFResponse.ObjType)
                {
                    case "13":
                        documentName = nCFResponse.DocSubType == "--" ? "Factura" : "NotaDebito";
                        break;
                    case "14":
                        documentName = "NotaCredito";
                        break;

                }


                try
                {
                    HttpResponseMessage result1 = httpClient.GetAsync(requestUri).Result;
                    DocBaseResponse docBase = new DocBaseResponse();
                    infoRequest.Token = token;

                    if (result1.IsSuccessStatusCode)
                    {
                        string result2 = result1.Content.ReadAsStringAsync().Result;
                        resultReponse = JsonConvert.DeserializeObject<ResultReponse>(result2);

                        docBase.U_MGS_FE_Estado = resultReponse.DGII.EstatusDGII == "Rechazada" ? "DR" : "DA";
                        docBase.U_MGS_FE_EstatusDGII = resultReponse.DGII.EstatusDGII;
                        docBase.U_MGS_FE_MensajeDGII = resultReponse.DGII.MensajeDGII;
                        bool result_v1 = await GenerateQr(nCFResponse.DocEntry.ToString(), resultReponse.DGII.QR, "1");
                        if (result_v1)
                            docBase.U_MGS_FE_PDF = ruta + "\\"+ documentName +"_v1_" + nCFResponse.DocEntry.ToString() + ".pdf"; // resultReponse.DGII.PDF;
                        else
                            docBase.U_MGS_FE_PDF = "";
                        bool result_v2 = await GenerateQr(nCFResponse.DocEntry.ToString(), resultReponse.DGII.QR, "2");
                        if (result_v2)
                            docBase.U_MGS_FE_PDF2 = ruta + "\\"+documentName+"_v2_" + nCFResponse.DocEntry.ToString() + ".pdf"; // resultReponse.DGII.PDF;
                        else
                            docBase.U_MGS_FE_PDF2 = "";


                        docBase.U_MGS_FE_QR = resultReponse.DGII.QR;
                        docBase.U_MGS_FE_FechaFirma = resultReponse.DGII.FechaFirma.ToString("dd/MM/yyyy");
                        docBase.U_MGS_FE_CodigoSeguridad = resultReponse.DGII.CodigoSeguridad;

                        string json = JsonConvert.SerializeObject(docBase);
                        infoRequest.Doc = json;
                        if(nCFResponse.ObjType == "13")
                            infoRequest.Route = "Invoices(" + nCFResponse.DocEntry + ")";
                        else
                            infoRequest.Route = "CreditNotes(" + nCFResponse.DocEntry + ")";

                        _servicioRepository.UpdateInfo(infoRequest);
                        _logRepository.Log(documentName + " " + nCFResponse.DocEntry + " Se ha obtenido respuesta con éxito", 2);
                        
                    }
                    else
                    {
                        //GenerateQr();

                        string result3 = result1.Content.ReadAsStringAsync().Result;
                        _logRepository.Log(documentName + " " + nCFResponse.DocEntry + ", con NCF: " + nCFResponse.NCF + ", no se ha encontrado en voxel", 2);

                    }
                }
                catch (Exception ex)
                {
                }
            }
            return null;
        }


        public async Task<bool> GenerateQr(string docEntry, string urlQr, string report)
        {

            //var client = new HttpClient();
            //var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44316/api/Report");
            bool resultado = false;
            try
            {
                ReportRequest re = new ReportRequest();
                re.DocEntry = docEntry;
                re.ObjectId = "13";
                re.UrlQr = urlQr;
                re.Report = report;
                re.Ruta = _configuration["Report:Ruta"].ToString();

                var client = _httpClientFactory.CreateClient("reportService");
                string json = JsonConvert.SerializeObject(re);

                var httpContenido = new StringContent(json, Encoding.UTF8, "application/json");
               // var respuesta = await client.PostAsync($"api/Values", null);
                var respuesta = await client.PostAsync($"api/Report", httpContenido);

                if (respuesta.IsSuccessStatusCode)
                {
                    var content = await respuesta.Content.ReadAsStringAsync();
                    ReportReponse reportReponse = JsonConvert.DeserializeObject<ReportReponse>(content);
                    resultado = reportReponse.ValorRecibido;

                }
                else
                {
                    resultado = false;  
                }

            }
            catch(Exception ex)
            {
                resultado = false;
            }

            return resultado;

        }

    }
}
