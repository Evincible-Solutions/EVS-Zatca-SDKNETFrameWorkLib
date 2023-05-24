// Decompiled with JetBrains decompiler
// Type: SDKNETFrameWorkLib.GeneralLogic.Utility
// Assembly: SDKNETFrameWorkLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7328BD70-F112-452F-B96E-929850EA6A46
// Assembly location: C:\Users\umarali\Desktop\faltu saudia ka kam\zatca-einvoicing-sdk-233-R3.1.9\zatca-einvoicing-sdk-233-R3.1.9\Lib\.Net\DLL\SDKNETFrameWorkLib.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace SDKNETFrameWorkLib.GeneralLogic
{
    internal class Utility
    {
        public static string GetNodeInnerText(XmlDocument doc, string xPath)
        {
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            nsmgr.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            nsmgr.AddNamespace("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
            XmlNode xmlNode = doc.SelectSingleNode(xPath, nsmgr);
            return xmlNode != null ? xmlNode.InnerText : "";
        }

        public static string GetNodeInnerXML(XmlDocument doc, string xPath)
        {
            XmlNode xmlNode = doc.SelectSingleNode(xPath);
            if (xmlNode == null)
                return "";
            string str = "";
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlNode.OuterXml)))
            {
                XmlDsigC14NTransform dsigC14Ntransform = new XmlDsigC14NTransform(false);
                dsigC14Ntransform.LoadInput((object)memoryStream);
                str = Encoding.UTF8.GetString((dsigC14Ntransform.GetOutput() as MemoryStream).ToArray());
            }
            return str.Replace("></ds:DigestMethod>", "/>");
        }

        public static XmlDocument RemoveXmlns(string xml)
        {
            XDocument xdocument = XDocument.Parse(xml, LoadOptions.PreserveWhitespace);
            xdocument.Root.Descendants().Attributes().Where<XAttribute>((Func<XAttribute, bool>)(x => x.IsNamespaceDeclaration)).Remove();
            foreach (XElement descendant in xdocument.Descendants())
                descendant.Name = (XName)descendant.Name.LocalName;
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.PreserveWhitespace = true;
            xmlDocument.LoadXml(xdocument.ToString());
            return xmlDocument;
        }

        public static void SetNodeValue(XmlDocument doc, string xPath, string value)
        {
            XmlNode xmlNode = doc.SelectSingleNode(xPath);
            if (xmlNode == null)
                return;
            xmlNode.InnerText = value;
        }

        public static string GetNodeAttributeValue(XmlDocument doc, string xPath, string attributeName)
        {
            XmlNode xmlNode = doc.SelectSingleNode(xPath);
            return xmlNode != null ? xmlNode.Attributes[attributeName].Value : "";
        }

        public static string ApplyXSLT(string xml, string xsltFilePath)
        {
            StringBuilder output = new StringBuilder();
            using (XmlWriter results = XmlWriter.Create(output, new XmlWriterSettings()
            {
                OmitXmlDeclaration = true,
                Encoding = Encoding.UTF8,
                Indent = false
            }))
            {
                XmlReader stylesheet = XmlReader.Create(Utility.ReadInternalEmbededResourceStream(xsltFilePath));
                XslCompiledTransform compiledTransform = new XslCompiledTransform();
                compiledTransform.Load(stylesheet);
                XmlReader xmlReader1 = XmlReader.Create(new StringReader(xml));
                compiledTransform.Transform(xmlReader1, results);
            }
            return output.ToString();
        }

        public static string ApplyXSLTPassingXML(string xml, string xsltFilePath)
        {
            StringBuilder output = new StringBuilder();
            using (XmlWriter results = XmlWriter.Create(output, new XmlWriterSettings()
            {
                OmitXmlDeclaration = true,
                Encoding = Encoding.UTF8,
                Indent = true,
                ConformanceLevel = ConformanceLevel.Auto
            }))
            {
                XmlReader stylesheet = XmlReader.Create(Utility.ReadInternalEmbededResourceStream(xsltFilePath));
                XmlReader input = XmlReader.Create((TextReader)new StringReader(xml));
                input.Read();
                XslCompiledTransform compiledTransform = new XslCompiledTransform();
                compiledTransform.Load(stylesheet);
                compiledTransform.Transform(input, results);
            }
            return output.ToString();
        }

        public static byte[] Sha256_hashAsBytes(string value)
        {
            using (SHA256 shA256 = SHA256.Create())
            {
                StringBuilder stringBuilder = new StringBuilder();
                Encoding utF8 = Encoding.UTF8;
                return shA256.ComputeHash(utF8.GetBytes(value));
            }
        }

        public static string Sha256_hashAsString(string rawData)
        {
            StringBuilder stringBuilder = new StringBuilder();
            using (SHA256 shA256 = SHA256.Create())
            {
                Encoding utF8 = Encoding.UTF8;
                foreach (byte num in shA256.ComputeHash(Encoding.UTF8.GetBytes(rawData)))
                    stringBuilder.Append(num.ToString("x2"));
            }
            return stringBuilder.ToString();
        }

        public static string ToBase64Encode(string toEncode) => Convert.ToBase64String(Encoding.ASCII.GetBytes(toEncode));

        public static string Sha256_hashAsBytesThenHexa(string value)
        {
            using (SHA256 shA256 = SHA256.Create())
            {
                StringBuilder stringBuilder = new StringBuilder();
                Encoding utF8 = Encoding.UTF8;
                return Utility.BytesToHex((byte[])(Array)((IEnumerable<byte>)shA256.ComputeHash(utF8.GetBytes(value))).Select<byte, sbyte>((Func<byte, sbyte>)(x => (sbyte)x)).ToArray<sbyte>());
            }
        }

        public static string ToBase64Encode(byte[] value) => value == null ? (string)null : Convert.ToBase64String(value);

        public static byte[] ToBase64DecodeAsBinary(string base64EncodedText) => string.IsNullOrEmpty(base64EncodedText) ? (byte[])null : Convert.FromBase64String(base64EncodedText);

        public static sbyte[] GetTlvVAlue(string tagnums, string tagvalue)
        {
            string[] source = new string[1] { tagnums };
            string s1 = tagvalue;
            sbyte[] array = ((IEnumerable<string>)source).Select<string, sbyte>((Func<string, sbyte>)(s => sbyte.Parse(s))).ToArray<sbyte>();
            byte[] bytes1 = Encoding.UTF8.GetBytes(s1);
            sbyte[] second = (sbyte[])(Array)bytes1;
            sbyte[] bytes2 = (sbyte[])(Array)Encoding.UTF8.GetBytes(bytes1.Length.ToString());
            return ((IEnumerable<sbyte>)array).Concat<sbyte>((IEnumerable<sbyte>)bytes2).Concat<sbyte>((IEnumerable<sbyte>)second).ToArray<sbyte>();
        }

        public static sbyte[] GetTlvVAlue(string tagnums, sbyte[] tagvalueb) => ((IEnumerable<sbyte>)((IEnumerable<string>)new string[1]
        {
      tagnums
        }).Select<string, sbyte>((Func<string, sbyte>)(s => sbyte.Parse(s))).ToArray<sbyte>()).Concat<sbyte>((IEnumerable<sbyte>)(Array)Encoding.UTF8.GetBytes(tagvalueb.Length.ToString())).Concat<sbyte>((IEnumerable<sbyte>)tagvalueb).ToArray<sbyte>();

        public static string GetInvoiceType(XmlDocument xmlDoc) => Utility.GetNodeAttributeValue(xmlDoc, SettingsParams.Invoice_Type_XPATH, "name").StartsWith("01") ? "Standard" : "Simplified";

        public static string HexToDecimal(string hex)
        {
            List<int> source = new List<int>() { 0 };
            foreach (char ch in hex)
            {
                int num1 = Convert.ToInt32(ch.ToString(), 16);
                for (int index = 0; index < source.Count; ++index)
                {
                    int num2 = source[index] * 16 + num1;
                    source[index] = num2 % 10;
                    num1 = num2 / 10;
                }
                for (; num1 > 0; num1 /= 10)
                    source.Add(num1 % 10);
            }
            return new string(source.Select<int, char>((Func<int, char>)(d => (char)(48 + d))).Reverse<char>().ToArray<char>());
        }

        private static string BytesToHex(byte[] hash)
        {
            StringBuilder stringBuilder = new StringBuilder(2 * hash.Length);
            for (int index = 0; index < hash.Length; ++index)
            {
                string str = hash[index].ToString("X");
                if (str.Length == 1)
                    stringBuilder.Append('0');
                stringBuilder.Append(str);
            }
            return stringBuilder.ToString();
        }

        public static Stream ReadInternalEmbededResourceStream(string resource) => Assembly.GetExecutingAssembly().GetManifestResourceStream( resource);

        public static void WriteTag(Stream stream, uint tag)
        {
            bool flag = true;
            for (int index = 3; index >= 0; --index)
            {
                byte num = (byte)(tag >> 8 * index);
                if (!(num == (byte)0 & flag) || index <= 0)
                {
                    if (flag)
                    {
                        if (index == 0)
                        {
                            if (((int)num & 31) == 31)
                                throw new Exception("Invalid tag value: first octet indicates subsequent octets, but no subsequent octets found");
                        }
                        else if (((int)num & 31) != 31)
                            throw new Exception("Invalid tag value: first octet indicates no subsequent octets, but subsequent octets found");
                    }
                    else if (index == 0)
                    {
                        if (((int)num & 128) == 128)
                            throw new Exception("Invalid tag value: last octet indicates subsequent octets");
                    }
                    else if (((int)num & 128) != 128)
                        throw new Exception("Invalid tag value: non-last octet indicates no subsequent octets");
                    stream.WriteByte(num);
                    flag = false;
                }
            }
        }

        public static void WriteLength(Stream stream, int? length)
        {
            if (!length.HasValue)
            {
                stream.WriteByte((byte)128);
            }
            else
            {
                int? nullable1 = length;
                int num1 = 0;
                long? nullable2;
                int num2;
                if (!(nullable1.GetValueOrDefault() < num1 & nullable1.HasValue))
                {
                    nullable1 = length;
                    nullable2 = nullable1.HasValue ? new long?((long)nullable1.GetValueOrDefault()) : new long?();
                    long maxValue = (long)uint.MaxValue;
                    num2 = nullable2.GetValueOrDefault() > maxValue & nullable2.HasValue ? 1 : 0;
                }
                else
                    num2 = 1;
                if (num2 != 0)
                    throw new Exception(string.Format("Invalid length value: {0}", (object)length));
                nullable1 = length;
                int maxValue1 = (int)sbyte.MaxValue;
                if (nullable1.GetValueOrDefault() <= maxValue1 & nullable1.HasValue)
                {
                    stream.WriteByte(checked((byte)length.Value));
                }
                else
                {
                    nullable1 = length;
                    int maxValue2 = (int)byte.MaxValue;
                    byte num3;
                    if (nullable1.GetValueOrDefault() <= maxValue2 & nullable1.HasValue)
                    {
                        num3 = (byte)1;
                    }
                    else
                    {
                        nullable1 = length;
                        int maxValue3 = (int)ushort.MaxValue;
                        if (nullable1.GetValueOrDefault() <= maxValue3 & nullable1.HasValue)
                        {
                            num3 = (byte)2;
                        }
                        else
                        {
                            nullable1 = length;
                            int num4 = 16777215;
                            if (nullable1.GetValueOrDefault() <= num4 & nullable1.HasValue)
                            {
                                num3 = (byte)3;
                            }
                            else
                            {
                                nullable1 = length;
                                nullable2 = nullable1.HasValue ? new long?((long)nullable1.GetValueOrDefault()) : new long?();
                                long maxValue4 = (long)uint.MaxValue;
                                if (!(nullable2.GetValueOrDefault() <= maxValue4 & nullable2.HasValue))
                                    throw new Exception(string.Format("Length value too big: {0}", (object)length));
                                num3 = (byte)4;
                            }
                        }
                    }
                    stream.WriteByte((byte)((uint)num3 | 128U));
                    for (int index = (int)num3 - 1; index >= 0; --index)
                    {
                        nullable1 = length;
                        int num5 = 8 * index;
                        byte num6 = (byte)(nullable1.HasValue ? new int?(nullable1.GetValueOrDefault() >> num5) : new int?()).Value;
                        stream.WriteByte(num6);
                    }
                }
            }
        }

        public static MemoryStream WriteTlv(uint tag, byte[] value)
        {
            MemoryStream memoryStream = new MemoryStream();
            Utility.WriteTag((Stream)memoryStream, tag);
            int count = value != null ? value.Length : 0;
            Utility.WriteLength((Stream)memoryStream, new int?(count));
            if (value == null)
                throw new Exception("Please provide a value!");
            memoryStream.Write(value, 0, count);
            return memoryStream;
        }

        public static byte[] StringToByteArray(string hex) => Enumerable.Range(0, hex.Length).Where<int>((Func<int, bool>)(x => x % 2 == 0)).Select<int, byte>((Func<int, byte>)(x => Convert.ToByte(hex.Substring(x, 2), 16))).ToArray<byte>();

        public static T DeserializeToObject<T>(string filepath) where T : class
        {
            using (StreamReader streamReader = new StreamReader(filepath))
                return (T)new XmlSerializer(typeof(T)).Deserialize((TextReader)streamReader);
        }

        public static void SerializeToXml<T>(T anyobject, string xmlFilePath)
        {
            using (StreamWriter streamWriter = new StreamWriter(xmlFilePath))
                new XmlSerializer(anyobject.GetType()).Serialize((TextWriter)streamWriter, (object)anyobject);
        }

        public static string[] getAllResources() => Assembly.GetExecutingAssembly().GetManifestResourceNames();
    }
}
