using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using ZanzibarBot.Commands;

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
            string ExitWord = "";
            while (ExitWord != "exit")
            {
                ExitWord = Console.ReadLine();
            }
            client.StopReceiving();
        }
    }
}
