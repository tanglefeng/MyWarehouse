using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Kengic.Was.CrossCutting.LinqToWcf.Types
{
    /// <summary>
    ///     InterLINQ representation of <see cref="ConstructorInfo" />.
    /// </summary>
    /// <seealso cref="InterLinqMethodBase" />
    /// <seealso cref="InterLinqMemberInfo" />
    /// <seealso cref="ConstructorInfo" />
    [Serializable]
    [DataContract(Namespace = "http://schemas.interlinq.com/2011/03/")]
    public class InterLinqConstructorInfo : InterLinqMethodBase
    {
        /// <summary>
        ///     Empty constructor.
        /// </summary>
        public InterLinqConstructorInfo()
        {
        }

        /// <summary>
        ///     Initializes this class.
        /// </summary>
        /// <param name="constrcutorInfo">Represented CLR <see cref="ConstructorInfo" />.</param>
        public InterLinqConstructorInfo(ConstructorInfo constrcutorInfo)
        {
            Initialize(constrcutorInfo);
        }

        /// <summary>
        ///     Gets the <see cref="MemberTypes">MemberType</see>.
        /// </summary>
        /// <seealso cref="InterLinqMemberInfo.MemberType" />
        /// <seealso cref="ConstructorInfo.MemberType" />
        public override MemberTypes MemberType => MemberTypes.Constructor;

        /// <summary>
        ///     Returns the CLR <see cref="MemberInfo" />.
        /// </summary>
        /// <returns>Returns the CLR <see cref="MemberInfo" />.</returns>
        public override MemberInfo GetClrVersion()
        {
            var tsInstance = InterLinqTypeSystem.Instance;
            lock (tsInstance)
            {
                if (tsInstance.IsInterLinqMemberInfoRegistered(this))
                {
                    return tsInstance.GetClrVersion<ConstructorInfo>(this);
                }
                var declaringType = (Type) DeclaringType.GetClrVersion();
                var foundConstructor =
                    declaringType.GetConstructor(ParameterTypes.Select(p => (Type) p.GetClrVersion()).ToArray());
                tsInstance.SetClrVersion(this, foundConstructor);
                return foundConstructor;
            }
        }
    }
}