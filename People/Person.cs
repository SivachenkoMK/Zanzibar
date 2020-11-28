using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using ZanzibarBot.People;

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
            else if (message.Text == "/start" || message.Text == "/changestatus")
            {
                MessageSender.SendMessage(ChatId, "Привіт. Я бот, котрий допоможе Вам з олімпіадою «Занзібар».");
                PickStatusDisplay();
            }
            else if (priority == Priorities.PickStatus)
            {
                if (message.Text == "Captain")
                {
                    AskTeamNameDisplay();
                }
                else if (message.Text == "Moderator")
                {
                    AskPasswordDisplay();
                }
            }
            else if (priority == Priorities.GetTeamName)
            {
                ProcessTeamNameDisplay(message);
            }
            else if (priority == Priorities.ProcessPassword)
            {
                ProcessModeratorPasswordForCorrectness(message);
            }
        }

        private void PickStatusDisplay()
        {
            ReplyKeyboardMarkup markup = new ReplyKeyboardMarkup(new[]
                {
                    new KeyboardButton("Captain"),
                    new KeyboardButton("Moderator"),
                })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            };
            MessageSender.SendMessage(ChatId, "Оберіть свій статус. Якщо Ви капітан команди - оберіть Captain, якщо перевіряючий - оберіть Moderator", markup);
            priority = Priorities.PickStatus;
        }

        private void AskTeamNameDisplay()
        {
            MessageSender.SendMessage(ChatId, "Введіть назву команди.");
            priority = Priorities.GetTeamName;
        }

        private void AskPasswordDisplay()
        {
            MessageSender.SendMessage(ChatId, "Введіть пароль.");
            priority = Priorities.ProcessPassword;
        }

        private void ProcessTeamNameDisplay(Message message)
        {
            if (message.Text.Length > 16)
            {
                MessageSender.SendMessage(ChatId, "Завелика назва команди. Спробуйте ще раз.");
            }
            else
            {
                CreateNewCaptain(message);

                RemoveThisFromWaitList();
                priority = Priorities.NoPriority;
            }
        }

        private void CreateNewCaptain(Message message)
        {
            string teamName = message.Text;

            Captain captain = new Captain
            {
                ChatId = this.ChatId,
                TeamName = teamName
            };

            ListOfPeople.AddToListOfPeople(captain);

            MessageSender.SendMessage(ChatId, AuthorizationWasSuccessful);
        }

        private void ProcessModeratorPasswordForCorrectness(Message message)
        {
            string supposedPassword = message.Text;

            if (supposedPassword == PeopleData.PasswordForModerator)
            {
                CreateNewModerator();

                RemoveThisFromWaitList();
                priority = Priorities.NoPriority;
            }
            else if (supposedPassword == PeopleData.PasswordForMainModerator)
            {
                CreateNewMainModerator();

                RemoveThisFromWaitList();
                priority = Priorities.NoPriority;
            }
            else
            {
                MessageSender.SendMessage(ChatId, "Неправильний пароль. Спробуйте знову, або скористуйтеся командою /changestatus, щоб змінити статус на капітана команди.");
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


        private enum Priorities
        {
            NoPriority,
            PickStatus,
            GetTeamName,
            ProcessPassword
        }
    }
}
