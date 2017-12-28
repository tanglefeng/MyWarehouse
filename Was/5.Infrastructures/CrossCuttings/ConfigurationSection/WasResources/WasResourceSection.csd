<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="d61034ea-7cbb-4bb2-a072-fedae86e980f" namespace="Kengic.Was.CrossCutting.ConfigurationSection.WasResources" xmlSchemaNamespace="urn:Kengic.Was.CrossCutting.ConfigurationSection.WasResources" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
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
    <configurationSection name="WasResourceSection" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="wasResourceSection">
      <elementProperties>
        <elementProperty name="WasResources" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="wasResources" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/d61034ea-7cbb-4bb2-a072-fedae86e980f/WasResourceElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElementCollection name="WasResourceElementCollection" xmlItemName="wasResourceElement" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/d61034ea-7cbb-4bb2-a072-fedae86e980f/WasResourceElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="WasResourceElement">
      <attributeProperties>
        <attributeProperty name="Id" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="id" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d61034ea-7cbb-4bb2-a072-fedae86e980f/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Code" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="code" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d61034ea-7cbb-4bb2-a072-fedae86e980f/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Name" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d61034ea-7cbb-4bb2-a072-fedae86e980f/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Description" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="description" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d61034ea-7cbb-4bb2-a072-fedae86e980f/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="AssemblyName" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="assemblyName" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d61034ea-7cbb-4bb2-a072-fedae86e980f/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="FactoryName" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="factoryName" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d61034ea-7cbb-4bb2-a072-fedae86e980f/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="ModuleName" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="moduleName" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d61034ea-7cbb-4bb2-a072-fedae86e980f/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="IfAutoStart" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="ifAutoStart" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d61034ea-7cbb-4bb2-a072-fedae86e980f/Boolean" />
          </type>
        </attributeProperty>
        <attributeProperty name="StartSequence" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="startSequence" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d61034ea-7cbb-4bb2-a072-fedae86e980f/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="StopSequence" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="stopSequence" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d61034ea-7cbb-4bb2-a072-fedae86e980f/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="LogName" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="logName" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d61034ea-7cbb-4bb2-a072-fedae86e980f/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Status" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="status" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d61034ea-7cbb-4bb2-a072-fedae86e980f/Boolean" />
          </type>
        </attributeProperty>
        <attributeProperty name="Comments" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="comments" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d61034ea-7cbb-4bb2-a072-fedae86e980f/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>