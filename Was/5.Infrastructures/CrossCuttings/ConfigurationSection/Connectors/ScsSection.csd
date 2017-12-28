<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="a07143b5-19ad-4cf8-b644-6f656d829b33" namespace="Kengic.Was.CrossCutting.ConfigurationSection.Connectors" xmlSchemaNamespace="urn:Kengic.Was.CrossCutting.ConfigurationSection.Connectors" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
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
    <configurationSection name="ScsSection" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="scsSection">
      <elementProperties>
        <elementProperty name="ScsBodys" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="scsBodys" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/a07143b5-19ad-4cf8-b644-6f656d829b33/ScsBodyElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElement name="ScsBodyElement">
      <attributeProperties>
        <attributeProperty name="Id" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="id" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/a07143b5-19ad-4cf8-b644-6f656d829b33/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Name" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/a07143b5-19ad-4cf8-b644-6f656d829b33/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Description" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="description" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/a07143b5-19ad-4cf8-b644-6f656d829b33/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Length" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="length" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/a07143b5-19ad-4cf8-b644-6f656d829b33/Int32" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <elementProperties>
        <elementProperty name="ScsBodyFields" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="scsBodyFields" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/a07143b5-19ad-4cf8-b644-6f656d829b33/ScsBodyFieldElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElementCollection name="ScsBodyElementCollection" xmlItemName="scsBodyElement" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/a07143b5-19ad-4cf8-b644-6f656d829b33/ScsBodyElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElementCollection name="ScsBodyFieldElementCollection" xmlItemName="scsBodyFieldElement" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/a07143b5-19ad-4cf8-b644-6f656d829b33/ScsBodyFieldElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="ScsBodyFieldElement">
      <attributeProperties>
        <attributeProperty name="Id" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="id" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/a07143b5-19ad-4cf8-b644-6f656d829b33/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Type" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="type" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/a07143b5-19ad-4cf8-b644-6f656d829b33/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="StartAddress" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="startAddress" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/a07143b5-19ad-4cf8-b644-6f656d829b33/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="Length" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="length" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/a07143b5-19ad-4cf8-b644-6f656d829b33/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="SequenceNo" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="sequenceNo" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/a07143b5-19ad-4cf8-b644-6f656d829b33/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="MapName" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="mapName" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/a07143b5-19ad-4cf8-b644-6f656d829b33/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>