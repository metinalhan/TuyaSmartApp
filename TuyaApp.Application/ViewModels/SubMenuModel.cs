using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TuyaApp.Application.ViewModels
{
    public class SubMenuModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public SubMenuModel(string subMenuName, string guid)
        {
            SubMenuName = subMenuName;
            Guid = guid;
        }

        public string SubMenuName { get; }
        public string Guid { get; }

        public int DeviceId { get; set; }

        private string deviceName;
        public string DeviceName { get => deviceName; set { deviceName = value; OnPropertyChanged(); } }

        private string buttonFunction;
        public string ButtonFunction { get => buttonFunction; set { buttonFunction = value; OnPropertyChanged(); } }
    }
}
