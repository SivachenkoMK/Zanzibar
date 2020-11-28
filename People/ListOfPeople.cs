using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Telegram.Bot.Types;

namespace ZanzibarBot.People
{
    public static class ListOfPeople
    {
        private static List<Person> WaitList = new List<Person>();

        public static List<Person> People = new List<Person>();

        public static bool IsPersonIdentified(long chatId)
        {
            foreach (Person person in People)
            {
                if (person.ChatId == chatId)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsPersonInWaitList(long chatId)
        {
            foreach (Person person in WaitList)
            {
                if (person.ChatId == chatId)
                {
                    return true;
                }
            }
            return false;
        }

        public static void AddToWaitList(Person person)
        {
            WaitList.Add(person);
        }

        public static void AddToListOfPeople(Person person)
        {
            People.Add(person);
        }

        public static void RemovePersonFromWaitList(long Id)
        {
            foreach (Person WaitingPerson in WaitList.ToList())
            {
                if (WaitingPerson.ChatId == Id)
                {
                    WaitList.Remove(WaitingPerson);
                    break;
                }
            }
        }

        public static void RemovePersonFromListOfPeople(long Id)
        {
            foreach (Person InitializedPerson in WaitList.ToList())
            {
                if (InitializedPerson.ChatId == Id)
                {
                    WaitList.Remove(InitializedPerson);
                    break;
                }
            }
        }

        public static Person GetPersonFromWaitList(long ChatId)
        {
            foreach (Person person in WaitList)
            {
                if (person.ChatId == ChatId)
                {
                    return person;
                }
            }
            throw new NotImplementedException("No such person initialized");
        }

        public static Person GetPersonFromList(long ChatId)
        {
            foreach (Person person in People)
            {
                if (person.ChatId == ChatId)
                {
                    return person;
                }
            }
            throw new NotImplementedException("No such person initialized");
        }
    }
}
