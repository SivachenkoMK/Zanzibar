using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace ZanzibarBot
{
    public static class MessageSender
    {
        private static TelegramBotClient client;

        public static void Start(TelegramBotClient someClient)
        {
            client = someClient;
        }

        public static void SendMessage(long chatId, InputOnlineFile document)
        {
            client?.SendDocumentAsync(chatId, document);
        }

        public static void SendMessage(long chatId, string text)
        {
            client?.SendTextMessageAsync(chatId, text);
        }
        
        public static void SendMessage(long chatId, string text, ReplyKeyboardMarkup replyKeyboardMarkup)
        {
            client?.SendTextMessageAsync(chatId, text, replyMarkup: replyKeyboardMarkup);
        }
    }
}
