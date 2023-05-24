// Decompiled with JetBrains decompiler
// Type: SDKNETFrameWorkLib.BLL.EInvoiceSigningLogic
// Assembly: SDKNETFrameWorkLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7328BD70-F112-452F-B96E-929850EA6A46
// Assembly location: C:\Users\umarali\Desktop\faltu saudia ka kam\zatca-einvoicing-sdk-233-R3.1.9\zatca-einvoicing-sdk-233-R3.1.9\Lib\.Net\DLL\SDKNETFrameWorkLib.dll

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using SDKNETFrameWorkLib.GeneralLogic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace SDKNETFrameWorkLib.BLL
{
    public class EInvoiceSigningLogic
    {
        public Result SignDocument(
          string xml,
          string certificateContent,
          string privateKeyContent)
        {
            Result result1 = new Result();
            result1.Operation = "Signing E-Invoice Operation";
            result1.IsValid = false;
            try
            {
                if (string.IsNullOrEmpty(certificateContent))
                {
                    result1.ErrorMessage = "Invalid certificate content.";
                    return result1;
                }
                if (string.IsNullOrEmpty(privateKeyContent))
                {
                    result1.ErrorMessage = "Invalid private key content.";
                    return result1;
                }
                XmlDocument xmlDocument1 = new XmlDocument();
                xmlDocument1.PreserveWhitespace = true;
                try
                {
                    xmlDocument1.LoadXml(xml);
                }
                catch
                {
                    result1.ErrorMessage = "Can not load XML file";
                    return result1;
                }
                if (string.IsNullOrEmpty(xmlDocument1.InnerText))
                {
                    result1.ErrorMessage = "Invalid invoice XML content";
                    return result1;
                }
                result1.lstSteps = new List<Result>();
                Result result2 = new Result();
                Result einvoiceHashing = new HashingValidator().GenerateEInvoiceHashing(xml);
                einvoiceHashing.Operation = "First Step : Generating Hashing";
                if (!einvoiceHashing.IsValid)
                {
                    result1.lstSteps.Add(einvoiceHashing);
                    return result1;
                }
                result1.lstSteps.Add(einvoiceHashing);
                Result result3 = new Result();
                Result digitalSignature = this.GetDigitalSignature(einvoiceHashing.ResultedValue, privateKeyContent);
                digitalSignature.Operation = "Second Step : Generating Digital Signature";
                if (!digitalSignature.IsValid)
                {
                    result1.lstSteps.Add(digitalSignature);
                    return result1;
                }
                result1.lstSteps.Add(digitalSignature);
                Result result4 = new Result();
                result4.IsValid = false;
                result4.Operation = "Third Step : Generating Certificate";
                X509Certificate2 x509Cert = new X509Certificate2((byte[])(Array)((IEnumerable<byte>)Encoding.UTF8.GetBytes(certificateContent)).Select<byte, sbyte>((Func<byte, sbyte>)(x => (sbyte)x)).ToArray<sbyte>());
                Org.BouncyCastle.X509.X509Certificate x509Certificate = DotNetUtilities.FromX509Certificate((System.Security.Cryptography.X509Certificates.X509Certificate)x509Cert);
                sbyte[] array = ((IEnumerable<byte>)SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(x509Certificate.GetPublicKey()).GetEncoded()).Select<byte, sbyte>((Func<byte, sbyte>)(x => (sbyte)x)).ToArray<sbyte>();
                BigInteger bigInteger = new BigInteger((byte[])(Array)((IEnumerable<byte>)x509Cert.GetSerialNumber()).Select<byte, sbyte>((Func<byte, sbyte>)(x => (sbyte)x)).ToArray<sbyte>());
                if (x509Cert != null)
                {
                    result4.IsValid = true;
                    result1.lstSteps.Add(result4);
                    //result1.lstSteps.Add(result4);
                    Result result5 = new Result();
                    result5.IsValid = false;
                    result5.Operation = "Forth Step : Generating Certificate Hashing";
                    Result result6 = new Result();
                    try
                    {
                        result5.IsValid = true;
                        result5.ResultedValue = Utility.ToBase64Encode(Utility.Sha256_hashAsString(certificateContent));
                        result1.lstSteps.Add(result5);
                    }
                    catch (Exception ex)
                    {
                        result6.ErrorMessage = ex.Message;
                        result1.lstSteps.Add(result5);
                        return result1;
                    }
                    new Result().IsValid = false;
                    Result result7 = this.TransformXML(xmlDocument1.OuterXml);
                    result7.Operation = "Fifth Step : Transform Xml Result";
                    if (!result7.IsValid)
                    {
                        result1.lstSteps.Add(result7);
                        return result1;
                    }
                    result1.lstSteps.Add(result7);
                    XmlDocument xmlDocument2 = new XmlDocument();
                    xmlDocument2.PreserveWhitespace = true;
                    xmlDocument2.LoadXml(result7.ResultedValue);
                    Dictionary<string, string> nameSpacesMap = this.getNameSpacesMap();
                    Result result8 = new Result();
                    Result result9 = this.PopulateSignedSignatureProperties(xmlDocument2, nameSpacesMap, result5.ResultedValue, this.GetCurrentTimestamp(), x509Cert.IssuerName.Name, bigInteger.ToString());
                    result9.Operation = "Sixth Step : Populate Signed Signature Properties";
                    if (!result9.IsValid)
                    {
                        result1.lstSteps.Add(result9);
                        return result1;
                    }
                    result1.lstSteps.Add(result9);
                    Result result10 = new Result();
                    Result result11 = this.PopulateUBLExtensions(xmlDocument2, digitalSignature.ResultedValue, result9.ResultedValue, einvoiceHashing.ResultedValue, certificateContent);
                    result11.Operation = "Seventh Step : Populate Populate UBL Extensions";
                    if (!result11.IsValid)
                    {
                        result1.lstSteps.Add(result11);
                        return result1;
                    }
                    result1.lstSteps.Add(result11);
                    Result result12 = new Result();
                    Result result13 = this.PopulateQRCode(xmlDocument2, array, digitalSignature.ResultedValue, einvoiceHashing.ResultedValue, ((IEnumerable<byte>)x509Certificate.GetSignature()).Select<byte, sbyte>((Func<byte, sbyte>)(x => (sbyte)x)).ToArray<sbyte>());
                    result13.Operation = "Eighth Step : Populate QR";
                    if (!result13.IsValid)
                    {
                        result1.lstSteps.Add(result13);
                        return result1;
                    }
                    result1.lstSteps.Add(result13);
                    result1.IsValid = true;
                    result1.ResultedValue = xmlDocument2.OuterXml;
                    //foreach (Result lstStep in result1.lstSteps)
                        //lstStep.ResultedValue = "";
                    //try
                    //{
                    //  xmlDocument2.Save("NewSigned.xml");
                    //}
                    //catch
                    //{
                    //}
                    return result1;
                }
                result4.ErrorMessage = "Invalid Certificate";
                result1.lstSteps.Add(result4);
                return result1;
            }
            catch (Exception ex)
            {
                result1.ErrorMessage = ex.Message;
                return result1;
            }
        }

        private Result PopulateUBLExtensions(
          XmlDocument xmlDoc,
          string digitalSignature,
          string signedPropertiesHashing,
          string xmlHashing,
          string certificate)
        {
            Result result = new Result();
            try
            {
                Utility.SetNodeValue(xmlDoc, SettingsParams.SIGNATURE_XPATH, digitalSignature);
                Utility.SetNodeValue(xmlDoc, SettingsParams.CERTIFICATE_XPATH, certificate);
                Utility.SetNodeValue(xmlDoc, SettingsParams.SIGNED_Properities_DIGEST_VALUE_XPATH, signedPropertiesHashing);
                Utility.SetNodeValue(xmlDoc, SettingsParams.Hash_XPATH, xmlHashing);
                result.IsValid = true;
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        private Result PopulateSignedSignatureProperties(
          XmlDocument document,
          Dictionary<string, string> nameSpacesMap,
          string publicKeyHashing,
          string signatureTimestamp,
          string x509IssuerName,
          string serialNumber)
        {
            Result result = new Result();
            try
            {
                Utility.SetNodeValue(document, SettingsParams.PUBLIC_KEY_HASHING_XPATH, publicKeyHashing);
                Utility.SetNodeValue(document, SettingsParams.SIGNING_TIME_XPATH, signatureTimestamp);
                Utility.SetNodeValue(document, SettingsParams.ISSUER_NAME_XPATH, x509IssuerName);
                Utility.SetNodeValue(document, SettingsParams.X509_SERIAL_NUMBER_XPATH, serialNumber);
                string str = Utility.GetNodeInnerXML(document, SettingsParams.SIGNED_PROPERTIES_XPATH).Replace(" />", "/>").Replace("></ds:DigestMethod>", "/>");
                Utility.Sha256_hashAsString(str.Replace("\r", ""));
                result.ResultedValue = Utility.ToBase64Encode(Utility.Sha256_hashAsString(str.Replace("\r", "")));
                result.IsValid = true;
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        private string GetCurrentTimestamp() => DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");

        private Dictionary<string, string> getNameSpacesMap() => new Dictionary<string, string>()
    {
      {
        "cac",
        "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2"
      },
      {
        "cbc",
        "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2"
      },
      {
        "ext",
        "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2"
      },
      {
        "sig",
        "urn:oasis:names:specification:ubl:schema:xsd:CommonSignatureComponents-2"
      },
      {
        "sac",
        "urn:oasis:names:specification:ubl:schema:xsd:SignatureAggregateComponents-2"
      },
      {
        "sbc",
        "urn:oasis:names:specification:ubl:schema:xsd:SignatureBasicComponents-2"
      },
      {
        "ds",
        "http://www.w3.org/2000/09/xmldsig#"
      },
      {
        "xades",
        "http://uri.etsi.org/01903/v1.3.2#"
      }
    };

        public Result GetDigitalSignature(string xmlHashing, string privateKeyContent)
        {
            Result digitalSignature = new Result();
            try
            {
                sbyte[] array = ((IEnumerable<byte>)Utility.ToBase64DecodeAsBinary(xmlHashing)).Select<byte, sbyte>((Func<byte, sbyte>)(x => (sbyte)x)).ToArray<sbyte>();
                privateKeyContent = privateKeyContent.Replace("\n", "").Replace("\t", "");
                if (!privateKeyContent.Contains("-----BEGIN EC PRIVATE KEY-----") && !privateKeyContent.Contains("-----END EC PRIVATE KEY-----"))
                    privateKeyContent = "-----BEGIN EC PRIVATE KEY-----\n" + privateKeyContent + "\n-----END EC PRIVATE KEY-----\n";
                byte[] signature;
                using (TextReader reader = (TextReader)new StringReader(privateKeyContent))
                {
                    AsymmetricKeyParameter parameters = ((AsymmetricCipherKeyPair)new PemReader(reader).ReadObject()).Private;
                    ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");
                    signer.Init(true, (ICipherParameters)parameters);
                    signer.BlockUpdate((byte[])(Array)array, 0, array.Length);
                    signature = signer.GenerateSignature();
                }
                digitalSignature.IsValid = true;
                digitalSignature.ResultedValue = Convert.ToBase64String(signature);
            }
            catch (Exception ex)
            {
                digitalSignature.IsValid = false;
            }
            return digitalSignature;
        }

        public Result GetDigitalSignature2(string xmlHashing, string privateKeyContent)
        {
            Result digitalSignature2 = new Result();
            try
            {
                sbyte[] array = ((IEnumerable<byte>)Utility.ToBase64DecodeAsBinary(xmlHashing)).Select<byte, sbyte>((Func<byte, sbyte>)(x => (sbyte)x)).ToArray<sbyte>();
                privateKeyContent = privateKeyContent.Replace("\n", "").Replace("\t", "");
                if (!privateKeyContent.Contains("-----BEGIN PRIVATE KEY-----") && !privateKeyContent.Contains("-----END PRIVATE KEY-----"))
                    privateKeyContent = "-----BEGIN PRIVATE KEY-----\n" + privateKeyContent + "\n-----END PRIVATE KEY-----\n";
                byte[] signature;
                using (TextReader reader = (TextReader)new StringReader(privateKeyContent))
                {
                    ECPrivateKeyParameters parameters = (ECPrivateKeyParameters)new PemReader(reader).ReadObject();
                    ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");
                    signer.Init(true, (ICipherParameters)parameters);
                    signer.BlockUpdate((byte[])(Array)array, 0, array.Length);
                    signature = signer.GenerateSignature();
                }
                digitalSignature2.IsValid = true;
                digitalSignature2.ResultedValue = Convert.ToBase64String(signature);
            }
            catch (Exception ex)
            {
                digitalSignature2.IsValid = false;
            }
            return digitalSignature2;
        }

        public Result TransformXML(string xmlContent)
        {
            string xml = "";
            Result result = new Result();
            result.IsValid = false;
            try
            {
                xml = Utility.ApplyXSLTPassingXML(xmlContent, SettingsParams.Embeded_Remove_Elements_PATH);
            }
            catch (Exception ex)
            {
                result.ErrorMessage = "Error in removing elements.";
            }
            try
            {
                xml = Utility.ApplyXSLTPassingXML(xml, SettingsParams.Embeded_Add_UBL_Element_PATH);
            }
            catch (Exception ex)
            {
                result.ErrorMessage = "Error in adding UBL elements.";
            }
            try
            {
                xml = xml.Replace("UBL-TO-BE-REPLACED", new StreamReader(Utility.ReadInternalEmbededResourceStream(SettingsParams.Embeded_UBL_File_PATH)).ReadToEnd());
            }
            catch (Exception ex)
            {
                result.ErrorMessage = "Error in replacing UBL elements.";
            }
            try
            {
                xml = Utility.ApplyXSLTPassingXML(xml, SettingsParams.Embeded_Add_QR_Element_PATH);
            }
            catch (Exception ex)
            {
                result.ErrorMessage = "Error in adding QR elements.";
            }
            try
            {
                xml = xml.Replace("QR-TO-BE-REPLACED", new StreamReader(Utility.ReadInternalEmbededResourceStream(SettingsParams.Embeded_QR_XML_File_PATH)).ReadToEnd());
            }
            catch (Exception ex)
            {
                result.ErrorMessage = "Error in replacing QR elements.";
            }
            try
            {
                xml = Utility.ApplyXSLTPassingXML(xml, SettingsParams.Embeded_Add_Signature_Element_PATH);
            }
            catch (Exception ex)
            {
                result.ErrorMessage = "Error in adding signature elements.";
            }
            try
            {
                xml = xml.Replace("SIGN-TO-BE-REPLACED", new StreamReader(Utility.ReadInternalEmbededResourceStream(SettingsParams.Embeded_Signature_File_PATH)).ReadToEnd());
            }
            catch (Exception ex)
            {
                result.ErrorMessage = "Error in replacing signature elements.";
            }
            if (xml != null)
            {
                result.ResultedValue = xml;
                result.IsValid = true;
            }
            return result;
        }

        private Result PopulateQRCode(
          XmlDocument xmlDoc,
          sbyte[] publicKeyArr,
          string signature,
          string hashedXml,
          sbyte[] certificateSignatureBytes)
        {
            Result result1 = new Result();
            result1.IsValid = false;
            string nodeInnerText1 = Utility.GetNodeInnerText(xmlDoc, SettingsParams.SELLER_NAME_XPATH);
            if (string.IsNullOrEmpty(nodeInnerText1))
            {
                result1.ErrorMessage = "Unable to get SELLER_NAME value";
                return result1;
            }
            string nodeInnerText2 = Utility.GetNodeInnerText(xmlDoc, SettingsParams.VAT_REGISTERATION_XPATH);
            if (string.IsNullOrEmpty(nodeInnerText2))
            {
                result1.ErrorMessage = "Unable to get VAT_REGISTERATION value";
                return result1;
            }
            string nodeInnerText3 = Utility.GetNodeInnerText(xmlDoc, SettingsParams.ISSUE_DATE_XPATH);
            if (string.IsNullOrEmpty(nodeInnerText3))
            {
                result1.ErrorMessage = "Unable to get ISSUE_DATE value";
                return result1;
            }
            string nodeInnerText4 = Utility.GetNodeInnerText(xmlDoc, SettingsParams.ISSUE_TIME_XPATH);
            if (string.IsNullOrEmpty(nodeInnerText4))
            {
                result1.ErrorMessage = "Unable to get ISSUE_TIME value";
                return result1;
            }
            string nodeInnerText5 = Utility.GetNodeInnerText(xmlDoc, SettingsParams.INVOICE_TOTAL_XPATH);
            if (string.IsNullOrEmpty(nodeInnerText5))
            {
                result1.ErrorMessage = "Unable to get INVOICE_TOTAL value";
                return result1;
            }
            string nodeInnerText6 = Utility.GetNodeInnerText(xmlDoc, SettingsParams.VAT_TOTAL_XPATH);
            if (string.IsNullOrEmpty(nodeInnerText6))
            {
                result1.ErrorMessage = "Unable to get VAT_TOTAL value";
                return result1;
            }
            Utility.GetNodeInnerText(xmlDoc, SettingsParams.QR_CODE_XPATH);
            DateTime result2 = new DateTime();
            string str = nodeInnerText3 + " " + nodeInnerText4;
            DateTime.TryParseExact(nodeInnerText3, SettingsParams.allDatesFormats, (IFormatProvider)CultureInfo.InvariantCulture, DateTimeStyles.None, out result2);
            string[] strArray = nodeInnerText4.Split(':');
            int result3 = 0;
            int result4 = 0;
            int result5 = 0;
            if (!string.IsNullOrEmpty(strArray[0]) && int.TryParse(strArray[0], out result3))
                result2 = result2.AddHours((double)result3);
            if (strArray.Length > 1 && !string.IsNullOrEmpty(strArray[1]) && int.TryParse(strArray[1], out result4))
                result2 = result2.AddMinutes((double)result4);
            if (strArray.Length > 2 && !string.IsNullOrEmpty(strArray[2]) && int.TryParse(strArray[2], out result5))
                result2 = result2.AddSeconds((double)result5);
            string timeStamp = result2.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
            bool isSimplified = false;
            if (Utility.GetInvoiceType(xmlDoc) == "Simplified")
                isSimplified = true;
            string qrCodeFromValues = new QRValidator().GenerateQrCodeFromValues(nodeInnerText1, nodeInnerText2, timeStamp, nodeInnerText5, nodeInnerText6, hashedXml, publicKeyArr, signature, isSimplified, certificateSignatureBytes);
            try
            {
                Utility.SetNodeValue(xmlDoc, SettingsParams.QR_CODE_XPATH, qrCodeFromValues);
            }
            catch
            {
                result1.ErrorMessage = "There is no node for QR in XML file.";
                result1.IsValid = false;
            }
            result1.ResultedValue = qrCodeFromValues;
            result1.IsValid = true;
            return result1;
        }
    }
}
