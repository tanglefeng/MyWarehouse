<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="b37af95c-8f4e-49b9-b47a-2bf9cd2e5bc8" namespace="Kengic.Was.CrossCutting.ConfigurationSection.TypeFormatters" xmlSchemaNamespace="urn:Kengic.Was.CrossCutting.ConfigurationSection.TypeFormatters" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
  <typeDefinitions>
    <externalType name="String" namespace="System" />
    <externalType name="Boolean" namespace="System" />
    <externalType name="Int32" namespace="System" />
    <externalType name="Int64" namespace="System" />
    <externalType name="Single" namespace="System" />
    <externalType name="Double" namespace="System" />
    <externalType name="DateTime" namespace="System" />
    <externalType name="TimeSpan" namespace="System" />
    <externalType name="Type" namespace="System" />
  </typeDefinitions>
  <configurationElements>
    <configurationSection name="TypeFormatterSection" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="typeFormatterSection">
      <elementProperties>
        <elementProperty name="TypeFormatters" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="typeFormatters" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/b37af95c-8f4e-49b9-b47a-2bf9cd2e5bc8/TypeFormatterElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElement name="TypeFormatterElement">
      <attributeProperties>
        <attributeProperty name="Type" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="type" isReadOnly="false" typeConverter="TypeNameConverter">
          <type>
            <externalTypeMoniker name="/b37af95c-8f4e-49b9-b47a-2bf9cd2e5bc8/Type" />
          </type>
        </attributeProperty>
        <attributeProperty name="Expression" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="expression" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/b37af95c-8f4e-49b9-b47a-2bf9cd2e5bc8/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElementCollection name="TypeFormatterElementCollection" xmlItemName="TypeFormatterElement" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/b37af95c-8f4e-49b9-b47a-2bf9cd2e5bc8/TypeFormatterElement" />
      </itemType>
    </configurationElementCollection>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>