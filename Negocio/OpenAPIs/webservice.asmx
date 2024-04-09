<?xml version="1.0" encoding="utf-8"?>
<wsdl:definitions xmlns:s="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://schemas.xmlsoap.org/wsdl/soap12/" xmlns:http="http://schemas.xmlsoap.org/wsdl/http/" xmlns:mime="http://schemas.xmlsoap.org/wsdl/mime/" xmlns:tns="www.XMLWebServiceSoapHeaderAuth.net" xmlns:soap="http://schemas.xmlsoap.org/wsdl/soap/" xmlns:tm="http://microsoft.com/wsdl/mime/textMatching/" xmlns:soapenc="http://schemas.xmlsoap.org/soap/encoding/" targetNamespace="www.XMLWebServiceSoapHeaderAuth.net" xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">
  <wsdl:types>
    <s:schema elementFormDefault="qualified" targetNamespace="www.XMLWebServiceSoapHeaderAuth.net">
      <s:element name="WSReportDigest">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="strDigest" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="strDate" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="reference" type="s:string" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSReportDigestResponse">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="1" maxOccurs="1" name="WSReportDigestResult" type="tns:WSResultReportDigest" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:complexType name="WSResultReportDigest">
        <s:sequence>
          <s:element minOccurs="1" maxOccurs="1" name="State" type="s:int" />
          <s:element minOccurs="0" maxOccurs="1" name="Description" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="StrNow" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="StrVector" type="s:string" />
          <s:element minOccurs="1" maxOccurs="1" name="Id" type="s:long" />
        </s:sequence>
      </s:complexType>
      <s:element name="AuthSoap" type="tns:AuthSoap" />
      <s:complexType name="AuthSoap">
        <s:sequence>
          <s:element minOccurs="0" maxOccurs="1" name="User" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Password" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Entity" type="s:string" />
        </s:sequence>
        <s:anyAttribute />
      </s:complexType>
      <s:element name="WSReportPkcs7">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="1" maxOccurs="1" name="id" type="s:long" />
            <s:element minOccurs="0" maxOccurs="1" name="strSign" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="strCertificate" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="reference" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="tsaName" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="nomName" type="s:string" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSReportPkcs7Response">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="1" maxOccurs="1" name="WSReportPkcs7Result" type="tns:WSResultReportPkcs7" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:complexType name="WSResultReportPkcs7">
        <s:sequence>
          <s:element minOccurs="1" maxOccurs="1" name="State" type="s:int" />
          <s:element minOccurs="0" maxOccurs="1" name="Description" type="s:string" />
          <s:element minOccurs="1" maxOccurs="1" name="Id" type="s:long" />
          <s:element minOccurs="0" maxOccurs="1" name="Now" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Evidence" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="FingerPrint" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Cn" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="HexSerie" type="s:string" />
        </s:sequence>
      </s:complexType>
      <s:element name="WSReportPkcs1">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="chain" type="s:string" />
            <s:element minOccurs="1" maxOccurs="1" name="code" type="s:int" />
            <s:element minOccurs="0" maxOccurs="1" name="strSign" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="strCertificate" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="reference" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="tsaName" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="nomName" type="s:string" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSReportPkcs1Response">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="1" maxOccurs="1" name="WSReportPkcs1Result" type="tns:WSResultReportPkcs1" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:complexType name="WSResultReportPkcs1">
        <s:sequence>
          <s:element minOccurs="1" maxOccurs="1" name="State" type="s:int" />
          <s:element minOccurs="0" maxOccurs="1" name="Description" type="s:string" />
          <s:element minOccurs="1" maxOccurs="1" name="Id" type="s:long" />
          <s:element minOccurs="0" maxOccurs="1" name="Now" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Evidence" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="FingerPrint" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Cn" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="HexSerie" type="s:string" />
        </s:sequence>
      </s:complexType>
      <s:element name="WSSignCentral">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="signer" type="s:string" />
            <s:element minOccurs="1" maxOccurs="1" name="kindData" type="s:int" />
            <s:element minOccurs="0" maxOccurs="1" name="strData" type="s:string" />
            <s:element minOccurs="1" maxOccurs="1" name="kindDigest" type="s:int" />
            <s:element minOccurs="0" maxOccurs="1" name="reference" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="tsaName" type="s:string" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSSignCentralResponse">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="1" maxOccurs="1" name="WSSignCentralResult" type="tns:WSResultSignCentral" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:complexType name="WSResultSignCentral">
        <s:sequence>
          <s:element minOccurs="1" maxOccurs="1" name="State" type="s:int" />
          <s:element minOccurs="0" maxOccurs="1" name="Description" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="StrSign" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="StrCertificate" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="StrCertificateCn" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="StrCertificateSerial" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="FingerPrint" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="StrNow" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="strBegin" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="strEnd" type="s:string" />
        </s:sequence>
      </s:complexType>
      <s:element name="WSDecodeCertificate">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="operation" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="reference" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="strCertificate" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="tsaName" type="s:string" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSDecodeCertificateResponse">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="1" maxOccurs="1" name="WSDecodeCertificateResult" type="tns:WSResultDecodeCertificate" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:complexType name="WSResultDecodeCertificate">
        <s:sequence>
          <s:element minOccurs="1" maxOccurs="1" name="State" type="s:int" />
          <s:element minOccurs="0" maxOccurs="1" name="Description" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="HexSerie" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="StrBegin" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="StrEnd" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="SubjectCn" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="SubjectEmail" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="SubjectOrganization" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="SubjectDepartament" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="SubjectState" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="SubjectCountry" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="SubjectRFC" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="SubjectCurp" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="SubjectLocality" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="SubjectPostalCode" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="SubjectTelephone" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="SubjectNoEmploy" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="SubjectAdemicDegree" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="IssuerCn" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="IssuerEmail" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="IssuerOrganization" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="IssuerDepartament" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="IssuerState" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="IssuerCountry" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="IssuerRFC" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="IssuerCurp" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="PublicKey" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="FingerPrint" type="s:string" />
          <s:element minOccurs="1" maxOccurs="1" name="Id" type="s:long" />
          <s:element minOccurs="0" maxOccurs="1" name="StrDate" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Acuse" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="OcspResponse" type="s:string" />
        </s:sequence>
      </s:complexType>
      <s:element name="WSRequestTsa">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="tsaName" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="reference" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="strDigest" type="s:string" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSRequestTsaResponse">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="1" maxOccurs="1" name="WSRequestTsaResult" type="tns:WSResultTsa" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:complexType name="WSResultTsa">
        <s:sequence>
          <s:element minOccurs="1" maxOccurs="1" name="State" type="s:int" />
          <s:element minOccurs="0" maxOccurs="1" name="Description" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="StrNow" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Response" type="s:string" />
          <s:element minOccurs="1" maxOccurs="1" name="Index" type="s:long" />
          <s:element minOccurs="0" maxOccurs="1" name="DateNow" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="InternalDigest" type="s:string" />
        </s:sequence>
      </s:complexType>
      <s:element name="WSRequestNom151">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="nomName" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="fileSource" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="reference" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="nameExpedient" type="s:string" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSRequestNom151Response">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="1" maxOccurs="1" name="WSRequestNom151Result" type="tns:WSResultNom151" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:complexType name="WSResultNom151">
        <s:sequence>
          <s:element minOccurs="1" maxOccurs="1" name="State" type="s:int" />
          <s:element minOccurs="0" maxOccurs="1" name="Description" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="StrConstancy" type="s:string" />
          <s:element minOccurs="1" maxOccurs="1" name="Id" type="s:long" />
          <s:element minOccurs="1" maxOccurs="1" name="Index" type="s:long" />
          <s:element minOccurs="0" maxOccurs="1" name="Moment" type="s:string" />
          <s:element minOccurs="1" maxOccurs="1" name="Parcials" type="s:int" />
          <s:element minOccurs="0" maxOccurs="1" name="NamePsc" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="SerialPsc" type="s:string" />
        </s:sequence>
      </s:complexType>
      <s:element name="WSPkcs7InformationFromNs">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="reference" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="ens" type="s:string" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSPkcs7InformationFromNsResponse">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="WSPkcs7InformationFromNsResult" type="tns:ArrayOfWSResultPkcs7InformationFromNs" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:complexType name="ArrayOfWSResultPkcs7InformationFromNs">
        <s:sequence>
          <s:element minOccurs="0" maxOccurs="unbounded" name="WSResultPkcs7InformationFromNs" type="tns:WSResultPkcs7InformationFromNs" />
        </s:sequence>
      </s:complexType>
      <s:complexType name="WSResultPkcs7InformationFromNs">
        <s:sequence>
          <s:element minOccurs="0" maxOccurs="1" name="Sign" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="SignerId" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="SignerCn" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="SignerHexSerie" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="SignerBegin" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="SignerEnd" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="SignMoment" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="SignAlgo" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="OcspCn" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="OcspHexSerie" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="OcspBegin" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="OcspEnd" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="OcspNow" type="s:string" />
          <s:element minOccurs="1" maxOccurs="1" name="OcspState" type="s:int" />
          <s:element minOccurs="0" maxOccurs="1" name="OcspRevocate" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="OcspUrl" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="OcspIssuerCn" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="TsCn" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="TsHexSerie" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="TsBegin" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="TsEnd" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="TsNow" type="s:string" />
          <s:element minOccurs="1" maxOccurs="1" name="TsIndex" type="s:long" />
          <s:element minOccurs="0" maxOccurs="1" name="TsDigest" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="TsIssuerCn" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="TsUrl" type="s:string" />
        </s:sequence>
      </s:complexType>
      <s:element name="WSPkcs7InformationStrFromNs">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="reference" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="ens" type="s:string" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSPkcs7InformationStrFromNsResponse">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="WSPkcs7InformationStrFromNsResult" type="tns:ArrayOfString" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:complexType name="ArrayOfString">
        <s:sequence>
          <s:element minOccurs="0" maxOccurs="unbounded" name="string" nillable="true" type="s:string" />
        </s:sequence>
      </s:complexType>
      <s:element name="WSInformationOcspFromNs">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="reference" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="ens" type="s:string" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSInformationOcspFromNsResponse">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="WSInformationOcspFromNsResult" type="tns:ArrayOfWSResultInformationOcspFromNs" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:complexType name="ArrayOfWSResultInformationOcspFromNs">
        <s:sequence>
          <s:element minOccurs="0" maxOccurs="unbounded" name="WSResultInformationOcspFromNs" type="tns:WSResultInformationOcspFromNs" />
        </s:sequence>
      </s:complexType>
      <s:complexType name="WSResultInformationOcspFromNs">
        <s:sequence>
          <s:element minOccurs="0" maxOccurs="1" name="OcspCn" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="OcspHexSerie" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="OcspBegin" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="OcspEnd" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="OcspNow" type="s:string" />
          <s:element minOccurs="1" maxOccurs="1" name="OcspState" type="s:int" />
          <s:element minOccurs="0" maxOccurs="1" name="OcspRevocate" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="OcspUrl" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="OcspIssuerCn" type="s:string" />
        </s:sequence>
      </s:complexType>
      <s:element name="WSInformationTspFromNs">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="reference" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="ens" type="s:string" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSInformationTspFromNsResponse">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="WSInformationTspFromNsResult" type="tns:ArrayOfWSResultInformationTspFromNs" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:complexType name="ArrayOfWSResultInformationTspFromNs">
        <s:sequence>
          <s:element minOccurs="0" maxOccurs="unbounded" name="WSResultInformationTspFromNs" type="tns:WSResultInformationTspFromNs" />
        </s:sequence>
      </s:complexType>
      <s:complexType name="WSResultInformationTspFromNs">
        <s:sequence>
          <s:element minOccurs="0" maxOccurs="1" name="TsCn" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="TsHexSerie" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="TsBegin" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="TsEnd" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="TsNow" type="s:string" />
          <s:element minOccurs="1" maxOccurs="1" name="TsIndex" type="s:long" />
          <s:element minOccurs="0" maxOccurs="1" name="TsDigest" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="TsIssuerCn" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="TsUrl" type="s:string" />
        </s:sequence>
      </s:complexType>
      <s:element name="WSFromHtmlToPdf">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="strHtml" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="reference" type="s:string" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSFromHtmlToPdfResponse">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="1" maxOccurs="1" name="WSFromHtmlToPdfResult" type="tns:WSResultFromHtmlToPdf" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:complexType name="WSResultFromHtmlToPdf">
        <s:sequence>
          <s:element minOccurs="1" maxOccurs="1" name="State" type="s:int" />
          <s:element minOccurs="0" maxOccurs="1" name="Description" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="StrPdf" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="StrCertificate" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="StrCertificateCn" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="StrCertificateSerial" type="s:string" />
        </s:sequence>
      </s:complexType>
      <s:element name="WSFromHtmlToPdfSecure">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="strHtml" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="signer" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="reference" type="s:string" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSFromHtmlToPdfSecureResponse">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="1" maxOccurs="1" name="WSFromHtmlToPdfSecureResult" type="tns:WSResultFromHtmlToPdf" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSCreatePkcs7FromNs">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="reference" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="source" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="target" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="ens" type="s:string" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSCreatePkcs7FromNsResponse">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="1" maxOccurs="1" name="WSCreatePkcs7FromNsResult" type="tns:WSTransState" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:complexType name="WSTransState">
        <s:sequence>
          <s:element minOccurs="1" maxOccurs="1" name="State" type="s:int" />
          <s:element minOccurs="0" maxOccurs="1" name="Description" type="s:string" />
        </s:sequence>
      </s:complexType>
      <s:element name="WSCreatePkcs7NoDocFromNs">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="reference" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="ens" type="s:string" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSCreatePkcs7NoDocFromNsResponse">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="1" maxOccurs="1" name="WSCreatePkcs7NoDocFromNsResult" type="tns:WSResultCreatePkcs7NoDocFromNs" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:complexType name="WSResultCreatePkcs7NoDocFromNs">
        <s:sequence>
          <s:element minOccurs="1" maxOccurs="1" name="State" type="s:int" />
          <s:element minOccurs="0" maxOccurs="1" name="Description" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Data" type="s:string" />
          <s:element minOccurs="1" maxOccurs="1" name="NoSigners" type="s:int" />
        </s:sequence>
      </s:complexType>
      <s:element name="WSCreatePdf">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="reference" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="source" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="target" type="s:string" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSCreatePdfResponse">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="1" maxOccurs="1" name="WSCreatePdfResult" type="tns:WSTransState" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSCreatePdfEvidence">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="reference" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="source" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="target" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="evidence" type="s:string" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSCreatePdfEvidenceResponse">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="1" maxOccurs="1" name="WSCreatePdfEvidenceResult" type="tns:WSTransState" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSCreatePdfEvidenceSecure">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="reference" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="signer" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="source" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="target" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="evidence" type="s:string" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSCreatePdfEvidenceSecureResponse">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="1" maxOccurs="1" name="WSCreatePdfEvidenceSecureResult" type="tns:WSTransState" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSNom151ForPkcs1Ns">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="nomName" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="ens" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="reference" type="s:string" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSNom151ForPkcs1NsResponse">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="1" maxOccurs="1" name="WSNom151ForPkcs1NsResult" type="tns:WSResultNom151ForPkcs1Ns" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:complexType name="WSResultNom151ForPkcs1Ns">
        <s:sequence>
          <s:element minOccurs="1" maxOccurs="1" name="State" type="s:int" />
          <s:element minOccurs="0" maxOccurs="1" name="Description" type="s:string" />
          <s:element minOccurs="1" maxOccurs="1" name="Id" type="s:long" />
          <s:element minOccurs="0" maxOccurs="1" name="StrNow" type="s:string" />
          <s:element minOccurs="1" maxOccurs="1" name="Index" type="s:long" />
          <s:element minOccurs="0" maxOccurs="1" name="Moment" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="PscName" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="PscSerial" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Constance" type="s:string" />
          <s:element minOccurs="1" maxOccurs="1" name="Parcials" type="s:int" />
        </s:sequence>
      </s:complexType>
      <s:element name="WSTransfer">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="1" maxOccurs="1" name="id" type="s:long" />
            <s:element minOccurs="0" maxOccurs="1" name="reference" type="s:string" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSTransferResponse">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="1" maxOccurs="1" name="WSTransferResult" type="tns:WSResultTransfer" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:complexType name="WSResultTransfer">
        <s:sequence>
          <s:element minOccurs="0" maxOccurs="1" name="Pkcs1Code" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Pkcs1KindDigest" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Pkcs1Chain" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Pkcs1Digest" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Pkcs1Sign" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Pkcs1Name" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Pkcs1Serie" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Pkcs1Certificate" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Pkcs7KindDigest" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Pkcs7Digest" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Pkcs7Sign" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Pkcs7Pkcs7" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Pkcs7Name" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Pkcs7Serie" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Pkcs7Certificate" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="OcspUserName" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="OcspUserSerie" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="OcspUserCurp" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="OcspUserRfc" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="OcspState" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="OcspMoment" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="OcspMomentRevocate" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="OcspOcsp" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="OcspResponderName" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="OcspResponderSerie" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="OcspResponderCertificate" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="TsaKindDigest" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="TsaDigest" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="TsaStamping" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="TsaName" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="TsaSerie" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="TsaCertificate" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Nom151Version" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Nom151Index" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Nom151Moment" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Nom151ConstanceName" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Nom151Constance" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Nom151Name" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Nom151Serie" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Nom151Certificate" type="s:string" />
        </s:sequence>
      </s:complexType>
      <s:element name="WSDownloadCertificate">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="reference" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="acName" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="rfc" type="s:string" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSDownloadCertificateResponse">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="1" maxOccurs="1" name="WSDownloadCertificateResult" type="tns:WSResultDecodeCertificate" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSACRegisterPkcs10">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="reference" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="strPkcs10" type="s:string" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSACRegisterPkcs10Response">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="1" maxOccurs="1" name="WSACRegisterPkcs10Result" type="tns:WSResultRegisterPkcs10" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:complexType name="WSResultRegisterPkcs10">
        <s:sequence>
          <s:element minOccurs="1" maxOccurs="1" name="State" type="s:int" />
          <s:element minOccurs="0" maxOccurs="1" name="Description" type="s:string" />
          <s:element minOccurs="1" maxOccurs="1" name="Id" type="s:long" />
          <s:element minOccurs="0" maxOccurs="1" name="Acuse" type="s:string" />
        </s:sequence>
      </s:complexType>
      <s:element name="WSACDownloadX509">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="reference" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="msg" type="s:string" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSACDownloadX509Response">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="1" maxOccurs="1" name="WSACDownloadX509Result" type="tns:WSResultDownloadX509" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:complexType name="WSResultDownloadX509">
        <s:sequence>
          <s:element minOccurs="1" maxOccurs="1" name="State" type="s:int" />
          <s:element minOccurs="0" maxOccurs="1" name="Description" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Pkcs7" type="s:base64Binary" />
        </s:sequence>
      </s:complexType>
      <s:element name="WSACAutoRevocateX509">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="reference" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="msg" type="s:string" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSACAutoRevocateX509Response">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="1" maxOccurs="1" name="WSACAutoRevocateX509Result" type="tns:WSResultAutoRevocateX509" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:complexType name="WSResultAutoRevocateX509">
        <s:sequence>
          <s:element minOccurs="1" maxOccurs="1" name="State" type="s:int" />
          <s:element minOccurs="0" maxOccurs="1" name="Description" type="s:string" />
        </s:sequence>
      </s:complexType>
      <s:element name="WSSVIRequest">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="reference" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="msg" type="s:string" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSSVIRequestResponse">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="1" maxOccurs="1" name="WSSVIRequestResult" type="tns:WSResultSVIRequest" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:complexType name="WSResultSVIRequest">
        <s:sequence>
          <s:element minOccurs="1" maxOccurs="1" name="State" type="s:int" />
          <s:element minOccurs="0" maxOccurs="1" name="Description" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Response" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="dataResult" type="tns:SVIValidateINE" />
        </s:sequence>
      </s:complexType>
      <s:complexType name="SVIValidateINE">
        <s:sequence>
          <s:element minOccurs="1" maxOccurs="1" name="State" type="s:int" />
          <s:element minOccurs="0" maxOccurs="1" name="Description" type="s:string" />
          <s:element minOccurs="1" maxOccurs="1" name="Transfer" type="s:long" />
          <s:element minOccurs="0" maxOccurs="1" name="SituacionRegistral" type="s:string" />
          <s:element minOccurs="1" maxOccurs="1" name="NumeroEmision" type="s:int" />
          <s:element minOccurs="1" maxOccurs="1" name="ClaveElector" type="s:int" />
          <s:element minOccurs="1" maxOccurs="1" name="Nombre" type="s:int" />
          <s:element minOccurs="1" maxOccurs="1" name="ApellidoPaterno" type="s:int" />
          <s:element minOccurs="1" maxOccurs="1" name="ApellidoMaterno" type="s:int" />
          <s:element minOccurs="1" maxOccurs="1" name="AnioRegistro" type="s:int" />
          <s:element minOccurs="1" maxOccurs="1" name="AnioEmision" type="s:int" />
          <s:element minOccurs="1" maxOccurs="1" name="Curp" type="s:int" />
          <s:element minOccurs="0" maxOccurs="1" name="Similitudansi2" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Similitudansi7" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="FirmaCentralINE" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="SerieCentralINE" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="EstampillaTiempoINE" type="s:string" />
          <s:element minOccurs="1" maxOccurs="1" name="TiempoTotalProcesamiento" type="s:double" />
          <s:element minOccurs="0" maxOccurs="1" name="TsaSerie" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="BusSerie" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="BusNombre" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="MomentoReporte" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="RespuestaTotal" type="s:string" />
        </s:sequence>
      </s:complexType>
      <s:element name="WSSVIFingers">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="1" maxOccurs="1" name="transfer" type="s:long" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSSVIFingersResponse">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="1" maxOccurs="1" name="WSSVIFingersResult" type="tns:WSResultSVIFingers" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:complexType name="WSResultSVIFingers">
        <s:sequence>
          <s:element minOccurs="1" maxOccurs="1" name="State" type="s:int" />
          <s:element minOccurs="0" maxOccurs="1" name="Description" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Finger2" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Finger7" type="s:string" />
        </s:sequence>
      </s:complexType>
      <s:element name="WSFacial">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="msg" type="s:string" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSFacialResponse">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="1" maxOccurs="1" name="WSFacialResult" type="tns:WSState" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:complexType name="WSState">
        <s:sequence>
          <s:element minOccurs="1" maxOccurs="1" name="State" type="s:int" />
          <s:element minOccurs="0" maxOccurs="1" name="Description" type="s:string" />
          <s:element minOccurs="1" maxOccurs="1" name="Id" type="s:long" />
          <s:element minOccurs="0" maxOccurs="1" name="StrNow" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="FingerPrint" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Cn" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="HexSerie" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Acuse" type="s:string" />
        </s:sequence>
      </s:complexType>
      <s:element name="PdfSecure">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="strPdf" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="signer" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="reference" type="s:string" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="PdfSecureResponse">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="1" maxOccurs="1" name="PdfSecureResult" type="tns:WSResultPdfSecure" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:complexType name="WSResultPdfSecure">
        <s:sequence>
          <s:element minOccurs="1" maxOccurs="1" name="State" type="s:int" />
          <s:element minOccurs="0" maxOccurs="1" name="Description" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="PdfSecure" type="s:string" />
        </s:sequence>
      </s:complexType>
      <s:element name="WSTransform">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="reference" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="xml" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="xslt" type="s:string" />
            <s:element minOccurs="1" maxOccurs="1" name="preserveSpaces" type="s:boolean" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSTransformResponse">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="1" maxOccurs="1" name="WSTransformResult" type="tns:WSResultXmlState" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:complexType name="WSResultXmlState">
        <s:sequence>
          <s:element minOccurs="1" maxOccurs="1" name="State" type="s:int" />
          <s:element minOccurs="0" maxOccurs="1" name="Description" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Chain" type="s:string" />
        </s:sequence>
      </s:complexType>
      <s:element name="WSVerifyXml">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="reference" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="xml" type="s:string" />
            <s:element minOccurs="0" maxOccurs="1" name="xslt" type="s:string" />
            <s:element minOccurs="1" maxOccurs="1" name="preserveSpaces" type="s:boolean" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="WSVerifyXmlResponse">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="1" maxOccurs="1" name="WSVerifyXmlResult" type="tns:WSResultXmlVerifyState" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:complexType name="WSResultXmlVerifyState">
        <s:sequence>
          <s:element minOccurs="1" maxOccurs="1" name="State" type="s:int" />
          <s:element minOccurs="0" maxOccurs="1" name="Description" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Cn" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="HexSerie" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Begin" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="End" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="Emisor" type="s:string" />
        </s:sequence>
      </s:complexType>
    </s:schema>
  </wsdl:types>
  <wsdl:message name="WSReportDigestSoapIn">
    <wsdl:part name="parameters" element="tns:WSReportDigest" />
  </wsdl:message>
  <wsdl:message name="WSReportDigestSoapOut">
    <wsdl:part name="parameters" element="tns:WSReportDigestResponse" />
  </wsdl:message>
  <wsdl:message name="WSReportDigestAuthSoap">
    <wsdl:part name="AuthSoap" element="tns:AuthSoap" />
  </wsdl:message>
  <wsdl:message name="WSReportPkcs7SoapIn">
    <wsdl:part name="parameters" element="tns:WSReportPkcs7" />
  </wsdl:message>
  <wsdl:message name="WSReportPkcs7SoapOut">
    <wsdl:part name="parameters" element="tns:WSReportPkcs7Response" />
  </wsdl:message>
  <wsdl:message name="WSReportPkcs7AuthSoap">
    <wsdl:part name="AuthSoap" element="tns:AuthSoap" />
  </wsdl:message>
  <wsdl:message name="WSReportPkcs1SoapIn">
    <wsdl:part name="parameters" element="tns:WSReportPkcs1" />
  </wsdl:message>
  <wsdl:message name="WSReportPkcs1SoapOut">
    <wsdl:part name="parameters" element="tns:WSReportPkcs1Response" />
  </wsdl:message>
  <wsdl:message name="WSReportPkcs1AuthSoap">
    <wsdl:part name="AuthSoap" element="tns:AuthSoap" />
  </wsdl:message>
  <wsdl:message name="WSSignCentralSoapIn">
    <wsdl:part name="parameters" element="tns:WSSignCentral" />
  </wsdl:message>
  <wsdl:message name="WSSignCentralSoapOut">
    <wsdl:part name="parameters" element="tns:WSSignCentralResponse" />
  </wsdl:message>
  <wsdl:message name="WSSignCentralAuthSoap">
    <wsdl:part name="AuthSoap" element="tns:AuthSoap" />
  </wsdl:message>
  <wsdl:message name="WSDecodeCertificateSoapIn">
    <wsdl:part name="parameters" element="tns:WSDecodeCertificate" />
  </wsdl:message>
  <wsdl:message name="WSDecodeCertificateSoapOut">
    <wsdl:part name="parameters" element="tns:WSDecodeCertificateResponse" />
  </wsdl:message>
  <wsdl:message name="WSDecodeCertificateAuthSoap">
    <wsdl:part name="AuthSoap" element="tns:AuthSoap" />
  </wsdl:message>
  <wsdl:message name="WSRequestTsaSoapIn">
    <wsdl:part name="parameters" element="tns:WSRequestTsa" />
  </wsdl:message>
  <wsdl:message name="WSRequestTsaSoapOut">
    <wsdl:part name="parameters" element="tns:WSRequestTsaResponse" />
  </wsdl:message>
  <wsdl:message name="WSRequestTsaAuthSoap">
    <wsdl:part name="AuthSoap" element="tns:AuthSoap" />
  </wsdl:message>
  <wsdl:message name="WSRequestNom151SoapIn">
    <wsdl:part name="parameters" element="tns:WSRequestNom151" />
  </wsdl:message>
  <wsdl:message name="WSRequestNom151SoapOut">
    <wsdl:part name="parameters" element="tns:WSRequestNom151Response" />
  </wsdl:message>
  <wsdl:message name="WSRequestNom151AuthSoap">
    <wsdl:part name="AuthSoap" element="tns:AuthSoap" />
  </wsdl:message>
  <wsdl:message name="WSPkcs7InformationFromNsSoapIn">
    <wsdl:part name="parameters" element="tns:WSPkcs7InformationFromNs" />
  </wsdl:message>
  <wsdl:message name="WSPkcs7InformationFromNsSoapOut">
    <wsdl:part name="parameters" element="tns:WSPkcs7InformationFromNsResponse" />
  </wsdl:message>
  <wsdl:message name="WSPkcs7InformationFromNsAuthSoap">
    <wsdl:part name="AuthSoap" element="tns:AuthSoap" />
  </wsdl:message>
  <wsdl:message name="WSPkcs7InformationStrFromNsSoapIn">
    <wsdl:part name="parameters" element="tns:WSPkcs7InformationStrFromNs" />
  </wsdl:message>
  <wsdl:message name="WSPkcs7InformationStrFromNsSoapOut">
    <wsdl:part name="parameters" element="tns:WSPkcs7InformationStrFromNsResponse" />
  </wsdl:message>
  <wsdl:message name="WSPkcs7InformationStrFromNsAuthSoap">
    <wsdl:part name="AuthSoap" element="tns:AuthSoap" />
  </wsdl:message>
  <wsdl:message name="WSInformationOcspFromNsSoapIn">
    <wsdl:part name="parameters" element="tns:WSInformationOcspFromNs" />
  </wsdl:message>
  <wsdl:message name="WSInformationOcspFromNsSoapOut">
    <wsdl:part name="parameters" element="tns:WSInformationOcspFromNsResponse" />
  </wsdl:message>
  <wsdl:message name="WSInformationOcspFromNsAuthSoap">
    <wsdl:part name="AuthSoap" element="tns:AuthSoap" />
  </wsdl:message>
  <wsdl:message name="WSInformationTspFromNsSoapIn">
    <wsdl:part name="parameters" element="tns:WSInformationTspFromNs" />
  </wsdl:message>
  <wsdl:message name="WSInformationTspFromNsSoapOut">
    <wsdl:part name="parameters" element="tns:WSInformationTspFromNsResponse" />
  </wsdl:message>
  <wsdl:message name="WSInformationTspFromNsAuthSoap">
    <wsdl:part name="AuthSoap" element="tns:AuthSoap" />
  </wsdl:message>
  <wsdl:message name="WSFromHtmlToPdfSoapIn">
    <wsdl:part name="parameters" element="tns:WSFromHtmlToPdf" />
  </wsdl:message>
  <wsdl:message name="WSFromHtmlToPdfSoapOut">
    <wsdl:part name="parameters" element="tns:WSFromHtmlToPdfResponse" />
  </wsdl:message>
  <wsdl:message name="WSFromHtmlToPdfAuthSoap">
    <wsdl:part name="AuthSoap" element="tns:AuthSoap" />
  </wsdl:message>
  <wsdl:message name="WSFromHtmlToPdfSecureSoapIn">
    <wsdl:part name="parameters" element="tns:WSFromHtmlToPdfSecure" />
  </wsdl:message>
  <wsdl:message name="WSFromHtmlToPdfSecureSoapOut">
    <wsdl:part name="parameters" element="tns:WSFromHtmlToPdfSecureResponse" />
  </wsdl:message>
  <wsdl:message name="WSFromHtmlToPdfSecureAuthSoap">
    <wsdl:part name="AuthSoap" element="tns:AuthSoap" />
  </wsdl:message>
  <wsdl:message name="WSCreatePkcs7FromNsSoapIn">
    <wsdl:part name="parameters" element="tns:WSCreatePkcs7FromNs" />
  </wsdl:message>
  <wsdl:message name="WSCreatePkcs7FromNsSoapOut">
    <wsdl:part name="parameters" element="tns:WSCreatePkcs7FromNsResponse" />
  </wsdl:message>
  <wsdl:message name="WSCreatePkcs7FromNsAuthSoap">
    <wsdl:part name="AuthSoap" element="tns:AuthSoap" />
  </wsdl:message>
  <wsdl:message name="WSCreatePkcs7NoDocFromNsSoapIn">
    <wsdl:part name="parameters" element="tns:WSCreatePkcs7NoDocFromNs" />
  </wsdl:message>
  <wsdl:message name="WSCreatePkcs7NoDocFromNsSoapOut">
    <wsdl:part name="parameters" element="tns:WSCreatePkcs7NoDocFromNsResponse" />
  </wsdl:message>
  <wsdl:message name="WSCreatePkcs7NoDocFromNsAuthSoap">
    <wsdl:part name="AuthSoap" element="tns:AuthSoap" />
  </wsdl:message>
  <wsdl:message name="WSCreatePdfSoapIn">
    <wsdl:part name="parameters" element="tns:WSCreatePdf" />
  </wsdl:message>
  <wsdl:message name="WSCreatePdfSoapOut">
    <wsdl:part name="parameters" element="tns:WSCreatePdfResponse" />
  </wsdl:message>
  <wsdl:message name="WSCreatePdfAuthSoap">
    <wsdl:part name="AuthSoap" element="tns:AuthSoap" />
  </wsdl:message>
  <wsdl:message name="WSCreatePdfEvidenceSoapIn">
    <wsdl:part name="parameters" element="tns:WSCreatePdfEvidence" />
  </wsdl:message>
  <wsdl:message name="WSCreatePdfEvidenceSoapOut">
    <wsdl:part name="parameters" element="tns:WSCreatePdfEvidenceResponse" />
  </wsdl:message>
  <wsdl:message name="WSCreatePdfEvidenceAuthSoap">
    <wsdl:part name="AuthSoap" element="tns:AuthSoap" />
  </wsdl:message>
  <wsdl:message name="WSCreatePdfEvidenceSecureSoapIn">
    <wsdl:part name="parameters" element="tns:WSCreatePdfEvidenceSecure" />
  </wsdl:message>
  <wsdl:message name="WSCreatePdfEvidenceSecureSoapOut">
    <wsdl:part name="parameters" element="tns:WSCreatePdfEvidenceSecureResponse" />
  </wsdl:message>
  <wsdl:message name="WSCreatePdfEvidenceSecureAuthSoap">
    <wsdl:part name="AuthSoap" element="tns:AuthSoap" />
  </wsdl:message>
  <wsdl:message name="WSNom151ForPkcs1NsSoapIn">
    <wsdl:part name="parameters" element="tns:WSNom151ForPkcs1Ns" />
  </wsdl:message>
  <wsdl:message name="WSNom151ForPkcs1NsSoapOut">
    <wsdl:part name="parameters" element="tns:WSNom151ForPkcs1NsResponse" />
  </wsdl:message>
  <wsdl:message name="WSNom151ForPkcs1NsAuthSoap">
    <wsdl:part name="AuthSoap" element="tns:AuthSoap" />
  </wsdl:message>
  <wsdl:message name="WSTransferSoapIn">
    <wsdl:part name="parameters" element="tns:WSTransfer" />
  </wsdl:message>
  <wsdl:message name="WSTransferSoapOut">
    <wsdl:part name="parameters" element="tns:WSTransferResponse" />
  </wsdl:message>
  <wsdl:message name="WSTransferAuthSoap">
    <wsdl:part name="AuthSoap" element="tns:AuthSoap" />
  </wsdl:message>
  <wsdl:message name="WSDownloadCertificateSoapIn">
    <wsdl:part name="parameters" element="tns:WSDownloadCertificate" />
  </wsdl:message>
  <wsdl:message name="WSDownloadCertificateSoapOut">
    <wsdl:part name="parameters" element="tns:WSDownloadCertificateResponse" />
  </wsdl:message>
  <wsdl:message name="WSDownloadCertificateAuthSoap">
    <wsdl:part name="AuthSoap" element="tns:AuthSoap" />
  </wsdl:message>
  <wsdl:message name="WSACRegisterPkcs10SoapIn">
    <wsdl:part name="parameters" element="tns:WSACRegisterPkcs10" />
  </wsdl:message>
  <wsdl:message name="WSACRegisterPkcs10SoapOut">
    <wsdl:part name="parameters" element="tns:WSACRegisterPkcs10Response" />
  </wsdl:message>
  <wsdl:message name="WSACRegisterPkcs10AuthSoap">
    <wsdl:part name="AuthSoap" element="tns:AuthSoap" />
  </wsdl:message>
  <wsdl:message name="WSACDownloadX509SoapIn">
    <wsdl:part name="parameters" element="tns:WSACDownloadX509" />
  </wsdl:message>
  <wsdl:message name="WSACDownloadX509SoapOut">
    <wsdl:part name="parameters" element="tns:WSACDownloadX509Response" />
  </wsdl:message>
  <wsdl:message name="WSACDownloadX509AuthSoap">
    <wsdl:part name="AuthSoap" element="tns:AuthSoap" />
  </wsdl:message>
  <wsdl:message name="WSACAutoRevocateX509SoapIn">
    <wsdl:part name="parameters" element="tns:WSACAutoRevocateX509" />
  </wsdl:message>
  <wsdl:message name="WSACAutoRevocateX509SoapOut">
    <wsdl:part name="parameters" element="tns:WSACAutoRevocateX509Response" />
  </wsdl:message>
  <wsdl:message name="WSACAutoRevocateX509AuthSoap">
    <wsdl:part name="AuthSoap" element="tns:AuthSoap" />
  </wsdl:message>
  <wsdl:message name="WSSVIRequestSoapIn">
    <wsdl:part name="parameters" element="tns:WSSVIRequest" />
  </wsdl:message>
  <wsdl:message name="WSSVIRequestSoapOut">
    <wsdl:part name="parameters" element="tns:WSSVIRequestResponse" />
  </wsdl:message>
  <wsdl:message name="WSSVIRequestAuthSoap">
    <wsdl:part name="AuthSoap" element="tns:AuthSoap" />
  </wsdl:message>
  <wsdl:message name="WSSVIFingersSoapIn">
    <wsdl:part name="parameters" element="tns:WSSVIFingers" />
  </wsdl:message>
  <wsdl:message name="WSSVIFingersSoapOut">
    <wsdl:part name="parameters" element="tns:WSSVIFingersResponse" />
  </wsdl:message>
  <wsdl:message name="WSSVIFingersAuthSoap">
    <wsdl:part name="AuthSoap" element="tns:AuthSoap" />
  </wsdl:message>
  <wsdl:message name="WSFacialSoapIn">
    <wsdl:part name="parameters" element="tns:WSFacial" />
  </wsdl:message>
  <wsdl:message name="WSFacialSoapOut">
    <wsdl:part name="parameters" element="tns:WSFacialResponse" />
  </wsdl:message>
  <wsdl:message name="PdfSecureSoapIn">
    <wsdl:part name="parameters" element="tns:PdfSecure" />
  </wsdl:message>
  <wsdl:message name="PdfSecureSoapOut">
    <wsdl:part name="parameters" element="tns:PdfSecureResponse" />
  </wsdl:message>
  <wsdl:message name="PdfSecureAuthSoap">
    <wsdl:part name="AuthSoap" element="tns:AuthSoap" />
  </wsdl:message>
  <wsdl:message name="WSTransformSoapIn">
    <wsdl:part name="parameters" element="tns:WSTransform" />
  </wsdl:message>
  <wsdl:message name="WSTransformSoapOut">
    <wsdl:part name="parameters" element="tns:WSTransformResponse" />
  </wsdl:message>
  <wsdl:message name="WSTransformAuthSoap">
    <wsdl:part name="AuthSoap" element="tns:AuthSoap" />
  </wsdl:message>
  <wsdl:message name="WSVerifyXmlSoapIn">
    <wsdl:part name="parameters" element="tns:WSVerifyXml" />
  </wsdl:message>
  <wsdl:message name="WSVerifyXmlSoapOut">
    <wsdl:part name="parameters" element="tns:WSVerifyXmlResponse" />
  </wsdl:message>
  <wsdl:message name="WSVerifyXmlAuthSoap">
    <wsdl:part name="AuthSoap" element="tns:AuthSoap" />
  </wsdl:message>
  <wsdl:portType name="WebServiceSoap">
    <wsdl:operation name="WSReportDigest">
      <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Método SoapHeader</wsdl:documentation>
      <wsdl:input message="tns:WSReportDigestSoapIn" />
      <wsdl:output message="tns:WSReportDigestSoapOut" />
    </wsdl:operation>
    <wsdl:operation name="WSReportPkcs7">
      <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Método SoapHeader</wsdl:documentation>
      <wsdl:input message="tns:WSReportPkcs7SoapIn" />
      <wsdl:output message="tns:WSReportPkcs7SoapOut" />
    </wsdl:operation>
    <wsdl:operation name="WSReportPkcs1">
      <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Método SoapHeader</wsdl:documentation>
      <wsdl:input message="tns:WSReportPkcs1SoapIn" />
      <wsdl:output message="tns:WSReportPkcs1SoapOut" />
    </wsdl:operation>
    <wsdl:operation name="WSSignCentral">
      <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Método SoapHeader</wsdl:documentation>
      <wsdl:input message="tns:WSSignCentralSoapIn" />
      <wsdl:output message="tns:WSSignCentralSoapOut" />
    </wsdl:operation>
    <wsdl:operation name="WSDecodeCertificate">
      <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Método SoapHeader</wsdl:documentation>
      <wsdl:input message="tns:WSDecodeCertificateSoapIn" />
      <wsdl:output message="tns:WSDecodeCertificateSoapOut" />
    </wsdl:operation>
    <wsdl:operation name="WSRequestTsa">
      <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Método SoapHeader</wsdl:documentation>
      <wsdl:input message="tns:WSRequestTsaSoapIn" />
      <wsdl:output message="tns:WSRequestTsaSoapOut" />
    </wsdl:operation>
    <wsdl:operation name="WSRequestNom151">
      <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Método SoapHeader</wsdl:documentation>
      <wsdl:input message="tns:WSRequestNom151SoapIn" />
      <wsdl:output message="tns:WSRequestNom151SoapOut" />
    </wsdl:operation>
    <wsdl:operation name="WSPkcs7InformationFromNs">
      <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Método SoapHeader</wsdl:documentation>
      <wsdl:input message="tns:WSPkcs7InformationFromNsSoapIn" />
      <wsdl:output message="tns:WSPkcs7InformationFromNsSoapOut" />
    </wsdl:operation>
    <wsdl:operation name="WSPkcs7InformationStrFromNs">
      <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Método SoapHeader</wsdl:documentation>
      <wsdl:input message="tns:WSPkcs7InformationStrFromNsSoapIn" />
      <wsdl:output message="tns:WSPkcs7InformationStrFromNsSoapOut" />
    </wsdl:operation>
    <wsdl:operation name="WSInformationOcspFromNs">
      <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Método SoapHeader</wsdl:documentation>
      <wsdl:input message="tns:WSInformationOcspFromNsSoapIn" />
      <wsdl:output message="tns:WSInformationOcspFromNsSoapOut" />
    </wsdl:operation>
    <wsdl:operation name="WSInformationTspFromNs">
      <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Método SoapHeader</wsdl:documentation>
      <wsdl:input message="tns:WSInformationTspFromNsSoapIn" />
      <wsdl:output message="tns:WSInformationTspFromNsSoapOut" />
    </wsdl:operation>
    <wsdl:operation name="WSFromHtmlToPdf">
      <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Método SoapHeader</wsdl:documentation>
      <wsdl:input message="tns:WSFromHtmlToPdfSoapIn" />
      <wsdl:output message="tns:WSFromHtmlToPdfSoapOut" />
    </wsdl:operation>
    <wsdl:operation name="WSFromHtmlToPdfSecure">
      <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Método SoapHeader</wsdl:documentation>
      <wsdl:input message="tns:WSFromHtmlToPdfSecureSoapIn" />
      <wsdl:output message="tns:WSFromHtmlToPdfSecureSoapOut" />
    </wsdl:operation>
    <wsdl:operation name="WSCreatePkcs7FromNs">
      <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Método SoapHeader</wsdl:documentation>
      <wsdl:input message="tns:WSCreatePkcs7FromNsSoapIn" />
      <wsdl:output message="tns:WSCreatePkcs7FromNsSoapOut" />
    </wsdl:operation>
    <wsdl:operation name="WSCreatePkcs7NoDocFromNs">
      <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Método SoapHeader</wsdl:documentation>
      <wsdl:input message="tns:WSCreatePkcs7NoDocFromNsSoapIn" />
      <wsdl:output message="tns:WSCreatePkcs7NoDocFromNsSoapOut" />
    </wsdl:operation>
    <wsdl:operation name="WSCreatePdf">
      <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Método SoapHeader</wsdl:documentation>
      <wsdl:input message="tns:WSCreatePdfSoapIn" />
      <wsdl:output message="tns:WSCreatePdfSoapOut" />
    </wsdl:operation>
    <wsdl:operation name="WSCreatePdfEvidence">
      <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Método SoapHeader</wsdl:documentation>
      <wsdl:input message="tns:WSCreatePdfEvidenceSoapIn" />
      <wsdl:output message="tns:WSCreatePdfEvidenceSoapOut" />
    </wsdl:operation>
    <wsdl:operation name="WSCreatePdfEvidenceSecure">
      <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Método SoapHeader</wsdl:documentation>
      <wsdl:input message="tns:WSCreatePdfEvidenceSecureSoapIn" />
      <wsdl:output message="tns:WSCreatePdfEvidenceSecureSoapOut" />
    </wsdl:operation>
    <wsdl:operation name="WSNom151ForPkcs1Ns">
      <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Método SoapHeader</wsdl:documentation>
      <wsdl:input message="tns:WSNom151ForPkcs1NsSoapIn" />
      <wsdl:output message="tns:WSNom151ForPkcs1NsSoapOut" />
    </wsdl:operation>
    <wsdl:operation name="WSTransfer">
      <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Método SoapHeader</wsdl:documentation>
      <wsdl:input message="tns:WSTransferSoapIn" />
      <wsdl:output message="tns:WSTransferSoapOut" />
    </wsdl:operation>
    <wsdl:operation name="WSDownloadCertificate">
      <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Método SoapHeader</wsdl:documentation>
      <wsdl:input message="tns:WSDownloadCertificateSoapIn" />
      <wsdl:output message="tns:WSDownloadCertificateSoapOut" />
    </wsdl:operation>
    <wsdl:operation name="WSACRegisterPkcs10">
      <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Método SoapHeader</wsdl:documentation>
      <wsdl:input message="tns:WSACRegisterPkcs10SoapIn" />
      <wsdl:output message="tns:WSACRegisterPkcs10SoapOut" />
    </wsdl:operation>
    <wsdl:operation name="WSACDownloadX509">
      <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Método SoapHeader</wsdl:documentation>
      <wsdl:input message="tns:WSACDownloadX509SoapIn" />
      <wsdl:output message="tns:WSACDownloadX509SoapOut" />
    </wsdl:operation>
    <wsdl:operation name="WSACAutoRevocateX509">
      <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Método SoapHeader</wsdl:documentation>
      <wsdl:input message="tns:WSACAutoRevocateX509SoapIn" />
      <wsdl:output message="tns:WSACAutoRevocateX509SoapOut" />
    </wsdl:operation>
    <wsdl:operation name="WSSVIRequest">
      <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Método SoapHeader</wsdl:documentation>
      <wsdl:input message="tns:WSSVIRequestSoapIn" />
      <wsdl:output message="tns:WSSVIRequestSoapOut" />
    </wsdl:operation>
    <wsdl:operation name="WSSVIFingers">
      <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Método SoapHeader</wsdl:documentation>
      <wsdl:input message="tns:WSSVIFingersSoapIn" />
      <wsdl:output message="tns:WSSVIFingersSoapOut" />
    </wsdl:operation>
    <wsdl:operation name="WSFacial">
      <wsdl:input message="tns:WSFacialSoapIn" />
      <wsdl:output message="tns:WSFacialSoapOut" />
    </wsdl:operation>
    <wsdl:operation name="PdfSecure">
      <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Método SoapHeader</wsdl:documentation>
      <wsdl:input message="tns:PdfSecureSoapIn" />
      <wsdl:output message="tns:PdfSecureSoapOut" />
    </wsdl:operation>
    <wsdl:operation name="WSTransform">
      <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Método SoapHeader</wsdl:documentation>
      <wsdl:input message="tns:WSTransformSoapIn" />
      <wsdl:output message="tns:WSTransformSoapOut" />
    </wsdl:operation>
    <wsdl:operation name="WSVerifyXml">
      <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Método SoapHeader</wsdl:documentation>
      <wsdl:input message="tns:WSVerifyXmlSoapIn" />
      <wsdl:output message="tns:WSVerifyXmlSoapOut" />
    </wsdl:operation>
  </wsdl:portType>
  <wsdl:binding name="WebServiceSoap" type="tns:WebServiceSoap">
    <soap:binding transport="http://schemas.xmlsoap.org/soap/http" />
    <wsdl:operation name="WSReportDigest">
      <soap:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSReportDigest" style="document" />
      <wsdl:input>
        <soap:body use="literal" />
        <soap:header message="tns:WSReportDigestAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSReportPkcs7">
      <soap:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSReportPkcs7" style="document" />
      <wsdl:input>
        <soap:body use="literal" />
        <soap:header message="tns:WSReportPkcs7AuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSReportPkcs1">
      <soap:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSReportPkcs1" style="document" />
      <wsdl:input>
        <soap:body use="literal" />
        <soap:header message="tns:WSReportPkcs1AuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSSignCentral">
      <soap:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSSignCentral" style="document" />
      <wsdl:input>
        <soap:body use="literal" />
        <soap:header message="tns:WSSignCentralAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSDecodeCertificate">
      <soap:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSDecodeCertificate" style="document" />
      <wsdl:input>
        <soap:body use="literal" />
        <soap:header message="tns:WSDecodeCertificateAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSRequestTsa">
      <soap:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSRequestTsa" style="document" />
      <wsdl:input>
        <soap:body use="literal" />
        <soap:header message="tns:WSRequestTsaAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSRequestNom151">
      <soap:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSRequestNom151" style="document" />
      <wsdl:input>
        <soap:body use="literal" />
        <soap:header message="tns:WSRequestNom151AuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSPkcs7InformationFromNs">
      <soap:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSPkcs7InformationFromNs" style="document" />
      <wsdl:input>
        <soap:body use="literal" />
        <soap:header message="tns:WSPkcs7InformationFromNsAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSPkcs7InformationStrFromNs">
      <soap:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSPkcs7InformationStrFromNs" style="document" />
      <wsdl:input>
        <soap:body use="literal" />
        <soap:header message="tns:WSPkcs7InformationStrFromNsAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSInformationOcspFromNs">
      <soap:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSInformationOcspFromNs" style="document" />
      <wsdl:input>
        <soap:body use="literal" />
        <soap:header message="tns:WSInformationOcspFromNsAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSInformationTspFromNs">
      <soap:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSInformationTspFromNs" style="document" />
      <wsdl:input>
        <soap:body use="literal" />
        <soap:header message="tns:WSInformationTspFromNsAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSFromHtmlToPdf">
      <soap:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSFromHtmlToPdf" style="document" />
      <wsdl:input>
        <soap:body use="literal" />
        <soap:header message="tns:WSFromHtmlToPdfAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSFromHtmlToPdfSecure">
      <soap:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSFromHtmlToPdfSecure" style="document" />
      <wsdl:input>
        <soap:body use="literal" />
        <soap:header message="tns:WSFromHtmlToPdfSecureAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSCreatePkcs7FromNs">
      <soap:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSCreatePkcs7FromNs" style="document" />
      <wsdl:input>
        <soap:body use="literal" />
        <soap:header message="tns:WSCreatePkcs7FromNsAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSCreatePkcs7NoDocFromNs">
      <soap:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSCreatePkcs7NoDocFromNs" style="document" />
      <wsdl:input>
        <soap:body use="literal" />
        <soap:header message="tns:WSCreatePkcs7NoDocFromNsAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSCreatePdf">
      <soap:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSCreatePdf" style="document" />
      <wsdl:input>
        <soap:body use="literal" />
        <soap:header message="tns:WSCreatePdfAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSCreatePdfEvidence">
      <soap:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSCreatePdfEvidence" style="document" />
      <wsdl:input>
        <soap:body use="literal" />
        <soap:header message="tns:WSCreatePdfEvidenceAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSCreatePdfEvidenceSecure">
      <soap:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSCreatePdfEvidenceSecure" style="document" />
      <wsdl:input>
        <soap:body use="literal" />
        <soap:header message="tns:WSCreatePdfEvidenceSecureAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSNom151ForPkcs1Ns">
      <soap:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSNom151ForPkcs1Ns" style="document" />
      <wsdl:input>
        <soap:body use="literal" />
        <soap:header message="tns:WSNom151ForPkcs1NsAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSTransfer">
      <soap:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSTransfer" style="document" />
      <wsdl:input>
        <soap:body use="literal" />
        <soap:header message="tns:WSTransferAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSDownloadCertificate">
      <soap:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSDownloadCertificate" style="document" />
      <wsdl:input>
        <soap:body use="literal" />
        <soap:header message="tns:WSDownloadCertificateAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSACRegisterPkcs10">
      <soap:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSACRegisterPkcs10" style="document" />
      <wsdl:input>
        <soap:body use="literal" />
        <soap:header message="tns:WSACRegisterPkcs10AuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSACDownloadX509">
      <soap:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSACDownloadX509" style="document" />
      <wsdl:input>
        <soap:body use="literal" />
        <soap:header message="tns:WSACDownloadX509AuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSACAutoRevocateX509">
      <soap:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSACAutoRevocateX509" style="document" />
      <wsdl:input>
        <soap:body use="literal" />
        <soap:header message="tns:WSACAutoRevocateX509AuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSSVIRequest">
      <soap:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSSVIRequest" style="document" />
      <wsdl:input>
        <soap:body use="literal" />
        <soap:header message="tns:WSSVIRequestAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSSVIFingers">
      <soap:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSSVIFingers" style="document" />
      <wsdl:input>
        <soap:body use="literal" />
        <soap:header message="tns:WSSVIFingersAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSFacial">
      <soap:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSFacial" style="document" />
      <wsdl:input>
        <soap:body use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="PdfSecure">
      <soap:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/PdfSecure" style="document" />
      <wsdl:input>
        <soap:body use="literal" />
        <soap:header message="tns:PdfSecureAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSTransform">
      <soap:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSTransform" style="document" />
      <wsdl:input>
        <soap:body use="literal" />
        <soap:header message="tns:WSTransformAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSVerifyXml">
      <soap:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSVerifyXml" style="document" />
      <wsdl:input>
        <soap:body use="literal" />
        <soap:header message="tns:WSVerifyXmlAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
  </wsdl:binding>
  <wsdl:binding name="WebServiceSoap12" type="tns:WebServiceSoap">
    <soap12:binding transport="http://schemas.xmlsoap.org/soap/http" />
    <wsdl:operation name="WSReportDigest">
      <soap12:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSReportDigest" style="document" />
      <wsdl:input>
        <soap12:body use="literal" />
        <soap12:header message="tns:WSReportDigestAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSReportPkcs7">
      <soap12:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSReportPkcs7" style="document" />
      <wsdl:input>
        <soap12:body use="literal" />
        <soap12:header message="tns:WSReportPkcs7AuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSReportPkcs1">
      <soap12:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSReportPkcs1" style="document" />
      <wsdl:input>
        <soap12:body use="literal" />
        <soap12:header message="tns:WSReportPkcs1AuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSSignCentral">
      <soap12:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSSignCentral" style="document" />
      <wsdl:input>
        <soap12:body use="literal" />
        <soap12:header message="tns:WSSignCentralAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSDecodeCertificate">
      <soap12:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSDecodeCertificate" style="document" />
      <wsdl:input>
        <soap12:body use="literal" />
        <soap12:header message="tns:WSDecodeCertificateAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSRequestTsa">
      <soap12:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSRequestTsa" style="document" />
      <wsdl:input>
        <soap12:body use="literal" />
        <soap12:header message="tns:WSRequestTsaAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSRequestNom151">
      <soap12:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSRequestNom151" style="document" />
      <wsdl:input>
        <soap12:body use="literal" />
        <soap12:header message="tns:WSRequestNom151AuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSPkcs7InformationFromNs">
      <soap12:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSPkcs7InformationFromNs" style="document" />
      <wsdl:input>
        <soap12:body use="literal" />
        <soap12:header message="tns:WSPkcs7InformationFromNsAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSPkcs7InformationStrFromNs">
      <soap12:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSPkcs7InformationStrFromNs" style="document" />
      <wsdl:input>
        <soap12:body use="literal" />
        <soap12:header message="tns:WSPkcs7InformationStrFromNsAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSInformationOcspFromNs">
      <soap12:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSInformationOcspFromNs" style="document" />
      <wsdl:input>
        <soap12:body use="literal" />
        <soap12:header message="tns:WSInformationOcspFromNsAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSInformationTspFromNs">
      <soap12:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSInformationTspFromNs" style="document" />
      <wsdl:input>
        <soap12:body use="literal" />
        <soap12:header message="tns:WSInformationTspFromNsAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSFromHtmlToPdf">
      <soap12:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSFromHtmlToPdf" style="document" />
      <wsdl:input>
        <soap12:body use="literal" />
        <soap12:header message="tns:WSFromHtmlToPdfAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSFromHtmlToPdfSecure">
      <soap12:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSFromHtmlToPdfSecure" style="document" />
      <wsdl:input>
        <soap12:body use="literal" />
        <soap12:header message="tns:WSFromHtmlToPdfSecureAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSCreatePkcs7FromNs">
      <soap12:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSCreatePkcs7FromNs" style="document" />
      <wsdl:input>
        <soap12:body use="literal" />
        <soap12:header message="tns:WSCreatePkcs7FromNsAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSCreatePkcs7NoDocFromNs">
      <soap12:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSCreatePkcs7NoDocFromNs" style="document" />
      <wsdl:input>
        <soap12:body use="literal" />
        <soap12:header message="tns:WSCreatePkcs7NoDocFromNsAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSCreatePdf">
      <soap12:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSCreatePdf" style="document" />
      <wsdl:input>
        <soap12:body use="literal" />
        <soap12:header message="tns:WSCreatePdfAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSCreatePdfEvidence">
      <soap12:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSCreatePdfEvidence" style="document" />
      <wsdl:input>
        <soap12:body use="literal" />
        <soap12:header message="tns:WSCreatePdfEvidenceAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSCreatePdfEvidenceSecure">
      <soap12:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSCreatePdfEvidenceSecure" style="document" />
      <wsdl:input>
        <soap12:body use="literal" />
        <soap12:header message="tns:WSCreatePdfEvidenceSecureAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSNom151ForPkcs1Ns">
      <soap12:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSNom151ForPkcs1Ns" style="document" />
      <wsdl:input>
        <soap12:body use="literal" />
        <soap12:header message="tns:WSNom151ForPkcs1NsAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSTransfer">
      <soap12:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSTransfer" style="document" />
      <wsdl:input>
        <soap12:body use="literal" />
        <soap12:header message="tns:WSTransferAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSDownloadCertificate">
      <soap12:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSDownloadCertificate" style="document" />
      <wsdl:input>
        <soap12:body use="literal" />
        <soap12:header message="tns:WSDownloadCertificateAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSACRegisterPkcs10">
      <soap12:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSACRegisterPkcs10" style="document" />
      <wsdl:input>
        <soap12:body use="literal" />
        <soap12:header message="tns:WSACRegisterPkcs10AuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSACDownloadX509">
      <soap12:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSACDownloadX509" style="document" />
      <wsdl:input>
        <soap12:body use="literal" />
        <soap12:header message="tns:WSACDownloadX509AuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSACAutoRevocateX509">
      <soap12:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSACAutoRevocateX509" style="document" />
      <wsdl:input>
        <soap12:body use="literal" />
        <soap12:header message="tns:WSACAutoRevocateX509AuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSSVIRequest">
      <soap12:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSSVIRequest" style="document" />
      <wsdl:input>
        <soap12:body use="literal" />
        <soap12:header message="tns:WSSVIRequestAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSSVIFingers">
      <soap12:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSSVIFingers" style="document" />
      <wsdl:input>
        <soap12:body use="literal" />
        <soap12:header message="tns:WSSVIFingersAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSFacial">
      <soap12:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSFacial" style="document" />
      <wsdl:input>
        <soap12:body use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="PdfSecure">
      <soap12:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/PdfSecure" style="document" />
      <wsdl:input>
        <soap12:body use="literal" />
        <soap12:header message="tns:PdfSecureAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSTransform">
      <soap12:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSTransform" style="document" />
      <wsdl:input>
        <soap12:body use="literal" />
        <soap12:header message="tns:WSTransformAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="WSVerifyXml">
      <soap12:operation soapAction="www.XMLWebServiceSoapHeaderAuth.net/WSVerifyXml" style="document" />
      <wsdl:input>
        <soap12:body use="literal" />
        <soap12:header message="tns:WSVerifyXmlAuthSoap" part="AuthSoap" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
  </wsdl:binding>
  <wsdl:service name="WebService">
    <wsdl:port name="WebServiceSoap" binding="tns:WebServiceSoap">
      <soap:address location="http://sfirmapruebas.sonora.gob.mx/WSPSFIEL/webservice.asmx" />
    </wsdl:port>
    <wsdl:port name="WebServiceSoap12" binding="tns:WebServiceSoap12">
      <soap12:address location="http://sfirmapruebas.sonora.gob.mx/WSPSFIEL/webservice.asmx" />
    </wsdl:port>
  </wsdl:service>
</wsdl:definitions>