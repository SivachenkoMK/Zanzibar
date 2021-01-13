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
        public StatusOfPerson status = StatusOfPerson.Waiting;

        public long ChatId;

        private Priorities priority;

        public virtual void ProcessMessage(Message message)
        {
            if (priority == Priorities.NoActionAvailable)
            {
                
            }
            else if (OlympiadConnected.Olympiad.IsStarted)
            {
                Front.PersonDisplay.OlympiadIsAlreadyInProgress(ChatId);
            }
            else if (message.Text == "/start")
            {
                Front.PersonDisplay.Hello(ChatId);
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
            Front.PersonDisplay.EnterPassword(ChatId);
            priority = Priorities.ProcessPassword;
        }

        private void CreateNewCaptain(string supposedPassword)
        {
            string teamName = _TeamsInfo.TeamNames[_TeamsInfo.GetNumberByTeamPassword(supposedPassword) - 1];

            Captain captain = new Captain
            {
                ChatId = this.ChatId,
                TeamName = teamName,
                status = StatusOfPerson.Captain
            };

            ListOfPeople.AddToListOfPeople(captain);

            captain.Start();
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
                            Front.PersonDisplay.IncorrectPassword(ChatId);
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
                status = StatusOfPerson.Moderator
            };

            
            ListOfPeople.AddToListOfPeople(moderator);

            moderator.Start();
        }

        private void CreateNewMainModerator()
        {
            MainModerator moderator = new MainModerator()
            {
                ChatId = this.ChatId,
                status = StatusOfPerson.MainModerator
            };
            ListOfPeople.AddToListOfPeople(moderator);

            moderator.Start();
        }

        public virtual void StartOlympiad()
        {
            RemoveThisFromWaitList();
            Front.PersonDisplay.OlympiadIsAlreadyInProgress(ChatId);
        }

        private void RemoveThisFromWaitList()
        {
            ListOfPeople.RemovePersonFromWaitList(ChatId);
        }

        public virtual void EndOlympiad()
        {

        }

        public virtual void SetNoActionAvailable()
        {
            priority = Priorities.NoActionAvailable;
        }

        private enum Priorities
        {
            NoPriority,
            ProcessPassword,
            NoActionAvailable
        }
    }

    public enum StatusOfPerson
    { 
        Waiting,
        Captain,
        Moderator,
        MainModerator
    }
}
