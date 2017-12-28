using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace Kengic.Was.CrossCutting.LinqToWcf.Types
{
    /// <summary>
    ///     InterLINQ representation of <see cref="PropertyInfo" />.
    /// </summary>
    /// <seealso cref="InterLinqMemberInfo" />
    /// <seealso cref="PropertyInfo" />
    [Serializable]
    [DataContract(Namespace = "http://schemas.interlinq.com/2011/03/")]
    public class InterLinqPropertyInfo : InterLinqMemberInfo
    {
        /// <summary>
        ///     Empty constructor.
        /// </summary>
        public InterLinqPropertyInfo()
        {
        }

        /// <summary>
        ///     Initializes this class.
        /// </summary>
        /// <param name="fieldInfo">Represented CLR <see cref="PropertyInfo" />.</param>
        public InterLinqPropertyInfo(PropertyInfo fieldInfo)
        {
            Initialize(fieldInfo);
        }

        /// <summary>
        ///     Gets the <see cref="MemberTypes">MemberType</see>.
        /// </summary>
        /// <seealso cref="InterLinqMemberInfo.MemberType" />
        /// <seealso cref="PropertyInfo.MemberType" />
        public override MemberTypes MemberType => MemberTypes.Property;

        /// <summary>
        ///     Gets or sets the <see cref="InterLinqType" /> of this property.
        /// </summary>
        /// <seealso cref="PropertyInfo.PropertyType" />
        [DataMember]
        public InterLinqType PropertyType { get; set; }

        /// <summary>
        ///     Initializes this class.
        /// </summary>
        /// <param name="memberInfo">Represented <see cref="MemberInfo" /></param>
        /// <seealso cref="InterLinqMemberInfo.Initialize" />
        public override void Initialize(MemberInfo memberInfo)
        {
            base.Initialize(memberInfo);
            var propertyInfo = memberInfo as PropertyInfo;
            PropertyType = InterLinqTypeSystem.Instance.GetInterLinqVersionOf<InterLinqType>(propertyInfo.PropertyType);
        }

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
                    return tsInstance.GetClrVersion<PropertyInfo>(this);
                }

                var declaringType = (Type) DeclaringType.GetClrVersion();
                var foundProperty = declaringType.GetProperty(Name);

                tsInstance.SetClrVersion(this, foundProperty);
                return foundProperty;
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
            if (!base.Equals(obj))
            {
                return false;
            }
            var other = (InterLinqPropertyInfo) obj;
            return PropertyType.Equals(other.PropertyType);
        }

        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current <see langword="object" />.</returns>
        public override int GetHashCode()
        {
            var num = -1141188190;
            num ^= EqualityComparer<InterLinqType>.Default.GetHashCode(PropertyType);
            return num ^ base.GetHashCode();
        }
    }
}