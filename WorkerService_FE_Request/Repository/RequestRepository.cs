using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Sap.Data.Hana;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Serialization;
using WorkerService_FE_Entities.Repository.Interfaces;
using WorkerService_FE_Entities.Request.Document;
using WorkerService_FE_Entities.ServiceLayer;
using WorkerService_FE_Entities.ServiceLayer.Document;
using WorkerService_FE_Request.Repository.Interfaces;
using WorkerService_FE_SL.Repository.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;
using DueDate = WorkerService_FE_Entities.Request.Document.DueDate;
using Transaction = WorkerService_FE_Entities.Request.Document.Transaction;

namespace WorkerService_FE_Request.Repository
{
    public class RequestRepository : IRequestRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IServicioRepository _servicioRepository;
        private readonly ILogRepository _logRepository; 
        public RequestRepository(IConfiguration configuration, IServicioRepository servicioRepository, ILogRepository logRepository)
        {
            _configuration = configuration;
            _servicioRepository = servicioRepository;
            _logRepository = logRepository;
        }

        public int GetDocumentSAP()
        {
            HanaConnection selectConnection = new HanaConnection("Server=saphaargendemo:30015;UserID=B1ADMIN;Password=7SkyOne*YjllM2ZkYz;Current Schema=LOCALIZACION_RDR");
            HanaDataAdapter hanaDataAdapter = new HanaDataAdapter("CALL \"MGS_SP_GET_INVOICE\"", selectConnection);
            DataSet dataSet = new DataSet();
            hanaDataAdapter.Fill(dataSet, "DATASet");

            DataTable dataTable = dataSet.Tables[0];
            string docEntry = "";

            

            foreach (var data in dataTable.AsEnumerable().Select(row => new
            {
                DocNum = row.Field<string>("Ref"),
                Nombre = row.Field<string>("Type")
            }).Distinct())
            {
                var item = data;
                DataTable dataTable1 = dataTable.AsEnumerable().Where<DataRow>((Func<DataRow, bool>)(row => row.Field<string>("REF") == item.DocNum)).CopyToDataTable<DataRow>();
                Transaction transaccion = new Transaction();
                IEnumerator enumerator = dataTable1.Rows.GetEnumerator();
                try
                {
                    if (enumerator.MoveNext())
                    {
                        DataRow current = (DataRow)enumerator.Current;

                        var sss = current["Ref"].ToString();
                        var sss1 = current["Type"].ToString();
                        var sdsdsd = Convert.ToDateTime(current["Date"]).ToString("yyyy-MM-dd");
                        var c1 = current["Currency"].ToString();
                        var c2 = Math.Round(Convert.ToDecimal(current["ExchangeRate"]), 3);
                        var c3 = Convert.ToBoolean(current["TaxIncluded"]);
                        var c4 = current["NCF"].ToString();
                        var c5 = Convert.ToDateTime(current["NCFExpirationDate"]).ToString("yyyy-MM-dd");
                        var c6 = current["TipoIngreso"].ToString();
                        var c7 = current["TipoPago"].ToString();
                        var c8 = Convert.ToInt32(current["LinesPerPrint"]);

                        docEntry = current["DocEntry"].ToString();

                        transaccion.GeneralData = new GeneralData()
                        {
                            Ref = current["Ref"].ToString(),
                            Type = current["Type"].ToString(),
                            Date = Convert.ToDateTime(current["Date"]).ToString("yyyy-MM-dd"),
                            Currency = current["Currency"].ToString(),
                            ExchangeRate = Math.Round(Convert.ToDecimal(current["ExchangeRate"]), 3),
                            TaxIncluded = Convert.ToBoolean(current["TaxIncluded"]),
                            NCF = current["NCF"].ToString(),
                            NCFExpirationDate = Convert.ToDateTime(current["NCFExpirationDate"]).ToString("yyyy-MM-dd"),
                            PublicAdministration = new PublicAdministration()
                            {
                                DOM = new DOM()
                                {
                                    TipoIngreso = current["TipoIngreso"].ToString(),
                                    TipoPago = current["TipoPago"].ToString(),
                                    LinesPerPrintedPage = Convert.ToInt32(current["LinesPerPrint"])
                                }
                            }
                        };
                        transaccion.Supplier = new Supplier()
                        {
                            SupplierID = current["sSupplierID"].ToString(),
                            Company = current["sCompany"].ToString(),
                            CIF = current["sCIF"].ToString(),
                            Address = current["sAddress"].ToString(),
                            Country = current["sCountry"].ToString(),
                            City = current["sCity"].ToString(),
                            PC = current["sPC"].ToString(),
                            Province = current["sProvince"].ToString(),
                            Email = current["sEmail"].ToString()
                        };
                        transaccion.Client = new Client()
                        {
                            CIF = current["cCIF"].ToString(),
                            Company = current["cCompany"].ToString(),
                            Email = current["cEMail"].ToString(),
                            Address = current["cAddress"].ToString(),
                            City = current["cCity"].ToString(),
                            PC = current["cPC"].ToString(),
                            Province = current["Province"].ToString(),
                            Country = current["Country"].ToString()
                        };
                        transaccion.ProductList = new ProductList()
                        {
                            Products = new List<Product>()
                        };
                        foreach (DataRow row in (InternalDataCollectionBase)dataTable1.Rows)
                        {
                            Product product = new Product()
                            {
                                SupplierSKU = row["SupplierSKU"].ToString(),
                                Item = row["Item"].ToString(),
                                Qty = Math.Round(Decimal.Parse(row["Qty"].ToString()), 3),
                                MU = row["MU"].ToString(),
                                CU = int.Parse(row["CU"].ToString()),
                                UP = Math.Round(Decimal.Parse(row["UP"].ToString()), 3),
                                Total = Math.Round(Decimal.Parse(row["Total"].ToString()), 3),
                                NetAmount = Math.Round(Decimal.Parse(row["NetAmount"].ToString()), 3),
                                SysLineType = row["SysLineType"].ToString(),
                                Taxes = new Taxes()
                                {
                                    TaxList = new List<Tax>()
                                      {
                                        new Tax()
                                        {
                                          Type = row["txType"].ToString(),
                                          Rate = Math.Round(Decimal.Parse(row["Rate"].ToString()), 3),
                                          Base = Math.Round(Decimal.Parse(row["txBase"].ToString()), 3),
                                          Amount = Math.Round(Decimal.Parse(row["txAmount"].ToString()), 3)
                                        }
                                      }
                                }
                            };
                            transaccion.ProductList.Products.Add(product);
                        }
                        transaccion.TaxSummary = new TaxSummary()
                        {
                            TaxList = new List<Tax>()
                              {
                                new Tax()
                                {
                                  Type = current["txType"].ToString(),
                                  Rate = Math.Round(Decimal.Parse(current["Rate"].ToString()), 3),
                                  Base = Math.Round(Decimal.Parse(current["SubTotalGrl"].ToString()), 3),
                                  Amount = Math.Round(Decimal.Parse(current["ImpuestoGrl"].ToString()), 3)
                                }
                              }
                        };
                        if (transaccion.GeneralData.PublicAdministration.DOM.TipoPago == "2")
                            transaccion.DueDates = new DueDates()
                            {
                                DueDate = new DueDate()
                                {
                                    Date = Convert.ToDateTime(current["DueDateCredit"]).ToString("yyyy-MM-dd"),
                                    Amount = Math.Round(Decimal.Parse(current["AmountCredit"].ToString()), 3).ToString(),
                                    PaymentID = current["PaymentID"].ToString()
                                }
                            };
                        transaccion.TotalSummary = new TotalSummary()
                        {
                            SubTotal = Math.Round(Decimal.Parse(current["SubTotalGrl"].ToString()), 3),
                            Tax = Math.Round(Decimal.Parse(current["ImpuestoGrl"].ToString()), 3),
                            Total = Math.Round(Decimal.Parse(current["TotalGrl"].ToString()), 3),
                            PublicAdministration = new PublicAdministration()
                            {
                                DOM = new DOM()
                                {
                                    MontoITBISRetenido = "0.00",
                                    MontoISRRetenido = current["CantidadRetenida"].ToString()
                                }
                            }
                        };
                    }
                }
                finally
                {
                    if (enumerator is IDisposable disposable)
                        disposable.Dispose();
                }
                string str = transaccion.GeneralData.Ref.PadLeft(13, '0');

                var sdfsdf = transaccion;

                DateTime dateTime = DateTime.Today;
               

                //var asdadasd = transaccion;
                //SerializarYGuardarXml(transaccion, @"C:\fe\request\" + dateTime.ToString("yyyyMMdd") +"_" + str + ".xml");
                SerializarYGuardarXml(transaccion, _configuration["Files:RouteRequest"].ToString()+ "\\"+docEntry + "_" + dateTime.ToString("yyyyMMdd") +"_" + str + ".xml");
            }

            return dataTable.Rows.Count;

        }

