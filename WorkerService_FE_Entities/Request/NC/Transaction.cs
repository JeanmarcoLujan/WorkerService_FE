// Decompiled with JetBrains decompiler
// Type: MGS_FE_Request.BusinessEntities.NC.Transaction
// Assembly: MGS_FE_Request, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: D9FBA4F2-DB5B-4D2A-A883-40657D538457
// Assembly location: C:\Users\mark1\OneDrive - HERNANDEZ AND CO\Main global\Cliente\FE\docs\MGS\64x\MGS_FE_Request.exe

using System.Xml.Serialization;

namespace WorkerService_FE_Entities.Request.NC
{
  [XmlRoot("Transaction")]
  public class Transaction
  {
    [XmlElement("GeneralData")]
    public GeneralData GeneralData { get; set; }

    [XmlElement("Supplier")]
    public Supplier Supplier { get; set; }

    [XmlElement("Client")]
    public Client Client { get; set; }

    [XmlElement("References")]
    public References References { get; set; }

    [XmlElement("ProductList")]
    public ProductList ProductList { get; set; }

    [XmlElement("TaxSummary")]
    public TaxSummary TaxSummary { get; set; }

    [XmlElement("TotalSummary")]
    public TotalSummary TotalSummary { get; set; }
  }
}
