using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Kengic.Was.CrossCutting.LinqToWcf.Types;

namespace Kengic.Was.CrossCutting.LinqToWcf.QueryProvider
{
    /// <summary>
    ///     Base Class for InterLinq Queries.
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "http://schemas.interlinq.com/2011/03/", Name = "ILQB")]
    public abstract class InterLinqQueryBase
    {
        /// <summary>
        ///     See <see cref="Type">Element Type</see> of the <see cref="Expression" />.
        /// </summary>
        [NonSerialized] protected Type elementType;

        /// <summary>
        ///     Gets or sets a <see cref="Type">Element Type</see> of the <see cref="Expression" />.
        /// </summary>
        public abstract Type ElementType { get; }

        /// <summary>
        ///     Gets or sets a <see cref="InterLinqType">InterLINQ Element Type</see> of the <see cref="Expression" />.
        /// </summary>
        [DataMember(Name = "E")]
        public InterLinqType ElementInterLinqType { get; set; }
    }
}