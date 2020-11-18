using System;
using ZanzibarBot.OlympiadConnected;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using ZanzibarBot.People;

namespace ZanzibarBot
{
    public static class Olympiad
    {
        public static readonly Results results;

        private static DateTime StartOfOlympiadTime;
        private static DateTime CurrentTime;
        private static DateTime EndOfOlympiadTime;

        public static bool ToStartOlimpiad = false;
        
        private static void SetUpTimer()
        {
            StartOfOlympiadTime = DateTime.Now;
            EndOfOlympiadTime = StartOfOlympiadTime.AddHours(2);
            while (CurrentTime < EndOfOlympiadTime)
            {
                CurrentTime = DateTime.Now;
            }
            EndOlympiad();
        }

        public static async void SetUpTimerAsync()
        {
            await Task.Run(() => SetUpTimer());
        }

        public static void TryStartingOlympiad()
        {
            int AmountOfTeams = 0;
            foreach (Person person in ListOfPeople.People)
            {
                if (person.Status == "Captain" && person.IsReady)
                    AmountOfTeams++;
            }
            foreach (Person person in ListOfPeople.People)
            {
                if (person.Status == "Moderator" && person.IsMainModerator && AmountOfTeams == PeopleData.NeededAmountOfTeams)
                {
                    person.AskForStartingOlympaiad();
                    break;
                }
            }
            while (true)
            {
                if (ToStartOlimpiad)
                {
                    StartOlympiad();
                }
            }
        }

        public static async void TryStartingOlympiadAsync()
        {
            await Task.Run(() => TryStartingOlympiad());
        }

        public static void StartOlympiad()
        {
            SetUpTimerAsync();
        }

        private static void EndOlympiad()
        {
            //Заглушка
            results.SendFinalResults();
        }
    }
}
