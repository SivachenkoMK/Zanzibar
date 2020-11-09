using System;
using System.Collections.Generic;
using System.Text;

namespace ZanzibarBot.People
{
    class Moderator : Person
    {
        public override long ChatId { get; set; }

        public override string Status { get; set; } = "Moderator";

        public Moderator(long Id)
        {
            ChatId = Id;
        }
    }
}
