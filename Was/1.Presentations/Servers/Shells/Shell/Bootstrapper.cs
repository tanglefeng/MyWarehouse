using System;
using System.Windows;
using System.Windows.Threading;
using Kengic.Was.CrossCutting.Configuration;
using Kengic.Was.CrossCutting.ExceptionHandling;
using Microsoft.Practices.ServiceLocation;
using Prism.Modularity;
using Prism.Unity;

namespace Kengic.Was.Presentation.Server.Shell
{
    internal class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell() => ServiceLocator.Current.GetInstance<Views.Shell>();

        protected override void InitializeShell()
        {
            CrossCutting.Unity.UnityBootstrapper.Run();
            Application.Current.MainWindow = (Window) Shell;
            Application.Current.MainWindow.Show();
            InitializeExceptionHanding();
        }

        private static void InitializeExceptionHanding()
        {
            HandleExceptions.LoadLogConfiguration(FilePathExtension.ExceptionHandingPath);
            Application.Current.DispatcherUnhandledException += ApplicationOnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        protected override void ConfigureModuleCatalog()
        {
            var configurationCatalog = new ConfigurationModuleCatalog
            {
                Store = new ConfigurationFileStore()
            };
            configurationCatalog.Load();
            foreach (var item in configurationCatalog.Modules)
            {
                ModuleCatalog.AddModule(item);
            }
        }

        private static void ApplicationOnDispatcherUnhandledException(object sender,
            DispatcherUnhandledExceptionEventArgs dispatcherUnhandledExceptionEventArgs)
        {
            HandleExceptions.Handle(dispatcherUnhandledExceptionEventArgs.Exception, "Default Policy");
            dispatcherUnhandledExceptionEventArgs.Handled = true;
        }

        private static void CurrentDomain_UnhandledException(object sender,
            UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                HandleExceptions.Handle(ex, "Default Policy");
            }
        }
    }
}