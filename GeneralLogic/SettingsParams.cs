// Decompiled with JetBrains decompiler
// Type: SDKNETFrameWorkLib.GeneralLogic.SettingsParams
// Assembly: SDKNETFrameWorkLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7328BD70-F112-452F-B96E-929850EA6A46
// Assembly location: C:\Users\umarali\Desktop\faltu saudia ka kam\zatca-einvoicing-sdk-233-R3.1.9\zatca-einvoicing-sdk-233-R3.1.9\Lib\.Net\DLL\SDKNETFrameWorkLib.dll

using System.Configuration;
using System.Reflection;

namespace SDKNETFrameWorkLib.GeneralLogic
{
  internal class SettingsParams
  {
    public static string SELLER_NAME_XPATH = "/*[local-name() = 'Invoice']/*[local-name() = 'AccountingSupplierParty']/*[local-name() = 'Party']/*[local-name() = 'PartyLegalEntity']//*[local-name() = 'RegistrationName']";
    public static string VAT_REGISTERATION_XPATH = "/*[local-name() = 'Invoice']/*[local-name() = 'AccountingSupplierParty']/*[local-name() = 'Party']/*[local-name() = 'PartyTaxScheme']/*[local-name() = 'CompanyID']";
    public static string ISSUE_DATE_XPATH = "/*[local-name() = 'Invoice']/*[local-name() = 'IssueDate']";
    public static string ISSUE_TIME_XPATH = "/*[local-name() = 'Invoice']/*[local-name() = 'IssueTime']";
    public static string INVOICE_TOTAL_XPATH = "/*[local-name() = 'Invoice']/*[local-name() = 'LegalMonetaryTotal']/*[local-name() = 'TaxInclusiveAmount']";
    public static string VAT_TOTAL_XPATH = "/*[local-name() = 'Invoice']/*[local-name() = 'TaxTotal']/*[local-name() = 'TaxAmount']";
    public static string SIGNATURE_XPATH = "/*[local-name() = 'Invoice']/*[local-name() = 'UBLExtensions']/*[local-name() = 'UBLExtension']/*[local-name() = 'ExtensionContent']/*[local-name() = 'UBLDocumentSignatures']/*[local-name() = 'SignatureInformation']/*[local-name() = 'Signature']/*[local-name() = 'SignatureValue']";
    public static string CERTIFICATE_XPATH = "/*[local-name() = 'Invoice']/*[local-name() = 'UBLExtensions']/*[local-name() = 'UBLExtension']/*[local-name() = 'ExtensionContent']/*[local-name() = 'UBLDocumentSignatures']/*[local-name() = 'SignatureInformation']/*[local-name() = 'Signature']/*[local-name() = 'KeyInfo']/*[local-name() = 'X509Data']/*[local-name() = 'X509Certificate']";
    public static string QR_CODE_XPATH = "/*[local-name() = 'Invoice']/*[local-name() = 'AdditionalDocumentReference' and *[local-name()='ID' and .='QR']]/*[local-name() = 'Attachment']/*[local-name() = 'EmbeddedDocumentBinaryObject']";
    public static string Hash_XPATH = "/*[local-name() = 'Invoice']/*[local-name() = 'UBLExtensions']/*[local-name() = 'UBLExtension']/*[local-name() = 'ExtensionContent']/*[local-name() = 'UBLDocumentSignatures']/*[local-name() = 'SignatureInformation']/*[local-name() = 'Signature']/*[local-name() = 'SignedInfo']/*[local-name() = 'Reference' and @Id='invoiceSignedData']/*[local-name() = 'DigestValue']";
    public static string Invoice_Type_XPATH = "//*[local-name()='Invoice']//*[local-name()='InvoiceTypeCode']";
    public static string PIH_XPATH = "/*[local-name() = 'Invoice']/*[local-name() = 'AdditionalDocumentReference' and *[local-name()='ID' and .='PIH']]/*[local-name() = 'Attachment']/*[local-name() = 'EmbeddedDocumentBinaryObject']";
    public static string SIGNED_PROPERTIES_XPATH = "//*[local-name()='Invoice']//*[local-name()='UBLExtensions']//*[local-name()='ExtensionContent']//*[local-name()='UBLDocumentSignatures']//*[local-name()='SignatureInformation']//*[local-name()='Signature']//*[local-name()='QualifyingProperties']//*[local-name()='SignedProperties']";
    public static string SIGNED_Properities_DIGEST_VALUE_XPATH = "//*[local-name()='Invoice']//*[local-name()='UBLExtensions']//*[local-name()='ExtensionContent']//*[local-name()='UBLDocumentSignatures']//*[local-name()='SignatureInformation']//*[local-name()='Signature']//*[local-name()='SignedInfo']//*[local-name()='Reference'][2]//*[local-name()='DigestValue']";
    public static string SIGNED_Certificate_DIGEST_VALUE_XPATH = "//*[local-name()='Invoice']//*[local-name()='UBLExtensions']//*[local-name()='UBLExtension']//*[local-name()='ExtensionContent']//*[local-name()='UBLDocumentSignatures']//*[local-name()='SignatureInformation']//*[local-name()='Signature']//*[local-name()='Object']//*[local-name()='QualifyingProperties']//*[local-name()='SignedProperties']//*[local-name()='SignedSignatureProperties']//*[local-name()='SigningCertificate']//*[local-name()='Cert']//*[local-name()='CertDigest']//*[local-name()='DigestValue']";
    public static string X509_SERIAL_NUMBER_XPATH = "//*[local-name()='Invoice']//*[local-name()='UBLExtensions']//*[local-name()='UBLExtension']//*[local-name()='ExtensionContent']//*[local-name()='UBLDocumentSignatures']//*[local-name()='SignatureInformation']//*[local-name()='Signature']//*[local-name()='Object']//*[local-name()='QualifyingProperties']//*[local-name()='SignedProperties']//*[local-name()='SignedSignatureProperties']//*[local-name()='SigningCertificate']//*[local-name()='Cert']//*[local-name()='IssuerSerial']//*[local-name()='X509SerialNumber']";
    public static string ISSUER_NAME_XPATH = "//*[local-name()='Invoice']//*[local-name()='UBLExtensions']//*[local-name()='UBLExtension']//*[local-name()='ExtensionContent']//*[local-name()='UBLDocumentSignatures']//*[local-name()='SignatureInformation']//*[local-name()='Signature']//*[local-name()='Object']//*[local-name()='QualifyingProperties']//*[local-name()='SignedProperties']//*[local-name()='SignedSignatureProperties']//*[local-name()='SigningCertificate']//*[local-name()='Cert']//*[local-name()='IssuerSerial']//*[local-name()='X509IssuerName']";
    public static string PUBLIC_KEY_HASHING_XPATH = "//*[local-name()='Invoice']//*[local-name()='UBLExtensions']//*[local-name()='ExtensionContent']//*[local-name()='UBLDocumentSignatures']//*[local-name()='SignatureInformation']//*[local-name()='Signature']//*[local-name()='QualifyingProperties']//*[local-name()='SignedProperties']//*[local-name()='SignedSignatureProperties']//*[local-name()='SigningCertificate']//*[local-name()='Cert']//*[local-name()='CertDigest']//*[local-name()='DigestValue']";
    public static string SIGNING_TIME_XPATH = "//*[local-name()='Invoice']//*[local-name()='UBLExtensions']//*[local-name()='ExtensionContent']//*[local-name()='UBLDocumentSignatures']//*[local-name()='SignatureInformation']//*[local-name()='Signature']//*[local-name()='QualifyingProperties']//*[local-name()='SignedProperties']//*[local-name()='SignedSignatureProperties']//*[local-name()='SigningTime']";
    public static string[] allDatesFormats = new string[19]
    {
      "yyyy-MM-dd",
      "yyyy/MM/dd",
      "yyyy/M/d",
      "dd/MM/yyyy",
      "d/M/yyyy",
      "dd/M/yyyy",
      "d/MM/yyyy",
      "yyyy-MM-dd",
      "yyyy-M-d",
      "dd-MM-yyyy",
      "d-M-yyyy",
      "dd-M-yyyy",
      "d-MM-yyyy",
      "yyyy MM dd",
      "yyyy M d",
      "dd MM yyyy",
      "d M yyyy",
      "dd M yyyy",
      "d MM yyyy"
    };
    public static string Embeded_InvoiceXSLFileForFormatting = "SDKNETFrameWorkLib.Data.format.xsl";
    public static string Embeded_InvoiceXSLFileForHashing = "SDKNETFrameWorkLib.Data.invoice.xsl";
    public static string Embeded_Remove_Elements_PATH = "SDKNETFrameWorkLib.Data.removeElements.xsl";
    public static string Embeded_Add_QR_Element_PATH = "SDKNETFrameWorkLib.Data.addQRElement.xsl";
    public static string Embeded_Add_Signature_Element_PATH = "SDKNETFrameWorkLib.Data.addSignatureElement.xsl";
    public static string Embeded_Add_UBL_Element_PATH = "SDKNETFrameWorkLib.Data.addUBLElement.xsl";
    public static string Embeded_QR_XML_File_PATH = "SDKNETFrameWorkLib.Data.qr.xml";
    public static string Embeded_Signature_File_PATH = "SDKNETFrameWorkLib.Data.signature.xml";
    public static string Embeded_UBL_File_PATH = "SDKNETFrameWorkLib.Data.ubl.xml";
    public static string Embeded_License_Config_File_PATH = "SDKNETFrameWorkLib.Data.config.xsd";

