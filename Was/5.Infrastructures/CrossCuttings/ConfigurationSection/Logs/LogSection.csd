<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="86a104d9-aec4-4333-b69d-bc89cee3fd27" namespace="Kengic.Was.CrossCutting.ConfigurationSection.Logs" xmlSchemaNamespace="urn:Kengic.Was.CrossCutting.ConfigurationSection.Logs" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
  <typeDefinitions>
    <externalType name="String" namespace="System" />
    <externalType name="Boolean" namespace="System" />
    <externalType name="Int32" namespace="System" />
    <externalType name="Int64" namespace="System" />
    <externalType name="Single" namespace="System" />
    <externalType name="Double" namespace="System" />
    <externalType name="DateTime" namespace="System" />
    <externalType name="TimeSpan" namespace="System" />
    <enumeratedType name="Interval" namespace="Kengic.Was.CrossCutting.ConfigurationSection.Log">
      <literals>
        <enumerationLiteral name="None" />
        <enumerationLiteral name="Minute" />
        <enumerationLiteral name="Hour" />
        <enumerationLiteral name="Day" />
        <enumerationLiteral name="Week" />
        <enumerationLiteral name="Month" />
        <enumerationLiteral name="Year" />
        <enumerationLiteral name="Midnight" />
      </literals>
    </enumeratedType>
    <enumeratedType name="FileExistsBehavior" namespace="Kengic.Was.CrossCutting.ConfigurationSection.Log">
      <literals>
        <enumerationLiteral name="Increment" />
        <enumerationLiteral name="Overwrite" />
      </literals>
    </enumeratedType>
  </typeDefinitions>
  <configurationElements>
    <configurationSection name="LogSection" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="logSection">
      <elementProperties>
        <elementProperty name="Logs" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="logs" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/86a104d9-aec4-4333-b69d-bc89cee3fd27/LogElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElementCollection name="LogElementCollection" xmlItemName="logElement" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/86a104d9-aec4-4333-b69d-bc89cee3fd27/LogElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="LogElement">
      <attributeProperties>
        <attributeProperty name="Id" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="id" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/86a104d9-aec4-4333-b69d-bc89cee3fd27/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Name" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/86a104d9-aec4-4333-b69d-bc89cee3fd27/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Code" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="code" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/86a104d9-aec4-4333-b69d-bc89cee3fd27/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Enable" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="enable" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/86a104d9-aec4-4333-b69d-bc89cee3fd27/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="LogFilePath" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="logFilePath" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/86a104d9-aec4-4333-b69d-bc89cee3fd27/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="LogType" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="logType" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/86a104d9-aec4-4333-b69d-bc89cee3fd27/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="LogFileName" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="logFileName" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/86a104d9-aec4-4333-b69d-bc89cee3fd27/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="TextFormatter" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="textFormatter" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/86a104d9-aec4-4333-b69d-bc89cee3fd27/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="MaxSize" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="maxSize" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/86a104d9-aec4-4333-b69d-bc89cee3fd27/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="MaxSizeUnit" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="maxSizeUnit" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/86a104d9-aec4-4333-b69d-bc89cee3fd27/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="LogGrade" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="logGrade" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/86a104d9-aec4-4333-b69d-bc89cee3fd27/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="TimeStampPattern" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="timeStampPattern" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/86a104d9-aec4-4333-b69d-bc89cee3fd27/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="MaxArchivedFile" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="maxArchivedFile" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/86a104d9-aec4-4333-b69d-bc89cee3fd27/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="FileExistsBehavior" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="fileExistsBehavior" isReadOnly="false">
          <type>
            <enumeratedTypeMoniker name="/86a104d9-aec4-4333-b69d-bc89cee3fd27/FileExistsBehavior" />
          </type>
        </attributeProperty>
        <attributeProperty name="Interval" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="interval" isReadOnly="false">
          <type>
            <enumeratedTypeMoniker name="/86a104d9-aec4-4333-b69d-bc89cee3fd27/Interval" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>