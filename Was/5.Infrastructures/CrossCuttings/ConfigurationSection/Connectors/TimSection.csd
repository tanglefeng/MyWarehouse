<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="ba9376be-d3a6-4e5d-a3d2-d6413bb2d318" namespace="Kengic.Was.CrossCutting.ConfigurationSection.Connectors" xmlSchemaNamespace="urn:Kengic.Was.CrossCutting.ConfigurationSection.Connectors" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
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
    <configurationSection name="TimSection" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="timSection">
      <elementProperties>
        <elementProperty name="TimBodys" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="timBodys" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/ba9376be-d3a6-4e5d-a3d2-d6413bb2d318/TimBodyElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElement name="TimBodyElement">
      <attributeProperties>
        <attributeProperty name="Id" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="id" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/ba9376be-d3a6-4e5d-a3d2-d6413bb2d318/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Name" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/ba9376be-d3a6-4e5d-a3d2-d6413bb2d318/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Description" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="description" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/ba9376be-d3a6-4e5d-a3d2-d6413bb2d318/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Length" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="length" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/ba9376be-d3a6-4e5d-a3d2-d6413bb2d318/Int32" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <elementProperties>
        <elementProperty name="TimBodyFields" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="timBodyFields" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/ba9376be-d3a6-4e5d-a3d2-d6413bb2d318/TimBodyFieldElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElementCollection name="TimBodyElementCollection" xmlItemName="timBodyElement" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/ba9376be-d3a6-4e5d-a3d2-d6413bb2d318/TimBodyElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElementCollection name="TimBodyFieldElementCollection" xmlItemName="timBodyFieldElement" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/ba9376be-d3a6-4e5d-a3d2-d6413bb2d318/TimBodyFieldElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="TimBodyFieldElement">
      <attributeProperties>
        <attributeProperty name="Id" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="id" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/ba9376be-d3a6-4e5d-a3d2-d6413bb2d318/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Type" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="type" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/ba9376be-d3a6-4e5d-a3d2-d6413bb2d318/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="StartAddress" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="startAddress" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/ba9376be-d3a6-4e5d-a3d2-d6413bb2d318/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="Length" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="length" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/ba9376be-d3a6-4e5d-a3d2-d6413bb2d318/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="SequenceNo" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="sequenceNo" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/ba9376be-d3a6-4e5d-a3d2-d6413bb2d318/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="MapName" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="mapName" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/ba9376be-d3a6-4e5d-a3d2-d6413bb2d318/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>