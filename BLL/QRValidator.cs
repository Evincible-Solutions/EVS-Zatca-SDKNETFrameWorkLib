// Decompiled with JetBrains decompiler
// Type: SDKNETFrameWorkLib.BLL.QRValidator
// Assembly: SDKNETFrameWorkLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7328BD70-F112-452F-B96E-929850EA6A46
// Assembly location: C:\Users\umarali\Desktop\faltu saudia ka kam\zatca-einvoicing-sdk-233-R3.1.9\zatca-einvoicing-sdk-233-R3.1.9\Lib\.Net\DLL\SDKNETFrameWorkLib.dll

using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using SDKNETFrameWorkLib.GeneralLogic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace SDKNETFrameWorkLib.BLL
{
  public class QRValidator
  {
    public Result GenerateEInvoiceQRCode(string xmlFilePath)
    {
      Result einvoiceQrCode = new Result();
      einvoiceQrCode.Operation = "Generate Invoice QR";
      einvoiceQrCode.IsValid = false;
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        try
        {
          xmlDocument.Load(xmlFilePath);
        }
        catch
        {
          einvoiceQrCode.ErrorMessage = "Can not load XML file";
          return einvoiceQrCode;
        }
        if (string.IsNullOrEmpty(xmlDocument.InnerText))
        {
          einvoiceQrCode.ErrorMessage = "Invalid invoice XML content";
          return einvoiceQrCode;
        }
        string nodeInnerText1 = Utility.GetNodeInnerText(xmlDocument, SettingsParams.SELLER_NAME_XPATH);
        if (string.IsNullOrEmpty(nodeInnerText1))
        {
          einvoiceQrCode.ErrorMessage = "Unable to get SELLER_NAME value";
          return einvoiceQrCode;
        }
        string nodeInnerText2 = Utility.GetNodeInnerText(xmlDocument, SettingsParams.VAT_REGISTERATION_XPATH);
        if (string.IsNullOrEmpty(nodeInnerText2))
        {
          einvoiceQrCode.ErrorMessage = "Unable to get VAT_REGISTERATION value";
          return einvoiceQrCode;
        }
        string nodeInnerText3 = Utility.GetNodeInnerText(xmlDocument, SettingsParams.ISSUE_DATE_XPATH);
        if (string.IsNullOrEmpty(nodeInnerText3))
        {
          einvoiceQrCode.ErrorMessage = "Unable to get ISSUE_DATE value";
          return einvoiceQrCode;
        }
        string nodeInnerText4 = Utility.GetNodeInnerText(xmlDocument, SettingsParams.ISSUE_TIME_XPATH);
        if (string.IsNullOrEmpty(nodeInnerText4))
        {
          einvoiceQrCode.ErrorMessage = "Unable to get ISSUE_TIME value";
          return einvoiceQrCode;
        }
        string nodeInnerText5 = Utility.GetNodeInnerText(xmlDocument, SettingsParams.INVOICE_TOTAL_XPATH);
        if (string.IsNullOrEmpty(nodeInnerText5))
        {
          einvoiceQrCode.ErrorMessage = "Unable to get INVOICE_TOTAL value";
          return einvoiceQrCode;
        }
        string nodeInnerText6 = Utility.GetNodeInnerText(xmlDocument, SettingsParams.VAT_TOTAL_XPATH);
        if (string.IsNullOrEmpty(nodeInnerText6))
        {
          einvoiceQrCode.ErrorMessage = "Unable to get VAT_TOTAL value";
          return einvoiceQrCode;
        }
        string nodeInnerText7 = Utility.GetNodeInnerText(xmlDocument, SettingsParams.SIGNATURE_XPATH);
        if (string.IsNullOrEmpty(nodeInnerText7))
        {
          einvoiceQrCode.ErrorMessage = "Unable to get SIGNATURE value";
          return einvoiceQrCode;
        }
        string nodeInnerText8 = Utility.GetNodeInnerText(xmlDocument, SettingsParams.CERTIFICATE_XPATH);
        if (string.IsNullOrEmpty(nodeInnerText8))
        {
          einvoiceQrCode.ErrorMessage = "Unable to get CERTIFICATE value";
          return einvoiceQrCode;
        }
        DateTime result1 = new DateTime();
        string str = nodeInnerText3 + " " + nodeInnerText4;
        DateTime.TryParseExact(nodeInnerText3, SettingsParams.allDatesFormats, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result1);
        string[] strArray = nodeInnerText4.Split(':');
        int result2 = 0;
        int result3 = 0;
        int result4 = 0;
        if (!string.IsNullOrEmpty(strArray[0]) && int.TryParse(strArray[0], out result2))
          result1 = result1.AddHours((double) result2);
        if (strArray.Length > 1 && !string.IsNullOrEmpty(strArray[1]) && int.TryParse(strArray[1], out result3))
          result1 = result1.AddMinutes((double) result3);
        if (strArray.Length > 2 && !string.IsNullOrEmpty(strArray[2]) && int.TryParse(strArray[2], out result4))
          result1 = result1.AddSeconds((double) result4);
        string timeStamp = result1.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
        sbyte[] array1 = ((IEnumerable<byte>) Utility.ToBase64DecodeAsBinary(nodeInnerText8)).Select<byte, sbyte>((Func<byte, sbyte>) (x => (sbyte) x)).ToArray<sbyte>();
        if (array1 != null && array1.Length == 0)
        {
          einvoiceQrCode.ErrorMessage = "Invalid CERTIFICATE";
          return einvoiceQrCode;
        }
        Org.BouncyCastle.X509.X509Certificate x509Certificate;
        sbyte[] array2;
        try
        {
          x509Certificate = DotNetUtilities.FromX509Certificate((System.Security.Cryptography.X509Certificates.X509Certificate) new X509Certificate2((byte[])(Array)array1));
          array2 = ((IEnumerable<byte>) SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(x509Certificate.GetPublicKey()).GetEncoded()).Select<byte, sbyte>((Func<byte, sbyte>) (x => (sbyte) x)).ToArray<sbyte>();
        }
        catch
        {
          einvoiceQrCode.ErrorMessage = "Invalid CERTIFICATE";
          return einvoiceQrCode;
        }
        Result einvoiceHashing = new HashingValidator().GenerateEInvoiceHashing(xmlFilePath);
        if (!einvoiceHashing.IsValid)
        {
          einvoiceQrCode.ErrorMessage = "Problem in generating hash step - " + einvoiceHashing.ErrorMessage;
          return einvoiceQrCode;
        }
        bool isSimplified = false;
        if (Utility.GetInvoiceType(xmlDocument) == "Simplified")
          isSimplified = true;
        einvoiceQrCode.ResultedValue = this.GenerateQrCodeFromValues(nodeInnerText1, nodeInnerText2, timeStamp, nodeInnerText5, nodeInnerText6, einvoiceHashing.ResultedValue, array2, nodeInnerText7, isSimplified, ((IEnumerable<byte>) x509Certificate.GetSignature()).Select<byte, sbyte>((Func<byte, sbyte>) (x => (sbyte) x)).ToArray<sbyte>());
        einvoiceQrCode.IsValid = true;
        return einvoiceQrCode;
      }
      catch (Exception ex)
      {
        einvoiceQrCode.ErrorMessage = ex.Message;
        return einvoiceQrCode;
      }
    }

    public string GenerateQrCodeFromValues(
      string sellerName,
      string vatRegistrationNumber,
      string timeStamp,
      string invoiceTotal,
      string vatTotal,
      string hashedXml,
      sbyte[] publicKey,
      string digitalSignature,
      bool isSimplified,
      sbyte[] certificateSignature)
    {
      sbyte[] array1 = ((IEnumerable<byte>) Encoding.UTF8.GetBytes(digitalSignature)).Select<byte, sbyte>((Func<byte, sbyte>) (x => (sbyte) x)).ToArray<sbyte>();
      byte[] array2 = ((IEnumerable<byte>) Utility.WriteTlv(1U, Encoding.UTF8.GetBytes(sellerName)).ToArray()).Concat<byte>((IEnumerable<byte>) Utility.WriteTlv(2U, Encoding.UTF8.GetBytes(vatRegistrationNumber)).ToArray()).Concat<byte>((IEnumerable<byte>) Utility.WriteTlv(3U, Encoding.UTF8.GetBytes(timeStamp)).ToArray()).Concat<byte>((IEnumerable<byte>) Utility.WriteTlv(4U, Encoding.UTF8.GetBytes(invoiceTotal)).ToArray()).Concat<byte>((IEnumerable<byte>) Utility.WriteTlv(5U, Encoding.UTF8.GetBytes(vatTotal)).ToArray()).Concat<byte>((IEnumerable<byte>) Utility.WriteTlv(6U, Encoding.UTF8.GetBytes(hashedXml)).ToArray()).Concat<byte>((IEnumerable<byte>) Utility.WriteTlv(7U, (byte[])(Array)array1).ToArray()).Concat<byte>((IEnumerable<byte>) Utility.WriteTlv(8U, (byte[])(Array)publicKey).ToArray()).ToArray<byte>();
      if (isSimplified)
        array2 = ((IEnumerable<byte>) array2).Concat<byte>((IEnumerable<byte>) Utility.WriteTlv(9U, (byte[])(Array)certificateSignature).ToArray()).ToArray<byte>();
      return Utility.ToBase64Encode(array2);
    }

    public Result ValidateEInvoiceQRCode(string xmlFilePath)
    {
      Result result = new Result();
      result.Operation = "Validating QR Code";
      result.IsValid = false;
      XmlDocument doc = new XmlDocument();
      try
      {
        doc.Load(xmlFilePath);
      }
      catch
      {
        result.ErrorMessage = "Can not load XML file";
        return result;
      }
      string nodeInnerText = Utility.GetNodeInnerText(doc, SettingsParams.QR_CODE_XPATH);
      if (string.IsNullOrEmpty(nodeInnerText))
      {
        result.ErrorMessage = "There is no QR node value in this XML file";
        return result;
      }
      Result einvoiceQrCode = this.GenerateEInvoiceQRCode(xmlFilePath);
      if (!einvoiceQrCode.IsValid)
      {
        result.ErrorMessage = einvoiceQrCode.ErrorMessage;
        return result;
      }
      if (nodeInnerText != einvoiceQrCode.ResultedValue)
      {
        result.ErrorMessage = "The generated QR code is different of the one exists in the XML file.";
        return result;
      }
      result.IsValid = true;
      return result;
    }

    private byte[] FromHexString(string hex)
    {
      int length = hex.Length;
      byte[] numArray = new byte[length / 2];
      for (int startIndex = 0; startIndex < length; startIndex += 2)
        numArray[startIndex / 2] = Convert.ToByte(hex.Substring(startIndex, 2), 16);
      return numArray;
    }
  }
}
