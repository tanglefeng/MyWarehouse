//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Configuration;

namespace Kengic.Was.CrossCutting.ConfigurationSection.Logs
{
    /// <summary>
    /// The LogSection Configuration Section.
    /// </summary>
    public partial class LogSection : System.Configuration.ConfigurationSection
    {
        /// <summary>
        /// The XML name of the LogSection Configuration Section.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string LogSectionSectionName = "logSection";
        
        /// <summary>
        /// Gets the LogSection instance.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public static LogSection Instance
        {
            get
            {
                return ((LogSection)(ConfigurationManager.GetSection(LogSectionSectionName)));
            }
        }

        /// <summary>
        /// The XML name of the <see cref="Xmlns"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string XmlnsPropertyName = "xmlns";
        
        /// <summary>
        /// Gets the XML namespace of this Configuration Section.
        /// </summary>
        /// <remarks>
        /// This property makes sure that if the configuration file contains the XML namespace,
        /// the parser doesn't throw an exception because it encounters the unknown "xmlns" attribute.
        /// </remarks>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [ConfigurationProperty(XmlnsPropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public string Xmlns
        {
            get
            {
                return ((string)(base[XmlnsPropertyName]));
            }
        }

        /// <summary>
        /// Gets a value indicating whether the element is read-only.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public override bool IsReadOnly()
        {
            return false;
        }

        /// <summary>
        /// The XML name of the <see cref="Logs"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string LogsPropertyName = "logs";
        
        /// <summary>
        /// Gets or sets the Logs.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The Logs.")]
        [ConfigurationProperty(LogsPropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual LogElementCollection Logs
        {
            get
            {
                return ((LogElementCollection)(base[LogsPropertyName]));
            }
            set
            {
                base[LogsPropertyName] = value;
            }
        }
    }

    /// <summary>
    /// A collection of LogElement instances.
    /// </summary>
    [ConfigurationCollection(typeof(LogElement), CollectionType=ConfigurationElementCollectionType.BasicMapAlternate, AddItemName=LogElementPropertyName)]
    public partial class LogElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// The XML name of the individual <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.Logs.LogElement"/> instances in this collection.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string LogElementPropertyName = "logElement";

        /// <summary>
        /// Gets the type of the <see cref="global::System.Configuration.ConfigurationElementCollection"/>.
        /// </summary>
        /// <returns>The <see cref="global::System.Configuration.ConfigurationElementCollectionType"/> of this collection.</returns>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMapAlternate;
            }
        }
        
        /// <summary>
        /// Gets the name used to identify this collection of elements
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        protected override string ElementName
        {
            get
            {
                return LogElementPropertyName;
            }
        }
        
        /// <summary>
        /// Indicates whether the specified <see cref="global::System.Configuration.ConfigurationElement"/> exists in the <see cref="global::System.Configuration.ConfigurationElementCollection"/>.
        /// </summary>
        /// <param name="elementName">The name of the element to verify.</param>
        /// <returns>
        /// <see langword="true"/> if the element exists in the collection; otherwise, <see langword="false"/>.
        /// </returns>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        protected override bool IsElementName(string elementName)
        {
            return (elementName == LogElementPropertyName);
        }
        
