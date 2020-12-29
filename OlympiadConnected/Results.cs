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

        public static Workbook workbook
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

        public static void WriteCorrectInWorksheet(int teamNumber, int taskNumber)
        {
            Microsoft.Office.Interop.Excel.Range r = worksheet.Cells[teamNumber + 1, taskNumber + 1] as Microsoft.Office.Interop.Excel.Range;
            r.Value2 = 1;
            workbook.Save();
        }

        public static void WriteIncorrectInWorksheet(int teamNumber, int taskNumber)
        {
            Microsoft.Office.Interop.Excel.Range r = worksheet.Cells[teamNumber + 1, taskNumber + 1] as Microsoft.Office.Interop.Excel.Range;
            r.Value2 = 0;
            workbook.Save();
        }

        public static void SendCurrentResults(long chatId)
        {
            MessageSender.SendResults(chatId);
        }
    }
}
