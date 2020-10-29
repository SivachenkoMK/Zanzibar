using System;
using System.Collections.Generic;
using System.Text;

namespace ZanzibarBot.People
{
    class Administrator : Person
    {
        public override long ChatId { get; set; }

        public Administrator(long Id)
        {
            ChatId = Id;
        }
    }
}
