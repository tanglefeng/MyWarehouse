using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Kengic.Was.CrossCutting.LinqToWcf.Types.Anonymous
{
    /// <summary>
    ///     Serializable abstraction of LINQ's <see cref="IGrouping{TKey,TElement}" />.
    /// </summary>
    /// <seealso cref="IGrouping{TKey, TElement}" />
    [Serializable]
    [DataContract(Namespace = "http://schemas.interlinq.com/2011/03/")]
    public abstract class InterLinqGroupingBase
    {
        /// <summary>
        ///     Sets the grouping <paramref name="key" />.
        /// </summary>
        /// <param name="key">Key to set.</param>
        public abstract void SetKey(object key);

        /// <summary>
        ///     Sets the grouping <paramref name="elements" />.
        /// </summary>
        /// <param name="elements">Elements to set.</param>
        public abstract void SetElements(object elements);
    }

    /// <summary>
    ///     Serializable abstraction of LINQ's <see cref="IGrouping{TKey, TElement}" />.
    /// </summary>
    /// <seealso cref="InterLinqGroupingBase" />
    /// <seealso cref="IGrouping{TKey, TElement}" />
    [Serializable]
    [DataContract(Namespace = "http://schemas.interlinq.com/2011/03/")]
    public class InterLinqGrouping<TKey, TElement> : InterLinqGroupingBase, IGrouping<TKey, TElement>
    {
        /// <summary>
        ///     Initializes this class.
        /// </summary>
        public InterLinqGrouping()
        {
            Elements = new List<TElement>();
        }

        /// <summary>
        ///     Gets or sets the elements.
        /// </summary>
        [DataMember]
        public IEnumerable<TElement> Elements { get; set; }

        /// <summary>
        ///     Gets or sets the key.
        /// </summary>
        [DataMember]
        public TKey Key { get; set; }

        /// <summary>
        ///     Returns an <see cref="IEnumerator{T}" /> that iterates through the collection.
        /// </summary>
        /// <returns>
        ///     Returns an <see cref="IEnumerator{T}" /> that iterates through the collection.
        /// </returns>
        public IEnumerator<TElement> GetEnumerator() => Elements.GetEnumerator();

        /// <summary>
        ///     Returns an <see cref="System.Collections.IEnumerator" /> that iterates through the collection.
        /// </summary>
        /// <returns>
        ///     Returns an <see cref="System.Collections.IEnumerator" /> that iterates through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        ///     Sets the grouping <paramref name="elements" />.
        /// </summary>
        /// <param name="elements">Elements to set.</param>
        public override void SetElements(object elements) => Elements = (IEnumerable<TElement>) elements;

        /// <summary>
        ///     Sets the grouping <paramref name="key" />.
        /// </summary>
        /// <param name="key">Key to set.</param>
        public override void SetKey(object key) => Key = (TKey) key;
    }
}