using System.ComponentModel;
using System.Runtime.CompilerServices;
using Kengic.Was.CrossCutting.ConfigurationSection;

namespace Kengic.Was.Presentation.Server.Module.Common.ViewModels
{
    public class SeviceBase : INotifyPropertyChanged, IStarStop
    {
        private string _description;
        private string _id;
        private string _name;
        private StartupType _startupType;
        private StatusType _status;

        public string Name
        {
            get { return _name; }

            set
            {
                if (_name == value)
                {
                    return;
                }
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Id
        {
            get { return _id; }

            set
            {
                if (_id == value)
                {
                    return;
                }
                _id = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (_description == value)
                {
                    return;
                }
                _description = value;
                OnPropertyChanged();
            }
        }

        public virtual StatusType Status
        {
            get { return _status; }
            set
            {
                if (_status == value)
                {
                    return;
                }
                _status = value;
                OnPropertyChanged();
            }
        }

        public StartupType StartupType
        {
            get { return _startupType; }
            set
            {
                if (_startupType == value)
                {
                    return;
                }
                _startupType = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void Start()
        {
        }

        public virtual void Stop()
        {
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}