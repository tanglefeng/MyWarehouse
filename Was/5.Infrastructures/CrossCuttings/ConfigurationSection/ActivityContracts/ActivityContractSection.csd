<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="34bc0454-183d-426e-9968-e08310e86e40" namespace="Kengic.Was.CrossCutting.ConfigurationSection.ActivityContracts" xmlSchemaNamespace="urn:Kengic.Was.CrossCutting.ConfigurationSection.ActivityContracts" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
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
    <configurationSection name="ActivityContractSection" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="activityContractSection">
      <elementProperties>
        <elementProperty name="ActivityContracts" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="activityContracts" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/34bc0454-183d-426e-9968-e08310e86e40/ActivityContractElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElement name="ActivityContractElement">
      <attributeProperties>
        <attributeProperty name="Id" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="id" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/34bc0454-183d-426e-9968-e08310e86e40/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Name" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/34bc0454-183d-426e-9968-e08310e86e40/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Code" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="code" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/34bc0454-183d-426e-9968-e08310e86e40/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Description" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="description" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/34bc0454-183d-426e-9968-e08310e86e40/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="OperatorName" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="operatorName" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/34bc0454-183d-426e-9968-e08310e86e40/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="OperatorMethod" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="operatorMethod" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/34bc0454-183d-426e-9968-e08310e86e40/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="ActivityContract" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="activityContract" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/34bc0454-183d-426e-9968-e08310e86e40/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElementCollection name="ActivityContractElementCollection" xmlItemName="activityContractElement" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/34bc0454-183d-426e-9968-e08310e86e40/ActivityContractElement" />
      </itemType>
    </configurationElementCollection>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>