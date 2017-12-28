//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Configuration;

namespace Kengic.Was.CrossCutting.ConfigurationSection.Operators
{
    
    
    /// <summary>
    /// The OperatorSection Configuration Section.
    /// </summary>
    public partial class OperatorSection : System.Configuration.ConfigurationSection
    {
        /// <summary>
        /// The XML name of the OperatorSection Configuration Section.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string OperatorSectionSectionName = "operatorSection";
        
        /// <summary>
        /// Gets the OperatorSection instance.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public static OperatorSection Instance
        {
            get
            {
                return ((OperatorSection)(ConfigurationManager.GetSection(OperatorSectionSectionName)));
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
        /// The XML name of the <see cref="Operators"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string OperatorsPropertyName = "operators";
        
        /// <summary>
        /// Gets or sets the Operators.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The Operators.")]
        [ConfigurationProperty(OperatorsPropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual OperatorElementCollection Operators
        {
            get
            {
                return ((OperatorElementCollection)(base[OperatorsPropertyName]));
            }
            set
            {
                base[OperatorsPropertyName] = value;
            }
        }
    }
}
namespace Kengic.Was.CrossCutting.ConfigurationSection.Operators
{
    
    
    /// <summary>
    /// The OperatorElement Configuration Element.
    /// </summary>
    public partial class OperatorElement : ConfigurationElement
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
        /// The XML name of the <see cref="Description"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string DescriptionPropertyName = "description";
        
        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The Description.")]
        [ConfigurationProperty(DescriptionPropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual string Description
        {
            get
            {
                return ((string)(base[DescriptionPropertyName]));
            }
            set
            {
                base[DescriptionPropertyName] = value;
            }
        }

        /// <summary>
        /// The XML name of the <see cref="Type"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string TypePropertyName = "type";
        
        /// <summary>
        /// Gets or sets the Type.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The Type.")]
        [TypeConverter(typeof(TypeNameConverter))]
        [ConfigurationProperty(TypePropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual Type Type
        {
            get
            {
                return ((Type)(base[TypePropertyName]));
            }
            set
            {
                base[TypePropertyName] = value;
            }
        }

        /// <summary>
        /// The XML name of the <see cref="StartupType"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string StartupTypePropertyName = "startupType";
        
        /// <summary>
        /// Gets or sets the StartupType.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The StartupType.")]
        [ConfigurationProperty(StartupTypePropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual StartupType StartupType
        {
            get
            {
                return ((StartupType)(base[StartupTypePropertyName]));
            }
            set
            {
                base[StartupTypePropertyName] = value;
            }
        }

        /// <summary>
        /// The XML name of the <see cref="StartSequence"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string StartSequencePropertyName = "startSequence";
        
        /// <summary>
        /// Gets or sets the StartSequence.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The StartSequence.")]
        [ConfigurationProperty(StartSequencePropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual int StartSequence
        {
            get
            {
                return ((int)(base[StartSequencePropertyName]));
            }
            set
            {
                base[StartSequencePropertyName] = value;
            }
        }

        /// <summary>
        /// The XML name of the <see cref="StopSequence"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string StopSequencePropertyName = "stopSequence";
        
        /// <summary>
        /// Gets or sets the StopSequence.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The StopSequence.")]
        [ConfigurationProperty(StopSequencePropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual int StopSequence
        {
            get
            {
                return ((int)(base[StopSequencePropertyName]));
            }
            set
            {
                base[StopSequencePropertyName] = value;
            }
        }

        /// <summary>
        /// The XML name of the <see cref="LogName"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string LogNamePropertyName = "logName";
        
        /// <summary>
        /// Gets or sets the LogName.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The LogName.")]
        [ConfigurationProperty(LogNamePropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual string LogName
        {
            get
            {
                return ((string)(base[LogNamePropertyName]));
            }
            set
            {
                base[LogNamePropertyName] = value;
            }
        }

        /// <summary>
        /// The XML name of the <see cref="FilePath"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string FilePathPropertyName = "filePath";
        
        /// <summary>
        /// Gets or sets the FilePath.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The FilePath.")]
        [ConfigurationProperty(FilePathPropertyName, IsRequired=true, IsKey=false, IsDefaultCollection=false)]
        public virtual string FilePath
        {
            get
            {
                return ((string)(base[FilePathPropertyName]));
            }
            set
            {
                base[FilePathPropertyName] = value;
            }
        }

        /// <summary>
        /// The XML name of the <see cref="SectionName"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string SectionNamePropertyName = "sectionName";
        
        /// <summary>
        /// Gets or sets the SectionName.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The SectionName.")]
        [ConfigurationProperty(SectionNamePropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual string SectionName
        {
            get
            {
                return ((string)(base[SectionNamePropertyName]));
            }
            set
            {
                base[SectionNamePropertyName] = value;
            }
        }

        /// <summary>
        /// The XML name of the <see cref="Comments"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string CommentsPropertyName = "comments";
        
        /// <summary>
        /// Gets or sets the Comments.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The Comments.")]
        [ConfigurationProperty(CommentsPropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual string Comments
        {
            get
            {
                return ((string)(base[CommentsPropertyName]));
            }
            set
            {
                base[CommentsPropertyName] = value;
            }
        }
    }
}
namespace Kengic.Was.CrossCutting.ConfigurationSection.Operators
{
    
    
    /// <summary>
    /// A collection of OperatorElement instances.
    /// </summary>
    [ConfigurationCollection(typeof(OperatorElement), CollectionType=ConfigurationElementCollectionType.BasicMapAlternate, AddItemName=OperatorElementPropertyName)]
    public partial class OperatorElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// The XML name of the individual <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.Operators.OperatorElement"/> instances in this collection.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string OperatorElementPropertyName = "operatorElement";

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
                return OperatorElementPropertyName;
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
            return (elementName == OperatorElementPropertyName);
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
            return ((OperatorElement)(element)).Id;
        }
        
        /// <summary>
        /// Creates a new <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.Operators.OperatorElement"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.Operators.OperatorElement"/>.
        /// </returns>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        protected override ConfigurationElement CreateNewElement()
        {
            return new OperatorElement();
        }

        /// <summary>
        /// Gets the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.Operators.OperatorElement"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.Operators.OperatorElement"/> to retrieve.</param>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public OperatorElement this[int index]
        {
            get
            {
                return ((OperatorElement)(base.BaseGet(index)));
            }
        }
        
        /// <summary>
        /// Gets the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.Operators.OperatorElement"/> with the specified key.
        /// </summary>
        /// <param name="id">The key of the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.Operators.OperatorElement"/> to retrieve.</param>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public OperatorElement this[object id]
        {
            get
            {
                return ((OperatorElement)(base.BaseGet(id)));
            }
        }

        /// <summary>
        /// Adds the specified <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.Operators.OperatorElement"/> to the <see cref="global::System.Configuration.ConfigurationElementCollection"/>.
        /// </summary>
        /// <param name="operatorElement">The <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.Operators.OperatorElement"/> to add.</param>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public void Add(OperatorElement operatorElement)
        {
            base.BaseAdd(operatorElement);
        }

        /// <summary>
        /// Removes the specified <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.Operators.OperatorElement"/> from the <see cref="global::System.Configuration.ConfigurationElementCollection"/>.
        /// </summary>
        /// <param name="operatorElement">The <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.Operators.OperatorElement"/> to remove.</param>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public void Remove(OperatorElement operatorElement)
        {
            base.BaseRemove(this.GetElementKey(operatorElement));
        }

        /// <summary>
        /// Gets the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.Operators.OperatorElement"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.Operators.OperatorElement"/> to retrieve.</param>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public OperatorElement GetItemAt(int index)
        {
            return ((OperatorElement)(base.BaseGet(index)));
        }
        
        /// <summary>
        /// Gets the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.Operators.OperatorElement"/> with the specified key.
        /// </summary>
        /// <param name="id">The key of the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.Operators.OperatorElement"/> to retrieve.</param>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public OperatorElement GetItemByKey(string id)
        {
            return ((OperatorElement)(base.BaseGet(((object)(id)))));
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
}
