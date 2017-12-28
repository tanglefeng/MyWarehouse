using System;
using System.IO;
using System.Windows;
using Kengic.Was.CrossCutting.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Kengic.Was.CrossCutting.ExceptionHandling
{
    public static class HandleExceptions
    {
        public static void Handle(Exception ex, string policy)
        {
            bool rethrow;
            try
            {
                rethrow = ExceptionPolicy.HandleException(ex, policy);
            }
            catch (Exception innerEx)
            {
                var errorMsg = "An unexpected exception occurred while " +
                               "calling HandleException with policy'" + policy + "'. ";
                errorMsg += Environment.NewLine + innerEx;

                MessageBox.Show(errorMsg, "Application Error");
                throw ex;
            }

            if (rethrow)
            {
                //警告:这种抛错会导致原始错误函数调用的堆栈信息丢失。throw;则不会。
                throw ex;
            }
            MessageBox.Show("An " + ex.GetType().Name + " occurred and has been logged. Please contact support.");
        }

        public static void LoadLogConfiguration(string filePath)
        {
            var file = Path.Combine(FilePathExtension.ProfileDirectory, filePath);
            var config = new FileConfigurationSource(file);
            var factory = new ExceptionPolicyFactory(config);
            Logger.SetLogWriter(new LogWriterFactory(config).Create());
            var exceptionManager = factory.CreateManager();
            ExceptionPolicy.SetExceptionManager(exceptionManager);
        }
    }
}