using System;
using System.Collections.Generic;
using System.Linq;

namespace Kengic.Was.CrossCutting.Common
{
    public static class TypeExtensions
    {
        public static bool IsStructuralTypeConfiguration(this Type type, Type structuralTypeConfiguration)
        {
            if (!type.IsAbstract)
                return TryGetElementType(type, structuralTypeConfiguration) != null;
            return false;
        }

        public static Type TryGetElementType(this Type type, Type interfaceOrBaseType)
        {
            if (type.IsGenericTypeDefinition)
                return null;
            var list = GetGenericTypeImplementations(type, interfaceOrBaseType).ToList();
            return list.Count != 1 ? null : list[0].GetGenericArguments().FirstOrDefault();
        }

        public static IEnumerable<Type> GetGenericTypeImplementations(this Type type, Type interfaceOrBaseType)
        {
            if (type.IsGenericTypeDefinition)
                return Enumerable.Empty<Type>();
            return (interfaceOrBaseType.IsInterface ? type.GetInterfaces() : GetBaseTypes(type)).Union(new[]
            {
                type
            }).Where(t =>
            {
                if (t.IsGenericType)
                    return t.GetGenericTypeDefinition() == interfaceOrBaseType;
                return false;
            });
        }

        public static IEnumerable<Type> GetBaseTypes(this Type type)
        {
            for (type = type.BaseType; type != null; type = type.BaseType)
                yield return type;
        }
    }
}