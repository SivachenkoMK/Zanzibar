using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace ZanzibarBot.People
{
    public class Participant : Person
    {
        public override long ChatId { get; set; }

        public string TeamName { get; set; }

        public Participant(long Id)
        {
            ChatId = Id;
        }
    }
}
