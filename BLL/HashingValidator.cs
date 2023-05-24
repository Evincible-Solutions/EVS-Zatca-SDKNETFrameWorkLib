// Decompiled with JetBrains decompiler
// Type: SDKNETFrameWorkLib.BLL.HashingValidator
// Assembly: SDKNETFrameWorkLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7328BD70-F112-452F-B96E-929850EA6A46
// Assembly location: C:\Users\umarali\Desktop\faltu saudia ka kam\zatca-einvoicing-sdk-233-R3.1.9\zatca-einvoicing-sdk-233-R3.1.9\Lib\.Net\DLL\SDKNETFrameWorkLib.dll

using SDKNETFrameWorkLib.GeneralLogic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;

namespace SDKNETFrameWorkLib.BLL
{
  public class HashingValidator
  {
    public Result GenerateEInvoiceHashing(string xml)
    {
      Result einvoiceHashing = new Result();
      einvoiceHashing.Operation = "Generate Invoice Hashing";
      einvoiceHashing.IsValid = false;
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        try
        {
          xmlDocument.LoadXml(xml);
        }
        catch
        {
          einvoiceHashing.ErrorMessage = "Can not load XML file";
          return einvoiceHashing;
        }
        if (string.IsNullOrEmpty(xmlDocument.OuterXml))
        {
          einvoiceHashing.ErrorMessage = "Invalid invoice XML content";
          return einvoiceHashing;
        }
        string s;
        try
        {
          s = Utility.ApplyXSLT(xml, SettingsParams.Embeded_InvoiceXSLFileForHashing);
        }
        catch
        {
          einvoiceHashing.ErrorMessage = "Can not apply XSL file";
          return einvoiceHashing;
        }
        if (string.IsNullOrEmpty(s))
        {
          einvoiceHashing.ErrorMessage = "Error In applying XSL file";
          return einvoiceHashing;
        }
        using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(s)))
        {
          XmlDsigC14NTransform dsigC14Ntransform = new XmlDsigC14NTransform(false);
          dsigC14Ntransform.LoadInput((object) memoryStream);
          sbyte[] array = ((IEnumerable<byte>) Utility.Sha256_hashAsBytes(Encoding.UTF8.GetString((dsigC14Ntransform.GetOutput() as MemoryStream).ToArray()))).Select<byte, sbyte>((Func<byte, sbyte>) (x => (sbyte) x)).ToArray<sbyte>();
          einvoiceHashing.ResultedValue = Utility.ToBase64Encode((byte[])(Array)array);
          einvoiceHashing.IsValid = true;
        }
        return einvoiceHashing;
      }
      catch (Exception ex)
      {
        einvoiceHashing.ErrorMessage = ex.Message;
        return einvoiceHashing;
      }
    }

    public Result ValidateEInvoiceHashing(string xml)
    {
      Result result = new Result();
      result.Operation = "Validating Invoice Hashing";
      result.IsValid = false;
      XmlDocument doc = new XmlDocument();
      try
      {
        doc.LoadXml(xml);
      }
      catch
      {
        result.ErrorMessage = "Can not load XML file";
        return result;
      }
      string nodeInnerText = Utility.GetNodeInnerText(doc, SettingsParams.Hash_XPATH);
      if (string.IsNullOrEmpty(nodeInnerText))
      {
        result.ErrorMessage = "There is no Hashing node value in this XML file";
        return result;
      }
      Result einvoiceHashing = this.GenerateEInvoiceHashing(xml);
      if (!einvoiceHashing.IsValid)
      {
        result.ErrorMessage = einvoiceHashing.ErrorMessage;
        return result;
      }
      if (nodeInnerText != einvoiceHashing.ResultedValue)
      {
        result.ErrorMessage = "The generated Hashing is different of the one exists in the XML file.";
        return result;
      }
      result.IsValid = true;
      return result;
    }
  }
}
