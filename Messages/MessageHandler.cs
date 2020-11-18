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
            bool SuchPersonIsInitialized = false; 
            foreach (Person somePerson in ListOfPeople.People)
            {
                if (somePerson.ChatId == message.Chat.Id)
                {
                    SuchPersonIsInitialized = true;
                    break;
                }
            }
            if (SuchPersonIsInitialized == false)
            {
                Person newPerson = new Person();
                newPerson.ChatId = message.Chat.Id;
                ListOfPeople.AddPersonToList(newPerson);
            }
            Person person = ListOfPeople.GetPerson(message.Chat.Id);
            person.ProcessMessage(message);
        }
    }
}
