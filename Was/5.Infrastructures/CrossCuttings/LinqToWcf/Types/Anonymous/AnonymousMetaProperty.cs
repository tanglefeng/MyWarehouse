using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace Kengic.Was.CrossCutting.LinqToWcf.Types.Anonymous
{
    /// <summary>
    ///     Class that represents a property of an anonymous type.
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "http://schemas.interlinq.com/2011/03/")]
    public class AnonymousMetaProperty
    {
        /// <summary>
        ///     Default constructor for serialization
        /// </summary>
        public AnonymousMetaProperty()
        {
        }

        /// <summary>
        ///     Instance an instance of the class <see cref="AnonymousMetaProperty" /> with a <see cref="PropertyInfo" />.
        /// </summary>
        /// <param name="property"><see cref="PropertyInfo" /> to create a <see cref="AnonymousMetaProperty" /> from.</param>
        public AnonymousMetaProperty(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            Name = property.Name;
            PropertyType = InterLinqTypeSystem.Instance.GetInterLinqVersionOf<InterLinqType>(property.PropertyType);
        }

        /// <summary>
        ///     The <see cref="InterLinqType" /> of the property.
        /// </summary>
        [DataMember]
        public InterLinqType PropertyType { get; set; }

        /// <summary>
        ///     The name of the property.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        ///     Overrides the equality check.
        /// </summary>
        /// <param name="obj">Object to compare with.</param>
        /// <returns>True, if the other <see langword="object" /> is equal to this. False, if not.</returns>
        public override bool Equals(object obj)
        {
            if ((obj == null) || (GetType() != obj.GetType()))
            {
                return false;
            }

            var other = (AnonymousMetaProperty) obj;
            return (other.Name == Name) && other.PropertyType.Equals(PropertyType);
        }

        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current <see langword="object" />.</returns>
        public override int GetHashCode()
        {
            var num = -871466652;
            num ^= EqualityComparer<InterLinqType>.Default.GetHashCode(PropertyType);
            num ^= EqualityComparer<string>.Default.GetHashCode(Name);
            return num;
        }
    }
}