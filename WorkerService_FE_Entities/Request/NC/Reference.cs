﻿// Decompiled with JetBrains decompiler
// Type: MGS_FE_Request.BusinessEntities.NC.Reference
// Assembly: MGS_FE_Request, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: D9FBA4F2-DB5B-4D2A-A883-40657D538457
// Assembly location: C:\Users\mark1\OneDrive - HERNANDEZ AND CO\Main global\Cliente\FE\docs\MGS\64x\MGS_FE_Request.exe

using System.Xml.Serialization;

namespace WorkerService_FE_Entities.Request.NC
{
  public class Reference
  {
    [XmlAttribute("InvoiceRef")]
    public string InvoiceRef { get; set; }

    [XmlAttribute("InvoiceNCF")]
    public string InvoiceNCF { get; set; }

    [XmlAttribute("InvoiceRefDate")]
    public string InvoiceRefDate { get; set; }

    public PublicAdministration PublicAdministration { get; set; }
  }
}