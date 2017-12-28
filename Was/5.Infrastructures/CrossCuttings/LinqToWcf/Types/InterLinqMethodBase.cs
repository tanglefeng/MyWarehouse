using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Kengic.Was.CrossCutting.LinqToWcf.Types
{
    /// <summary>
    ///     InterLINQ representation of <see cref="MethodBase" />.
    /// </summary>
    /// <seealso cref="InterLinqMemberInfo" />
    /// <seealso cref="MethodBase" />
    [Serializable]
    [DataContract(Namespace = "http://schemas.interlinq.com/2011/03/")]
    [KnownType(typeof (InterLinqConstructorInfo))]
    [KnownType(typeof (InterLinqMethodInfo))]
    public abstract class InterLinqMethodBase : InterLinqMemberInfo
    {
        /// <summary>
        ///     Empty constructor.
        /// </summary>
        protected InterLinqMethodBase()
        {
            ParameterTypes = new List<InterLinqType>();
        }

        /// <summary>
        ///     Initializes this class.
        /// </summary>
        /// <param name="methodBase">Represented CLR <see cref="MethodBase" />.</param>
        protected InterLinqMethodBase(MethodBase methodBase)
        {
            ParameterTypes = new List<InterLinqType>();
            Initialize(methodBase);
        }

        /// <summary>
        ///     Gets or sets the ParameterTypes.
        /// </summary>
        [DataMember]
        public List<InterLinqType> ParameterTypes { get; set; }

        /// <summary>
        ///     Initializes this class.
        /// </summary>
        /// <param name="memberInfo">Represented <see cref="MemberInfo" /></param>
        /// <seealso cref="InterLinqMemberInfo.Initialize" />
        public override void Initialize(MemberInfo memberInfo)
        {
            base.Initialize(memberInfo);
            var methodBase = memberInfo as MethodBase;
            foreach (var parameter in methodBase.GetParameters())
            {
                ParameterTypes.Add(
                    InterLinqTypeSystem.Instance.GetInterLinqVersionOf<InterLinqType>(parameter.ParameterType));
            }
        }

        /// <summary>
        ///     Compares <paramref name="obj" /> to this instance.
        /// </summary>
        /// <param name="obj"><see langword="object" /> to compare.</param>
        /// <returns>True if equal, false if not.</returns>
        public override bool Equals(object obj)
        {
            if ((obj == null) || (GetType() != obj.GetType()))
            {
                return false;
            }
            var other = (InterLinqMethodBase) obj;
            if (ParameterTypes.Count != other.ParameterTypes.Count)
            {
                return false;
            }

            if (ParameterTypes.Where((t, i) => !t.Equals(other.ParameterTypes[i])).Any())
            {
                return false;
            }

            return base.Equals(obj);
        }

        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current <see langword="object" />.</returns>
        public override int GetHashCode()
        {
            var num = -103514268;
            ParameterTypes.ForEach(o => num ^= EqualityComparer<InterLinqType>.Default.GetHashCode(o));
            return num ^ base.GetHashCode();
        }
    }
}