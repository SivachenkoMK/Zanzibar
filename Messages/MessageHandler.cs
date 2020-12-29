using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using ZanzibarBot.People;

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
            if (message.Type != MessageType.Text)
            {
                MessageSender.SendMessage(message.Chat.Id, "Бот не розпізнає нічого крім тектсу. Введіть коректні дані.");
            }
            else if (message.Text == "test")
            {
                MessageSender.SendResults(message.Chat.Id);
            }
            else if (!ListOfPeople.IsPersonIdentified(message.Chat.Id) && !ListOfPeople.IsPersonInWaitList(message.Chat.Id))
            {
                Person person = new Person
                {
                    ChatId = message.Chat.Id
                };
                ListOfPeople.AddToWaitList(person);
                person.ProcessMessage(message);
            }
            else if (ListOfPeople.IsPersonInWaitList(message.Chat.Id))
            {
                Person person = ListOfPeople.GetPersonFromWaitList(message.Chat.Id);
                person.ProcessMessage(message);
            }
            else if (ListOfPeople.IsPersonIdentified(message.Chat.Id))
            {
                Person person = ListOfPeople.GetPersonFromList(message.Chat.Id);
                person.ProcessMessage(message);
            }
        }
    }
}
