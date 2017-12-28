<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="4fc98919-1bcb-4d6f-ae91-5fc62cb01f43" namespace="Kengic.Was.CrossCutting.ConfigurationSection.FileConfigs" xmlSchemaNamespace="urn:Kengic.Was.CrossCutting.ConfigurationSection.FileConfigs" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
  <typeDefinitions>
    <externalType name="String" namespace="System" />
    <externalType name="Boolean" namespace="System" />
    <externalType name="Int32" namespace="System" />
    <externalType name="Int64" namespace="System" />
    <externalType name="Single" namespace="System" />
    <externalType name="Double" namespace="System" />
    <externalType name="DateTime" namespace="System" />
    <externalType name="TimeSpan" namespace="System" />
  </typeDefinitions>
  <configurationElements>
    <configurationSection name="FileConfigSection" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="fileConfigSection">
      <elementProperties>
        <elementProperty name="FileConfigs" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="fileConfigs" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/4fc98919-1bcb-4d6f-ae91-5fc62cb01f43/FileConfigElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElement name="FileConfigElement">
      <attributeProperties>
        <attributeProperty name="Id" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="id" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/4fc98919-1bcb-4d6f-ae91-5fc62cb01f43/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Name" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/4fc98919-1bcb-4d6f-ae91-5fc62cb01f43/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Code" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="code" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/4fc98919-1bcb-4d6f-ae91-5fc62cb01f43/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="FilePath" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="filePath" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/4fc98919-1bcb-4d6f-ae91-5fc62cb01f43/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="SectionName" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="sectionName" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/4fc98919-1bcb-4d6f-ae91-5fc62cb01f43/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElementCollection name="FileConfigElementCollection" xmlItemName="fileConfigElement" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/4fc98919-1bcb-4d6f-ae91-5fc62cb01f43/FileConfigElement" />
      </itemType>
    </configurationElementCollection>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>