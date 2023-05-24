// Decompiled with JetBrains decompiler
// Type: SDKNETFrameWorkLib.BLL.CSRGenerator
// Assembly: SDKNETFrameWorkLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7328BD70-F112-452F-B96E-929850EA6A46
// Assembly location: C:\Users\umarali\Desktop\faltu saudia ka kam\zatca-einvoicing-sdk-233-R3.1.9\zatca-einvoicing-sdk-233-R3.1.9\Lib\.Net\DLL\SDKNETFrameWorkLib.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Microsoft;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.IO.Pem;
using SDKNETFrameWorkLib.GeneralLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SDKNETFrameWorkLib.BLL
{
  public class CSRGenerator
  {
    private static SecureRandom secureRandom = new SecureRandom();
    private Dictionary<string, string> data = new Dictionary<string, string>();
    private CSRGeneratorDTO cSRGeneratorDTO = new CSRGeneratorDTO();

    private Result GenerateCSR()
    {
      this.data = new Dictionary<string, string>();
      this.cSRGeneratorDTO = new CSRGeneratorDTO();
      Result csr = new Result();
      csr.lstSteps = new List<Result>();
      csr.Operation = "Generate CSR Operation";
      csr.IsValid = false;
      Result result1 = new Result();
      result1.Operation = "First Step : Load CSR Properities";
      try
      {
        this.LoadCSRProperities();
        this.cSRGeneratorDTO = this.MappDataTocSRGeneratorDTO();
        result1.IsValid = true;
        csr.lstSteps.Add(result1);
      }
      catch (Exception ex)
      {
        result1.ErrorMessage = ex.Message;
        result1.IsValid = false;
        csr.lstSteps.Add(result1);
        return csr;
      }
      Result result2 = new Result();
      result2.Operation = "Second Step : Validate  CSR Properities";
      string str = this.ValidateCsrConfigInputFile();
      if (!string.IsNullOrEmpty(str))
      {
        result2.ErrorMessage = str;
        result2.IsValid = false;
        csr.lstSteps.Add(result2);
        return csr;
      }
      result2.IsValid = true;
      csr.lstSteps.Add(result2);
      AsymmetricCipherKeyPair keyPair = this.GenerateKeyPair();
      IDictionary dictionary = (IDictionary) new Hashtable();
      dictionary.Add((object) X509Name.CN, (object) this.cSRGeneratorDTO.commonName);
      dictionary.Add((object) X509Name.O, (object) this.cSRGeneratorDTO.organizationName);
      dictionary.Add((object) X509Name.OU, (object) this.cSRGeneratorDTO.organizationUnitName);
      dictionary.Add((object) X509Name.C, (object) this.cSRGeneratorDTO.countryName);
      ArrayList oids1 = new ArrayList();
      oids1.Add((object) X509Name.C);
      oids1.Add((object) X509Name.OU);
      oids1.Add((object) X509Name.O);
      oids1.Add((object) X509Name.CN);
      ArrayList values1 = new ArrayList();
      values1.Add((object) this.cSRGeneratorDTO.countryName);
      values1.Add((object) this.cSRGeneratorDTO.organizationUnitName);
      values1.Add((object) this.cSRGeneratorDTO.organizationName);
      values1.Add((object) this.cSRGeneratorDTO.commonName);
      ArrayList oids2 = new ArrayList();
      oids2.Add((object) X509Name.Surname);
      oids2.Add((object) X509Name.UID);
      oids2.Add((object) X509Name.T);
      oids2.Add((object) X509Name.L);
      oids2.Add((object) X509Name.BusinessCategory);
      ArrayList values2 = new ArrayList();
      values2.Add((object) this.cSRGeneratorDTO.serialNumber);
      values2.Add((object) this.cSRGeneratorDTO.organizationIdentifier);
      values2.Add((object) this.cSRGeneratorDTO.invoiceType);
      values2.Add((object) this.cSRGeneratorDTO.location);
      values2.Add((object) this.cSRGeneratorDTO.industry);
      Result result3 = new Result();
      result3.Operation = "Third Step : Generate  CSR String";
      try
      {
        X509Name subject = new X509Name((IList) oids1, (IList) values1);
        GeneralNames extValue = new GeneralNames(new GeneralName[1]
        {
          new GeneralName(new X509Name((IList) oids2, (IList) values2))
        });
        X509ExtensionsGenerator extensionsGenerator = new X509ExtensionsGenerator();
        extensionsGenerator.AddExtension(MicrosoftObjectIdentifiers.MicrosoftCertTemplateV1, false, (Asn1Encodable) new DerOctetString((Asn1Encodable) new DisplayText(2, "ZATCA-Code-Signing")));
        extensionsGenerator.AddExtension(X509Extensions.SubjectAlternativeName, false, (Asn1Encodable) extValue);
        X509Extensions element1 = extensionsGenerator.Generate();
        AttributePkcs element2 = new AttributePkcs(PkcsObjectIdentifiers.Pkcs9AtExtensionRequest, (Asn1Set) new DerSet((Asn1Encodable) element1));
        Pkcs10CertificationRequest certificationRequest = new Pkcs10CertificationRequest("SHA256withECDSA", subject, keyPair.Public, (Asn1Set) new DerSet((Asn1Encodable) element2), keyPair.Private);
        StringBuilder sb = new StringBuilder();
        Org.BouncyCastle.OpenSsl.PemWriter pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter((TextWriter) new StringWriter(sb));
        pemWriter.WriteObject((object) certificationRequest);
        pemWriter.Writer.Flush();
        string contents = sb.ToString();
        try
        {
          File.WriteAllText("generatedCSR.txt", contents);
        }
        catch
        {
        }
        result3.IsValid = true;
        result3.ResultedValue = contents;
        csr.lstSteps.Add(result3);
      }
      catch (Exception ex)
      {
        result3.ErrorMessage = ex.Message;
        result3.IsValid = false;
        csr.lstSteps.Add(result3);
        return csr;
      }
      Result result4 = new Result();
      result4.Operation = "Forth Step : Generate EC Private Key String";
      try
      {
        string contents = this.GetPrivateKey(keyPair).Replace("-----BEGIN EC PRIVATE KEY-----", "").Replace("\n", "").Replace("\r", "").Replace("-----END EC PRIVATE KEY-----", "");
        result4.ResultedValue = contents;
        result4.IsValid = true;
        try
        {
          File.WriteAllText("generatedPrivateKey.txt", contents);
        }
        catch
        {
        }
        csr.lstSteps.Add(result4);
      }
      catch (Exception ex)
      {
        result3.ErrorMessage = ex.Message;
        result3.IsValid = false;
        csr.lstSteps.Add(result3);
        return csr;
      }
      csr.IsValid = true;
      return csr;
    }

    private void LoadCSRProperities()
    {
      foreach (string readAllLine in File.ReadAllLines("csr-config-example.properties"))
        this.data.Add(readAllLine.Split('=')[0], string.Join("=", ((IEnumerable<string>) readAllLine.Split('=')).Skip<string>(1).ToArray<string>()));
    }

    private CSRGeneratorDTO MappDataTocSRGeneratorDTO() => new CSRGeneratorDTO((object) this.data["csr.common.name"], (object) this.data["csr.serial.number"], (object) this.data["csr.organization.identifier"], (object) this.data["csr.organization.unit.name"], (object) this.data["csr.organization.name"], (object) this.data["csr.country.name"], (object) this.data["csr.invoice.type"], (object) this.data["csr.location.address"], (object) this.data["csr.industry.business.category"]);

    private string ValidateCsrConfigInputFile()
    {
      if (string.IsNullOrEmpty(this.cSRGeneratorDTO.countryName))
        return "Common name is mandatory field";
      if (string.IsNullOrEmpty(this.cSRGeneratorDTO.serialNumber))
        return "Serial number is mandatory field";
      if (!Regex.Match(this.cSRGeneratorDTO.serialNumber, "1-(.+)\\|2-(.+)\\|3-(.+)", RegexOptions.IgnoreCase).Success)
        return "Invalid serial number, serial number should be in regular expression format (1-...|2-...|3-....)";
      if (string.IsNullOrEmpty(this.cSRGeneratorDTO.organizationIdentifier))
        return "Organization identifier is mandatory field";
      if (this.cSRGeneratorDTO.organizationIdentifier.Length != 15)
        return "Invalid organization identifier, please provide a valid 15 digit of your vat number";
      if (this.cSRGeneratorDTO.organizationIdentifier.Substring(0, 1) != "3")
        return "Invalid organization identifier, organization identifier should be started with digit 3";
      if (this.cSRGeneratorDTO.organizationIdentifier.Substring(this.cSRGeneratorDTO.organizationIdentifier.Length - 1, 1) != "3")
        return "Invalid organization identifier, organization identifier should be end with digit 3";
      if (string.IsNullOrEmpty(this.cSRGeneratorDTO.organizationUnitName))
        return "Organization unit name is mandatory field";
      if (this.cSRGeneratorDTO.organizationIdentifier.Substring(10, 1) == "1" && this.cSRGeneratorDTO.organizationUnitName.Length != 10)
        return "Invalid organization unit name, please provide a valid 10 digit of your group tin number";
      if (string.IsNullOrEmpty(this.cSRGeneratorDTO.organizationName))
        return "Organization name is mandatory field";
      if (string.IsNullOrEmpty(this.cSRGeneratorDTO.countryName))
        return "Country code name is mandatory field";
      if (this.cSRGeneratorDTO.countryName.Length > 3 || this.cSRGeneratorDTO.countryName.Length < 2)
        return "Invalid country code name, please provide a valid country code name";
      if (string.IsNullOrEmpty(this.cSRGeneratorDTO.invoiceType))
        return "Invoice type is mandatory field";
      if (this.cSRGeneratorDTO.invoiceType.Length != 4 || !Regex.Match(this.cSRGeneratorDTO.invoiceType, "^[0-1]{4}$", RegexOptions.IgnoreCase).Success)
        return "Invalid invoice type, please provide a valid invoice type";
      if (string.IsNullOrEmpty(this.cSRGeneratorDTO.location))
        return "Location is mandatory field";
      return string.IsNullOrEmpty(this.cSRGeneratorDTO.industry) ? "Industry is mandatory filed" : "";
    }

    private void WriteFile(string fileName, string content) => File.WriteAllText(fileName, content);

    private AsymmetricCipherKeyPair GenerateKeyPair()
    {
      ECKeyPairGenerator keyPairGenerator = new ECKeyPairGenerator("ECDSA");
      KeyGenerationParameters parameters = new KeyGenerationParameters(new SecureRandom(), 256);
      keyPairGenerator.Init(parameters);
      return keyPairGenerator.GenerateKeyPair();
    }

    private string ToHex(byte[] data) => string.Concat(((IEnumerable<byte>) data).Select<byte, string>((Func<byte, string>) (x => x.ToString("x2"))));

    private string Transform(string type, byte[] certificateRequest)
    {
      PemObject pemObject = new PemObject(type, certificateRequest);
      StringWriter writer = new StringWriter();
      new Org.BouncyCastle.OpenSsl.PemWriter((TextWriter) writer).WriteObject((object) pemObject);
      writer.Close();
      return writer.ToString();
    }

    private string GetPrivateKey(AsymmetricCipherKeyPair keys)
    {
      TextWriter writer = (TextWriter) new StringWriter();
      Org.BouncyCastle.OpenSsl.PemWriter pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(writer);
      pemWriter.WriteObject((object) keys.Private);
      pemWriter.Writer.Flush();
      return writer.ToString();
    }
  }
}
