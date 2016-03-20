using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HSE_transport_manager.Properties;

namespace HSE_transport_manager.ViewModel
{
    public class StatusViewModel : ViewModelBase
    {
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

        private ICommand _stopCommand;

        public ICommand StopCommand
        {
            get
            {
                if (_stopCommand == null)
                {
                    _stopCommand = new RelayCommand(
                    ViewLogFile);
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



        void Start()
        {
            //TG Service 
            BotStatus = Resources.StatusViewModel_Start_Bot_is_active_message;
        }

        void ViewLogFile()
        {

        }

    }
}
