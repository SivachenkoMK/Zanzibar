using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using ZanzibarBot.People;

namespace ZanzibarBot.Commands
{
    class Start : Command
    {
        public override string Name => "start";

        public override void Execute(MessageEventArgs message)
        {
            string TextInUkrainian = "Привіт. Я бот, котрий допоможе Тобі з олімпіадою «Занзібар».";
            MessageSender.SendMessage(message.Message.Chat.Id, TextInUkrainian);
            Person person = new Person();
            person.ChatId = message.Message.Chat.Id;
            ListOfPeople.AddPersonToList(person);
        }
    }
}
