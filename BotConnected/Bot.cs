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
            handler = new MessageHandler(client);
            StartMessageSenderAndMessageHandler();
            Tasks.ListOfTasks.Start();
            string ExitWord = "";
            while (ExitWord != "exit")
            {
                ExitWord = Console.ReadLine();
            }
            client.StopReceiving();
        }
    }
}
