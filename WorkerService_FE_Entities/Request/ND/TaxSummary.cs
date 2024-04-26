// Decompiled with JetBrains decompiler
// Type: MGS_FE_Request.BusinessEntities.ND.TaxSummary
// Assembly: MGS_FE_Request, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: D9FBA4F2-DB5B-4D2A-A883-40657D538457
// Assembly location: C:\Users\mark1\OneDrive - HERNANDEZ AND CO\Main global\Cliente\FE\docs\MGS\64x\MGS_FE_Request.exe

using System.Xml.Serialization;

namespace WorkerService_FE_Entities.Request.ND
{
  public class TaxSummary
  {
    [XmlElement("Tax")]
    public Tax Tax { get; set; }
  }
}
