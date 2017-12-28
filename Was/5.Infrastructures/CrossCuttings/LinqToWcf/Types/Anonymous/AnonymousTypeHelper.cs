using System;
using System.Collections.Generic;
using System.Linq;

namespace Kengic.Was.CrossCutting.LinqToWcf.Types.Anonymous
{
    /// <summary>
    ///     A static class that contains static methods to recognize some special types like anonymous types and
    ///     display classes.
    /// </summary>
    internal static class AnonymousTypeHelper
    {
        /// <summary>
        ///     Returns true if the given <see cref="Type" /> is an anonymous type.
        /// </summary>
        /// <param name="t"><see cref="Type" /> to test.</param>
        /// <returns>True, if the type is an anonymous type. False, if not.</returns>
        /// <remarks>
        ///     A anonymous type is not really marked as anonymous.
        ///     The only way to recognize it is it's name.
        ///     Maybe in future versions they will be marked.
        /// </remarks>
        public static bool IsAnonymous(this Type t) => t.Name.StartsWith("<>f__AnonymousType", StringComparison.Ordinal);

        /// <summary>
        ///     Returns true if the given <see cref="Type" /> is a display class.
        /// </summary>
        /// <param name="t"><see cref="Type" /> to test.</param>
        /// <returns>True, if the type is a display class. False, if not.</returns>
        /// <remarks>
        ///     A display class is not really marked as display class.
        ///     The only way to recognize it is it's name.
        ///     Maybe in future versions they will be marked.
        /// </remarks>
        public static bool IsDisplayClass(this Type t)
            => t.Name.StartsWith("<>c__DisplayClass", StringComparison.Ordinal);

        /// <summary>
        ///     Returns true if the given <see cref="Type" /> is a <see cref="IGrouping{TKey,TElement}" /> class.
        /// </summary>
        /// <param name="t"><see cref="Type" /> to test.</param>
        /// <returns>Returns true if the given <see cref="Type" /> is a <see cref="IGrouping{TKey, TElement}" /> class.</returns>
        public static bool IsIGrouping(this Type t)
        {
            if (!t.IsGenericType)

            {
                return false;
            }
            if (t.GetGenericTypeDefinition() == typeof (IGrouping<,>))
            {
                return true;
            }
            var result =
                t.GetInterfaces()
                    .Any(
                        interfaceType =>
                            interfaceType.IsGenericType &&
                            (interfaceType.GetGenericTypeDefinition() == typeof (IGrouping<,>)));
            return result;
        }

        /// <summary>
        ///     Returns true if the given <see cref="Type" /> is a <see cref="IGrouping{TKey, TElement}" /> class.
        /// </summary>
        /// <param name="t"><see cref="Type" /> to test.</param>
        /// <returns>Returns true if the given <see cref="Type" /> is a <see cref="IGrouping{TKey, TElement}" /> class.</returns>
        public static bool IsIGroupingArray(this Type t)
        {
            if (!t.IsArray)
            {
                return false;
            }
            return t.GetElementType().IsIGrouping();
        }

        /// <summary>
        ///     Returns true if the given <see cref="Type" /> is a <see cref="IEnumerator{T}" /> class.
        /// </summary>
        /// <param name="t"><see cref="Type" /> to test.</param>
        /// <returns>Returns true if the given <see cref="Type" /> is a <see cref="IEnumerator{t}" /> class.</returns>
        public static bool IsEnumerator(this Type t)
        {
            if (!t.IsGenericType)
            {
                return false;
            }
            if (t.GetGenericTypeDefinition() == typeof (IEnumerator<>))
            {
                return true;
            }
            var result =
                t.GetInterfaces()
                    .Any(
                        interfaceType =>
                            interfaceType.IsGenericType &&
                            (interfaceType.GetGenericTypeDefinition() == typeof (IEnumerator<>)));
            return result;
        }
    }
}