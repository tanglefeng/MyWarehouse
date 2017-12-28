using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using Kengic.Was.CrossCutting.Configuration;
using Kengic.Was.CrossCutting.ConfigurationSection.TypeFormatters;
using DynamicExpression = System.Linq.Dynamic.DynamicExpression;

namespace Kengic.Was.CrossCutting.Common
{
    public static class JsonFormatters
    {
        public static readonly ConcurrentDictionary<Type, Delegate> TypeConcurrentDictionary =
            new ConcurrentDictionary<Type, Delegate>();

        static JsonFormatters()
        {
            LoadConfiguration();
        }

        private static void LoadConfiguration()
        {
            var typeFormatterConfiguration = ConfigurationOperation<TypeFormatterSection>.GetCustomSection(
                FilePathExtension.TypeFormatterPath, "typeFormatterSection");
            if (typeFormatterConfiguration == null)
            {
                return;
            }
            foreach (TypeFormatterElement typeFormatter in typeFormatterConfiguration.TypeFormatters)
            {
                var p = Expression.Parameter(typeFormatter.Type, "r");
                var e = DynamicExpression.ParseLambda(new[] {p}, typeof (object), typeFormatter.Expression);
                var func = e.Compile();
                TypeConcurrentDictionary.TryAdd(typeFormatter.Type, func);
            }
        }
    }
}