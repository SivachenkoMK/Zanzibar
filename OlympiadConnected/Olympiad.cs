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

namespace ZanzibarBot.OlympiadConnected
{
    public static class Olympiad
    {
        public static bool IsStarted = false;
        public static bool IsEnded = false;

        private static void StartOlympiadForEveryone()
        { 
            foreach (Person person in ListOfPeople.People)
            {
                person.StartOlympiad();
            }
        }


        public static void TryStartingOlympiad()
        {
            if (!IsStarted)
            {
                StartOlympiadForEveryone();
                IsStarted = true;
            }
        }

        public static void TryEndingOlympiad()
        {
            if (!IsEnded)
                EndOlympiad();
        }

        private static void EndOlympiad()
        {
            foreach (Person person in ListOfPeople.People)
            {
                MessageSender.SendMessage(person.ChatId, "Олімпіаду закінчено.");
                Results.workbook.Close();
                Results.SendCurrentResults(person.ChatId);
            }
            foreach (Person person in ListOfPeople.People)
            {
                if (person.Status == "Captain")
                {
                    person.EndOlympiad();
                }
            }
            foreach (Person person in ListOfPeople.People)
            {
                if (person.Status == "Moderator")
                {
                    MessageSender.SendMessage(person.ChatId, "Якщо ще не закінчили перевіряти - закінчуйте.");
                }
            }
            Console.WriteLine("Олімпіаду закінчено");
            IsEnded = true;
        }
    }
}
