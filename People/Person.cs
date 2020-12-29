using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using ZanzibarBot.People;
using _TeamsInfo = ZanzibarBot.OlympiadConnected.TeamsInfo;

namespace ZanzibarBot.People
{
    public class Person
    {
        private const string AuthorizationWasSuccessful = "Авторізація пройшла успішно.";

        public virtual string Status { get; } = "Person";

        public long ChatId;

        private Priorities priority;

        public virtual void ProcessMessage(Message message)
        {
            if (OlympiadConnected.Olympiad.IsStarted)
            {
                MessageSender.SendMessage(ChatId, "Ви вже не можете доєднатись - олімпіаду розпочато.");
            }
            else if (message.Text == "/start")
            {
                MessageSender.SendMessage(ChatId, "Привіт. Я бот, котрий допоможе Вам з олімпіадою «Занзібар».");
                EnterPasswordDisplay();
            }
            else if (message.Text == "/changestatus")
            {
                EnterPasswordDisplay();
            }
            else if (priority == Priorities.ProcessPassword)
            {
                ProcessPasswordForCorrectness(message);
            }
        }

        private void EnterPasswordDisplay()
        {
            MessageSender.SendMessage(ChatId, "Введіть пароль");
            priority = Priorities.ProcessPassword;
        }

        private void CreateNewCaptain(string supposedPassword)
        {
            string teamName = _TeamsInfo.TeamNames[_TeamsInfo.GetNumberByTeamPassword(supposedPassword) - 1];

            Captain captain = new Captain
            {
                ChatId = this.ChatId,
                TeamName = teamName
            };

            ListOfPeople.AddToListOfPeople(captain);

            MessageSender.SendMessage(ChatId, AuthorizationWasSuccessful);
        }

        private void ProcessPasswordForCorrectness(Message message)
        {
            string supposedPassword = message.Text;

            if (_TeamsInfo.IsOneOfPasswordsForCaptain(supposedPassword))
            {
                CreateNewCaptain(supposedPassword);

                RemoveThisFromWaitList();
                priority = Priorities.NoPriority;
            }
            else
            {

                switch (supposedPassword)
                {
                    case (PeopleData.PasswordForModerator):
                        {
                            CreateNewModerator();

                            RemoveThisFromWaitList();
                            priority = Priorities.NoPriority;
                            break;
                        }
                    case (PeopleData.PasswordForMainModerator):
                        {
                            CreateNewMainModerator();

                            RemoveThisFromWaitList();
                            priority = Priorities.NoPriority;
                            break;
                        }
                    default:
                        {
                            MessageSender.SendMessage(ChatId, "Неправильний пароль. Спробуйте ще раз.");
                            break;
                        }
                }
            }
        }

        private void CreateNewModerator()
        {
            Moderator moderator = new Moderator()
            {
                ChatId = this.ChatId,
                IsMain = false
            };

            
            ListOfPeople.AddToListOfPeople(moderator);

            MessageSender.SendMessage(ChatId, AuthorizationWasSuccessful);
        }

        private void CreateNewMainModerator()
        {
            Moderator moderator = new Moderator()
            {
                ChatId = this.ChatId,
                IsMain = true,
            };

            ListOfPeople.AddToListOfPeople(moderator);
            moderator.GiveAllInformation();
            moderator.SetPriorityForStartOlympiad();
            MessageSender.SendMessage(ChatId, AuthorizationWasSuccessful);
        }

        public virtual void StartOlympiad()
        {
            RemoveThisFromWaitList();
            MessageSender.SendMessage(ChatId, "Ви вже не можете доєднатись - олімпіаду розпочато.");
        }

        private void RemoveThisFromWaitList()
        {
            ListOfPeople.RemovePersonFromWaitList(ChatId);
        }

        public virtual void EndOlympiad()
        {

        }

        private enum Priorities
        {
            NoPriority,
            ProcessPassword
        }
    }
}
