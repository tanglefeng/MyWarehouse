using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kengic.Was.CrossCutting.LinqToWcf.Types.Anonymous;

namespace Kengic.Was.CrossCutting.LinqToWcf.Types
{
    /// <summary>
    ///     The <see cref="TypeConverter" /> is a helper class providing
    ///     several static methods to convert <see cref="AnonymousObject" /> to
    ///     C# Anonymous Types and back.
    /// </summary>
    internal class TypeConverter
    {
        /// <summary>
        ///     Converts an <see langword="object" /> into a target <see cref="Type" />.
        /// </summary>
        /// <param name="wantedType">Target <see cref="Type" />.</param>
        /// <param name="objectToConvert"><see langword="object" /> to convert.</param>
        /// <returns>Returns the converted <see langword="object" />.</returns>
        public static object ConvertFromSerializable(Type wantedType, object objectToConvert)
        {
            if (objectToConvert == null)
            {
                return null;
            }
            if (wantedType.IsIGrouping() && objectToConvert is InterLinqGroupingBase)
            {
                var genericType = objectToConvert.GetType().GetGenericArguments();

                var method =
                    typeof (TypeConverter).GetMethod("ConvertFromInterLinqGrouping",
                        BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(genericType);

                return method.Invoke(null, new[] {wantedType, objectToConvert});
            }

            if (wantedType.IsIGroupingArray() && objectToConvert is InterLinqGroupingBase)
            {
                var genericType = objectToConvert.GetType().GetGenericArguments();


                var method =
                    typeof (TypeConverter).GetMethod("ConvertFromInterLinqGroupingArray",
                        BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(genericType);


                return method.Invoke(null, new[] {wantedType, objectToConvert});
            }

            var wantedElementType = InterLinqTypeSystem.FindIEnumerable(wantedType);
            if ((wantedElementType != null) && wantedElementType.GetGenericArguments()[0].IsAnonymous())

            {
                var typeOfObject = objectToConvert.GetType();
                var elementType = InterLinqTypeSystem.FindIEnumerable(typeOfObject);
                if ((elementType != null) && (elementType.GetGenericArguments()[0] == typeof (AnonymousObject)))

                {
                    var method =
                        typeof (TypeConverter).GetMethod("ConvertFromSerializableCollection",
                            BindingFlags.NonPublic | BindingFlags.Static)
                            .MakeGenericMethod(wantedElementType.GetGenericArguments()[0]);

                    return method.Invoke(null, new[] {objectToConvert});
                }
            }
            if (wantedType.IsAnonymous() && objectToConvert is AnonymousObject)
            {
                var dynamicObject = (AnonymousObject) objectToConvert;
                var properties = new List<object>();
                var constructors = wantedType.GetConstructors();

                if (constructors.Length != 1)
                {
                    throw new Exception("Usualy, anonymous types have just one constructor.");
                }
                var constructor = constructors[0];
                foreach (var parameter in constructor.GetParameters())
                {
                    object propertyValue = null;
                    var propertyHasBeenSet = false;
                    foreach (var dynProperty in dynamicObject.Properties)
                    {
                        if (dynProperty.Name == parameter.Name)
                        {
                            propertyValue = dynProperty.Value;
                            propertyHasBeenSet = true;
                            break;
                        }
                    }
                    if (!propertyHasBeenSet)
                    {
                        throw new Exception($"Property {parameter.Name} could not be found in the dynamic object.");
                    }
                    properties.Add(ConvertFromSerializable(parameter.ParameterType, propertyValue));
                }
                return constructor.Invoke(properties.ToArray());
            }
            return objectToConvert;
        }

        private static object ConvertFromInterLinqGrouping<TKey, TElement>(Type wantedType,
            InterLinqGrouping<TKey, TElement> grouping)
        {
            var genericArguments = wantedType.GetGenericArguments();
            var key = ConvertFromSerializable(genericArguments[0], grouping.Key);

            var method =
                typeof (TypeConverter).GetMethod("ConvertFromSerializableCollection",
                    BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(genericArguments[1]);
            var elements = method.Invoke(null, new object[] {grouping});

            //object elements = ConvertFromSerializableCollection<TElement>( typeof( IEnumerable<> ).MakeGenericType( genericArguments[1] ),  );
            var elementType = InterLinqTypeSystem.FindIEnumerable(elements.GetType());
            if (elementType == null)
            {
                throw new Exception("ElementType could not be found.");
            }
            Type[] genericTypes = {key.GetType(), elementType.GetGenericArguments()[0]};
            var newGrouping =
                (InterLinqGroupingBase)
                    Activator.CreateInstance(typeof (InterLinqGrouping<,>).MakeGenericType(genericTypes));
            newGrouping.SetKey(key);
            newGrouping.SetElements(elements);
            return newGrouping;
        }

        private static object ConvertFromInterLinqGroupingArray<TKey, TElement>(Type wantedType,
            InterLinqGrouping<TKey, TElement>[] grouping)
        {
            var retVal = new List<InterLinqGroupingBase>();
            var tp = wantedType.GetElementType();
            foreach (var interLinqGrouping in grouping)
            {
                retVal.Add((InterLinqGroupingBase) ConvertFromInterLinqGrouping(tp, interLinqGrouping));
            }
            return retVal.ToArray();
        }

        /// <summary>
        ///     Converts each element of an <see cref="IEnumerable" />
        ///     into a target <see cref="Type" />.
        /// </summary>
        /// <typeparam name="T">Target <see cref="Type" />.</typeparam>
        /// <param name="enumerable"><see cref="IEnumerable" />.</param>
        /// <returns>Returns the converted <see cref="IEnumerable" />.</returns>
        private static IEnumerable ConvertFromSerializableCollection<T>(IEnumerable enumerable)
        {
            var enumerableType = typeof (List<>).MakeGenericType(typeof (T));
            var newList = (IEnumerable) Activator.CreateInstance(enumerableType);
            var addMethod = enumerableType.GetMethod("Add");
            foreach (var item in enumerable)
            {
                addMethod.Invoke(newList, new[] {ConvertFromSerializable(typeof (T), item)});
            }
            return newList;
        }


        /// <summary>
        ///     Converts an object to an <see cref="AnonymousObject" />
        ///     or an <see cref="IEnumerable{T}" />.
        /// </summary>
        /// <param name="objectToConvert"><see langword="object" /> to convert.</param>
        /// <returns>Returns the converted <see langword="object" />.</returns>
        public static object ConvertToSerializable(object objectToConvert)
        {
            if (objectToConvert == null)
            {
                return null;
            }

            var typeOfObject = objectToConvert.GetType();
            var elementType = InterLinqTypeSystem.FindIEnumerable(typeOfObject);

            // Handle "IGrouping<TKey, TElement>"
            if (typeOfObject.IsIGrouping())
            {
                var genericType = typeOfObject.GetGenericArguments();

                var method =
                    typeof (TypeConverter).GetMethod("ConvertToInterLinqGrouping",
                        BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(genericType);

                return method.Invoke(null, new[] {objectToConvert});
            }

            // Handle "IGrouping<TKey, TElement>[]"
            if (typeOfObject.IsIGroupingArray())
            {
                var genericType = typeOfObject.GetGenericArguments();

                var method =
                    typeof (TypeConverter).GetMethod("ConvertToInterLinqGroupingArray",
                        BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(genericType);

                return method.Invoke(null, new[] {objectToConvert});
            }

            // Handle "IEnumerable<AnonymousType>" / "IEnumerator<T>"
            if (((elementType != null) && elementType.GetGenericArguments()[0].IsAnonymous()) ||
                typeOfObject.IsEnumerator())

            {
                var method =
                    typeof (TypeConverter).GetMethod("ConvertToSerializableCollection",
                        BindingFlags.NonPublic | BindingFlags.Static)
                        .MakeGenericMethod(elementType.GetGenericArguments()[0]);

                return method.Invoke(null, new[] {objectToConvert});
            }
            // Handle "AnonymousType"
            if (typeOfObject.IsAnonymous())
            {
                var newObject = new AnonymousObject();
                foreach (
                    var property in
                        typeOfObject.GetProperties(BindingFlags.Instance | BindingFlags.GetProperty |
                                                   BindingFlags.Public))

                {
                    var objectValue = ConvertToSerializable(property.GetValue(objectToConvert, new object[] {}));
                    newObject.Properties.Add(new AnonymousProperty(property.Name, objectValue));
                }
                return newObject;
            }

            return objectToConvert;
        }

        private static object ConvertToInterLinqGrouping<TKey, TElement>(IGrouping<TKey, TElement> grouping)
        {
            var key = ConvertToSerializable(grouping.Key);
            object elements = ConvertToSerializableCollection<TElement>(grouping);
            var elementType = InterLinqTypeSystem.FindIEnumerable(elements.GetType());
            if (elementType == null)
            {
                throw new Exception("ElementType could not be found.");
            }
            Type[] genericTypes = {key.GetType(), elementType.GetGenericArguments()[0]};
            var newGrouping =
                (InterLinqGroupingBase)
                    Activator.CreateInstance(typeof (InterLinqGrouping<,>).MakeGenericType(genericTypes));
            newGrouping.SetKey(key);
            newGrouping.SetElements(elements);
            return newGrouping;
        }

        private static object ConvertToInterLinqGroupingArray<TKey, TElement>(IGrouping<TKey, TElement>[] grouping)
        {
            var retVal = new List<InterLinqGrouping<TKey, TElement>>();
            foreach (var g in grouping)
            {
                retVal.Add((InterLinqGrouping<TKey, TElement>) ConvertToInterLinqGrouping(g));
            }

            return retVal.ToArray();
        }

        /// <summary>
        ///     Converts each element of an <see cref="IEnumerable" /> to
        ///     an <see cref="IEnumerable{AnonymousObject}" />
        /// </summary>
        /// <typeparam name="T">Target <see cref="Type" />.</typeparam>
        /// <param name="enumerable"><see cref="IEnumerable" />.</param>
        /// <returns>Returns the converted <see cref="IEnumerable" />.</returns>
        private static IEnumerable ConvertToSerializableCollection<T>(IEnumerable enumerable)
        {
            var typeToEnumerate = typeof (T);
            if (typeToEnumerate.IsAnonymous())
            {
                typeToEnumerate = typeof (AnonymousObject);
            }
            var enumerableType = typeof (List<>).MakeGenericType(typeToEnumerate);
            var newList = (IEnumerable) Activator.CreateInstance(enumerableType);
            var addMethod = enumerableType.GetMethod("Add");
            foreach (var item in enumerable)
            {
                addMethod.Invoke(newList, new[] {ConvertToSerializable(item)});
            }
            return newList;
        }
    }
}