using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.IO;
using System.Windows.Input;
using System.Xml.Serialization;
using HSE_transport_manager.Common.Interfaces;
using HSE_transport_manager.Common.Models;
using HSE_transport_manager.Properties;
using System.Threading.Tasks;
using HSE_transport_manager.Common.Models.TrainSchedulesData;

namespace HSE_transport_manager.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {

        private ICommand _saveCommand;
        private const string FileName = "settings.xml";
        private IDialogProvider _dialogProvider;
        private PluginManager plaginManager = new PluginManager();

        public SettingsViewModel()
        {
            var keyData = new KeyData();
            if (File.Exists(FileName))
            {
                try
                {
                    keyData = ReadXml();
                }
                catch
                {
                    // ignored
                }
            }

            TGKey = keyData.BotServiceKey;
            YandexKey = keyData.ScheduleServiceKey;
            GoogleKey = keyData.MonitoringServiceKey;
            UberKey = keyData.TaxiServiceKey;
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


        void Save()
        {
            var keyData = new KeyData
            {
                BotServiceKey = TGKey,
                MonitoringServiceKey = GoogleKey,
                ScheduleServiceKey = YandexKey,
                TaxiServiceKey = UberKey
            };
            try
            {
                SaveXml(keyData);
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
            try
            {
                SaveXml(new KeyData());
            }
            catch
            {
                _dialogProvider.ShowMessage(Resources.SettingsViewModel_Reset_Error_saving_file_message);
            }
        }

        async void Update()
        {
            UpdateEnable = false;
            var dbService = plaginManager.LoadDbService();
            var keyData = ReadXml();
            var scheduleService = plaginManager.LoadScheduleService();
            scheduleService.Initialize(keyData.ScheduleServiceKey);
            var task = await scheduleService.GetDailyScheduleAsync("s9600721", "s2000006");
            await Task.Run(() => dbService.RefreshTrainSchedule(task));

            // ПОТОМ ЭТО УБРАТЬ
            _dialogProvider = new WpfMessageProvider();
           _dialogProvider.ShowMessage("Complete");
            //
        }



        private void SaveXml(KeyData keyData)
        {
            using (var fs = new FileStream(FileName, FileMode.Create))
            {
                var formatter = new XmlSerializer(typeof(KeyData));
                formatter.Serialize(fs, keyData);
            }
        }

        private KeyData ReadXml()
        {
            KeyData keyData;
            using (var fs = new FileStream(FileName, FileMode.OpenOrCreate))
            {
                    var formatter = new XmlSerializer(typeof(KeyData));
                    keyData = (KeyData)formatter.Deserialize(fs);
            }
            return keyData;
        }
    }
}
