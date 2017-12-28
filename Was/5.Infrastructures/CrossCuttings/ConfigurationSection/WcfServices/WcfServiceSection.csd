<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="f35b6dc3-d2de-4981-9d27-86dfb2a8280a" namespace="Kengic.Was.CrossCutting.ConfigurationSection.WcfServices" xmlSchemaNamespace="urn:Kengic.Was.CrossCutting.ConfigurationSection.WcfServices" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
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
    <configurationSection name="WcfServiceSection" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="wcfServiceSection">
      <elementProperties>
        <elementProperty name="WcfServices" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="wcfServices" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/f35b6dc3-d2de-4981-9d27-86dfb2a8280a/WcfServiceElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElement name="WcfServiceElement">
      <attributeProperties>
        <attributeProperty name="Id" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="id" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/f35b6dc3-d2de-4981-9d27-86dfb2a8280a/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Name" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/f35b6dc3-d2de-4981-9d27-86dfb2a8280a/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Description" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="description" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/f35b6dc3-d2de-4981-9d27-86dfb2a8280a/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="StartupType" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="startupType" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/f35b6dc3-d2de-4981-9d27-86dfb2a8280a/StartupType" />
          </type>
        </attributeProperty>
        <attributeProperty name="ServiceType" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="serviceType" isReadOnly="false" typeConverter="TypeNameConverter">
          <type>
            <externalTypeMoniker name="/f35b6dc3-d2de-4981-9d27-86dfb2a8280a/Type" />
          </type>
        </attributeProperty>
        <attributeProperty name="FilePath" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="filePath" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/f35b6dc3-d2de-4981-9d27-86dfb2a8280a/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElementCollection name="WcfServiceElementCollection" xmlItemName="wcfServiceElement" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/f35b6dc3-d2de-4981-9d27-86dfb2a8280a/WcfServiceElement" />
      </itemType>
    </configurationElementCollection>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>