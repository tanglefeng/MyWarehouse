using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using System.Linq;
using Kengic.Was.CrossCutting.Common;
using Kengic.Was.CrossCutting.Configuration;
using Kengic.Was.CrossCutting.ConfigurationSection.Conventions;
using Kengic.Was.CrossCutting.ConfigurationSection.FileConfigs;
using Kengic.Was.CrossCutting.ConfigurationSection.TypeConfigurations;
using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Model.Was.Migrations;

namespace Kengic.Was.Domain.Model.Was
{
    public class WasModel
        : DbContext, IQueryableUnitOfWork
    {
        public WasModel()
        {
            //LogDebug();
            Configuration.ValidateOnSaveEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            Configuration.UseDatabaseNullSemantics = true;
        }
        static WasModel()
        {
            InitializeDataStore();
        }

        private static void InitializeDataStore()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<WasModel, WasModelConfiguration>());

            var configuration = new WasModelConfiguration();
            var migrator = new DbMigrator(configuration);
            if (migrator.GetPendingMigrations().Any())
            {
                migrator.Update();
            }
        }

        public bool AutoDetectChangesEnabled
        {
            get { return Configuration.AutoDetectChangesEnabled; }
            set { Configuration.AutoDetectChangesEnabled = value; }
        }

        public IDbSet<TEntity> CreateSet<TEntity>()
            where TEntity : class => Set<TEntity>();

        public void Attach<TEntity>(TEntity item)
            where TEntity : class => Set<TEntity>().Attach(item);

        public void SetModified<TEntity>(TEntity item)
            where TEntity : class => Entry(item).State = EntityState.Modified;

        public void SetDeleted<TEntity>(TEntity item) where TEntity : class => Entry(item).State = EntityState.Deleted;
        public void SetDetached<TEntity>(TEntity item) where TEntity : class => Entry(item).State = EntityState.Detached;

        public void Commit() => SaveChanges();

        public void CommitAndRefreshChanges()
        {
            bool saveFailed;

            do
            {
                try
                {
                    SaveChanges();

                    saveFailed = false;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    ex.Entries.ToList()
                        .ForEach(entry => entry.OriginalValues.SetValues(entry.GetDatabaseValues()));
                }
            } while (saveFailed);
        }

        public void RollbackChanges() => ChangeTracker.Entries()
            .ToList()
            .ForEach(entry => entry.State = EntityState.Unchanged);

        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters)
            => Database.SqlQuery<TEntity>(sqlQuery, parameters);

        public int ExecuteCommand(string sqlCommand, params object[] parameters)
            => Database.ExecuteSqlCommand(sqlCommand, parameters);

        public void SetPropertiesModified<TEntity>(TEntity item, List<string> properties) where TEntity : class
        {
            Set<TEntity>().Add(item);
            var entry = Entry(item);
            entry.State = EntityState.Unchanged;
            foreach (var property in properties)
            {
                entry.Property(property).IsModified = true;
            }
        }

        [Conditional("DEBUG")]
        // ReSharper disable once UnusedMember.Local
        private void LogDebug() => Database.Log = r => Debug.WriteLine(r);

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(nameof(WasModel).Replace("Model", string.Empty).ToUpper());
            AddConfigurations(modelBuilder);
            if (Database.Connection.GetType().Name == "SqlConnection")
            {
                AddConventions(modelBuilder);
            }
        }

        public void AddConfigurations(DbModelBuilder modelBuilder)
        {
            var configurations =
                ConfigurationOperation<FileConfigSection>.GetCustomSection(FilePathExtension.TypeConfigurationPath,
                    "fileConfigSection");
            if (configurations == null)
            {
                throw new NullReferenceException("ConfigFileSection");
            }
            foreach (
                var typeConfigurationSection in from FileConfigElement configFileElement in configurations.FileConfigs
                    select
                        ConfigurationOperation<TypeConfigurationSection>.GetCustomSection(configFileElement.FilePath,
                            configFileElement.SectionName))
            {
                if (typeConfigurationSection == null)
                {
                    throw new NullReferenceException("TypeConfigurationSection");
                }

                foreach (
                    dynamic configurationInstance in
                        from TypeConfigurationElement typeConfigurationElement in
                            typeConfigurationSection.TypeConfigurations
                        select typeConfigurationElement.Type
                        into type
                        where
                            type.IsStructuralTypeConfiguration(typeof (EntityTypeConfiguration<>)) ||
                            type.IsStructuralTypeConfiguration(typeof (ComplexTypeConfiguration<>))
                        select Activator.CreateInstance(type))
                {
                    modelBuilder.Configurations.Add(configurationInstance);
                }
            }
        }

        public void AddConventions(DbModelBuilder modelBuilder)
        {
            var conventions =
                ConfigurationOperation<FileConfigSection>.GetCustomSection(FilePathExtension.ConventionPath,
                    "fileConfigSection");
            if (conventions == null)
            {
                throw new NullReferenceException("ConfigFileSection");
            }
            foreach (var conventionSection in from FileConfigElement configFileElement in conventions.FileConfigs
                select
                    ConfigurationOperation<ConventionSection>.GetCustomSection(configFileElement.FilePath,
                        configFileElement.SectionName))
            {
                if (conventionSection == null)
                {
                    throw new NullReferenceException("ConventionSection");
                }

                foreach (Convention configurationInstance in
                    from ConventionElement typeConfigurationElement in
                        conventionSection.Conventions
                    select typeConfigurationElement.Type
                    into type
                    where
                        (type.BaseType != null) && (type.BaseType == typeof (Convention))
                    select Activator.CreateInstance(type))
                {
                    modelBuilder.Conventions.Add(configurationInstance);
                }
            }
        }
    }
}