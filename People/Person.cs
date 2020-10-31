﻿using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace ZanzibarBot
{
    public class Person
    {
        public virtual long ChatId { get; set; }

        public void SendMessageToPerson(string Text, TelegramBotClient client)
        {
            client.SendTextMessageAsync(ChatId, Text);
        }
    }
}