        /// <summary>
        /// Gets the element key for the specified configuration element.
        /// </summary>
        /// <param name="element">The <see cref="global::System.Configuration.ConfigurationElement"/> to return the key for.</param>
        /// <returns>
        /// An <see cref="object"/> that acts as the key for the specified <see cref="global::System.Configuration.ConfigurationElement"/>.
        /// </returns>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((LogElement)(element)).Id;
        }
        
        /// <summary>
        /// Creates a new <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.Logs.LogElement"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.Logs.LogElement"/>.
        /// </returns>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        protected override ConfigurationElement CreateNewElement()
        {
            return new LogElement();
        }

        /// <summary>
        /// Gets the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.Logs.LogElement"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.Logs.LogElement"/> to retrieve.</param>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public LogElement this[int index]
        {
            get
            {
                return ((LogElement)(base.BaseGet(index)));
            }
        }
        
        /// <summary>
        /// Gets the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.Logs.LogElement"/> with the specified key.
        /// </summary>
        /// <param name="id">The key of the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.Logs.LogElement"/> to retrieve.</param>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public LogElement this[object id]
        {
            get
            {
                return ((LogElement)(base.BaseGet(id)));
            }
        }

        /// <summary>
        /// Adds the specified <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.Logs.LogElement"/> to the <see cref="global::System.Configuration.ConfigurationElementCollection"/>.
        /// </summary>
        /// <param name="logElement">The <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.Logs.LogElement"/> to add.</param>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public void Add(LogElement logElement)
        {
            base.BaseAdd(logElement);
        }

        /// <summary>
        /// Removes the specified <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.Logs.LogElement"/> from the <see cref="global::System.Configuration.ConfigurationElementCollection"/>.
        /// </summary>
        /// <param name="logElement">The <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.Logs.LogElement"/> to remove.</param>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public void Remove(LogElement logElement)
        {
            base.BaseRemove(this.GetElementKey(logElement));
        }

        /// <summary>
        /// Gets the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.Logs.LogElement"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.Logs.LogElement"/> to retrieve.</param>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public LogElement GetItemAt(int index)
        {
            return ((LogElement)(base.BaseGet(index)));
        }
        
        /// <summary>
        /// Gets the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.Logs.LogElement"/> with the specified key.
        /// </summary>
        /// <param name="id">The key of the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.Logs.LogElement"/> to retrieve.</param>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public LogElement GetItemByKey(string id)
        {
            return ((LogElement)(base.BaseGet(((object)(id)))));
        }

        /// <summary>
        /// Gets a value indicating whether the element is read-only.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public override bool IsReadOnly()
        {
            return false;
        }
    }

    /// <summary>
    /// The LogElement Configuration Element.
    /// </summary>
    public partial class LogElement : ConfigurationElement
    {
        /// <summary>
        /// Gets a value indicating whether the element is read-only.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public override bool IsReadOnly()
        {
            return false;
        }

        /// <summary>
        /// The XML name of the <see cref="Id"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string IdPropertyName = "id";
        
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The Id.")]
        [ConfigurationProperty(IdPropertyName, IsRequired=true, IsKey=true, IsDefaultCollection=false)]
        public virtual string Id
        {
            get
            {
                return ((string)(base[IdPropertyName]));
            }
            set
            {
                base[IdPropertyName] = value;
            }
        }

        /// <summary>
        /// The XML name of the <see cref="Name"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string NamePropertyName = "name";
        
        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The Name.")]
        [ConfigurationProperty(NamePropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual string Name
        {
            get
            {
                return ((string)(base[NamePropertyName]));
            }
            set
            {
                base[NamePropertyName] = value;
            }
        }

        /// <summary>
        /// The XML name of the <see cref="Code"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string CodePropertyName = "code";
        
        /// <summary>
        /// Gets or sets the Code.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The Code.")]
        [ConfigurationProperty(CodePropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual string Code
        {
            get
            {
                return ((string)(base[CodePropertyName]));
            }
            set
            {
                base[CodePropertyName] = value;
            }
        }

        /// <summary>
        /// The XML name of the <see cref="Enable"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string EnablePropertyName = "enable";
        
        /// <summary>
        /// Gets or sets the Enable.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The Enable.")]
        [ConfigurationProperty(EnablePropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual string Enable
        {
            get
            {
                return ((string)(base[EnablePropertyName]));
            }
            set
            {
                base[EnablePropertyName] = value;
            }
        }

        /// <summary>
        /// The XML name of the <see cref="LogFilePath"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string LogFilePathPropertyName = "logFilePath";
        
        /// <summary>
        /// Gets or sets the LogFilePath.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The LogFilePath.")]
        [ConfigurationProperty(LogFilePathPropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual string LogFilePath
        {
            get
            {
                return ((string)(base[LogFilePathPropertyName]));
            }
            set
            {
                base[LogFilePathPropertyName] = value;
            }
        }

        /// <summary>
        /// The XML name of the <see cref="LogType"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string LogTypePropertyName = "logType";
        
        /// <summary>
        /// Gets or sets the LogType.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The LogType.")]
        [ConfigurationProperty(LogTypePropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual string LogType
        {
            get
            {
                return ((string)(base[LogTypePropertyName]));
            }
            set
            {
                base[LogTypePropertyName] = value;
            }
        }

        /// <summary>
        /// The XML name of the <see cref="LogFileName"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string LogFileNamePropertyName = "logFileName";
        
        /// <summary>
        /// Gets or sets the LogFileName.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The LogFileName.")]
        [ConfigurationProperty(LogFileNamePropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual string LogFileName
        {
            get
            {
                return ((string)(base[LogFileNamePropertyName]));
            }
            set
            {
                base[LogFileNamePropertyName] = value;
            }
        }

        /// <summary>
        /// The XML name of the <see cref="TextFormatter"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string TextFormatterPropertyName = "textFormatter";
        
        /// <summary>
        /// Gets or sets the TextFormatter.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The TextFormatter.")]
        [ConfigurationProperty(TextFormatterPropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual string TextFormatter
        {
            get
            {
                return ((string)(base[TextFormatterPropertyName]));
            }
            set
            {
                base[TextFormatterPropertyName] = value;
            }
        }

        /// <summary>
        /// The XML name of the <see cref="MaxSize"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string MaxSizePropertyName = "maxSize";
        
        /// <summary>
        /// Gets or sets the MaxSize.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The MaxSize.")]
        [ConfigurationProperty(MaxSizePropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual int MaxSize
        {
            get
            {
                return ((int)(base[MaxSizePropertyName]));
            }
            set
            {
                base[MaxSizePropertyName] = value;
            }
        }

        /// <summary>
        /// The XML name of the <see cref="MaxSizeUnit"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string MaxSizeUnitPropertyName = "maxSizeUnit";
        
        /// <summary>
        /// Gets or sets the MaxSizeUnit.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The MaxSizeUnit.")]
        [ConfigurationProperty(MaxSizeUnitPropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual string MaxSizeUnit
        {
            get
            {
                return ((string)(base[MaxSizeUnitPropertyName]));
            }
            set
            {
                base[MaxSizeUnitPropertyName] = value;
            }
        }

        /// <summary>
        /// The XML name of the <see cref="LogGrade"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string LogGradePropertyName = "logGrade";
        
        /// <summary>
        /// Gets or sets the LogGrade.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The LogGrade.")]
        [ConfigurationProperty(LogGradePropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual int LogGrade
        {
            get
            {
                return ((int)(base[LogGradePropertyName]));
            }
            set
            {
                base[LogGradePropertyName] = value;
            }
        }

        /// <summary>
        /// The XML name of the <see cref="TimeStampPattern"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string TimeStampPatternPropertyName = "timeStampPattern";
        
        /// <summary>
        /// Gets or sets the TimeStampPattern.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The TimeStampPattern.")]
        [ConfigurationProperty(TimeStampPatternPropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual string TimeStampPattern
        {
            get
            {
                return ((string)(base[TimeStampPatternPropertyName]));
            }
            set
            {
                base[TimeStampPatternPropertyName] = value;
            }
        }

        /// <summary>
        /// The XML name of the <see cref="MaxArchivedFile"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string MaxArchivedFilePropertyName = "maxArchivedFile";
        
        /// <summary>
        /// Gets or sets the MaxArchivedFile.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The MaxArchivedFile.")]
        [ConfigurationProperty(MaxArchivedFilePropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual int MaxArchivedFile
        {
            get
            {
                return ((int)(base[MaxArchivedFilePropertyName]));
            }
            set
            {
                base[MaxArchivedFilePropertyName] = value;
            }
        }

        /// <summary>
        /// The XML name of the <see cref="FileExistsBehavior"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string FileExistsBehaviorPropertyName = "fileExistsBehavior";
        
        /// <summary>
        /// Gets or sets the FileExistsBehavior.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The FileExistsBehavior.")]
        [ConfigurationProperty(FileExistsBehaviorPropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual FileExistsBehavior FileExistsBehavior
        {
            get
            {
                return ((FileExistsBehavior)(base[FileExistsBehaviorPropertyName]));
            }
            set
            {
                base[FileExistsBehaviorPropertyName] = value;
            }
        }

        /// <summary>
        /// The XML name of the <see cref="Interval"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string IntervalPropertyName = "interval";
        
        /// <summary>
        /// Gets or sets the Interval.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The Interval.")]
        [ConfigurationProperty(IntervalPropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual Interval Interval
        {
            get
            {
                return ((Interval)(base[IntervalPropertyName]));
            }
            set
            {
                base[IntervalPropertyName] = value;
            }
        }
    }

    /// <summary>
    /// Interval.
    /// </summary>
    [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
    public enum Interval
    {
        
        /// <summary>
        /// None.
        /// </summary>
        None,
        
        /// <summary>
        /// Minute.
        /// </summary>
        Minute,
        
        /// <summary>
        /// Hour.
        /// </summary>
        Hour,
        
        /// <summary>
        /// Day.
        /// </summary>
        Day,
        
        /// <summary>
        /// Week.
        /// </summary>
        Week,
        
        /// <summary>
        /// Month.
        /// </summary>
        Month,
        
        /// <summary>
        /// Year.
        /// </summary>
        Year,
        
        /// <summary>
        /// Midnight.
        /// </summary>
        Midnight,
    }

    /// <summary>
    /// FileExistsBehavior.
    /// </summary>
    [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
    public enum FileExistsBehavior
    {
        
        /// <summary>
        /// Increment.
        /// </summary>
        Increment,
        
        /// <summary>
        /// Overwrite.
        /// </summary>
        Overwrite,
    }
}