    public static string Embeded_EN_Schematrons_PATH
    {
      get
      {
        string enSchematronsPath = "";
        System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
        if (configuration != null && configuration.AppSettings != null && configuration.AppSettings.Settings["ENSchematronsPath"] != null)
          enSchematronsPath = configuration.AppSettings.Settings["ENSchematronsPath"].Value;
        if (string.IsNullOrEmpty(enSchematronsPath))
          enSchematronsPath = ConfigurationManager.AppSettings["ENSchematronsPath"];
        return enSchematronsPath;
      }
    }

    public static string Embeded_KSA_Schematrons_PATH
    {
      get
      {
        string ksaSchematronsPath = "";
        System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
        if (configuration != null && configuration.AppSettings != null && configuration.AppSettings.Settings["KSASchematronsPath"] != null)
          ksaSchematronsPath = configuration.AppSettings.Settings["KSASchematronsPath"].Value;
        if (string.IsNullOrEmpty(ksaSchematronsPath))
          ksaSchematronsPath = ConfigurationManager.AppSettings["KSASchematronsPath"];
        return ksaSchematronsPath;
      }
    }

    public static string Embeded_UBL_XSD_PATH
    {
      get
      {
        string embededUblXsdPath = "";
        System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
        if (configuration != null && configuration.AppSettings != null && configuration.AppSettings.Settings[nameof (Embeded_UBL_XSD_PATH)] != null)
          embededUblXsdPath = configuration.AppSettings.Settings[nameof (Embeded_UBL_XSD_PATH)].Value;
        if (string.IsNullOrEmpty(embededUblXsdPath))
          embededUblXsdPath = ConfigurationManager.AppSettings[nameof (Embeded_UBL_XSD_PATH)];
        return embededUblXsdPath;
      }
    }

