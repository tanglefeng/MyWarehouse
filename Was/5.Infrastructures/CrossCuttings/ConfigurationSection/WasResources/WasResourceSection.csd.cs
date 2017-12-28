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

namespace Kengic.Was.CrossCutting.ConfigurationSection.WasResources
{
    
    
    /// <summary>
    /// The WasResourceSection Configuration Section.
    /// </summary>
    public partial class WasResourceSection : System.Configuration.ConfigurationSection
    {
        /// <summary>
        /// The XML name of the WasResourceSection Configuration Section.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string WasResourceSectionSectionName = "wasResourceSection";
        
        /// <summary>
        /// Gets the WasResourceSection instance.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public static WasResourceSection Instance
        {
            get
            {
                return ((WasResourceSection)(ConfigurationManager.GetSection(WasResourceSectionSectionName)));
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
        /// The XML name of the <see cref="WasResources"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string WasResourcesPropertyName = "wasResources";
        
        /// <summary>
        /// Gets or sets the WasResources.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The WasResources.")]
        [ConfigurationProperty(WasResourcesPropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual WasResourceElementCollection WasResources
        {
            get
            {
                return ((WasResourceElementCollection)(base[WasResourcesPropertyName]));
            }
            set
            {
                base[WasResourcesPropertyName] = value;
            }
        }
    }
}
namespace Kengic.Was.CrossCutting.ConfigurationSection.WasResources
{
    
    
    /// <summary>
    /// A collection of WasResourceElement instances.
    /// </summary>
    [ConfigurationCollection(typeof(WasResourceElement), CollectionType=ConfigurationElementCollectionType.BasicMapAlternate, AddItemName=WasResourceElementPropertyName)]
    public partial class WasResourceElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// The XML name of the individual <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.WasResources.WasResourceElement"/> instances in this collection.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string WasResourceElementPropertyName = "wasResourceElement";

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
                return WasResourceElementPropertyName;
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
            return (elementName == WasResourceElementPropertyName);
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
            return ((WasResourceElement)(element)).Id;
        }
        
        /// <summary>
        /// Creates a new <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.WasResources.WasResourceElement"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.WasResources.WasResourceElement"/>.
        /// </returns>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        protected override ConfigurationElement CreateNewElement()
        {
            return new WasResourceElement();
        }

        /// <summary>
        /// Gets the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.WasResources.WasResourceElement"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.WasResources.WasResourceElement"/> to retrieve.</param>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public WasResourceElement this[int index]
        {
            get
            {
                return ((WasResourceElement)(base.BaseGet(index)));
            }
        }
        
        /// <summary>
        /// Gets the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.WasResources.WasResourceElement"/> with the specified key.
        /// </summary>
        /// <param name="id">The key of the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.WasResources.WasResourceElement"/> to retrieve.</param>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public WasResourceElement this[object id]
        {
            get
            {
                return ((WasResourceElement)(base.BaseGet(id)));
            }
        }

        /// <summary>
        /// Adds the specified <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.WasResources.WasResourceElement"/> to the <see cref="global::System.Configuration.ConfigurationElementCollection"/>.
        /// </summary>
        /// <param name="wasResourceElement">The <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.WasResources.WasResourceElement"/> to add.</param>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public void Add(WasResourceElement wasResourceElement)
        {
            base.BaseAdd(wasResourceElement);
        }

        /// <summary>
        /// Removes the specified <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.WasResources.WasResourceElement"/> from the <see cref="global::System.Configuration.ConfigurationElementCollection"/>.
        /// </summary>
        /// <param name="wasResourceElement">The <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.WasResources.WasResourceElement"/> to remove.</param>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public void Remove(WasResourceElement wasResourceElement)
        {
            base.BaseRemove(this.GetElementKey(wasResourceElement));
        }

        /// <summary>
        /// Gets the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.WasResources.WasResourceElement"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.WasResources.WasResourceElement"/> to retrieve.</param>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public WasResourceElement GetItemAt(int index)
        {
            return ((WasResourceElement)(base.BaseGet(index)));
        }
        
        /// <summary>
        /// Gets the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.WasResources.WasResourceElement"/> with the specified key.
        /// </summary>
        /// <param name="id">The key of the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.WasResources.WasResourceElement"/> to retrieve.</param>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public WasResourceElement GetItemByKey(string id)
        {
            return ((WasResourceElement)(base.BaseGet(((object)(id)))));
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
namespace Kengic.Was.CrossCutting.ConfigurationSection.WasResources
{
    
    
    /// <summary>
    /// The WasResourceElement Configuration Element.
    /// </summary>
    public partial class WasResourceElement : ConfigurationElement
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
        /// The XML name of the <see cref="AssemblyName"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string AssemblyNamePropertyName = "assemblyName";
        
        /// <summary>
        /// Gets or sets the AssemblyName.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The AssemblyName.")]
        [ConfigurationProperty(AssemblyNamePropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual string AssemblyName
        {
            get
            {
                return ((string)(base[AssemblyNamePropertyName]));
            }
            set
            {
                base[AssemblyNamePropertyName] = value;
            }
        }

        /// <summary>
        /// The XML name of the <see cref="FactoryName"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string FactoryNamePropertyName = "factoryName";
        
        /// <summary>
        /// Gets or sets the FactoryName.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The FactoryName.")]
        [ConfigurationProperty(FactoryNamePropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual string FactoryName
        {
            get
            {
                return ((string)(base[FactoryNamePropertyName]));
            }
            set
            {
                base[FactoryNamePropertyName] = value;
            }
        }

        /// <summary>
        /// The XML name of the <see cref="ModuleName"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string ModuleNamePropertyName = "moduleName";
        
        /// <summary>
        /// Gets or sets the ModuleName.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The ModuleName.")]
        [ConfigurationProperty(ModuleNamePropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual string ModuleName
        {
            get
            {
                return ((string)(base[ModuleNamePropertyName]));
            }
            set
            {
                base[ModuleNamePropertyName] = value;
            }
        }

        /// <summary>
        /// The XML name of the <see cref="IfAutoStart"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string IfAutoStartPropertyName = "ifAutoStart";
        
        /// <summary>
        /// Gets or sets the IfAutoStart.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The IfAutoStart.")]
        [ConfigurationProperty(IfAutoStartPropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual bool IfAutoStart
        {
            get
            {
                return ((bool)(base[IfAutoStartPropertyName]));
            }
            set
            {
                base[IfAutoStartPropertyName] = value;
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
        /// The XML name of the <see cref="Status"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string StatusPropertyName = "status";
        
        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The Status.")]
        [ConfigurationProperty(StatusPropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual bool Status
        {
            get
            {
                return ((bool)(base[StatusPropertyName]));
            }
            set
            {
                base[StatusPropertyName] = value;
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