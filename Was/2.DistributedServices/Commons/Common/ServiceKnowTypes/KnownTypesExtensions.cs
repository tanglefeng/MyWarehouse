using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kengic.Was.CrossCutting.LinqToWcf.QueryProvider;

namespace Kengic.Was.DistributedServices.Common.ServiceKnowTypes
{
    public static class KnownTypesExtensions
    {
        private static readonly List<Type> KnowTypes = new List<Type>();

        public static IEnumerable<Type> GetListOfKnownTypes
        {
            get
            {
                if (!KnowTypes.Any())
                {
                    var coreTypes =
                        AppDomain.CurrentDomain.GetAssemblies()
                            .Where(r => r.FullName.StartsWith("Kengic.Was.Domain.Entity", StringComparison.Ordinal))
                            .SelectMany(
                                r =>
                                    r.GetTypes()
                                        .Where(
                                            v =>
                                                v.IsPublic && !v.IsGenericType && !v.IsInterface &&
                                                !(v.IsAbstract && v.IsSealed) /*Satic is abstract and sealed*/)
                                        .SelectMany(g => g.GetBaseTypes().Concat(new[] {g})))
                            .Where(
                                h =>
                                    h.Assembly.FullName.StartsWith("Kengic.Was.Domain.Entity", StringComparison.Ordinal))
                            .GroupBy(i => i.FullName)
                            .Select(o => o.First()).ToList();

                    KnowTypes.AddRange(coreTypes);
                    foreach (var coreType in coreTypes)
                    {
                        var interLinqQueryType = typeof (InterLinqQuery<>).MakeGenericType(coreType);
                        var arraryType = coreType.MakeArrayType();
                        KnowTypes.Add(interLinqQueryType);
                        KnowTypes.Add(arraryType);
                    }
                    KnowTypes.Add(typeof (bool[]));
                    KnowTypes.Add(typeof (byte[]));
                    KnowTypes.Add(typeof (sbyte[]));
                    KnowTypes.Add(typeof (char[]));
                    KnowTypes.Add(typeof (decimal[]));
                    KnowTypes.Add(typeof (double[]));
                    KnowTypes.Add(typeof (float[]));
                    KnowTypes.Add(typeof (int[]));
                    KnowTypes.Add(typeof (uint[]));
                    KnowTypes.Add(typeof (long[]));
                    KnowTypes.Add(typeof (ulong[]));
                    KnowTypes.Add(typeof (object[]));
                    KnowTypes.Add(typeof (short[]));
                    KnowTypes.Add(typeof (ushort[]));
                    KnowTypes.Add(typeof (string[]));
                }
                return KnowTypes;
            }
        }

        public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
            => GetListOfKnownTypes;

        public static IEnumerable<Type> GetBaseTypes(this Type type)
        {
            if (type.BaseType == null) return type.GetInterfaces().Where(r => r.IsConstructedGenericType);

            return Enumerable.Repeat(type.BaseType, 1)
                .Concat(type.BaseType.GetBaseTypes());
        }
    }
}