using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace HSE_transport_manager.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        private ViewModelBase _currentViewModel;

        readonly static SettingsViewModel _settingsViewModel = new SettingsViewModel();
        readonly static ControlViewModel _controlViewModel = new ControlViewModel();

        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                if (_currentViewModel == value)
                    return;
                _currentViewModel = value;
                RaisePropertyChanged("CurrentViewModel");
            }
        }
        public ICommand ControlViewCommand { get; private set; }
        public ICommand SettingsViewCommand { get; private set; }

        public MainViewModel()
        {
            CurrentViewModel = MainViewModel._controlViewModel;
            ControlViewCommand = new RelayCommand(() => ExecuteControlViewCommand());
            SettingsViewCommand = new RelayCommand(() => ExecuteSettingsViewCommand());
        }

        private void ExecuteControlViewCommand()
        {
            CurrentViewModel = MainViewModel._controlViewModel;
        }

        private void ExecuteSettingsViewCommand()
        {
            CurrentViewModel = MainViewModel._settingsViewModel;
        }
    }
}