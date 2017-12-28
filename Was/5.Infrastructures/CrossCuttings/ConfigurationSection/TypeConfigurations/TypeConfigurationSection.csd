<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="541812ac-3f94-4c9f-8614-eebdfd35715f" namespace="Kengic.Was.CrossCutting.ConfigurationSection.TypeConfigurations" xmlSchemaNamespace="urn:Kengic.Was.CrossCutting.ConfigurationSection.TypeConfigurations" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
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
    <configurationSection name="TypeConfigurationSection" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="typeConfigurationSection">
      <elementProperties>
        <elementProperty name="TypeConfigurations" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="typeConfigurations" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/541812ac-3f94-4c9f-8614-eebdfd35715f/TypeConfigurationElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElementCollection name="TypeConfigurationElementCollection" xmlItemName="typeConfigurationElement" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/541812ac-3f94-4c9f-8614-eebdfd35715f/TypeConfigurationElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="TypeConfigurationElement">
      <attributeProperties>
        <attributeProperty name="Id" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="id" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/541812ac-3f94-4c9f-8614-eebdfd35715f/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Code" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="code" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/541812ac-3f94-4c9f-8614-eebdfd35715f/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Name" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/541812ac-3f94-4c9f-8614-eebdfd35715f/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Type" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="type" isReadOnly="false" typeConverter="TypeNameConverter">
          <type>
            <externalTypeMoniker name="/541812ac-3f94-4c9f-8614-eebdfd35715f/Type" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>