    public static string Embeded_CommonAggregateComponents_XSD_PATH
    {
      get
      {
        string componentsXsdPath = "";
        System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
        if (configuration != null && configuration.AppSettings != null && configuration.AppSettings.Settings[nameof (Embeded_CommonAggregateComponents_XSD_PATH)] != null)
          componentsXsdPath = configuration.AppSettings.Settings[nameof (Embeded_CommonAggregateComponents_XSD_PATH)].Value;
        if (string.IsNullOrEmpty(componentsXsdPath))
          componentsXsdPath = ConfigurationManager.AppSettings[nameof (Embeded_CommonAggregateComponents_XSD_PATH)];
        return componentsXsdPath;
      }
    }

    public static string Embeded_CommonBasicComponents_XSD_PATH
    {
      get
      {
        string componentsXsdPath = "";
        System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
        if (configuration != null && configuration.AppSettings != null && configuration.AppSettings.Settings[nameof (Embeded_CommonBasicComponents_XSD_PATH)] != null)
          componentsXsdPath = configuration.AppSettings.Settings[nameof (Embeded_CommonBasicComponents_XSD_PATH)].Value;
        if (string.IsNullOrEmpty(componentsXsdPath))
          componentsXsdPath = ConfigurationManager.AppSettings[nameof (Embeded_CommonBasicComponents_XSD_PATH)];
        return componentsXsdPath;
      }
    }

