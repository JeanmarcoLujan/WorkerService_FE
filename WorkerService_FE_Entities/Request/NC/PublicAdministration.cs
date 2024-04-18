// Decompiled with JetBrains decompiler
// Type: MGS_FE_Request.BusinessEntities.NC.PublicAdministration
// Assembly: MGS_FE_Request, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: D9FBA4F2-DB5B-4D2A-A883-40657D538457
// Assembly location: C:\Users\mark1\OneDrive - HERNANDEZ AND CO\Main global\Cliente\FE\docs\MGS\64x\MGS_FE_Request.exe

using System.Xml.Serialization;

namespace WorkerService_FE_Entities.Request.NC
{
  public class PublicAdministration
  {
    [XmlAttribute("TipoIngreso")]
    public string TipoIngreso { get; set; }

    [XmlAttribute("TipoPago")]
    public string TipoPago { get; set; }

    [XmlAttribute("LinesPerPrintedPage")]
    public string LinesPerPrintedPage { get; set; }

    [XmlAttribute("CodigoModificacion")]
    public string CodigoModificacion { get; set; }

    [XmlAttribute("IndicadorNotaCredito")]
    public string IndicadorNotaCredito { get; set; }
  }
}
