using System;
using System.Collections.Generic;
using System.Text;

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
                    string Text = "Ви вже доєднались до олімпіади.";
                    MessageSender.SendMessage(person.ChatId, Text);
                    return;
                }
            }
            People.Add(person);
        }
    }
}
