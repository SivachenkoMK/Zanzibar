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

        public void Start()
        {
            client = new TelegramBotClient(ApiSettings.Token);
            client.StartReceiving();
            client.OnMessage += MessageHandler;
            string ExitWord = "";
            while (ExitWord != "exit")
            {
                ExitWord = Console.ReadLine();
            }
            client.StopReceiving();
        }

        public void MessageHandler(object Sender, MessageEventArgs messageEventArgs)
        {
            Message message = messageEventArgs.Message;
            List<Command> commands = ListOfCommands.Commands;
            if (message.Type == MessageType.Text)
            {
                foreach (Command command in commands)
                {
                    if (command.Contains(message.Text))
                    {
                        command.Execute(messageEventArgs, client);
                    }
                }    
            }
        }
    }
}
