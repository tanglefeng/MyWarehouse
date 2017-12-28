using System;
using System.Runtime.Serialization;

namespace Kengic.Was.CrossCutting.LinqToWcf.Types.Anonymous
{
    /// <summary>
    ///     Represents an instance of an <see cref="AnonymousMetaProperty" />.
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "http://schemas.interlinq.com/2011/03/")]
    public class AnonymousProperty
    {
        /// <summary>
        ///     Default constructor for serialization.
        /// </summary>
        public AnonymousProperty()
        {
        }

        /// <summary>
        ///     Instance a new instance of the class <see cref="AnonymousProperty" /> and initialize it.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <param name="value">Value of the property.</param>
        public AnonymousProperty(string name, object value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        ///     The name of the property.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        ///     The value of the property.
        /// </summary>
        [DataMember]
        public object Value { get; set; }

        /// <summary>
        ///     Returns a <see langword="string" /> representing this object.
        /// </summary>
        /// <returns>Returns a <see langword="string" /> representing this object.</returns>
        public override string ToString() => $"{Name} = {Value}";
    }
}