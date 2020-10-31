using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ZanzibarBot
{
    public static class MessageSender
    {
        private static TelegramBotClient client;

        public static void SetClient(TelegramBotClient someClient)
        {
            client = someClient;
        }

        public static void SendMessage(long ChatId, Message message)
        {
            client?.SendTextMessageAsync(ChatId, message.Text);
        }

        public static void SendMessage(long ChatId, string text)
        {
            client?.SendTextMessageAsync(ChatId, text);
        }
    }
}
