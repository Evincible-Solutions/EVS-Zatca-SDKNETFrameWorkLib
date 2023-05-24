// Decompiled with JetBrains decompiler
// Type: SDKNETFrameWorkLib.GeneralLogic.Result
// Assembly: SDKNETFrameWorkLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7328BD70-F112-452F-B96E-929850EA6A46
// Assembly location: C:\Users\umarali\Desktop\faltu saudia ka kam\zatca-einvoicing-sdk-233-R3.1.9\zatca-einvoicing-sdk-233-R3.1.9\Lib\.Net\DLL\SDKNETFrameWorkLib.dll

using System.Collections.Generic;

namespace SDKNETFrameWorkLib.GeneralLogic
{
  public class Result
  {
    public string Operation { get; set; }

    public bool IsValid { get; set; }

    public string ErrorMessage { get; set; }

    public string ResultedValue { get; set; }

    public List<Result> lstSteps { get; set; }

    public string SingedXML { get; set; }
  }
}
