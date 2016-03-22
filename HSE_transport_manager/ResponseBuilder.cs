using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSE_transport_manager.Common.Interfaces;
using HSE_transport_manager.Properties;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HSE_transport_manager
{
    public class ResponseBuilder
    {
        private readonly Api _bot;
        private const int DubkiOdi = 30;
        private const int DubkiSlav = 60;

        public ResponseBuilder(Api bot)
        {
            _bot = bot;
        }

        public void TaxiResponse(Update update, IDatabaseService dbService, ITaxiService taxiService)
        {
            Task.Run(async () =>
            {
                try
                {
                    var response = update.Message.Text.Split(Resources.ResponseBuilder_TaxiResponse_Separator);
                    await _bot.SendChatAction(update.Message.Chat.Id, ChatAction.Typing);
                    var fromString = response[0].Trim();
                    var toString = response[1].Trim();
                    var c1 = dbService.GetCoordinates(fromString);
                    var c2 = dbService.GetCoordinates(toString);
                    var response2 = await taxiService.GetRouteAsync(c1, c2);
                    _bot.SendTextMessage(update.Message.Chat.Id,
                    string.Format(
                        Resources.StatusViewModel_BotWork_Uber_response_message,
                        fromString, toString, response2.Duration.Minute, response2.Price));
                }
                catch (IndexOutOfRangeException)
                {
                    _bot.SendTextMessage(update.Message.Chat.Id, Resources.ResponseBuilder_Input_separator_error_message);
                }
                catch (ArgumentException)
                {
                    _bot.SendTextMessage(update.Message.Chat.Id, Resources.ResponseBuilder_Input_data_value_error);
                }
                catch (Exception)
                {
                    _bot.SendTextMessage(update.Message.Chat.Id, Resources.ResponseBuilder_Internal_error_message);
                }
            });
        }

        public void FastestWayResponse(Update update, IDatabaseService dbService)
        {
            Task.Run(async () =>
            {
                try
                {
                    await _bot.SendChatAction(update.Message.Chat.Id, ChatAction.Typing);
                    var response = update.Message.Text.Split(Resources.ResponseBuilder_TaxiResponse_Separator);
                    var route = dbService.GetFastestRoute(response[0].Trim(), response[1].Trim(), update.Message.Date);
                    _bot.SendTextMessage(update.Message.Chat.Id, route.Routes.Capacity.ToString());
                }
                catch (IndexOutOfRangeException)
                {
                    _bot.SendTextMessage(update.Message.Chat.Id, Resources.ResponseBuilder_Input_separator_error_message);
                }
                catch (ArgumentNullException)
                {
                    _bot.SendTextMessage(update.Message.Chat.Id,
                        Resources.ResponseBuilder_No_public_transport_error_message);
                }
                catch (ArgumentException)
                {
                    _bot.SendTextMessage(update.Message.Chat.Id, Resources.ResponseBuilder_Input_data_value_error);
                }
                catch (Exception)
                {
                    _bot.SendTextMessage(update.Message.Chat.Id, Resources.ResponseBuilder_Internal_error_message);
                }
            });
        }

        public void GetBusResponse(Update update, IDatabaseService dbService)
        {

            Task.Run(async () =>
            {
                try
                {
                    await _bot.SendChatAction(update.Message.Chat.Id, ChatAction.Typing);
                    var busSchedule = dbService.GetDubkiSchedule(update.Message.Text);
                    var stb = new StringBuilder();
                    var tripTime = update.Message.Text.Equals("Славянский Бульвар") ? DubkiSlav : DubkiOdi;
                    foreach (var bus in busSchedule)
                    {
                        stb.Append(string.Format(Resources.ResponseBuilder_GetBusResponse_Bus_schedule_line_pattern,
                            bus.From.Substring(0, 3), bus.To.Substring(0, 3), bus.DepartureTime.ToString("t"),
                            bus.DepartureTime.AddMinutes(tripTime).ToString("t")));
                        stb.Append("\n");
                    }
                    _bot.SendTextMessage(update.Message.Chat.Id,
                        string.Format(Resources.ResponseBuilder_GetBusResponse_Bus_response_header_message,
                            update.Message.Text, stb.ToString()));
                }
                catch (ArgumentException)
                {
                    _bot.SendTextMessage(update.Message.Chat.Id, Resources.ResponseBuilder_Input_data_value_error);
                }
                catch (Exception)
                {
                    _bot.SendTextMessage(update.Message.Chat.Id, Resources.ResponseBuilder_Internal_error_message);
                }
            });
        }

        public void GetPlacesResponse(Update update, IDatabaseService dbService)
        {
            Task.Run(async () =>
            {
                try
                {
                    await _bot.SendChatAction(update.Message.Chat.Id, ChatAction.Typing);
                    var stb = new StringBuilder();
                    var places = dbService.GetAllBuildings();
                    foreach (var place in places)
                    {
                        stb.Append(place);
                        stb.Append("\n ");
                    }
                    _bot.SendTextMessage(update.Message.Chat.Id,
                        string.Format(Resources.StatusViewModel_BotWork_Places_response_message, stb.ToString()));
                }
                catch (Exception)
                {
                    _bot.SendTextMessage(update.Message.Chat.Id, Resources.ResponseBuilder_Internal_error_message);
                }
            });
        }
    }
}
