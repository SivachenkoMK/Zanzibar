using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace ZanzibarBot.People
{
    interface StatusFeatures
    {
        public Person person { get; set; }
        public void SetTeamName(Message message);

        public void CheckTask(Message message);

        public void GetTeamName();

        public bool IsKing { get; }
    }
}
