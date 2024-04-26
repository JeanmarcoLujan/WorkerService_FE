// Decompiled with JetBrains decompiler
// Type: MGS_FE_Request.BusinessEntities.ND.Product
// Assembly: MGS_FE_Request, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: D9FBA4F2-DB5B-4D2A-A883-40657D538457
// Assembly location: C:\Users\mark1\OneDrive - HERNANDEZ AND CO\Main global\Cliente\FE\docs\MGS\64x\MGS_FE_Request.exe

using System;
using System.Xml.Serialization;

namespace WorkerService_FE_Entities.Request.ND
{
  public class Product
  {
    [XmlAttribute("SupplierSKU")]
    public string SupplierSKU { get; set; }

    [XmlAttribute("Item")]
    public string Item { get; set; }

    [XmlAttribute("Qty")]
    public Decimal Qty { get; set; }

    [XmlAttribute("MU")]
    public string MU { get; set; }

    [XmlAttribute("CU")]
    public int CU { get; set; }

    [XmlAttribute("UP")]
    public Decimal UP { get; set; }

    [XmlAttribute("Total")]
    public Decimal Total { get; set; }

    [XmlAttribute("NetAmount")]
    public Decimal NetAmount { get; set; }

    [XmlAttribute("SysLineType")]
    public string SysLineType { get; set; }

    public Taxes Taxes { get; set; }
  }
}
