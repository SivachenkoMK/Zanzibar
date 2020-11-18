using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Telegram.Bot.Types;

namespace ZanzibarBot.People
{
    public static class ListOfPeople
    {
        public static List<Person> People = new List<Person>();

        public static void AddPersonToList(Person person)
        {
            foreach (Person initializedPerson in People)
            {
                if (person.ChatId == initializedPerson.ChatId)
                {
                    return;
                }
            }
            People.Add(person);
        }

        public static Person GetPerson(long ChatId)
        {
            foreach (Person person in People)
            {
                if (person.ChatId == ChatId)
                    return person;
            }
            throw new NotImplementedException("Such person is not initialized. Create a new peroson.");
        }
    }
}
