using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TuyaApp.Application.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
       
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public abstract class ViewModelBase<TModel> : ViewModelBase
    where TModel : class
    {
        private TModel _model;

        public ViewModelBase(TModel model)
            => _model = model;

        public TModel Model
        {
            get => _model;
            set
            {
                if (ReferenceEquals(_model, value))
                {
                    return;
                }

                _model = value;
                OnPropertyChanged();
            }
        }
    }
}
