using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace ZanzibarBot.People
{
    class Captain : StatusFeatures
    {
        public Person person { get; set; }
        public bool IsKing { get; } = false;

        public void CheckTask(Message message)
        {
            
        }

        public void SetTeamName(Message message)
        {
            if (message.Text.Length > 16)
            {
                MessageSender.SendMessage(person.ChatId, "Завелика назва команди, спробуйте ще раз");
                GetTeamName();
            }
            else
            {
                person.TeamName = message.Text;
                MessageSender.SendMessage(person.ChatId, $"Назва команди успішно встановлена - {person.TeamName}");
                person.Priority = Person.Priorities.NoPriority;
            }
        }

        public void GetTeamName()
        {
            MessageSender.SendMessage(person.ChatId, "Введіть назву команди.");
            person.Priority = Person.Priorities.SetTeamName;
        }
    }
}
