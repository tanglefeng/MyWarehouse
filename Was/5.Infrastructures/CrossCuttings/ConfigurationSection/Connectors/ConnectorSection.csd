<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="4b7d3519-2d74-451c-919f-c5fd4cbf8c79" namespace="Kengic.Was.CrossCutting.ConfigurationSection.Connectors" xmlSchemaNamespace="urn:Kengic.Was.CrossCutting.ConfigurationSection.Connectors" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
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
    <configurationSection name="ConnectorSection" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="connectorSection">
      <elementProperties>
        <elementProperty name="Connectors" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="connectors" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/4b7d3519-2d74-451c-919f-c5fd4cbf8c79/ConnectorElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElement name="ConnectorElement">
      <attributeProperties>
        <attributeProperty name="Id" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="id" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/4b7d3519-2d74-451c-919f-c5fd4cbf8c79/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Name" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/4b7d3519-2d74-451c-919f-c5fd4cbf8c79/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Code" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="code" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/4b7d3519-2d74-451c-919f-c5fd4cbf8c79/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Description" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="description" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/4b7d3519-2d74-451c-919f-c5fd4cbf8c79/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="StartupType" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="startupType" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/4b7d3519-2d74-451c-919f-c5fd4cbf8c79/StartupType" />
          </type>
        </attributeProperty>
        <attributeProperty name="StartSequence" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="startSequence" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/4b7d3519-2d74-451c-919f-c5fd4cbf8c79/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="StopSequence" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="stopSequence" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/4b7d3519-2d74-451c-919f-c5fd4cbf8c79/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="LogName" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="logName" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/4b7d3519-2d74-451c-919f-c5fd4cbf8c79/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Type" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="type" isReadOnly="false" typeConverter="TypeNameConverter">
          <type>
            <externalTypeMoniker name="/4b7d3519-2d74-451c-919f-c5fd4cbf8c79/Type" />
          </type>
        </attributeProperty>
        <attributeProperty name="TimMessageLength" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="timMessageLength" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/4b7d3519-2d74-451c-919f-c5fd4cbf8c79/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="TimType" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="timType" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/4b7d3519-2d74-451c-919f-c5fd4cbf8c79/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <elementProperties>
        <elementProperty name="Connection" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="connection" isReadOnly="false">
          <type>
            <configurationElementMoniker name="/4b7d3519-2d74-451c-919f-c5fd4cbf8c79/ConnectionElement" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElementCollection name="ConnectorElementCollection" xmlItemName="connectorElement" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/4b7d3519-2d74-451c-919f-c5fd4cbf8c79/ConnectorElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="ConnectionInformationElement">
      <attributeProperties>
        <attributeProperty name="Id" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="id" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/4b7d3519-2d74-451c-919f-c5fd4cbf8c79/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Name" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/4b7d3519-2d74-451c-919f-c5fd4cbf8c79/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Node" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="node" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/4b7d3519-2d74-451c-919f-c5fd4cbf8c79/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Ip" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="ip" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/4b7d3519-2d74-451c-919f-c5fd4cbf8c79/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Port" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="port" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/4b7d3519-2d74-451c-919f-c5fd4cbf8c79/Int32" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElement name="ConnectionElement">
      <attributeProperties>
        <attributeProperty name="Id" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="id" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/4b7d3519-2d74-451c-919f-c5fd4cbf8c79/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Name" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/4b7d3519-2d74-451c-919f-c5fd4cbf8c79/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Code" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="code" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/4b7d3519-2d74-451c-919f-c5fd4cbf8c79/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Description" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="description" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/4b7d3519-2d74-451c-919f-c5fd4cbf8c79/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Version" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="version" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/4b7d3519-2d74-451c-919f-c5fd4cbf8c79/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Protocol" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="protocol" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/4b7d3519-2d74-451c-919f-c5fd4cbf8c79/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="ConnectTimeOut" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="connectTimeOut" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/4b7d3519-2d74-451c-919f-c5fd4cbf8c79/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="FilePath" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="filePath" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/4b7d3519-2d74-451c-919f-c5fd4cbf8c79/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="SectionName" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="sectionName" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/4b7d3519-2d74-451c-919f-c5fd4cbf8c79/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <elementProperties>
        <elementProperty name="Local" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="local" isReadOnly="false">
          <type>
            <configurationElementMoniker name="/4b7d3519-2d74-451c-919f-c5fd4cbf8c79/ConnectionInformationElement" />
          </type>
        </elementProperty>
        <elementProperty name="Remote" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="remote" isReadOnly="false">
          <type>
            <configurationElementMoniker name="/4b7d3519-2d74-451c-919f-c5fd4cbf8c79/ConnectionInformationElement" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>