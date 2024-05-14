using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WorkerService_FE_Entities.Repository.Interfaces;
using WorkerService_FE_Entities.ServiceLayer.Document;
using WorkerService_FE_Entities.ServiceLayer;
using WorkerService_FE_Request.Repository.Interfaces;
using WorkerService_FE_SL.Repository.Interfaces;
using System.Collections;
using System.Data;
using System.IO;
using System.Xml.Serialization;
using Sap.Data.Hana;
using WorkerService_FE_Entities.Request.ND;

namespace WorkerService_FE_Request.Repository
{
    public class RequestNDRepository : IRequestNDRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IServicioRepository _servicioRepository;
        private readonly ILogRepository _logRepository;
        public RequestNDRepository(IConfiguration configuration, IServicioRepository servicioRepository, ILogRepository logRepository)
        {
            _configuration = configuration;
            _servicioRepository = servicioRepository;
            _logRepository = logRepository;
        }
        public int GetDocumentSAPND()
        {
            HanaConnection selectConnection = new HanaConnection(_configuration["Acceso:ConnectionStringsSAP"].ToString());
            HanaDataAdapter hanaDataAdapter = new HanaDataAdapter("CALL \"MGS_SP_GET_DEBITNOTE\"", selectConnection);
            DataSet dataSet = new DataSet();
            hanaDataAdapter.Fill(dataSet, "DATASet");

            DataTable dataTable = dataSet.Tables[0];
            string docEntry = "";


            foreach (var data in dataTable.AsEnumerable().Select(row => new
            {
                DocNum = row.Field<int>("Ref"),
                Nombre = row.Field<string>("Type")
            }).Distinct())
            {
                var item = data;
                DataTable dataTable1 = dataTable.AsEnumerable().Where<DataRow>((Func<DataRow, bool>)(row => row.Field<int>("REF") == item.DocNum)).CopyToDataTable<DataRow>();
                Transaction transaccion = new Transaction();
                IEnumerator enumerator = dataTable1.Rows.GetEnumerator();
                try
                {
                    


                    if (enumerator.MoveNext())
                    {
                        DataRow current = (DataRow)enumerator.Current;

                        var c5 = Convert.ToDateTime(current["NCFExpirationDate"]).ToString("yyyy-MM-dd");

                        docEntry = current["DocEntry"].ToString();

                        transaccion.GeneralData = new GeneralData()
                        {
                            Ref = current["Ref"].ToString(),
                            Type = current["Type"].ToString(),
                            Date = Convert.ToDateTime(current["Date"]).ToString("yyyy-MM-dd"),
                            Currency = current["Currency"].ToString(),
                            TaxIncluded = Convert.ToBoolean(current["TaxIncluded"]),
                            NCF = current["NCF"].ToString(),
                            NCFExpirationDate = "2025-01-01", // Convert.ToDateTime(current["NCFExpirationDate"]).ToString("yyyy-MM-dd"),
                            PublicAdministration = new PublicAdministration()
                            {
                                DOM = new DOM()
                                {
                                    TipoIngreso = current["TipoIngreso"].ToString(),
                                    TipoPago = current["FormaPago"].ToString(),
                                    LinesPerPrintedPage = current["LinesPerPrint"].ToString()
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
                            PC = Convert.ToInt32(current["sPC"]),
                            Province = current["sProvince"].ToString(),
                            Email = current["sEmail"].ToString()
                        };
                        transaccion.Client = new Client()
                        {
                            CIF = current["cCIF"].ToString(),
                            Company = current["cCompany"].ToString(),
                            Email = current["cEmail"].ToString(),
                            Address = current["cAddress"].ToString(),
                            City = current["cCity"].ToString(),
                            PC = current["cPC"].ToString(),
                            Province = current["Province"].ToString(),
                            Country = current["Country"].ToString()
                        };
                        transaccion.References = new References()
                        {
                            Reference = new Reference()
                            {
                                InvoiceRef = current["InvoiceRef"].ToString(),
                                InvoiceNCF = current["InvoiceNCF"].ToString(),
                                InvoiceRefDate = current["InvoiceRedDate"].ToString(),
                                PublicAdministration = new PublicAdministration()
                                {
                                    DOM = new DOM()
                                    {
                                        CodigoModificacion = current["CodigoModificacion"].ToString()
                                        //IndicadorNotaCredito = current["DOM.IndicadorNotaCredito"].ToString()
                                    }
                                    
                                }
                            }
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
                                Qty = Convert.ToDecimal(row["Qty"]),
                                MU = row["MU"].ToString(),
                                CU = Convert.ToInt32(row["CU"]),
                                UP = Convert.ToDecimal(row["UP"]),
                                Total = Convert.ToDecimal(row["Total"]),
                                NetAmount = Convert.ToDecimal(row["NetAmount"]),
                                SysLineType = row["SysLineType"].ToString(),
                                Taxes = new Taxes()
                                {
                                    TaxList = new List<Tax>()
                                      {
                                        new Tax()
                                        {
                                          Type = row["txType"].ToString(),
                                          Rate = Decimal.Parse(row["Rate"].ToString()),
                                          Base = Decimal.Parse(row["txBase"].ToString()),
                                          Amount = Decimal.Parse(row["txAmount"].ToString())
                                        }
                                      }
                                }
                            };
                            transaccion.ProductList.Products.Add(product);
                        }
                        transaccion.TaxSummary = new TaxSummary()
                        {
                            Tax = new Tax()
                            {
                                Type = current["txType"].ToString(),
                                Rate = Convert.ToDecimal(current["Rate"]),
                                Base = Convert.ToDecimal(current["txBase"]),
                                Amount = Convert.ToDecimal(current["txAmount"])
                            }
                        };
                        DataRow row1 = dataTable.Rows[0];
                        transaccion.TotalSummary = new TotalSummary()
                        {
                            SubTotal = Convert.ToDecimal(row1["tSubTotal"]),
                            Tax = Convert.ToDecimal(row1["tTax"]),
                            Total = Convert.ToDecimal(row1["tTotal"])
                        };
                    }
                }
                finally
                {
                    if (enumerator is IDisposable disposable)
                        disposable.Dispose();
                }
                string str = transaccion.GeneralData.Ref.PadLeft(13, '0');
                DateTime dateTime = DateTime.Today;

                string nomDirectorio = _configuration["Files:RouteRequest"].ToString() + "\\ND";
                if (!Directory.Exists(nomDirectorio))
                {
                    Directory.CreateDirectory(nomDirectorio);
                }

                string nomArchivo = docEntry + "_" + dateTime.ToString("yyyyMMdd") + "_" + str + ".xml";
                string rutaCompleta = Path.Combine(nomDirectorio, nomArchivo);


                SerializarYGuardarXml(transaccion, rutaCompleta);


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

        public void SendDocuementND(string token)
        {
            string path = _configuration["Files:RouteRequest"].ToString() + "\\ND";
            string pathOut = _configuration["Files:RouteRequest"].ToString() + "\\ND\\out";

            if (!Directory.Exists(pathOut))
            {
                Directory.CreateDirectory(pathOut);
            }


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
                                DocBase docBase = new DocBase();
                                if (result1.IsSuccessStatusCode)
                                {
                                    string result2 = result1.Content.ReadAsStringAsync().Result;

                                    string[] docE = fileName.Split('_');

                                    docBase.U_MGS_FE_Estado = "DS";
                                    docBase.U_MGS_FE_RespEnvio = "Envío Correcto";

                                    string json = JsonConvert.SerializeObject(docBase);
                                    infoRequest.Doc = json;
                                    infoRequest.Route = "Invoices(" + docE[0] + ")";

                                    _servicioRepository.UpdateInfo(infoRequest);
                                    _logRepository.Log("Documento ND " + docE[0] + " Se ha enviado con éxito", 1);

                                    if (File.Exists(Path.Combine(pathOut, fileName)))
                                    {
                                        File.Delete(Path.Combine(pathOut, fileName));
                                    }
                                    System.IO.File.Move(Path.Combine(path, fileName), Path.Combine(pathOut, fileName));
                                }
                                else
                                {
                                    string result3 = result1.Content.ReadAsStringAsync().Result;

                                    if (File.Exists(Path.Combine(pathOut, fileName)))
                                    {
                                        File.Delete(Path.Combine(pathOut, fileName));
                                    }
                                    System.IO.File.Move(Path.Combine(path, fileName), Path.Combine(pathOut, fileName));

                                    string[] docE = fileName.Split('_');

                                    docBase.U_MGS_FE_Estado = "DE";
                                    docBase.U_MGS_FE_RespEnvio = "Se presento error al enviar";

                                    _logRepository.Log("Documento ND " + docE[0] + " Se presento error al enviar", 1);

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

        }
    }
}
