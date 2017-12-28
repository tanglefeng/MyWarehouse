<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="37afce89-a8e0-4cf7-9c4f-9b172f77c743" namespace="Kengic.Was.CrossCutting.ConfigurationSection.Connectors" xmlSchemaNamespace="urn:Kengic.Was.CrossCutting.ConfigurationSection.Connectors" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
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
    <configurationSection name="OpcSection" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="opcSection">
      <elementProperties>
        <elementProperty name="OpcGroups" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="opcGroups" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/37afce89-a8e0-4cf7-9c4f-9b172f77c743/OpcGroupElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElement name="OpcGroupElement">
      <attributeProperties>
        <attributeProperty name="Id" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="id" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/37afce89-a8e0-4cf7-9c4f-9b172f77c743/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="ReadWriteType" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="readWriteType" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/37afce89-a8e0-4cf7-9c4f-9b172f77c743/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="SyncType" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="syncType" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/37afce89-a8e0-4cf7-9c4f-9b172f77c743/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="DeviceCode" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="deviceCode" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/37afce89-a8e0-4cf7-9c4f-9b172f77c743/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Protocal" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="protocal" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/37afce89-a8e0-4cf7-9c4f-9b172f77c743/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="ActiceStatus" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="acticeStatus" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/37afce89-a8e0-4cf7-9c4f-9b172f77c743/Boolean" />
          </type>
        </attributeProperty>
        <attributeProperty name="ConnectionName" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="connectionName" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/37afce89-a8e0-4cf7-9c4f-9b172f77c743/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="StorageDb" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="storageDb" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/37afce89-a8e0-4cf7-9c4f-9b172f77c743/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="StartAddress" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="startAddress" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/37afce89-a8e0-4cf7-9c4f-9b172f77c743/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="UpdateRateSetting" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="updateRateSetting" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/37afce89-a8e0-4cf7-9c4f-9b172f77c743/Int32" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <elementProperties>
        <elementProperty name="OpcItems" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="opcItems" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/37afce89-a8e0-4cf7-9c4f-9b172f77c743/OpcItemElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElementCollection name="OpcGroupElementCollection" xmlItemName="opcGroupElement" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/37afce89-a8e0-4cf7-9c4f-9b172f77c743/OpcGroupElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElementCollection name="OpcItemElementCollection" xmlItemName="opcItemElement" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/37afce89-a8e0-4cf7-9c4f-9b172f77c743/OpcItemElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="OpcItemElement">
      <attributeProperties>
        <attributeProperty name="Id" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="id" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/37afce89-a8e0-4cf7-9c4f-9b172f77c743/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="OppositeAddress" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="oppositeAddress" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/37afce89-a8e0-4cf7-9c4f-9b172f77c743/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="DataType" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="dataType" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/37afce89-a8e0-4cf7-9c4f-9b172f77c743/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="DataTypeChar" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="dataTypeChar" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/37afce89-a8e0-4cf7-9c4f-9b172f77c743/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="DataLength" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="dataLength" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/37afce89-a8e0-4cf7-9c4f-9b172f77c743/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="AlarmFlag" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="alarmFlag" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/37afce89-a8e0-4cf7-9c4f-9b172f77c743/Boolean" />
          </type>
        </attributeProperty>
        <attributeProperty name="AlarmCode" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="alarmCode" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/37afce89-a8e0-4cf7-9c4f-9b172f77c743/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>