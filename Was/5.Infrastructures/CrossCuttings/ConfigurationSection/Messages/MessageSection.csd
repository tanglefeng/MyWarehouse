<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="7260220c-f4c2-4d77-869e-00641a869138" namespace="Kengic.Was.CrossCutting.ConfigurationSection.Messages" xmlSchemaNamespace="urn:Kengic.Was.CrossCutting.ConfigurationSection.Messages" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
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
    <configurationSection name="MessageSection" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="messageSection">
      <elementProperties>
        <elementProperty name="Messages" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="messages" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/7260220c-f4c2-4d77-869e-00641a869138/MessageElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElement name="MessageElement">
      <attributeProperties>
        <attributeProperty name="Id" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="id" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/7260220c-f4c2-4d77-869e-00641a869138/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Name" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/7260220c-f4c2-4d77-869e-00641a869138/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Code" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="code" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/7260220c-f4c2-4d77-869e-00641a869138/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <elementProperties>
        <elementProperty name="Languages" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="languages" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/7260220c-f4c2-4d77-869e-00641a869138/LanguageElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElementCollection name="MessageElementCollection" xmlItemName="messageElement" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/7260220c-f4c2-4d77-869e-00641a869138/MessageElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElementCollection name="LanguageElementCollection" xmlItemName="languageElement" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/7260220c-f4c2-4d77-869e-00641a869138/LanguageElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="LanguageElement">
      <attributeProperties>
        <attributeProperty name="Id" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="id" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/7260220c-f4c2-4d77-869e-00641a869138/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="MessageInfo" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="messageInfo" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/7260220c-f4c2-4d77-869e-00641a869138/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>