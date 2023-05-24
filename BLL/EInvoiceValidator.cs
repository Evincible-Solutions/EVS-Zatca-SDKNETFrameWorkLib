// Decompiled with JetBrains decompiler
// Type: SDKNETFrameWorkLib.BLL.EInvoiceValidator
// Assembly: SDKNETFrameWorkLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7328BD70-F112-452F-B96E-929850EA6A46
// Assembly location: C:\Users\umarali\Desktop\faltu saudia ka kam\zatca-einvoicing-sdk-233-R3.1.9\zatca-einvoicing-sdk-233-R3.1.9\Lib\.Net\DLL\SDKNETFrameWorkLib.dll

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Saxon.Api;
using SDKNETFrameWorkLib.GeneralLogic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace SDKNETFrameWorkLib.BLL
{
  public class EInvoiceValidator
  {
    public Result ValidateEInvoice(
      string xmlFilePath,
      string certificateContent,
      string pihContent)
    {
      Result result1 = new Result();
      result1.Operation = "Validating E-Invoice";
      result1.IsValid = false;
      try
      {
        if (string.IsNullOrEmpty(certificateContent))
        {
          result1.ErrorMessage = "Invalid certificate content.";
          return result1;
        }
        if (string.IsNullOrEmpty(pihContent))
        {
          result1.ErrorMessage = "Invalid PIH file content.";
          return result1;
        }
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.PreserveWhitespace = true;
        try
        {
          xmlDoc.Load(xmlFilePath);
        }
        catch
        {
          result1.ErrorMessage = "Can not load XML file";
          return result1;
        }
        if (string.IsNullOrEmpty(xmlDoc.InnerText))
        {
          result1.ErrorMessage = "Invalid invoice XML content";
          return result1;
        }
        result1.lstSteps = new List<Result>();
        Result result2 = new Result();
        result2.Operation = "First Step : XSD Validation";
        result2.IsValid = this.ValidateXSD(xmlFilePath);
        if (!result2.IsValid)
        {
          result2.ErrorMessage = "Schema validation failed; XML does not comply with UBL 2.1 standards";
          result1.lstSteps.Add(result2);
          return result1;
        }
        result1.lstSteps.Add(result2);
        string nodeErrors1 = "";
        Result result3 = new Result();
        result3.Operation = "Second Step : EN Schematrons";
        result3.IsValid = this.ValidateSchematrons(xmlFilePath, SettingsParams.Embeded_EN_Schematrons_PATH, ref nodeErrors1);
        if (!result3.IsValid)
        {
          result3.ErrorMessage = nodeErrors1;
          result1.lstSteps.Add(result3);
          return result1;
        }
        result1.lstSteps.Add(result3);
        string nodeErrors2 = "";
        Result result4 = new Result();
        result4.Operation = "Third Step : KSA Schematrons";
        result4.IsValid = this.ValidateSchematrons(xmlFilePath, SettingsParams.Embeded_KSA_Schematrons_PATH, ref nodeErrors2);
        if (!result4.IsValid)
        {
          result4.ErrorMessage = nodeErrors2;
          result1.lstSteps.Add(result4);
          return result1;
        }
        result1.lstSteps.Add(result4);
        string errorMessage;
        if (Utility.GetInvoiceType(xmlDoc) == "Simplified")
        {
          result1.Operation += " : ( Simplified )";
          Result result5 = new Result();
          Result result6 = new QRValidator().ValidateEInvoiceQRCode(xmlFilePath);
          result6.Operation = "Forth Step : QR Validation";
          if (!result6.IsValid)
          {
            result1.lstSteps.Add(result6);
            return result1;
          }
          result1.lstSteps.Add(result6);
          errorMessage = "";
          Result result7 = new Result();
          result7.IsValid = this.ValidateSignature(xmlDoc, ref errorMessage, xmlFilePath, certificateContent);
          result7.Operation = "Fifth Step : Signature Validation";
          if (!result7.IsValid)
          {
            result7.ErrorMessage = errorMessage;
            result1.lstSteps.Add(result7);
            return result1;
          }
          result1.lstSteps.Add(result7);
        }
        else
          result1.Operation += " : ( Standard )";
        errorMessage = "";
        Result result8 = new Result();
        result8.IsValid = this.ValidatePIH(xmlDoc, ref errorMessage, pihContent);
        result8.Operation = "Sixth Step : PIH Validation";
        if (!result8.IsValid)
        {
          result8.ErrorMessage = errorMessage;
          result1.lstSteps.Add(result8);
          return result1;
        }
        result1.lstSteps.Add(result8);
        result1.IsValid = true;
        return result1;
      }
      catch (Exception ex)
      {
        result1.ErrorMessage = ex.Message;
        return result1;
      }
    }

    private bool ValidateXSD(string xmlFilePath)
    {
      try
      {
        XmlReaderSettings settings = new XmlReaderSettings();
        XmlReader schemaDocument1 = XmlReader.Create((Stream) new FileStream(SettingsParams.Embeded_UBL_XSD_PATH, FileMode.Open, FileAccess.Read));
        settings.Schemas.Add("urn:oasis:names:specification:ubl:schema:xsd:Invoice-2", schemaDocument1);
        XmlReader schemaDocument2 = XmlReader.Create((Stream) new FileStream(SettingsParams.Embeded_CommonExtensionComponents_XSD_PATH, FileMode.Open, FileAccess.Read));
        settings.Schemas.Add("urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2", schemaDocument2);
        XmlReader schemaDocument3 = XmlReader.Create((Stream) new FileStream(SettingsParams.Embeded_CommonBasicComponents_XSD_PATH, FileMode.Open, FileAccess.Read));
        settings.Schemas.Add("urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2", schemaDocument3);
        XmlReader schemaDocument4 = XmlReader.Create((Stream) new FileStream(SettingsParams.Embeded_CommonAggregateComponents_XSD_PATH, FileMode.Open, FileAccess.Read));
        settings.Schemas.Add("urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", schemaDocument4);
        XmlReader schemaDocument5 = XmlReader.Create((Stream) new FileStream(SettingsParams.Embeded_CommonSignatureComponents_XSD_PATH, FileMode.Open, FileAccess.Read));
        settings.Schemas.Add("urn:oasis:names:specification:ubl:schema:xsd:CommonSignatureComponents-2", schemaDocument5);
        XmlReader schemaDocument6 = XmlReader.Create((Stream) new FileStream(SettingsParams.Embeded_SignatureAggregateComponents_XSD_PATH, FileMode.Open, FileAccess.Read));
        settings.Schemas.Add("urn:oasis:names:specification:ubl:schema:xsd:SignatureAggregateComponents-2", schemaDocument6);
        XmlReader schemaDocument7 = XmlReader.Create((Stream) new FileStream(SettingsParams.Embeded_SignatureBasicComponents_XSD_PATH, FileMode.Open, FileAccess.Read));
        settings.Schemas.Add("urn:oasis:names:specification:ubl:schema:xsd:SignatureBasicComponents-2", schemaDocument7);
        XmlReader schemaDocument8 = XmlReader.Create((Stream) new FileStream(SettingsParams.Embeded_UBL_UnqualifiedDataTypes_XSD_PATH, FileMode.Open, FileAccess.Read));
        settings.Schemas.Add("urn:oasis:names:specification:ubl:schema:xsd:UnqualifiedDataTypes-2", schemaDocument8);
        XmlReader schemaDocument9 = XmlReader.Create((Stream) new FileStream(SettingsParams.Embeded_CoreComponentTypeSchemaModule_XSD_PATH, FileMode.Open, FileAccess.Read));
        settings.Schemas.Add("urn:un:unece:uncefact:documentation:2", schemaDocument9);
        XmlReader schemaDocument10 = XmlReader.Create((Stream) new FileStream(SettingsParams.Embeded_ExtensionContentDataType_XSD_PATH, FileMode.Open, FileAccess.Read));
        settings.Schemas.Add("urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2", schemaDocument10);
        XmlReader schemaDocument11 = XmlReader.Create((Stream) new FileStream(SettingsParams.Embeded_QualifiedDataTypes_XSD_PATH, FileMode.Open, FileAccess.Read));
        settings.Schemas.Add("urn:oasis:names:specification:ubl:schema:xsd:QualifiedDataTypes-2", schemaDocument11);
        XmlReader schemaDocument12 = XmlReader.Create((Stream) new FileStream(SettingsParams.Embeded_CCTS_CCT_SchemaModule_XSD_PATH, FileMode.Open, FileAccess.Read));
        settings.Schemas.Add("urn:un:unece:uncefact:data:specification:CoreComponentTypeSchemaModule:2", schemaDocument12);
        XmlReader schemaDocument13 = XmlReader.Create((Stream) new FileStream(SettingsParams.Embeded_UBL_XAdESv_XSD_PATH, FileMode.Open, FileAccess.Read));
        settings.Schemas.Add("http://uri.etsi.org/01903/v1.4.1#", schemaDocument13);
        XmlReader schemaDocument14 = XmlReader.Create((Stream) new FileStream(SettingsParams.Embeded_UBL_Xmldsig_Core_XSD_PATH, FileMode.Open, FileAccess.Read));
        settings.Schemas.Add("http://www.w3.org/2000/09/xmldsig#", schemaDocument14);
        settings.DtdProcessing = DtdProcessing.Parse;
        settings.ValidationType = ValidationType.DTD;
        settings.ValidationEventHandler += new ValidationEventHandler(EInvoiceValidator.DocumentValidationHandler);
        XmlReader xmlReader = XmlReader.Create(xmlFilePath, settings);
        do
          ;
        while (xmlReader.Read());
        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return false;
      }
    }

    private static void DocumentValidationHandler(object sender, ValidationEventArgs e)
    {
      if (e.Severity == XmlSeverityType.Error)
        throw new Exception(e.Message);
    }

    private bool ValidateSchematrons(string xmlPath, string xslPath, ref string nodeErrors)
    {
      try
      {
        Uri uri = new Uri(xmlPath);
        XmlDocument source = new XmlDocument();
        source.Load(xmlPath);
        Processor processor = new Processor();
        XsltCompiler xsltCompiler = processor.NewXsltCompiler();
        XdmNode xdmNode = processor.NewDocumentBuilder().Build((XmlNode) source);
        FileStream input = new FileStream(xslPath, FileMode.Open, FileAccess.Read);
        XsltTransformer xsltTransformer = xsltCompiler.Compile((Stream) input).Load();
        xsltTransformer.InitialContextNode = xdmNode;
        XdmDestination destination = new XdmDestination();
        MemoryStream memoryStream = new MemoryStream();
        source.Save((Stream) memoryStream);
        memoryStream.Flush();
        memoryStream.Position = 0L;
        xsltTransformer.SetInputStream((Stream) memoryStream, new Uri(Path.GetTempPath()));
        xsltTransformer.Run((XmlDestination) destination);
        StreamWriter streamWriter = new StreamWriter((Stream) new MemoryStream());
        foreach (XdmNode child1 in destination.XdmNode.Children())
        {
          foreach (XdmNode child2 in child1.Children())
          {
            if (child2.NodeName != null && "failed-assert".Equals(child2.NodeName.LocalName))
              nodeErrors = nodeErrors + child2.GetAttributeValue("id") + ":" + child2.StringValue + " - ";
          }
        }
        if (!string.IsNullOrEmpty(nodeErrors))
          return false;
        try
        {
          input.Close();
          input.Dispose();
        }
        catch
        {
        }
        return true;
      }
      catch (Exception ex)
      {
        nodeErrors = nodeErrors + ex.GetType().ToString() + " : " + ex.Message;
        return false;
      }
    }

    private bool ValidatePIH(XmlDocument xmlDoc, ref string errorMessage, string newPIH)
    {
      try
      {
        if (string.IsNullOrEmpty(newPIH))
        {
          errorMessage = "Empty PIH file.";
          return false;
        }
        string nodeInnerText = Utility.GetNodeInnerText(xmlDoc, SettingsParams.PIH_XPATH);
        if (string.IsNullOrEmpty(nodeInnerText))
        {
          errorMessage = "Empty PIH node value.";
          return false;
        }
        if (!(newPIH != nodeInnerText))
          return true;
        errorMessage = "PIH is inValid";
        return false;
      }
      catch (IOException ex)
      {
        errorMessage = "PIH file not found.";
        return false;
      }
      catch (Exception ex)
      {
        errorMessage = "Error Occurred in validating PIH.";
        return false;
      }
    }

    private bool ValidateSignature(
      XmlDocument xmlDoc,
      ref string errorMessage,
      string xmlFilePath,
      string certificateContent)
    {
      try
      {
        string str1 = Utility.GetNodeInnerText(xmlDoc, SettingsParams.CERTIFICATE_XPATH).Trim();
        if (string.IsNullOrEmpty(str1))
        {
          errorMessage = "Unable to get CERTIFICATE value from E-invoice XML";
          return false;
        }
        sbyte[] array1 = ((IEnumerable<byte>) Utility.ToBase64DecodeAsBinary(str1)).Select<byte, sbyte>((Func<byte, sbyte>) (x => (sbyte) x)).ToArray<sbyte>();
        if (array1 != null && array1.Length == 0)
        {
          errorMessage = "Invalid CERTIFICATE";
          return false;
        }
        HashingValidator hashingValidator = new HashingValidator();
        Result einvoiceHashing = hashingValidator.GenerateEInvoiceHashing(xmlFilePath);
        if (!einvoiceHashing.IsValid)
        {
          errorMessage = "Invalid Hashing Generation";
          return false;
        }
        Result result = hashingValidator.ValidateEInvoiceHashing(xmlFilePath);
        if (!result.IsValid)
        {
          errorMessage = result.ErrorMessage;
          return false;
        }
        sbyte[] array2;
        try
        {
          array2 = ((IEnumerable<byte>) SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(DotNetUtilities.FromX509Certificate((System.Security.Cryptography.X509Certificates.X509Certificate) new X509Certificate2((byte[])(Array)array1)).GetPublicKey()).GetEncoded()).Select<byte, sbyte>((Func<byte, sbyte>) (x => (sbyte) x)).ToArray<sbyte>();
        }
        catch
        {
          errorMessage = "Invalid CERTIFICATE";
          return false;
        }
        string nodeInnerText1 = Utility.GetNodeInnerText(xmlDoc, SettingsParams.SIGNATURE_XPATH);
        if (string.IsNullOrEmpty(nodeInnerText1))
        {
          errorMessage = "Unable to get Signature value in E-invoice XML.";
          return false;
        }
        sbyte[] array3 = ((IEnumerable<byte>) Utility.ToBase64DecodeAsBinary(einvoiceHashing.ResultedValue)).Select<byte, sbyte>((Func<byte, sbyte>) (x => (sbyte) x)).ToArray<sbyte>();
        AsymmetricKeyParameter key = PublicKeyFactory.CreateKey((byte[])(Array)array2);
        ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");
        signer.Init(false, (ICipherParameters) key);
        signer.BlockUpdate((byte[])(Array)array3, 0, array3.Length);
        sbyte[] array4 = ((IEnumerable<byte>) Convert.FromBase64String(nodeInnerText1)).Select<byte, sbyte>((Func<byte, sbyte>) (x => (sbyte) x)).ToArray<sbyte>();
        bool flag = signer.VerifySignature((byte[])(Array)array4);
        if (!flag)
        {
          errorMessage = "Wrong signature value.";
          return false;
        }
        string str2 = Utility.GetNodeInnerXML(xmlDoc, SettingsParams.SIGNED_PROPERTIES_XPATH).Replace(" />", "/>").Replace("></ds:DigestMethod>", "/>");
        string nodeInnerText2 = Utility.GetNodeInnerText(xmlDoc, SettingsParams.SIGNED_Properities_DIGEST_VALUE_XPATH);
        if (Utility.ToBase64Encode(Utility.Sha256_hashAsString(str2.Replace("\r", ""))) != nodeInnerText2)
        {
          errorMessage = "Wrong signing properties digest value.";
          return false;
        }
        if (Utility.ToBase64Encode(Utility.Sha256_hashAsString(str1)) != Utility.GetNodeInnerText(xmlDoc, SettingsParams.SIGNED_Certificate_DIGEST_VALUE_XPATH))
        {
          errorMessage = "Wrong signing certificate digest value.";
          return false;
        }
        X509Certificate2 x509Certificate2 = new X509Certificate2((byte[])(Array)((IEnumerable<byte>) Encoding.UTF8.GetBytes(certificateContent)).Select<byte, sbyte>((Func<byte, sbyte>) (x => (sbyte) x)).ToArray<sbyte>());
        if (Utility.GetNodeInnerText(xmlDoc, SettingsParams.X509_SERIAL_NUMBER_XPATH) != new BigInteger((byte[])(Array)((IEnumerable<byte>) x509Certificate2.GetSerialNumber()).Select<byte, sbyte>((Func<byte, sbyte>) (x => (sbyte) x)).ToArray<sbyte>()).ToString())
        {
          errorMessage = "Invalid certificate serial number.";
          return false;
        }
        if (!(Utility.GetNodeInnerText(xmlDoc, SettingsParams.ISSUER_NAME_XPATH) != x509Certificate2.IssuerName.Name))
          return flag;
        errorMessage = "Invalid certificate issuer name.";
        return false;
      }
      catch (Exception ex)
      {
        errorMessage = "Error occurred in validating signature.";
        return false;
      }
    }
  }
}
