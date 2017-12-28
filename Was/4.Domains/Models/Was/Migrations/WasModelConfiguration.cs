using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using Kengic.Was.CrossCutting.Configuration;
using Newtonsoft.Json;

namespace Kengic.Was.Domain.Model.Was.Migrations
{
    internal sealed class WasModelConfiguration : DbMigrationsConfiguration<WasModel>
    {
        public WasModelConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = typeof(WasModel).FullName;
        }

        protected override void Seed(WasModel context) => DataInitialize();

        private readonly IEnumerable<Type> _types = AppDomain.CurrentDomain.GetAssemblies().Where(r => r.FullName.Contains("Kengic")).SelectMany(r => r.GetTypes());

        internal void DataInitialize()
        {
            var dataInitConfigsFilePath = Path.Combine(FilePathExtension.ProfileDirectory, FilePathExtension.DataInitializationPath);
            var dataInitConfigsFile = File.ReadAllText(dataInitConfigsFilePath);
            var fileConfigs = JsonConvert.DeserializeObject<List<FileConfigs>>(dataInitConfigsFile);
            foreach (var fileConfig in fileConfigs)
            {
                var dataInitFilePath = Path.Combine(FilePathExtension.ProfileDirectory, fileConfig.FilePath);
                var json = File.ReadAllText(dataInitFilePath);
                var type = _types.FirstOrDefault(r => r.Name == fileConfig.Type);
                if (type == null) continue;
                {
                    using (var wasModel = new WasModel())
                    {
                        var listType = typeof(List<>);
                        var typeList = listType.MakeGenericType(type);
                        dynamic objs = JsonConvert.DeserializeObject(json, typeList);
                        var typeContext = wasModel.GetType();
                        var setGenericMethod = typeContext.GetMethods().FirstOrDefault(r => r.IsGenericMethod && (r.Name == "Set"));
                        var setType = setGenericMethod?.MakeGenericMethod(type);
                        var resultSet = setType?.Invoke(wasModel, null);
                        var addOrUpdateMethodGeneric = typeof(DbSetMigrationsExtensions).GetMethods().FirstOrDefault(r => r.Name == "AddOrUpdate");
                        var addOrUpdateMethod = addOrUpdateMethodGeneric?.MakeGenericMethod(type);
                        addOrUpdateMethod?.Invoke(resultSet, new object[] { resultSet, objs.ToArray() });
                        var changes = wasModel.ChangeTracker.Entries().Where(r => r.Entity.GetType() != type);
                        foreach (var change in changes)
                        {
                            change.State = EntityState.Unchanged;
                        }
                        wasModel.SaveChanges();
                    }
                }
            }
        }

        public class FileConfigs
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Code { get; set; }
            public string FilePath { get; set; }
            public string Type { get; set; }
        }
    }
}