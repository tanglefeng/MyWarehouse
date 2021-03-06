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

namespace Kengic.Was.CrossCutting.ConfigurationSection.TypeFormatters
{
    
    
    /// <summary>
    /// The TypeFormatterSection Configuration Section.
    /// </summary>
    public partial class TypeFormatterSection : System.Configuration.ConfigurationSection
    {
        /// <summary>
        /// The XML name of the TypeFormatterSection Configuration Section.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string TypeFormatterSectionSectionName = "typeFormatterSection";
        
        /// <summary>
        /// Gets the TypeFormatterSection instance.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public static TypeFormatterSection Instance
        {
            get
            {
                return ((TypeFormatterSection)(ConfigurationManager.GetSection(TypeFormatterSectionSectionName)));
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
        /// The XML name of the <see cref="TypeFormatters"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string TypeFormattersPropertyName = "typeFormatters";
        
        /// <summary>
        /// Gets or sets the TypeFormatters.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The TypeFormatters.")]
        [ConfigurationProperty(TypeFormattersPropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual TypeFormatterElementCollection TypeFormatters
        {
            get
            {
                return ((TypeFormatterElementCollection)(base[TypeFormattersPropertyName]));
            }
            set
            {
                base[TypeFormattersPropertyName] = value;
            }
        }
    }
}
namespace Kengic.Was.CrossCutting.ConfigurationSection.TypeFormatters
{
    
    
    /// <summary>
    /// The TypeFormatterElement Configuration Element.
    /// </summary>
    public partial class TypeFormatterElement : ConfigurationElement
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
        [ConfigurationProperty(TypePropertyName, IsRequired=true, IsKey=true, IsDefaultCollection=false)]
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
        /// The XML name of the <see cref="Expression"/> property.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string ExpressionPropertyName = "expression";
        
        /// <summary>
        /// Gets or sets the Expression.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [Description("The Expression.")]
        [ConfigurationProperty(ExpressionPropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual string Expression
        {
            get
            {
                return ((string)(base[ExpressionPropertyName]));
            }
            set
            {
                base[ExpressionPropertyName] = value;
            }
        }
    }
}
namespace Kengic.Was.CrossCutting.ConfigurationSection.TypeFormatters
{
    
    
    /// <summary>
    /// A collection of TypeFormatterElement instances.
    /// </summary>
    [ConfigurationCollection(typeof(TypeFormatterElement), CollectionType=ConfigurationElementCollectionType.BasicMapAlternate, AddItemName=TypeFormatterElementPropertyName)]
    public partial class TypeFormatterElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// The XML name of the individual <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.TypeFormatters.TypeFormatterElement"/> instances in this collection.
        /// </summary>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string TypeFormatterElementPropertyName = "TypeFormatterElement";

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
                return TypeFormatterElementPropertyName;
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
            return (elementName == TypeFormatterElementPropertyName);
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
            return ((TypeFormatterElement)(element)).Type;
        }
        
        /// <summary>
        /// Creates a new <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.TypeFormatters.TypeFormatterElement"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.TypeFormatters.TypeFormatterElement"/>.
        /// </returns>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        protected override ConfigurationElement CreateNewElement()
        {
            return new TypeFormatterElement();
        }

        /// <summary>
        /// Gets the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.TypeFormatters.TypeFormatterElement"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.TypeFormatters.TypeFormatterElement"/> to retrieve.</param>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public TypeFormatterElement this[int index]
        {
            get
            {
                return ((TypeFormatterElement)(base.BaseGet(index)));
            }
        }
        
        /// <summary>
        /// Gets the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.TypeFormatters.TypeFormatterElement"/> with the specified key.
        /// </summary>
        /// <param name="type">The key of the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.TypeFormatters.TypeFormatterElement"/> to retrieve.</param>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public TypeFormatterElement this[object type]
        {
            get
            {
                return ((TypeFormatterElement)(base.BaseGet(type)));
            }
        }

        /// <summary>
        /// Adds the specified <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.TypeFormatters.TypeFormatterElement"/> to the <see cref="global::System.Configuration.ConfigurationElementCollection"/>.
        /// </summary>
        /// <param name="TypeFormatterElement">The <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.TypeFormatters.TypeFormatterElement"/> to add.</param>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public void Add(TypeFormatterElement TypeFormatterElement)
        {
            base.BaseAdd(TypeFormatterElement);
        }

        /// <summary>
        /// Removes the specified <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.TypeFormatters.TypeFormatterElement"/> from the <see cref="global::System.Configuration.ConfigurationElementCollection"/>.
        /// </summary>
        /// <param name="TypeFormatterElement">The <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.TypeFormatters.TypeFormatterElement"/> to remove.</param>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public void Remove(TypeFormatterElement TypeFormatterElement)
        {
            base.BaseRemove(this.GetElementKey(TypeFormatterElement));
        }

        /// <summary>
        /// Gets the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.TypeFormatters.TypeFormatterElement"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.TypeFormatters.TypeFormatterElement"/> to retrieve.</param>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public TypeFormatterElement GetItemAt(int index)
        {
            return ((TypeFormatterElement)(base.BaseGet(index)));
        }
        
        /// <summary>
        /// Gets the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.TypeFormatters.TypeFormatterElement"/> with the specified key.
        /// </summary>
        /// <param name="type">The key of the <see cref="global::Kengic.Was.CrossCutting.ConfigurationSection.TypeFormatters.TypeFormatterElement"/> to retrieve.</param>
        [GeneratedCode("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public TypeFormatterElement GetItemByKey(Type type)
        {
            return ((TypeFormatterElement)(base.BaseGet(((object)(type)))));
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
