﻿// Decompiled with JetBrains decompiler
// Type: MGS_FE_Request.BusinessEntities.NC.Taxes
// Assembly: MGS_FE_Request, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: D9FBA4F2-DB5B-4D2A-A883-40657D538457
// Assembly location: C:\Users\mark1\OneDrive - HERNANDEZ AND CO\Main global\Cliente\FE\docs\MGS\64x\MGS_FE_Request.exe

using System.Collections.Generic;
using System.Xml.Serialization;

namespace WorkerService_FE_Entities.Request.NC
{
  public class Taxes
  {
    [XmlElement("Tax")]
    public List<Tax> TaxList { get; set; }
  }
}