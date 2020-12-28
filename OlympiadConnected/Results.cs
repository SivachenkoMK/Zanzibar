using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Telegram.Bot.Types.InputFiles;
using Microsoft.Office.Interop.Excel;
using Telegram.Bot;

namespace ZanzibarBot.OlympiadConnected
{
    public static class Results
    {
        public static TelegramBotClient bot;

        public static Application xlApp = new Application();

        public static string CurrentDirection = @"D:\Programming\C#\TelegramBots\Zanzibar\results\Results.xlsx";

        private static Workbook workbook
        {
            get
            {
                return xlApp.Workbooks.Open(CurrentDirection);
            }
            set
            {
                workbook = value;
            }
        }

        private static Worksheet worksheet
        {
            get
            {
                return workbook.Sheets[1];
            }
            set
            {
                worksheet = value;
            }
        }

        public static async void SendCurrentResults(long chatId)
        {
            MessageSender.SendResults(chatId);

        }
    }
}
