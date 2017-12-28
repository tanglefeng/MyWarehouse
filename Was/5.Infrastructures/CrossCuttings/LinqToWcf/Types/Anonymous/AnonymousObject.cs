using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Kengic.Was.CrossCutting.LinqToWcf.Types.Anonymous
{
    /// <summary>
    ///     Represents an instance of an <see cref="AnonymousMetaType" />.
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "http://schemas.interlinq.com/2011/03/")]
    public class AnonymousObject
    {
        /// <summary>
        ///     Instance a new instance of the class <see cref="AnonymousObject" />.
        /// </summary>
        public AnonymousObject()
        {
            Properties = new List<AnonymousProperty>();
        }

        /// <summary>
        ///     Instance a new instance of the class <see cref="AnonymousObject" />
        ///     and initialze it with a list of properties.
        /// </summary>
        /// <param name="properties"><see cref="AnonymousProperty">Anonymous properties</see> to add.</param>
        public AnonymousObject(IEnumerable<AnonymousProperty> properties)
        {
            Properties = new List<AnonymousProperty>();
            Properties.AddRange(properties);
        }

        /// <summary>
        ///     The properties of the instance.
        /// </summary>
        [DataMember]
        public List<AnonymousProperty> Properties { get; set; }

        /// <summary>
        ///     Returns a string representing this <see cref="AnonymousObject" />.
        /// </summary>
        /// <returns>Returns a string representing this <see cref="AnonymousObject" />.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("{ ");
            var first = true;
            foreach (var property in Properties)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(", ");
                }
                sb.Append(property);
            }
            sb.Append(" }");
            return sb.ToString();
        }
    }
}