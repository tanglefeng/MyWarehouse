using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Kengic.Was.CrossCutting.LinqToWcf.Communications
{
    public static class DataContractResolverExtensions
    {
        private static IEnumerable<Type> _knownTypes;
        private static DataContractResolver _dataContractResolver;

        private static IEnumerable<Type> KnownTypes
        {
            get
            {
                if (_knownTypes == null)
                {
                    var currentContext = OperationContext.Current;
                    _knownTypes =
                        currentContext.Host.Description.Endpoints.SelectMany(r => r.Contract.Operations)
                            .SelectMany(r => r.KnownTypes)
                            .Distinct().ToList();
                }
                return _knownTypes;
            }
        }

        public static DataContractResolver KnownTypeDataContractResolver
        {
            get
            {
                if (_dataContractResolver == null)
                {
                    var dcs = new DataContractSerializer(typeof (object), KnownTypes);
                    var dataContractType =
                        Type.GetType(
                            "System.Runtime.Serialization.DataContract, System.Runtime.Serialization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
                    var xmlObjectSerializerContextType = Type.GetType(
                        "System.Runtime.Serialization.XmlObjectSerializerContext, System.Runtime.Serialization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
                    var xmlObjectSerializerContextConstructor = xmlObjectSerializerContextType.GetConstructor(
                        BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static, null,
                        new[]
                        {
                            typeof (DataContractSerializer), dataContractType, typeof (DataContractResolver)
                        }, null);
                    var xmlObjectSerializerContext =
                        xmlObjectSerializerContextConstructor.Invoke(new object[] {dcs, null, null});
                    var knownTypeDataContractResolverType = Type.GetType(
                        "System.Runtime.Serialization.KnownTypeDataContractResolver, System.Runtime.Serialization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
                    var knownTypeDataContractResolverConstructor = knownTypeDataContractResolverType.GetConstructor(
                        BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static, null,
                        new[] {xmlObjectSerializerContextType}, null);
                    _dataContractResolver =
                        knownTypeDataContractResolverConstructor.Invoke(new[] {xmlObjectSerializerContext}) as
                            DataContractResolver;
                }
                return _dataContractResolver;
            }
        }
    }
}