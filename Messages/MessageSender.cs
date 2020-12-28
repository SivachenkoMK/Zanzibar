using System;
using System.Collections.Generic;
using System.IO;
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

        public async static void SendResults(long chatId)
        {
            using (FileStream fs = System.IO.File.OpenRead(OlympiadConnected.Results.CurrentDirection))
            {
                NotifyingInputOnlineFile onlineFile = new NotifyingInputOnlineFile(fs, fs.Length, "Results.xlsx");
                onlineFile.OnProgressUpdated += (s, progress)
                    => Console.WriteLine($"Uploaded {progress.Uploaded} out of {progress.TotalSize} bytes. Progress: {progress.ProgressPercentage} %");
                await client.SendDocumentAsync(chatId, onlineFile);
            }
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
