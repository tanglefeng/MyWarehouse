<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="3c3a8cff-908b-4fda-b2c3-e1add38739cd" namespace="Kengic.Was.CrossCutting.ConfigurationSection.Operators" xmlSchemaNamespace="urn:Kengic.Was.CrossCutting.ConfigurationSection.Operators" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
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
    <configurationSection name="OperatorPropertySection" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="operatorPropertySection">
      <elementProperties>
        <elementProperty name="Parameters" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="parameters" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/3c3a8cff-908b-4fda-b2c3-e1add38739cd/ParameterElementCollection" />
          </type>
        </elementProperty>
        <elementProperty name="Threads" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="threads" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/3c3a8cff-908b-4fda-b2c3-e1add38739cd/ThreadElementCollection" />
          </type>
        </elementProperty>
        <elementProperty name="MessageQueues" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="messageQueues" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/3c3a8cff-908b-4fda-b2c3-e1add38739cd/MessageQueueElementCollection" />
          </type>
        </elementProperty>
        <elementProperty name="Activitys" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="activitys" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/3c3a8cff-908b-4fda-b2c3-e1add38739cd/ActivityElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElementCollection name="ParameterElementCollection" xmlItemName="parameterElement" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/3c3a8cff-908b-4fda-b2c3-e1add38739cd/ParameterElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElementCollection name="ThreadElementCollection" xmlItemName="threadElement" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/3c3a8cff-908b-4fda-b2c3-e1add38739cd/ThreadElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElementCollection name="ProcessElementCollection" xmlItemName="processElement" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/3c3a8cff-908b-4fda-b2c3-e1add38739cd/ProcessElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElementCollection name="MessageQueueElementCollection" xmlItemName="messageQueueElement" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/3c3a8cff-908b-4fda-b2c3-e1add38739cd/MessageQueueElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElementCollection name="ActivityElementCollection" xmlItemName="activityElement" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/3c3a8cff-908b-4fda-b2c3-e1add38739cd/ActivityElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="ParameterElement">
      <attributeProperties>
        <attributeProperty name="Id" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="id" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/3c3a8cff-908b-4fda-b2c3-e1add38739cd/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Type" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="type" isReadOnly="false" typeConverter="TypeNameConverter">
          <type>
            <externalTypeMoniker name="/3c3a8cff-908b-4fda-b2c3-e1add38739cd/Type" />
          </type>
        </attributeProperty>
        <attributeProperty name="Value" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="value" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/3c3a8cff-908b-4fda-b2c3-e1add38739cd/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElement name="ThreadElement">
      <attributeProperties>
        <attributeProperty name="Id" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="id" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/3c3a8cff-908b-4fda-b2c3-e1add38739cd/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Name" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/3c3a8cff-908b-4fda-b2c3-e1add38739cd/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Interval" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="interval" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/3c3a8cff-908b-4fda-b2c3-e1add38739cd/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="Enable" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="enable" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/3c3a8cff-908b-4fda-b2c3-e1add38739cd/Boolean" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <elementProperties>
        <elementProperty name="Processes" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="processes" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/3c3a8cff-908b-4fda-b2c3-e1add38739cd/ProcessElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElement name="ProcessElement">
      <attributeProperties>
        <attributeProperty name="Id" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="id" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/3c3a8cff-908b-4fda-b2c3-e1add38739cd/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Name" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/3c3a8cff-908b-4fda-b2c3-e1add38739cd/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Priority" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="priority" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/3c3a8cff-908b-4fda-b2c3-e1add38739cd/Int32" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElement name="MessageQueueElement">
      <attributeProperties>
        <attributeProperty name="Id" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="id" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/3c3a8cff-908b-4fda-b2c3-e1add38739cd/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Name" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/3c3a8cff-908b-4fda-b2c3-e1add38739cd/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Enable" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="enable" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/3c3a8cff-908b-4fda-b2c3-e1add38739cd/Boolean" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElement name="ActivityElement">
      <attributeProperties>
        <attributeProperty name="Id" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="id" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/3c3a8cff-908b-4fda-b2c3-e1add38739cd/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Code" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="code" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/3c3a8cff-908b-4fda-b2c3-e1add38739cd/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="ExecuteOperator" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="executeOperator" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/3c3a8cff-908b-4fda-b2c3-e1add38739cd/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="ActivityContractName" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="activityContractName" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/3c3a8cff-908b-4fda-b2c3-e1add38739cd/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="DataType" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="dataType" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/3c3a8cff-908b-4fda-b2c3-e1add38739cd/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>