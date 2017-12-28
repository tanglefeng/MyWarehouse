<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="65220175-6ade-46be-9afb-8f518f57dd89" namespace="Kengic.Was.CrossCutting.ConfigurationSection.Operators" xmlSchemaNamespace="urn:Kengic.Was.CrossCutting.ConfigurationSection.Operators" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
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
    <externalType name="StartupType" namespace="Kengic.Was.CrossCutting.ConfigurationSection" />
  </typeDefinitions>
  <configurationElements>
    <configurationSection name="OperatorSection" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="operatorSection">
      <elementProperties>
        <elementProperty name="Operators" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="operators" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/65220175-6ade-46be-9afb-8f518f57dd89/OperatorElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElement name="OperatorElement">
      <attributeProperties>
        <attributeProperty name="Id" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="id" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/65220175-6ade-46be-9afb-8f518f57dd89/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Name" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/65220175-6ade-46be-9afb-8f518f57dd89/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Description" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="description" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/65220175-6ade-46be-9afb-8f518f57dd89/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Type" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="type" isReadOnly="false" typeConverter="TypeNameConverter">
          <type>
            <externalTypeMoniker name="/65220175-6ade-46be-9afb-8f518f57dd89/Type" />
          </type>
        </attributeProperty>
        <attributeProperty name="StartupType" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="startupType" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/65220175-6ade-46be-9afb-8f518f57dd89/StartupType" />
          </type>
        </attributeProperty>
        <attributeProperty name="StartSequence" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="startSequence" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/65220175-6ade-46be-9afb-8f518f57dd89/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="StopSequence" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="stopSequence" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/65220175-6ade-46be-9afb-8f518f57dd89/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="LogName" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="logName" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/65220175-6ade-46be-9afb-8f518f57dd89/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="FilePath" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="filePath" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/65220175-6ade-46be-9afb-8f518f57dd89/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="SectionName" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="sectionName" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/65220175-6ade-46be-9afb-8f518f57dd89/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Comments" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="comments" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/65220175-6ade-46be-9afb-8f518f57dd89/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElementCollection name="OperatorElementCollection" xmlItemName="operatorElement" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/65220175-6ade-46be-9afb-8f518f57dd89/OperatorElement" />
      </itemType>
    </configurationElementCollection>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>