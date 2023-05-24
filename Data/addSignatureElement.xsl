<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"   xmlns:ext="urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2" xmlns:sac="urn:oasis:names:specification:ubl:schema:xsd:SignatureAggregateComponents-2"
                xmlns:sbc="urn:oasis:names:specification:ubl:schema:xsd:SignatureBasicComponents-2"
                xmlns:cbc="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2"
                xmlns:sig="urn:oasis:names:specification:ubl:schema:xsd:CommonSignatureComponents-2" version="1.0">
  <xsl:output method="xml"  encoding="utf-8" indent="no"/>
  <xsl:param name="signElement">SIGN-TO-BE-REPLACED</xsl:param>

  <xsl:template match="//*[local-name()='AdditionalDocumentReference'][last()]">
       <xsl:copy>
            <xsl:copy-of select="@*"/>
            <xsl:copy-of select="node()"/>
        </xsl:copy>
        <xsl:copy-of select="$signElement" />
    </xsl:template>
    <xsl:template match="@* | node()">
        <xsl:copy>
            <xsl:apply-templates select="@* | node()"/>
        </xsl:copy>
    </xsl:template>
  
</xsl:stylesheet>
