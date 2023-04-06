using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TuyaApp.Application.ViewModels
{
    public class MenuModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public MenuModel(string name, string guid, uint priority)
        {
            Name = name;
            Guid = guid;
            Priority = priority;
        }

        public string Name { get; }
        public string Guid { get; }


        public int DeviceId { get; set; }

        private string deviceName;
        public string DeviceName { get => deviceName; set { deviceName = value; OnPropertyChanged(); } }

        private string buttonFunction;
        public string ButtonFunction { get => buttonFunction; set { buttonFunction = value; OnPropertyChanged(); } }


        private uint _priority;
        public uint Priority
        {
            get => _priority;
            set
            {
                _priority = value;
                OnPropertyChanged();
            }

        }
    }
}
