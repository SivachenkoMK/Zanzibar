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
    public class MessageHandler
    {
        TelegramBotClient client;

        public MessageHandler(TelegramBotClient telegramBotClient)
        {
            client = telegramBotClient;
        }

        public void Start(TelegramBotClient telegramBotClient)
        {
            if (client == null)
                client = telegramBotClient;
            client.StartReceiving();
            client.OnMessage += MessageHandle;
        }

        private void MessageHandle(object Sender, MessageEventArgs messageEventArgs)
        {
            Message message = messageEventArgs.Message;
            List<Command> commands = ListOfCommands.Commands;
            if (message.Type == MessageType.Text)
            {
                foreach (Command command in commands)
                {
                    if (command.Contains(message.Text))
                    {
                        command.Execute(messageEventArgs);
                    }
                }
                People.ListOfPeople.TrySettingStatusForNewPerson(message);
            }
        }
    }
}
