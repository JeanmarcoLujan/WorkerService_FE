﻿// Decompiled with JetBrains decompiler
// Type: MGS_FE_Request.BusinessEntities.NC.GeneralData
// Assembly: MGS_FE_Request, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: D9FBA4F2-DB5B-4D2A-A883-40657D538457
// Assembly location: C:\Users\mark1\OneDrive - HERNANDEZ AND CO\Main global\Cliente\FE\docs\MGS\64x\MGS_FE_Request.exe

using System.Xml.Serialization;

namespace WorkerService_FE_Entities.Request.NC
{
  public class GeneralData
  {
    [XmlAttribute("Ref")]
    public string Ref { get; set; }

    [XmlAttribute("Type")]
    public string Type { get; set; }

    [XmlAttribute("Date")]
    public string Date { get; set; }

    [XmlAttribute("Currency")]
    public string Currency { get; set; }

    [XmlAttribute("TaxIncluded")]
    public bool TaxIncluded { get; set; }

    [XmlAttribute("NCF")]
    public string NCF { get; set; }

    public PublicAdministration PublicAdministration { get; set; }
  }
}