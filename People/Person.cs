using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using ZanzibarBot.People;

namespace ZanzibarBot
{
    public class Person
    {
        public Priorities Priority = Priorities.NoPriority;

        //flags
        public bool IsAuthorized = false;
        public bool IsReady = false;
        public bool IsMainModerator = false;

        public long ChatId { get; set; }

        public string Status { get; set; }

        public string TeamName;

        private StatusFeatures statusFeatures;

        public bool IsStatusSetted()
        {
            return (Status != null);
        }

        public virtual void ProcessMessage(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
            {
                MessageSender.SendMessage(ChatId, "Бот не розпізнає нічого крім тексту, введи коректні дані.");
            }
            else if (Priority != Priorities.NoPriority)
            {
                ProcessMessageWithPriority(message);
            }
            else
            {
                switch (message.Text)
                {
                    case ("/start"):
                        {
                            if (!IsAuthorized)
                            {
                                IsAuthorized = true;
                                Start();
                            }
                            break;
                        }
                    case ("/changestatus"):
                        {
                            PickStatus();
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }

        private void Start()
        {
            string TextInUkrainian = "Привіт. Я бот, котрий допоможе Тобі з олімпіадою «Занзібар».";
            MessageSender.SendMessage(ChatId, TextInUkrainian);
            PickStatus();
        }

        public void PickStatus()
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
            MessageSender.SendMessage(ChatId, "Обери свій статус. Якщо Ти капітан команди - обери Captain, якщо перевіряючий - обери Moderator", markup);
            Priority = Priorities.PickStatus;
        }

        public void ProcessMessageWithPriority(Message message)
        {
            switch (Priority)
            {
                case (Priorities.SetTeamName):
                    {
                        statusFeatures.SetTeamName(message);
                        GetReady();
                        break;
                    }
                case (Priorities.PickStatus):
                    {
                        if (message.Text == "Captain")
                        {
                            Status = "Captain";
                            statusFeatures = new Captain
                            {
                                person = this
                            };
                            Priority = Priorities.NoPriority;
                            statusFeatures.GetTeamName();
                        }
                        else if (message.Text == "Moderator")
                        {
                            AskForPassword();
                        }
                        else
                        {
                            MessageSender.SendMessage(ChatId, "Введено неправильні дані. Спропуй ще раз. Натисни на одну з кнопок яка задовольняє Твоєму статусу.");
                            PickStatus();
                        }
                        break;
                    }
                case (Priorities.ComparePasswords):
                    {
                        if (message.Text == "/changestatus")
                        {
                            PickStatus();
                        }
                        else if (message.Text == PeopleData.PasswordForModerator)
                        {
                            Status = "Moderator";
                            statusFeatures = new Moderator()
                            {
                                person = this,
                                Level = "Common"
                            };
                            MessageSender.SendMessage(ChatId, "Авторізація пройшла успішно.");
                            Priority = Priorities.NoPriority;
                            GetReady();
                        }
                        else if (message.Text == PeopleData.PasswordForMainModerator)
                        {
                            Status = "Moderator";
                            statusFeatures = new Moderator()
                            {
                                person = this,
                                Level = "Main"
                            };
                            MessageSender.SendMessage(ChatId, "Авторізація пройшла успішно.");
                            Priority = Priorities.NoPriority;
                            IsReady = true;
                            Olympiad.TryStartingOlympiadAsync();
                        }
                        else
                        {
                            MessageSender.SendMessage(ChatId, "Некоректний пароль. Спробуй ще раз, або скористуйся командою /changestatus щоб змінити статус на капітана команди");
                        }
                        break;
                    }
                case (Priorities.GetReady):
                    {
                        if (message.Text == "/changestatus")
                        {
                            PickStatus();
                        }
                        else if (message.Text == "Ready")
                        {
                            IsReady = true;
                            Priority = Priorities.WaitingForStart;
                        }
                        else
                        {
                            MessageSender.SendMessage(ChatId, "Приготуйся до олімпіади - натисни кнопку Ready. Якщо Ти неправильно обрав свій статус, можеш змінити його за допомогою команди /changestatus.", new ReplyKeyboardMarkup(new KeyboardButton("Ready")) { OneTimeKeyboard = true, ResizeKeyboard = true });
                        }
                        break;
                    }
                case (Priorities.WaitingForStart):
                    {
                        MessageSender.SendMessage(ChatId, "Ти вже приготувався до олімпіади, зачекай доки інші учасники та перевіряючі підготуються.");
                        break;
                    }
                case (Priorities.WaitingForPermissionToStart):
                    {
                        if (message.Text == "Start" && statusFeatures.IsKing)
                        {
                            Olympiad.ToStartOlimpiad = true;
                            Priority = Priorities.NoPriority;
                        }
                        break;
                    }
            }
        }

        private void AskForPassword()
        {
            MessageSender.SendMessage(ChatId, "Введи пароль.");
            Priority = Priorities.ComparePasswords;
        }

        public void GetReady()
        {
            MessageSender.SendMessage(ChatId, "Приготуйся до олімпіади - натисни кнопку Ready. Якщо Ти неправильно обрав свій статус, можеш змінити його за допомогою команди /changestatus", new ReplyKeyboardMarkup(new KeyboardButton("Ready")) { OneTimeKeyboard = true, ResizeKeyboard = true }); 
            Priority = Priorities.GetReady;
        }

        public void AskForStartingOlympaiad()
        {
            MessageSender.SendMessage(ChatId, "Ви - головний модератор. Перевірте що в аудиторії усі готові та розпочніть олімпіаду, натиснувши на кнопку нижче.", new ReplyKeyboardMarkup(new KeyboardButton("Start")) { OneTimeKeyboard = true, ResizeKeyboard = true });
            Priority = Priorities.WaitingForPermissionToStart;
        }

        public enum Priorities
        {
            NoPriority,
            SetTeamName,
            PickStatus,
            ComparePasswords,
            GetReady,
            WaitingForStart,
            WaitingForPermissionToStart
        }
    }
}
