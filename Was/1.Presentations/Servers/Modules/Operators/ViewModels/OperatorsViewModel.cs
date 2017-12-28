using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Kengic.Was.CrossCutting.Configuration;
using Kengic.Was.CrossCutting.ConfigurationSection;
using Kengic.Was.CrossCutting.ConfigurationSection.Operators;
using Kengic.Was.Operator.Common;
using Kengic.Was.Presentation.Server.Module.Common.ViewModels;
using Kengic.Was.Systems.ActivityContracts;
using Microsoft.Practices.ServiceLocation;
using Prism.Commands;

namespace Kengic.Was.Presentation.Server.Module.Operators.ViewModels
{
    public class OperatorsViewModel : CommonViewModel
    {
        private ICommand _activeCommand;
        private ICommand _deactiveCommand;

        public OperatorsViewModel()
        {
            ActivityContractRepository.LoadActivityContractConfiguration(FilePathExtension.ActivityContractPath);
            LoadOperators();
            AutoStart();
        }

        public ObservableCollection<Operator> Operators { get; set; } = new ObservableCollection<Operator>();

        public ICommand ActiveCommand
        {
            get
            {
                if (_activeCommand != null)
                {
                    return _activeCommand;
                }
                _activeCommand = new DelegateCommand<IEnumerable>(Active);
                return _activeCommand;
            }
        }

        public ICommand DeactiveCommand
        {
            get
            {
                if (_deactiveCommand != null)
                {
                    return _deactiveCommand;
                }
                _deactiveCommand = new DelegateCommand<IEnumerable>(Deactive);
                return _deactiveCommand;
            }
        }

        private void LoadOperators()
        {
            OperatorRepository.LoadOperatorConfiguration(FilePathExtension.OperatorPath);
            var operatorSection =
                ConfigurationOperation<OperatorSection>.GetCustomSection(FilePathExtension.OperatorPath,
                    "operatorSection");
            if (operatorSection == null)
            {
                throw new Exception("File not found or file format is not correct");
            }
            foreach (OperatorElement operatorElement in operatorSection.Operators)
            {
                var iOperator = ServiceLocator.Current.GetInstance<IOperator>(operatorElement.Name);
                iOperator.OperatorElement = operatorElement;
                iOperator.Id = operatorElement.Id;
                var tempOperator = new Operator(iOperator)
                {
                    Id = operatorElement.Id,
                    Name = operatorElement.Name,
                    Description = operatorElement.Description,
                    StartupType = operatorElement.StartupType,
                    Status = StatusType.Empty
                };
                Operators.Add(tempOperator);
            }
        }

        private static void Active(IEnumerable enumerable)
        {
            foreach (Operator selectedSeviceBase in enumerable)
            {
                var bw = new BackgroundWorker();
                var closure = selectedSeviceBase;
                bw.DoWork += (sender, args) => closure.Active();
                bw.RunWorkerAsync();
            }
        }

        private static void Deactive(IEnumerable enumerable)
        {
            foreach (Operator selectedSeviceBase in enumerable)
            {
                var bw = new BackgroundWorker();
                var closure = selectedSeviceBase;
                bw.DoWork += (sender, args) => closure.Deactive();
                bw.RunWorkerAsync();
            }
        }

        private void AutoStart()
        {
            foreach (var item in Operators.Where(r => r.StartupType == StartupType.Automatic))
            {
                item.Start();
                item.Active();
            }
        }
    }
}