using System;
using ZanzibarBot.OlympiadConnected;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace ZanzibarBot
{
    public class Olympiad
    {
        public readonly Results results;

        private DateTime StartOfOlympiadTime;
        private DateTime CurrentTime;
        private DateTime EndOfOlympiadTime;

        private void SetUpTimer()
        {
            StartOfOlympiadTime = DateTime.Now;
            EndOfOlympiadTime = StartOfOlympiadTime.AddHours(2);
            while (CurrentTime < EndOfOlympiadTime)
            {
                CurrentTime = DateTime.Now;
            }
            EndOlympiad();
        }

        public void StartOlympiad()
        {
            SetUpTimer();
        }

        private void EndOlympiad()
        {
            Commands.ListOfCommands.DisableCommandsOnOlympiadEnd();
            //Заглушка
            results.SendFinalResults();
        }
    }
}
