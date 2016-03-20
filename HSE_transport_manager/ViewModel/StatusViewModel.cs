using System.IO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;
using HSE_transport_manager.Common.Interfaces;
using HSE_transport_manager.Common.Models;
using HSE_transport_manager.Properties;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HSE_transport_manager.ViewModel
{
    public class StatusViewModel : ViewModelBase
    {
        private ICommand _startCommand;
        private CancellationTokenSource _ctoken;
        private readonly IDialogProvider _dialogProvider;
        private const string FileName = "settings.xml";

        public StatusViewModel()
        {
            _dialogProvider = new WpfMessageProvider();
        }


        public ICommand StartCommand
        {
            get
            {
                if (_startCommand == null)
                {
                    _startCommand = new RelayCommand(
                    Start);
                }
                return _startCommand;
            }
        }

        private ICommand _stopCommand;

        public ICommand StopCommand
        {
            get
            {
                if (_stopCommand == null)
                {
                    _stopCommand = new RelayCommand(
                    Stop);
                }
                return _stopCommand;
            }
        }

        private string _uberStatus;

        public string UberStatus
        {
            get { return _uberStatus; }
            set
            {
                if (value != _uberStatus)
                {
                    _uberStatus = value;
                    RaisePropertyChanged("UberStatus");
                }
            }
        }

        private string _yandexStatus;

        public string YandexStatus
        {
            get { return _yandexStatus; }
            set
            {
                if (value != _yandexStatus)
                {
                    _yandexStatus = value;
                    RaisePropertyChanged("YandexStatus");
                }
            }
        }

        private string _googleStatus;

        public string GoogleStatus
        {
            get { return _googleStatus; }
            set
            {
                if (value != _googleStatus)
                {
                    _googleStatus = value;
                    RaisePropertyChanged("GoogleStatus");
                }
            }
        }

        private string _tgStatus;

        public string TGStatus
        {
            get { return _tgStatus; }
            set
            {
                if (value != _tgStatus)
                {
                    _tgStatus = value;
                    RaisePropertyChanged("TGStatus");
                }
            }
        }


        private string _botStatus = Resources.StatusViewModel__botStatus_Bot_is_inactive_message;

        public string BotStatus
        {
            get { return _botStatus; }
            set
            {
                if (value != _botStatus)
                {
                    _botStatus = value;
                    RaisePropertyChanged("BotStatus");
                }
            }
        }


        private int _errors;

        public int Errors
        {
            get { return _errors; }
            set
            {
                if (value != _errors)
                {
                    _errors = value;
                    RaisePropertyChanged("Errors");
                }
            }
        }


        async void Start()
        {
            try
            {
                var keyData = ReadXml();
                var bot = new Api(keyData.BotServiceKey);
                var me = await bot.GetMe();
                BotStatus = Resources.StatusViewModel_Start_Bot_is_active_message;
                BotWork(bot);
            }
            catch
            {
                _dialogProvider.ShowMessage(Resources.StatusViewModel_Start_Error_contacting_bot_message);
            }
        }

        void Stop()
        {
            if (_ctoken != null)
            {
                _ctoken.Cancel();
            }
            BotStatus = Resources.StatusViewModel__botStatus_Bot_is_inactive_message;
        }

        async void BotWork(Api bot)
        {
            await Task.Run(() =>
            {
                try
                {
                    var offset = 0;
                    while (true)
                    {
                        var updates = bot.GetUpdates(offset).Result;
                        _ctoken.Token.ThrowIfCancellationRequested();
                        foreach (var update in updates)
                        {
                            if (update.Message.Type == MessageType.TextMessage)
                            {
                                if (update.Message.Text.Equals(Resources.StatusViewModel_BotWork__check_message))
                                {
                                    bot.SendTextMessage(update.Message.Chat.Id, "Даша, пили базу.");
                                }
                                else
                                {
                                    bot.SendTextMessage(update.Message.Chat.Id, Resources.StatusViewModel_BotWork_Unknown_input_message);
                                }
                            }
                            offset = update.Id + 1;
                        }
                    }
                }

                catch
                {
                    _dialogProvider.ShowMessage(Resources.StatusViewModel_BotWork_Bot_is_stopped_message);
                }
            }, _ctoken.Token);
            
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
