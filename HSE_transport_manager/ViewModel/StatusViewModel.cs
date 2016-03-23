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

namespace HSE_transport_manager.ViewModel
{
    public class StatusViewModel : ViewModelBase
    {
        private CancellationTokenSource _ctoken;
        private readonly IDialogProvider _dialogProvider;
        private const string FileName = "settings.xml";

        private const string InitialRequest = "/start";
        private const string PlacesRequest = "/places";
        private const string FastestRouteRequest = "/get_route";
        private const string AllRoutesRequest = "/get_all_routes";
        private const string TaxiRequest = "/taxi_route";
        private const string SuburbanRequest = "/get_suburban";
        private const string BusRequest = "/get_bus";
        private const string ShmowzowRequest = "/shmowzow";

        public StatusViewModel()
        {
            _dialogProvider = new WpfMessageProvider();
        }

        private ICommand _startCommand;
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


        private bool _startEnable = true;

        public bool StartEnable
        {
            get { return _startEnable; }
            set
            {
                if (value != _startEnable)
                {
                    _startEnable = value;
                    RaisePropertyChanged("StartEnable");
                }
            }
        }

        private bool _stopEnable = true;

        public bool StopEnable
        {
            get { return _stopEnable; }
            set
            {
                if (value != _stopEnable)
                {
                    _stopEnable = value;
                    RaisePropertyChanged("StopEnable");
                }
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


        private string _lastUpdate;

        public string LastUpdate
        {
            get { return _lastUpdate; }
            set
            {
                if (value != _lastUpdate)
                {
                    _lastUpdate = value;
                    RaisePropertyChanged("LastUpdate");
                }
            }
        }


        void Start()
        {
            StartEnable = false;
            StopEnable = true;
            _ctoken = new CancellationTokenSource();
            var plaginManager = new PluginManager();
            try
            {
                var taxiService = plaginManager.LoadTaxiService();
                var dbService = plaginManager.LoadDbService();
                var keyData = ReadXml();
                if (CheckSettings(keyData))
                {
                    var bot = new Api(keyData.BotServiceKey);
                    taxiService.Initialize(keyData.TaxiServiceKey);
                    BotStatus = Resources.StatusViewModel_Start_Bot_is_active_message;
                    BotWork(bot, dbService, taxiService);
                }
                else
                {
                    StartEnable = false;
                    _dialogProvider.ShowMessage(Resources.StatusViewModel_No_keys_or_DB_message);
                }
            }

            catch (NullReferenceException)
            {
                _dialogProvider.ShowMessage(Resources.StatusViewModel_Start_DLL_load_error_message);
                StartEnable = true;
                StopEnable = false;
            }
            catch (InvalidOperationException)
            {
                _dialogProvider.ShowMessage(Resources.StatusViewModel_Start_Load_error_message);
            }
            catch (Exception)
            {
                _dialogProvider.ShowMessage(Resources.Start_Unknown_error_message);
            }

        }

        void Stop()
        {
            if (_ctoken != null)
            {
                _ctoken.Cancel();
            }
            StopEnable = false;
            StartEnable = true;
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
                        var updates = await bot.GetUpdates(offset);
                        _ctoken.Token.ThrowIfCancellationRequested();
                        foreach (var update in updates)
                        {
                            if (update.Message.Type == MessageType.TextMessage)
                            {
                                if (dict.ContainsKey(update.Message.Chat.Id))
                                {
                                    switch (dict[update.Message.Chat.Id])
                                    {
                                        case FastestRouteRequest:
                                        {
                                            rb.FastestWayResponse(update, dbService, taxiService);
                                            break;
                                        }

                                        case AllRoutesRequest:
                                        {
                                            rb.AllRoutesRequest(update, dbService, taxiService);
                                            break;
                                        }
                                        
                                        case TaxiRequest:
                                        {
                                            rb.TaxiResponse(update, dbService, taxiService);
                                            break;
                                        }

                                        case SuburbanRequest:
                                        {
                                            rb.GetSuburbanResponse(update, dbService);
                                            break;
                                        }

                                        case BusRequest:
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
                                        case InitialRequest:
                                        {
                                            bot.SendTextMessage(update.Message.Chat.Id, Resources.StatusViewModel_BotWork_Introduce_message).Wait();
                                            bot.SendTextMessage(update.Message.Chat.Id, Resources.StatusViewModel_BotWork_Intro_message);
                                            break;
                                        }
                                        case PlacesRequest:
                                        {
                                            rb.GetPlacesResponse(update, dbService);
                                            break;
                                        }
                                        case FastestRouteRequest:
                                        {
                                            bot.SendTextMessage(update.Message.Chat.Id, Resources.StatusViewModel_BotWork_Get_route_intro);
                                            dict.Add(update.Message.Chat.Id, FastestRouteRequest);
                                            break;
                                        }

                                        case AllRoutesRequest:
                                        {
                                            bot.SendTextMessage(update.Message.Chat.Id, Resources.StatusViewModel_BotWork_Get_All_routes_response_message);
                                            dict.Add(update.Message.Chat.Id, AllRoutesRequest);
                                            break;
                                        }

                                        case TaxiRequest:
                                        {
                                            bot.SendTextMessage(update.Message.Chat.Id, Resources.StatusViewModel_BotWork_Get_route_intro);
                                            dict.Add(update.Message.Chat.Id, TaxiRequest);
                                            break;
                                        }

                                        case SuburbanRequest:
                                        {
                                            bot.SendTextMessage(update.Message.Chat.Id, Resources.StatusViewModel_BotWork_Get_suburban_message);
                                            dict.Add(update.Message.Chat.Id, SuburbanRequest);
                                            break;
                                        }

                                        case BusRequest:
                                        {
                                            bot.SendTextMessage(update.Message.Chat.Id, Resources.StatusViewModel_BotWork_Get_Bus_response_message);
                                            dict.Add(update.Message.Chat.Id, BusRequest);
                                            break;
                                        }
                                        case ShmowzowRequest:
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

        private bool CheckSettings(SettingsData st)
        {
            bool all = true;
            if (!string.IsNullOrEmpty(st.BotServiceKey))
                TGStatus = Resources.StatusViewModel_CheckSettings_OK_status_message;
            else all = false;
            if (!string.IsNullOrEmpty(st.TaxiServiceKey))
                UberStatus = Resources.StatusViewModel_CheckSettings_OK_status_message;
            else all = false;
            if (!string.IsNullOrEmpty(st.ScheduleServiceKey))
                YandexStatus = Resources.StatusViewModel_CheckSettings_OK_status_message;
            else all = false;
            if (!string.IsNullOrEmpty(st.MonitoringServiceKey))
                GoogleStatus = Resources.StatusViewModel_CheckSettings_OK_status_message;
            else all = false;
            if (st.UpdateTime.Ticks > 0)
                LastUpdate = st.UpdateTime.ToString("d");
            return all;
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