    public static string Embeded_CommonExtensionComponents_XSD_PATH
    {
      get
      {
        string componentsXsdPath = "";
        System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
        if (configuration != null && configuration.AppSettings != null && configuration.AppSettings.Settings[nameof (Embeded_CommonExtensionComponents_XSD_PATH)] != null)
          componentsXsdPath = configuration.AppSettings.Settings[nameof (Embeded_CommonExtensionComponents_XSD_PATH)].Value;
        if (string.IsNullOrEmpty(componentsXsdPath))
          componentsXsdPath = ConfigurationManager.AppSettings[nameof (Embeded_CommonExtensionComponents_XSD_PATH)];
        return componentsXsdPath;
      }
    }

    public static string Embeded_CommonSignatureComponents_XSD_PATH
    {
      get
      {
        string componentsXsdPath = "";
        System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
        if (configuration != null && configuration.AppSettings != null && configuration.AppSettings.Settings[nameof (Embeded_CommonSignatureComponents_XSD_PATH)] != null)
          componentsXsdPath = configuration.AppSettings.Settings[nameof (Embeded_CommonSignatureComponents_XSD_PATH)].Value;
        if (string.IsNullOrEmpty(componentsXsdPath))
          componentsXsdPath = ConfigurationManager.AppSettings[nameof (Embeded_CommonSignatureComponents_XSD_PATH)];
        return componentsXsdPath;
      }
    }

    public static string Embeded_SignatureAggregateComponents_XSD_PATH
    {
      get
      {
        string componentsXsdPath = "";
        System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
        if (configuration != null && configuration.AppSettings != null && configuration.AppSettings.Settings[nameof (Embeded_SignatureAggregateComponents_XSD_PATH)] != null)
          componentsXsdPath = configuration.AppSettings.Settings[nameof (Embeded_SignatureAggregateComponents_XSD_PATH)].Value;
        if (string.IsNullOrEmpty(componentsXsdPath))
          componentsXsdPath = ConfigurationManager.AppSettings[nameof (Embeded_SignatureAggregateComponents_XSD_PATH)];
        return componentsXsdPath;
      }
    }

    public static string Embeded_SignatureBasicComponents_XSD_PATH
    {
      get
      {
        string componentsXsdPath = "";
        System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
        if (configuration != null && configuration.AppSettings != null && configuration.AppSettings.Settings[nameof (Embeded_SignatureBasicComponents_XSD_PATH)] != null)
          componentsXsdPath = configuration.AppSettings.Settings[nameof (Embeded_SignatureBasicComponents_XSD_PATH)].Value;
        if (string.IsNullOrEmpty(componentsXsdPath))
          componentsXsdPath = ConfigurationManager.AppSettings[nameof (Embeded_SignatureBasicComponents_XSD_PATH)];
        return componentsXsdPath;
      }
    }

    public static string Embeded_UBL_UnqualifiedDataTypes_XSD_PATH
    {
      get
      {
        string dataTypesXsdPath = "";
        System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
        if (configuration != null && configuration.AppSettings != null && configuration.AppSettings.Settings[nameof (Embeded_UBL_UnqualifiedDataTypes_XSD_PATH)] != null)
          dataTypesXsdPath = configuration.AppSettings.Settings[nameof (Embeded_UBL_UnqualifiedDataTypes_XSD_PATH)].Value;
        if (string.IsNullOrEmpty(dataTypesXsdPath))
          dataTypesXsdPath = ConfigurationManager.AppSettings[nameof (Embeded_UBL_UnqualifiedDataTypes_XSD_PATH)];
        return dataTypesXsdPath;
      }
    }

    public static string Embeded_CoreComponentTypeSchemaModule_XSD_PATH
    {
      get
      {
        string schemaModuleXsdPath = "";
        System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
        if (configuration != null && configuration.AppSettings != null && configuration.AppSettings.Settings[nameof (Embeded_CoreComponentTypeSchemaModule_XSD_PATH)] != null)
          schemaModuleXsdPath = configuration.AppSettings.Settings[nameof (Embeded_CoreComponentTypeSchemaModule_XSD_PATH)].Value;
        if (string.IsNullOrEmpty(schemaModuleXsdPath))
          schemaModuleXsdPath = ConfigurationManager.AppSettings[nameof (Embeded_CoreComponentTypeSchemaModule_XSD_PATH)];
        return schemaModuleXsdPath;
      }
    }

