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
                catch
                {
                    _bot.SendTextMessage(update.Message.Chat.Id, "Возникла ошибка, повторите попытку!");
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
                    var response = update.Message.Text.Split('-');
                    _bot.SendTextMessage(update.Message.Chat.Id,
                        string.Format("{0} : {1}", response[0].Trim(), response[1]).Trim());
                    var route = dbService.GetFastestRoute(response[0].Trim(), response[1].Trim(), update.Message.Date);
                    _bot.SendTextMessage(update.Message.Chat.Id, route.Routes.Capacity.ToString());
                }
                catch (Exception e)
                {
                    _bot.SendTextMessage(update.Message.Chat.Id, e.Message);
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
                    foreach (var bus in busSchedule)
                    {
                        stb.Append(bus.DepartureTime.ToString("t"));
                        stb.Append("\n");
                    }
                    _bot.SendTextMessage(update.Message.Chat.Id,
                        string.Format("Расписание автобусов из {0}:\n{1}", update.Message.Text, stb.ToString()));
                }
                catch (Exception e)
                {
                    _bot.SendTextMessage(update.Message.Chat.Id, e.Message);
                }
            });
        }
    }
}
