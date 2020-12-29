using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace ZanzibarBot
{
    class Bot
    {
        private TelegramBotClient client;

        private MessageHandler handler;

        private void StartMessageSenderAndMessageHandler()
        {
            handler.Start(client);
            MessageSender.Start(client);
        }

        public void Start()
        {
            client = new TelegramBotClient(ApiSettings.Token);
            OlympiadConnected.Results.bot = client;
            handler = new MessageHandler(client);
            StartMessageSenderAndMessageHandler();
            Tasks.ListOfTasks.Start();
            OlympiadConnected.TeamsInfo.InitializeTeams();
            string ExitWord = "";
            while (ExitWord != "exit")
            {
                ExitWord = Console.ReadLine();
            }
            OlympiadConnected.Results.xlApp.Quit();
            client.StopReceiving();
        }
    }
}
