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
                    string Text = "Ви вже доєднались до олімпіади.";
                    MessageSender.SendMessage(person.ChatId, Text);
                    return;
                }
            }
            People.Add(person);
        }

        //С этого момента код - большой кусок говна который надо начистую переписать потому-что портит все мыслимые и немыслимые законы программирования

        public static void TrySettingStatusForNewPerson(Message message)
        {
            if (message.Text == "Captain" || message.Text == "Moderator")
            {
                CreateNewPersonWithStatus(message);
            }
        }

        private static void CreateNewPersonWithStatus(Message message)
        {
            // Проверка существующего статуса, если еще нет - установить, если уже есть убедится что человек хочет его поменять
            Person person = People[(int)message.Chat.Id];
            if (person.IsStatusSetted())
            {
                MessageSender.SendMessage(person.ChatId, $"Ви вже обрали свій статус - {person?.Status}. Якщо ви хочете змінити статус, скористуйтеся командою /changestatus. Внесені дані не зберігуться.");
            }
            else if (message.Text == "Captain")
            {
                foreach (Person unneadedPerson in People)
                {
                    if (person.ChatId == unneadedPerson.ChatId)
                        People.Remove(unneadedPerson);
                }
                person = new Captain(message.Chat.Id);
                People.Add(person);
                // Узнать название команды.
                //person.SetTeamName();
            }
            else if (message.Text == "Moderator")
            {

            }
        }
    }
}
