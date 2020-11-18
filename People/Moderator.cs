using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace ZanzibarBot.People
{
    class Moderator : StatusFeatures
    {
        public Person person { get; set; }
        
        public bool IsKing
        { 
            get
            {
                return (this.Level != "Common");
            }
        }

        public string Level = "Common";

        public void CheckTask(Message message)
        {
            // Реализовать тут проверку задачи проверяющим.
        }

        public void SetTeamName(Message message)
        {
            MessageSender.SendMessage(person.ChatId, "Ця команда призначена капітанам команд.");
        }

        public void GetTeamName()
        {

        }
    }
}
