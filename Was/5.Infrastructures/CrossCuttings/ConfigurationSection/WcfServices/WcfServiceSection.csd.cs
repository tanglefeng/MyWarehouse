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

namespace Kengic.Was.CrossCutting.ConfigurationSection.WcfServices
{
    
    
    /// <summary>
    /// The WcfServiceSection Configuration Section.
    /// </summary>
    public partial class WcfServiceSection : System.Configuration.ConfigurationSection
    {
        /// <summary>
        /// The XML name of the WcfServiceSection Configuration Section.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string WcfServiceSectionSectionName = "wcfServiceSection";
        
        /// <summary>
        /// Gets the WcfServiceSection instance.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public static WcfServiceSection Instance
        {
            get
            {
                return ((WcfServiceSection)(ConfigurationManager.GetSection(WcfServiceSectionSectionName)));
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
        /// The XML name of the <see cref="WcfServices"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string WcfServicesPropertyName = "wcfServices";
        
        /// <summary>
        /// Gets or sets the WcfServices.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The WcfServices.")]
        [ConfigurationProperty(WcfServicesPropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual WcfServiceElementCollection WcfServices
        {
            get
            {
                return ((WcfServiceElementCollection)(base[WcfServicesPropertyName]));
            }
            set
            {
                base[WcfServicesPropertyName] = value;
            }
        }
    }
}
namespace Kengic.Was.CrossCutting.ConfigurationSection.WcfServices
{
    
    
    /// <summary>
    /// The WcfServiceElement Configuration Element.
    /// </summary>
    public partial class WcfServiceElement : ConfigurationElement
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
        /// The XML name of the <see cref="ServiceType"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string ServiceTypePropertyName = "serviceType";
        
        /// <summary>
        /// Gets or sets the ServiceType.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The ServiceType.")]
        [TypeConverter(typeof(TypeNameConverter))]
        [ConfigurationProperty(ServiceTypePropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual Type ServiceType
        {
            get
            {
                return ((Type)(base[ServiceTypePropertyName]));
            }
            set
            {
                base[ServiceTypePropertyName] = value;
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
    }
}
namespace Kengic.Was.CrossCutting.ConfigurationSection.WcfServices
{
    
    
    /// <summary>
    /// A collection of WcfServiceElement instances.
    /// </summary>
    [ConfigurationCollection(typeof(WcfServiceElement), CollectionType=ConfigurationElementCollectionType.BasicMapAlternate, AddItemName=WcfServiceElementPropertyName)]
    public partial class WcfServiceElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// The XML name of the individual <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.WcfServices.WcfServiceElement"/> instances in this collection.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string WcfServiceElementPropertyName = "wcfServiceElement";

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
                return WcfServiceElementPropertyName;
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
            return (elementName == WcfServiceElementPropertyName);
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
            return ((WcfServiceElement)(element)).Id;
        }
        
        /// <summary>
        /// Creates a new <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.WcfServices.WcfServiceElement"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.WcfServices.WcfServiceElement"/>.
        /// </returns>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        protected override ConfigurationElement CreateNewElement()
        {
            return new WcfServiceElement();
        }

        /// <summary>
        /// Gets the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.WcfServices.WcfServiceElement"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.WcfServices.WcfServiceElement"/> to retrieve.</param>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public WcfServiceElement this[int index]
        {
            get
            {
                return ((WcfServiceElement)(base.BaseGet(index)));
            }
        }
        
        /// <summary>
        /// Gets the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.WcfServices.WcfServiceElement"/> with the specified key.
        /// </summary>
        /// <param name="id">The key of the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.WcfServices.WcfServiceElement"/> to retrieve.</param>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public WcfServiceElement this[object id]
        {
            get
            {
                return ((WcfServiceElement)(base.BaseGet(id)));
            }
        }

        /// <summary>
        /// Adds the specified <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.WcfServices.WcfServiceElement"/> to the <see cref="global::System.Configuration.ConfigurationElementCollection"/>.
        /// </summary>
        /// <param name="wcfServiceElement">The <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.WcfServices.WcfServiceElement"/> to add.</param>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public void Add(WcfServiceElement wcfServiceElement)
        {
            base.BaseAdd(wcfServiceElement);
        }

        /// <summary>
        /// Removes the specified <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.WcfServices.WcfServiceElement"/> from the <see cref="global::System.Configuration.ConfigurationElementCollection"/>.
        /// </summary>
        /// <param name="wcfServiceElement">The <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.WcfServices.WcfServiceElement"/> to remove.</param>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public void Remove(WcfServiceElement wcfServiceElement)
        {
            base.BaseRemove(this.GetElementKey(wcfServiceElement));
        }

        /// <summary>
        /// Gets the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.WcfServices.WcfServiceElement"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.WcfServices.WcfServiceElement"/> to retrieve.</param>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public WcfServiceElement GetItemAt(int index)
        {
            return ((WcfServiceElement)(base.BaseGet(index)));
        }
        
        /// <summary>
        /// Gets the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.WcfServices.WcfServiceElement"/> with the specified key.
        /// </summary>
        /// <param name="id">The key of the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.WcfServices.WcfServiceElement"/> to retrieve.</param>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public WcfServiceElement GetItemByKey(string id)
        {
            return ((WcfServiceElement)(base.BaseGet(((object)(id)))));
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
