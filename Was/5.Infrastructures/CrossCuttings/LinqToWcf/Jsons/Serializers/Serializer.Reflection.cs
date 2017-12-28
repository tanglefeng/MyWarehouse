using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace Kengic.Was.CrossCutting.LinqToWcf.Jsons
{
    partial class Serializer
    {
        private static readonly Dictionary<Type, Tuple<string, string, Type[]>>
            TypeCache = new Dictionary<Type, Tuple<string, string, Type[]>>();

        private Action Type(Type type)
        {
            return () => TypeInternal(type);
        }

        private void TypeInternal(Type type)
        {
            if (type == null)
            {
                _writer.WriteNull();
            }
            else
            {
                Tuple<string, string, Type[]> tuple;
                var dataContractAttribute = type.GetCustomAttribute<DataContractAttribute>();
                if (dataContractAttribute != null)
                {
                    var typeName = dataContractAttribute.Name;
                    var nameSpace = dataContractAttribute.Namespace;
                    if (type.IsGenericType)
                    {
                        var def = type.GetGenericTypeDefinition();
                        tuple = Tuple.Create(def.Assembly.FullName, def.FullName, type.GetGenericArguments());
                    }
                    else
                    {
                        tuple = Tuple.Create<string, string, Type[]>(nameSpace,typeName,null);
                    }
                }
                else if (type.FullName.StartsWith("<>", StringComparison.Ordinal) &&
                         type.FullName.Contains("AnonymousType"))
                {
                    tuple = Tuple.Create<string, string, Type[]>("AnonymousType", type.FullName, null);
                }
                else
                {
                    if (!TypeCache.TryGetValue(type, out tuple))
                    {
                        var assemblyName = type.Assembly.FullName;
                        if (type.IsGenericType)
                        {
                            var def = type.GetGenericTypeDefinition();
                            tuple = new Tuple<string, string, Type[]>(
                                def.Assembly.FullName, def.FullName,
                                type.GetGenericArguments()
                                );
                        }
                        else
                        {
                            tuple = new Tuple<string, string, Type[]>(
                                assemblyName, type.FullName, null);
                        }
                        TypeCache[type] = tuple;
                    }
                }
                _writer.WriteStartObject();
                Prop("assemblyName", tuple.Item1);
                Prop("typeName", tuple.Item2);
                Prop("genericArguments", Enumerable(tuple.Item3, Type));
                _writer.WriteEndObject();
            }
        }

        private Action Constructor(ConstructorInfo constructor)
        {
            return () => ConstructorInternal(constructor);
        }

        private void ConstructorInternal(ConstructorInfo constructor)
        {
            if (constructor == null)
            {
                _writer.WriteNull();
            }
            else
            {
                _writer.WriteStartObject();
                Prop("type", Type(constructor.DeclaringType));
                Prop("name", constructor.Name);
                Prop("signature", constructor.ToString());
                _writer.WriteEndObject();
            }
        }

        private Action Method(MethodInfo method)
        {
            return () => MethodInternal(method);
        }

        private void MethodInternal(MethodInfo method)
        {
            if (method == null)
            {
                _writer.WriteNull();
            }
            else
            {
                _writer.WriteStartObject();
                if (method.IsGenericMethod)
                {
                    var meth = method.GetGenericMethodDefinition();
                    var generic = method.GetGenericArguments();

                    Prop("type", Type(meth.DeclaringType));
                    Prop("name", meth.Name);
                    Prop("signature", meth.ToString());
                    Prop("generic", Enumerable(generic, Type));
                }
                else
                {
                    Prop("type", Type(method.DeclaringType));
                    Prop("name", method.Name);
                    Prop("signature", method.ToString());
                }
                _writer.WriteEndObject();
            }
        }

        private Action Property(PropertyInfo property)
        {
            return () => PropertyInternal(property);
        }

        private void PropertyInternal(PropertyInfo property)
        {
            if (property == null)
            {
                _writer.WriteNull();
            }
            else
            {
                _writer.WriteStartObject();
                Prop("type", Type(property.DeclaringType));
                Prop("name", property.Name);
                Prop("signature", property.ToString());
                _writer.WriteEndObject();
            }
        }

        private Action Member(MemberInfo member)
        {
            return () => MemberInternal(member);
        }

        private void MemberInternal(MemberInfo member)
        {
            if (member == null)
            {
                _writer.WriteNull();
            }
            else
            {
                _writer.WriteStartObject();
                Prop("type", Type(member.DeclaringType));
                Prop("memberType", (int) member.MemberType);
                Prop("name", member.Name);
                Prop("signature", member.ToString());
                _writer.WriteEndObject();
            }
        }
    }
}