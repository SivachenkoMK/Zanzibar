using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ZanzibarBot.People
{
    public class Captain : Person
    {
        private Priorities Priority = Priorities.NoPriority;

        public override long ChatId { get; set; }

        public override string Status { get; set; } = "Captain";

        public string TeamName { get; set; }

        public Captain(long Id)
        {
            ChatId = Id;
        }

        //Переписать все это говно, оставить только идею с processmessage.

        public void DisplaySetCommandName()
        {
            MessageSender.SendMessage(ChatId, "Введіть назву команди");
            Priority = Priorities.SetTeamName;
        }

        private void SetTeamName(Message message)
        {
            if (message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
                MessageSender.SendMessage(ChatId, "Бот не може розпізнати ваше повідомлення.");
            }
            else if (message.Text.Length > 12)
            {
                MessageSender.SendMessage(ChatId, "Завелика назва команди. Спробуйте ще раз.");
            }
            else
            {
                TeamName = message.Text;
                Priority = Priorities.NoPriority;
            }
        }

        public void ProcessMessage(Message message)
        { 
            if (Priority == Priorities.SetTeamName)
            {
                //SetCommandName(message);
            }
        }
    }

    enum Priorities
    {
        NoPriority,
        SetTeamName
    }
}
