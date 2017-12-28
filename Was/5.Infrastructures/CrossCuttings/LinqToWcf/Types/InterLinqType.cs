using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Kengic.Was.CrossCutting.LinqToWcf.Communications;
using Kengic.Was.CrossCutting.LinqToWcf.Types.Anonymous;

namespace Kengic.Was.CrossCutting.LinqToWcf.Types
{
    /// <summary>
    ///     InterLINQ representation of <see cref="Type" />.
    /// </summary>
    /// <seealso cref="InterLinqMemberInfo" />
    /// <seealso cref="Type" />
    [Serializable]
    [DataContract(Namespace = "http://schemas.interlinq.com/2011/03/")]
    [KnownType(typeof (AnonymousMetaType))]
    public class InterLinqType : InterLinqMemberInfo
    {
        private static readonly Dictionary<string, Type> CurrentDomainTypes =
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .GroupBy(r => r.FullName)
                .Select(r => r.First())
                .ToDictionary(r => r.FullName);

        private static readonly Dictionary<string, Type> TypeMap = new Dictionary<string, Type>();

        [DataMember(Name = "RepresentedType")] internal string representedType;

        /// <summary>
        ///     Empty constructor.
        /// </summary>
        public InterLinqType()
        {
            GenericArguments = new List<InterLinqType>();
        }

        /// <summary>
        ///     Initializes this class.
        /// </summary>
        /// <param name="representedType">Represented CLR <see cref="Type" />.</param>
        public InterLinqType(Type representedType) : this()
        {
            Initialize(representedType);
        }

        /// <summary>
        ///     Gets or sets if this is a generic <see cref="Type" />.
        /// </summary>
        /// <seealso cref="Type.IsGenericType" />
        [DataMember]
        public virtual bool IsGeneric { get; set; }

        /// <summary>
        ///     Gets the <see cref="MemberTypes">MemberType</see>.
        /// </summary>
        /// <seealso cref="Type.MemberType" />
        public override MemberTypes MemberType => MemberTypes.TypeInfo;

        /// <summary>
        ///     Gets or sets the represented <see cref="Type" />.
        /// </summary>
        public Type RepresentedType
        {
            get
            {
                var tp = Type.GetType(representedType);
                if (tp != null)
                    return tp;

                TypeMap.TryGetValue(representedType, out tp);
                if (tp == null)
                {
                    lock (TypeMap)
                    {
                        if (!TypeMap.ContainsKey(representedType))
                        {
                            var dataTypeResovler = DataContractResolverExtensions.KnownTypeDataContractResolver;
                            var lastIndex = representedType.LastIndexOf(".");
                            var namespaceSting = representedType.Substring(0, lastIndex);
                            var typeNameString = representedType.Substring(lastIndex + 1);
                            var type = dataTypeResovler.ResolveName(typeNameString, namespaceSting, null,
                                dataTypeResovler);
                            if (type == null)
                            {
                                CurrentDomainTypes.TryGetValue(representedType, out type);
                            }
                            //if (type==null)
                            //{
                            //    throw new Exception($"type {representedType} can not resolve");
                            //}
                            TypeMap[representedType] = type;
                        }
                    }
                }

                return TypeMap[representedType];
            }
            set
            {
                var dataContractAttribute = value.GetCustomAttribute<DataContractAttribute>();
                if (dataContractAttribute != null)
                {
                    representedType = dataContractAttribute.Namespace + "." + dataContractAttribute.Name;
                    return;
                }
                representedType = value.FullName;
            } // AssemblyQualifiedName; }
        }

        /// <summary>
        ///     Gets or sets the generic Arguments.
        /// </summary>
        /// <seealso cref="Type.GetGenericArguments" />
        [DataMember]
        public List<InterLinqType> GenericArguments { get; set; }

        /// <summary>
        ///     Initializes this class.
        /// </summary>
        /// <param name="memberInfo">Represented <see cref="MemberInfo" /></param>
        /// <seealso cref="InterLinqMemberInfo.Initialize" />
        public override void Initialize(MemberInfo memberInfo)
        {
            var repType = memberInfo as Type;

            Name = repType.Name;
            if (repType.IsGenericType)
            {
                RepresentedType = repType.GetGenericTypeDefinition();
                IsGeneric = true;
                foreach (var genericArgument in repType.GetGenericArguments())

                {
                    GenericArguments.Add(
                        InterLinqTypeSystem.Instance.GetInterLinqVersionOf<InterLinqType>(genericArgument));
                }
            }
            else if (repType.IsArray)
            {
                RepresentedType = typeof (IEnumerable<>);
                var elementType = repType.GetElementType();
                GenericArguments.Add(InterLinqTypeSystem.Instance.GetInterLinqVersionOf<InterLinqType>(elementType));
                IsGeneric = true;
            }
            else
            {
                RepresentedType = repType;

                IsGeneric = false;
            }
        }

        /// <summary>
        ///     Creates and returns the CLR <see cref="Type" />.
        /// </summary>
        /// <returns>Creates and returns the CLR <see cref="Type" />.</returns>
        protected virtual Type CreateClrType()
        {
            if (!IsGeneric)
            {
                return RepresentedType;
            }
            return RepresentedType.MakeGenericType(GenericArguments.Select(arg => (Type) arg.GetClrVersion()).ToArray());
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
                    return tsInstance.GetClrVersion<Type>(this);
                }
                var createdType = CreateClrType();
                tsInstance.SetClrVersion(this, createdType);
                return createdType;
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
            var other = (InterLinqType) obj;
            if (GenericArguments.Count != other.GenericArguments.Count)
            {
                return false;
            }
            for (var i = 0; i < GenericArguments.Count; i++)
            {
                if (!GenericArguments[i].Equals(other.GenericArguments[i]))
                {
                    return false;
                }
            }
            return (MemberType == other.MemberType) && (representedType == other.representedType) &&
                   (Name == other.Name) && (IsGeneric == other.IsGeneric);
        }

        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current <see langword="object" />.</returns>
        public override int GetHashCode()
        {
            var num = -657803396;
            num ^= EqualityComparer<bool>.Default.GetHashCode(IsGeneric);
            num ^= EqualityComparer<Type>.Default.GetHashCode(RepresentedType);
            GenericArguments.ForEach(o => num ^= EqualityComparer<InterLinqType>.Default.GetHashCode(o));
            return num ^ base.GetHashCode();
        }
    }
}