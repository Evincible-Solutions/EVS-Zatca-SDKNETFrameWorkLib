// Decompiled with JetBrains decompiler
// Type: SDKNETFrameWorkLib.GeneralLogic.CSRGeneratorDTO
// Assembly: SDKNETFrameWorkLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7328BD70-F112-452F-B96E-929850EA6A46
// Assembly location: C:\Users\umarali\Desktop\faltu saudia ka kam\zatca-einvoicing-sdk-233-R3.1.9\zatca-einvoicing-sdk-233-R3.1.9\Lib\.Net\DLL\SDKNETFrameWorkLib.dll

namespace SDKNETFrameWorkLib.GeneralLogic
{
  public class CSRGeneratorDTO
  {
    public string csrPemFormat { get; set; }

    public string publicKeyPemFormat { get; set; }

    public string commonName { get; set; }

    public string serialNumber { get; set; }

    public string organizationIdentifier { get; set; }

    public string organizationUnitName { get; set; }

    public string organizationName { get; set; }

    public string countryName { get; set; }

    public string invoiceType { get; set; }

    public string location { get; set; }

    public string industry { get; set; }

    public CSRGeneratorDTO()
    {
    }

    public CSRGeneratorDTO(
      object commonName,
      object serialNumber,
      object organizationIdentifier,
      object organizationUnitName,
      object organizationName,
      object countryName,
      object invoiceType,
      object location,
      object industry)
    {
      this.commonName = (string) commonName;
      this.serialNumber = (string) serialNumber;
      this.organizationIdentifier = (string) organizationIdentifier;
      this.organizationUnitName = (string) organizationUnitName;
      this.organizationName = (string) organizationName;
      this.countryName = (string) countryName;
      this.invoiceType = (string) invoiceType;
      this.location = (string) location;
      this.industry = (string) industry;
    }
  }
}
