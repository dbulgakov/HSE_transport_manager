using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Input;
using HSE_transport_manager.Common.Interfaces;

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
        // <summary> 
        /// Initializes a new instance of the MainViewModel class. 
        /// </summary> 
        /// 
        private ViewModelBase _currentViewModel;

        private IDialogProvider _dialogProvider;

        static readonly StatusViewModel StatusViewModel = new StatusViewModel();
        static readonly SettingsViewModel SettingsViewModel = new SettingsViewModel();
        static readonly AboutViewModel AboutViewModel = new AboutViewModel();

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

        public ICommand StatusCommand { get; private set; }
        public ICommand SettingsCommand { get; private set; }
        public ICommand AboutCommand { get; private set; }


        public MainViewModel(IDialogProvider dialogProvider)
        {
            _dialogProvider = dialogProvider;
            CurrentViewModel = SettingsViewModel;
            StatusCommand = new RelayCommand(ExecuteStatusCommand);
            SettingsCommand = new RelayCommand(ExecuteSettingsCommand);
            AboutCommand = new RelayCommand(ExecuteAboutCommand);
        }

        private void ExecuteStatusCommand()
        {
            CurrentViewModel = StatusViewModel;
        }

        private void ExecuteSettingsCommand()
        {
            CurrentViewModel = SettingsViewModel;
        }
        private void ExecuteAboutCommand()
        {
            CurrentViewModel = AboutViewModel;
        }

    }
}