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
using System;
using System.Collections.Generic;
using System.Text;

namespace HSE_transport_manager.ViewModel
{
    public class StatusViewModel : ViewModelBase
    {
        private ICommand _startCommand;
        private CancellationTokenSource _ctoken;
        private readonly IDialogProvider _dialogProvider;
        private const string FileName = "settings.xml";
        private PluginManager plaginManager = new PluginManager();

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
            var dbService = plaginManager.LoadDbService();
            var taxiService = plaginManager.LoadTaxiService();
            DateTime timeNow = DateTime.Now;
            //var k =dbService.GetRoute("Общежитие 1", "Трехсвятительский 3", timeNow);
            //var c = dbService.GetCoordinates("Общежитие 6");
            var p = dbService.GetDubkiSchedule("Дубки");
            //var h = dbService.GetFastestRoute("Общежитие 6", "Трехсвятительский 3", timeNow);
            int gv=9;
            try
            {
                _ctoken = new CancellationTokenSource();
                var keyData = ReadXml();
                var bot = new Api(keyData.BotServiceKey);
                taxiService.Initialize(keyData.TaxiServiceKey);
                BotStatus = Resources.StatusViewModel_Start_Bot_is_active_message;
                BotWork(bot, dbService, taxiService);
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

        async void BotWork(Api bot, IDatabaseService dbService, ITaxiService taxiService)
        {
            var dict = new Dictionary<long,string>();
            var rb = new ResponseBuilder(bot);
            await Task.Run(async () =>
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
                                if (dict.ContainsKey(update.Message.Chat.Id))
                                {
                                    switch (dict[update.Message.Chat.Id])
                                    {
                                        case "/get_route":
                                        {
                                            rb.FastestWayResponse(update, dbService);
                                            break;
                                        }
                                        case "/taxi_route":
                                        {
                                            rb.TaxiResponse(update, dbService, taxiService);
                                            break;
                                        }
                                        case "/get_bus":
                                        {
                                            rb.GetBusResponse(update, dbService);
                                            break;
                                        }
                                    }
                                    dict.Remove(update.Message.Chat.Id);
                                }
                                else
                                {
                                    switch (update.Message.Text)
                                    {
                                        case "/start":
                                        {
                                            bot.SendTextMessage(update.Message.Chat.Id, Resources.StatusViewModel_BotWork_Introduce_message).Wait();
                                            bot.SendTextMessage(update.Message.Chat.Id, Resources.StatusViewModel_BotWork_Intro_message);
                                            break;
                                        }
                                        case "/places":
                                        {
                                            var stb = new StringBuilder();
                                            var places = dbService.GetAllBuildings();
                                            foreach (var place in places)
                                            {
                                                stb.Append(place);
                                                stb.Append("\n ");
                                            }
                                            bot.SendTextMessage(update.Message.Chat.Id, string.Format(Resources.StatusViewModel_BotWork_Places_response_message, stb.ToString()));
                                            break;
                                        }
                                        case "/get_route":
                                        {
                                            bot.SendTextMessage(update.Message.Chat.Id, Resources.StatusViewModel_BotWork_Get_route_intro);
                                            dict.Add(update.Message.Chat.Id, "/get_route");
                                            break;
                                        }
                                        case "/taxi_route":
                                        {
                                            bot.SendTextMessage(update.Message.Chat.Id, Resources.StatusViewModel_BotWork_Get_route_intro);
                                            dict.Add(update.Message.Chat.Id, "/taxi_route");
                                            break;
                                        }
                                        case "/get_bus":
                                        {
                                            bot.SendTextMessage(update.Message.Chat.Id, "Введите станцию отправления. Поддерживаются следующие станции:\n1. Одинцово\n2. Дубки\n3. Славянский Бульвар");
                                            dict.Add(update.Message.Chat.Id, "/get_bus");
                                            break;
                                        }
                                        case "/shmowzow":
                                        {
                                            bot.SendTextMessage(update.Message.Chat.Id, Resources.StatusViewModel_BotWork_That_message);
                                            break;
                                        }
                                        default:
                                        {
                                            bot.SendTextMessage(update.Message.Chat.Id, Resources.StatusViewModel_BotWork_Unknown_message);
                                            break;
                                        }
                                    }
                                }
                                }
                            else
                            {
                                bot.SendTextMessage(update.Message.Chat.Id, Resources.StatusViewModel_BotWork_Unsupported_message);
                            }
                            offset = update.Id + 1;
                        }
                    }
                }

                catch
                {
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
