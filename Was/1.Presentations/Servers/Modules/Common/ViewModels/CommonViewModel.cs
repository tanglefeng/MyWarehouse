using System.Collections;
using System.ComponentModel;
using System.Windows.Input;
using Kengic.Was.CrossCutting.Configuration;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.TypeAdapter;
using Kengic.Was.Systems.Message;
using Prism.Commands;

namespace Kengic.Was.Presentation.Server.Module.Common.ViewModels
{
    public class CommonViewModel
    {
        private ICommand _restartCommand;
        private ICommand _startCommand;
        private ICommand _stopCommand;

        public CommonViewModel()
        {
            LogRepository.LoadLogConfiguration(FilePathExtension.LogPath);
            MessageRepository.LoadMessageConfiguration(FilePathExtension.MessagePath);
            AutomapperBootstrapper.Run();
        }

        public ICommand StartCommand
        {
            get
            {
                if (_startCommand != null)
                {
                    return _startCommand;
                }
                _startCommand = new DelegateCommand<IEnumerable>(Start);
                return _startCommand;
            }
        }

        public ICommand StopCommand
        {
            get
            {
                if (_stopCommand != null)
                {
                    return _stopCommand;
                }
                _stopCommand = new DelegateCommand<IEnumerable>(Stop);
                return _stopCommand;
            }
        }

        public ICommand RestartCommand
        {
            get
            {
                if (_restartCommand != null)
                {
                    return _restartCommand;
                }
                _restartCommand = new DelegateCommand<IEnumerable>(Restart);
                return _restartCommand;
            }
        }

        private static void Start(IEnumerable enumerable)
        {
            foreach (SeviceBase selectedSeviceBase in enumerable)
            {
                var bw = new BackgroundWorker();
                var closure = selectedSeviceBase;
                bw.DoWork += (sender, args) => closure.Start();
                bw.RunWorkerAsync();
            }
        }

        private static void Stop(IEnumerable enumerable)
        {
            foreach (SeviceBase selectedSeviceBase in enumerable)
            {
                var bw = new BackgroundWorker();
                var closure = selectedSeviceBase;
                bw.DoWork += (sender, args) => closure.Stop();
                bw.RunWorkerAsync();
            }
        }

        private static void Restart(IEnumerable enumerable)
        {
            foreach (SeviceBase selectedSeviceBase in enumerable)
            {
                var bw = new BackgroundWorker();
                var closure = selectedSeviceBase;
                bw.DoWork += (sender, args) =>
                {
                    closure.Stop();
                    closure.Start();
                };
                bw.RunWorkerAsync();
            }
        }
    }
}