    public static string Embeded_ExtensionContentDataType_XSD_PATH
    {
      get
      {
        string contentDataTypeXsdPath = "";
        System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
        if (configuration != null && configuration.AppSettings != null && configuration.AppSettings.Settings[nameof (Embeded_ExtensionContentDataType_XSD_PATH)] != null)
          contentDataTypeXsdPath = configuration.AppSettings.Settings[nameof (Embeded_ExtensionContentDataType_XSD_PATH)].Value;
        if (string.IsNullOrEmpty(contentDataTypeXsdPath))
          contentDataTypeXsdPath = ConfigurationManager.AppSettings[nameof (Embeded_ExtensionContentDataType_XSD_PATH)];
        return contentDataTypeXsdPath;
      }
    }

    public static string Embeded_QualifiedDataTypes_XSD_PATH
    {
      get
      {
        string dataTypesXsdPath = "";
        System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
        if (configuration != null && configuration.AppSettings != null && configuration.AppSettings.Settings[nameof (Embeded_QualifiedDataTypes_XSD_PATH)] != null)
          dataTypesXsdPath = configuration.AppSettings.Settings[nameof (Embeded_QualifiedDataTypes_XSD_PATH)].Value;
        if (string.IsNullOrEmpty(dataTypesXsdPath))
          dataTypesXsdPath = ConfigurationManager.AppSettings[nameof (Embeded_QualifiedDataTypes_XSD_PATH)];
        return dataTypesXsdPath;
      }
    }

    public static string Embeded_CCTS_CCT_SchemaModule_XSD_PATH
    {
      get
      {
        string schemaModuleXsdPath = "";
        System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
        if (configuration != null && configuration.AppSettings != null && configuration.AppSettings.Settings[nameof (Embeded_CCTS_CCT_SchemaModule_XSD_PATH)] != null)
          schemaModuleXsdPath = configuration.AppSettings.Settings[nameof (Embeded_CCTS_CCT_SchemaModule_XSD_PATH)].Value;
        if (string.IsNullOrEmpty(schemaModuleXsdPath))
          schemaModuleXsdPath = ConfigurationManager.AppSettings[nameof (Embeded_CCTS_CCT_SchemaModule_XSD_PATH)];
        return schemaModuleXsdPath;
      }
    }

    public static string Embeded_UBL_XAdESv_XSD_PATH
    {
      get
      {
        string ublXadEsvXsdPath = "";
        System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
        if (configuration != null && configuration.AppSettings != null && configuration.AppSettings.Settings[nameof (Embeded_UBL_XAdESv_XSD_PATH)] != null)
          ublXadEsvXsdPath = configuration.AppSettings.Settings[nameof (Embeded_UBL_XAdESv_XSD_PATH)].Value;
        if (string.IsNullOrEmpty(ublXadEsvXsdPath))
          ublXadEsvXsdPath = ConfigurationManager.AppSettings[nameof (Embeded_UBL_XAdESv_XSD_PATH)];
        return ublXadEsvXsdPath;
      }
    }

    public static string Embeded_UBL_Xmldsig_Core_XSD_PATH
    {
      get
      {
        string xmldsigCoreXsdPath = "";
        System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
        if (configuration != null && configuration.AppSettings != null && configuration.AppSettings.Settings[nameof (Embeded_UBL_Xmldsig_Core_XSD_PATH)] != null)
          xmldsigCoreXsdPath = configuration.AppSettings.Settings[nameof (Embeded_UBL_Xmldsig_Core_XSD_PATH)].Value;
        if (string.IsNullOrEmpty(xmldsigCoreXsdPath))
          xmldsigCoreXsdPath = ConfigurationManager.AppSettings[nameof (Embeded_UBL_Xmldsig_Core_XSD_PATH)];
        return xmldsigCoreXsdPath;
      }
    }
  }
}
