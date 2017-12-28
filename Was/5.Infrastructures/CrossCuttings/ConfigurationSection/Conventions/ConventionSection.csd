<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="dd07b9c1-8db6-464d-999c-f9f33b7de1f8" namespace="Kengic.Was.CrossCutting.ConfigurationSection.Conventions" xmlSchemaNamespace="urn:Kengic.Was.CrossCutting.ConfigurationSection.Conventions" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
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
    <configurationSection name="ConventionSection" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="conventionSection">
      <elementProperties>
        <elementProperty name="Conventions" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="conventions" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/dd07b9c1-8db6-464d-999c-f9f33b7de1f8/ConventionElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElementCollection name="ConventionElementCollection" xmlItemName="conventionElement" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/dd07b9c1-8db6-464d-999c-f9f33b7de1f8/ConventionElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="ConventionElement">
      <attributeProperties>
        <attributeProperty name="Id" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="id" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/dd07b9c1-8db6-464d-999c-f9f33b7de1f8/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Code" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="code" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/dd07b9c1-8db6-464d-999c-f9f33b7de1f8/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Name" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/dd07b9c1-8db6-464d-999c-f9f33b7de1f8/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Type" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="type" isReadOnly="false" typeConverter="TypeNameConverter">
          <type>
            <externalTypeMoniker name="/dd07b9c1-8db6-464d-999c-f9f33b7de1f8/Type" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>