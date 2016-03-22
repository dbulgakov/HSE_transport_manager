using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.IO;
using System.Windows.Input;
using System.Xml.Serialization;
using HSE_transport_manager.Common.Interfaces;
using HSE_transport_manager.Common.Models;
using HSE_transport_manager.Properties;
using System.Threading.Tasks;
using System;


namespace HSE_transport_manager.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private StatusViewModel s = new StatusViewModel();

        private ICommand _saveCommand;
        private const string FileName = "settings.xml";
        private IDialogProvider _dialogProvider;
        private PluginManager plaginManager = new PluginManager();


        public SettingsViewModel()
        {
            var settingsData = new SettingsData();
            if (File.Exists(FileName))
            {
                try
                {
                    Enable = false;
                    settingsData = ReadXml();
                }
                catch
                {
                    // ignored
                }
            }

            TGKey = settingsData.BotServiceKey;
            YandexKey = settingsData.ScheduleServiceKey;
            GoogleKey = settingsData.MonitoringServiceKey;
            UberKey = settingsData.TaxiServiceKey;
            if (settingsData.UpdateTime.Ticks > 0)
                UpdateStatus = Resources.SettingsViewModel_Last_update_message + settingsData.UpdateTime.ToString("dd.MM.yyyy HH:mm:ss");
            _dialogProvider = new WpfMessageProvider();
        }


        private bool _updateEnable = true;

        public bool UpdateEnable
        {
            get { return _updateEnable; }
            set
            {
                if (value != _updateEnable)
                {
                    _updateEnable = value;
                    RaisePropertyChanged("UpdateEnable");
                }
            }
        }

        private bool _enable = true;

        public bool Enable
        {
            get { return _enable; }
            set
            {
                if (value != _enable)
                {
                    _enable = value;
                    RaisePropertyChanged("Enable");
                }
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(
                    Save,
                    CheckKeys);
                }
                return _saveCommand;
            }
        }

        private bool CheckKeys()
        {
            return (UberKey != null && YandexKey != null && GoogleKey != null && TGKey != null);
        }


        private ICommand _updateCommand;

        public ICommand UpdateCommand
        {
            get
            {
                if (_updateCommand == null)
                {
                    _updateCommand = new RelayCommand(
                    Update);
                }
                return _updateCommand;
            }
        }

        private ICommand _resetCommand;

        public ICommand ResetCommand
        {
            get
            {
                if (_resetCommand == null)
                {
                    _resetCommand = new RelayCommand(
                    Reset);
                }
                return _resetCommand;
            }
        }

        private string _updateStatus;

        public string UpdateStatus
        {
            get { return _updateStatus; }
            set
            {
                if (value != _updateStatus)
                {
                    _updateStatus = value;
                    RaisePropertyChanged("UpdateStatus");
                }
            }
        }


        private string _uberKey;

        public string UberKey
        {
            get { return _uberKey; }
            set
            {
                if (value != _uberKey)
                {
                    _uberKey = value;
                    RaisePropertyChanged("UberKey");
                }
            }
        }


        private string _yandexKey;

        public string YandexKey
        {
            get { return _yandexKey; }
            set
            {
                if (value != _yandexKey)
                {
                    _yandexKey = value;
                    RaisePropertyChanged("YandexKey");
                }
            }
        }

        private string _googleKey;

        public string GoogleKey
        {
            get { return _googleKey; }
            set
            {
                if (value != _googleKey)
                {
                    _googleKey = value;
                    RaisePropertyChanged("GoogleKey");
                }
            }
        }

        private string _tgKey;

        public string TGKey
        {
            get { return _tgKey; }
            set
            {
                if (value != _tgKey)
                {
                    _tgKey = value;
                    RaisePropertyChanged("TGKey");
                }
            }
        }

        private string _statusBarText;

        public string StatusBarText
        {
            get { return _statusBarText; }
            set
            {
                if (value != _statusBarText)
                {
                    _statusBarText = value;
                    RaisePropertyChanged("StatusBarText");
                }
            }
        }


        void Save()
        {
            Enable = false;
            var keyData = new SettingsData
            {
                BotServiceKey = TGKey,
                MonitoringServiceKey = GoogleKey,
                ScheduleServiceKey = YandexKey,
                TaxiServiceKey = UberKey
            };
            try
            {
                SaveXml(keyData);
                s.UberStatus = "OK";
            }
            catch
            {
                _dialogProvider.ShowMessage(Resources.SettingsViewModel_Reset_Error_saving_file_message);
            }
        }

        void Reset()
        {
            UberKey = null;
            YandexKey = null;
            GoogleKey = null;
            TGKey = null;
            Enable = true;
            try
            {
                SaveXml(new SettingsData());

            }
            catch
            {
                _dialogProvider.ShowMessage(Resources.SettingsViewModel_Reset_Error_saving_file_message);
            }
        }

        async void Update()
        {
            try
            {
                UpdateEnable = false;
                var dbService = plaginManager.LoadDbService();
                var settingsData = ReadXml();
                var scheduleService = plaginManager.LoadScheduleService();
                scheduleService.Initialize(settingsData.ScheduleServiceKey);
                StatusBarText = Resources.SettingsViewModel_Update_Get_Schedule_message;
                var task = await scheduleService.GetDailyScheduleAsync("s9600721", "s2000006");
                StatusBarText = Resources.SettingsViewModel_Update_DB_message;
                await Task.Run(() => dbService.RefreshTrainSchedule(task));
                UpdateStatus = Resources.SettingsViewModel_Last_update_message +
                               DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
                SaveXml(settingsData);
            }
            catch (InvalidOperationException e)
            {
                _dialogProvider.ShowMessage(Resources.Start_No_keys_error_message);
            }
            catch
            {
                _dialogProvider.ShowMessage(Resources.Start_Unknown_error_message);
            }
        }

       


        private void SaveXml(SettingsData keyData)
        {
            using (var fs = new FileStream(FileName, FileMode.Create))
            {
                var formatter = new XmlSerializer(typeof(SettingsData));
                formatter.Serialize(fs, keyData);
            }
        }

        private SettingsData ReadXml()
        {
            SettingsData keyData;
            using (var fs = new FileStream(FileName, FileMode.OpenOrCreate))
            {
                    var formatter = new XmlSerializer(typeof(SettingsData));
                    keyData = (SettingsData)formatter.Deserialize(fs);
            }
            return keyData;
        }
    }
}