        public void SerializarYGuardarXml(Transaction transaccion, string rutaArchivo)
        {
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(rutaArchivo))
                    new XmlSerializer(typeof(Transaction)).Serialize((TextWriter)streamWriter, (object)transaccion);
                Console.WriteLine("Se ha guardado el archivo XML en: " + rutaArchivo);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al serializar y guardar el archivo XML: " + ex.Message);
            }
        }

        public void SendDocuement(string token)
        {


            string path = _configuration["Files:RouteRequest"].ToString();
            InfoRequest infoRequest = new InfoRequest();
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);
                if (files.Length != 0)
                {
                    infoRequest.Token = token;
                    foreach (string oPath in files)
                    {
                        var ssd = oPath;
                        string fileName = Path.GetFileName(oPath);
                        string content1 = System.IO.File.ReadAllText(oPath);
                        string str1 = _configuration["Voxel:User"].ToString(); 
                        string str2 = _configuration["Voxel:Pass"].ToString();
                        string requestUri = _configuration["Voxel:Url"] + "outbox/" + fileName;

                        using (HttpClient httpClient = new HttpClient())
                        {
                            string base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(str1 + ":" + str2));
                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64String);
                            StringContent content2 = new StringContent(content1, Encoding.UTF8, "application/xml");
                            try
                            {
                                HttpResponseMessage result1 = httpClient.PutAsync(requestUri, (HttpContent)content2).Result;
                                //ParamsOfResult oParamsOfResult = new ParamsOfResult();
                                //string xml = System.IO.File.ReadAllText(oPath);
                                //XmlDocument xmlDocument = new XmlDocument();
                                //xmlDocument.LoadXml(xml);
                                //oParamsOfResult.DocEntry = BusinessOneServices.GetDocEntry(xmlDocument);
                                DocBase docBase = new DocBase();
                                if (result1.IsSuccessStatusCode)
                                {
                                    string result2 = result1.Content.ReadAsStringAsync().Result;

                                    string[] docE = fileName.Split('_');
                                    
                                    docBase.U_MGS_FE_Estado = "DS";
                                    docBase.U_MGS_FE_RespEnvio = "Envío Correcto";

                                    string json = JsonConvert.SerializeObject(docBase);
                                    infoRequest.Doc = json;
                                    infoRequest.Route = "Invoices("+ docE[0] +")";

                                    _servicioRepository.UpdateInfo(infoRequest);
                                    _logRepository.Log("Documento " + docE[0] + " Se ha enviado con éxito", 1);

                                    //oParamsOfResult.Estado = "DS";
                                    //oParamsOfResult.ResultDscrp = "Envío Correcto";
                                    //BusinessOneServices.SetResultInvoice(oParamsOfResult);
                                    System.IO.File.Move(path + fileName, path + "out\\" + fileName);
                                }
                                else
                                {
                                    string result3 = result1.Content.ReadAsStringAsync().Result;
                                    //System.IO.File.Move("C:\\MGS - Facturación Electrónica\\xml\\" + fileName, "C:\\MGS - Facturación Electrónica\\xml\\out\\Error\\" + fileName);
                                    System.IO.File.Move(path + fileName, path + "out\\" + fileName);
                                    //oParamsOfResult.Estado = "DE";
                                    //oParamsOfResult.ResultDscrp = result3;
                                    //BusinessOneServices.SetResultInvoice(oParamsOfResult);

                                    string[] docE = fileName.Split('_');

                                    docBase.U_MGS_FE_Estado = "DE";
                                    docBase.U_MGS_FE_RespEnvio = "Se presento error al enviar";

                                    _logRepository.Log("Documento " + docE[0] + " Se presento error al enviar", 1);

                                    string json = JsonConvert.SerializeObject(docBase);
                                    infoRequest.Doc = json;
                                    infoRequest.Route = "Invoices(" + docE[0] + ")";

                                    _servicioRepository.UpdateInfo(infoRequest);
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }
            }


            //string fileName1 = Path.GetFileName(@"C:\fe\factura_ejemplo.xml");
            //string content11 = System.IO.File.ReadAllText(@"C:\fe\factura_ejemplo.xml");
            //string str11 = "voxelcaribetest";
            //string str21 = "Voxelcaribe01@";
            //string requestUri1 = "https://fileconnector.voxelgroup.net/outbox/" + fileName1;
            //using (HttpClient httpClient = new HttpClient())
            //{
            //    string base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(str1 + ":" + str2));
            //    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64String);
            //    StringContent content2 = new StringContent(content1, Encoding.UTF8, "application/xml");
            //    try
            //    {
            //        HttpResponseMessage result1 = httpClient.PutAsync(requestUri, (HttpContent)content2).Result;
            //        //ParamsOfResult oParamsOfResult = new ParamsOfResult();
            //        //string xml = System.IO.File.ReadAllText(oPath);
            //        //XmlDocument xmlDocument = new XmlDocument();
            //        //xmlDocument.LoadXml(xml);
            //        //oParamsOfResult.DocEntry = BusinessOneServices.GetDocEntry(xmlDocument);
            //        if (result1.IsSuccessStatusCode)
            //        {
            //            string result2 = result1.Content.ReadAsStringAsync().Result;
            //            //oParamsOfResult.Estado = "DS";
            //            //oParamsOfResult.ResultDscrp = "Envío Correcto";
            //            //BusinessOneServices.SetResultInvoice(oParamsOfResult);
            //            //System.IO.File.Move("C:\\MGS - Facturación Electrónica\\xml\\" + fileName, "C:\\MGS - Facturación Electrónica\\xml\\out\\" + fileName);
            //        }
            //        else
            //        {
            //            string result3 = result1.Content.ReadAsStringAsync().Result;
            //            //System.IO.File.Move("C:\\MGS - Facturación Electrónica\\xml\\" + fileName, "C:\\MGS - Facturación Electrónica\\xml\\out\\Error\\" + fileName);
            //            //oParamsOfResult.Estado = "DE";
            //            //oParamsOfResult.ResultDscrp = result3;
            //            //BusinessOneServices.SetResultInvoice(oParamsOfResult);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //}
        }

        
    }
}
