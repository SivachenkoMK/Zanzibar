using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;
using ZanzibarBot.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
namespace ZanzibarBot.People
{
    class Moderator : Person
    {
        public bool IsMain = false;

        private bool IsChecking = false;

        private List<Attempt> AttemptsToProcess = new List<Attempt>();

        public override string Status => "Moderator";

        private Priorities prioriy = Priorities.NoPriority;

        private Attempt attemptOnCheck = new Attempt();

        public void GiveAllInformation()
        {
            MessageSender.SendMessage(ChatId, "Ви - головний перевіряючий. Щоб розпочати олімпіаду, введіть в чат слово 'Розпочати'. Перед цим перевірте, чи всі учасники підготувалися до олімпіади.");
        }

        public void SetPriorityForStartOlympiad()
        {
            prioriy = Priorities.StartOlympiad;
        }

        public override void ProcessMessage(Message message)
        {
            switch (prioriy)
            {
                case (Priorities.NoPriority):
                    {
                        ProcessMessageWithoutPriority(message);
                        break;
                    }
                case (Priorities.StartOlympiad):
                    {
                        if (message.Text.ToLower().Contains("розпочати"))
                        {
                            if (IsMain)
                            {
                                OlympiadConnected.Olympiad.TryStartingOlympiad();
                                OlympiadConnected.Timer timer = new OlympiadConnected.Timer();
                            }
                            else
                            {
                                MessageSender.SendMessage(ChatId, "Ви не можете розпочати олімпіаду.");
                            }
                        }
                        break;
                    }
                case (Priorities.WaitForStart):
                    {
                        MessageSender.SendMessage(ChatId, "Зачекайте початку олімпіади.");
                        break;
                    }
                case (Priorities.CheckingTask):
                    {
                        if (message.Text == "Правильно")
                        {
                            prioriy = Priorities.StartedOlympiad;
                            ModeratorCaptainAdapter.SetPersonalResultOfAttempt(attemptOnCheck, true);
                            TryCheckingNextTask();
                        }
                        else if (message.Text == "Неправильно")
                        {
                            prioriy = Priorities.StartedOlympiad;
                            ModeratorCaptainAdapter.SetPersonalResultOfAttempt(attemptOnCheck, false);
                            TryCheckingNextTask();
                        }
                        else
                        {
                            MessageSender.SendMessage(ChatId, "Будь ласка, відправляйте коректні дані.");
                        }
                        break;
                    }
                case (Priorities.StartedOlympiad):
                    {

                        break;
                    }
            }
        }

        private void TryCheckingNextTask()
        {
            if (AttemptsToProcess.Count != 0)
            {
                Attempt attemptToCheck = AttemptsToProcess[0];
                CheckTask(attemptToCheck);
                AttemptsToProcess.RemoveAt(0);
            }
            else
            {
                prioriy = Priorities.StartedOlympiad;
                IsChecking = false;
            }
        }

        public void TryCheckingTask(Attempt attempt)
        {
            if (!IsChecking)
                CheckTask(attempt);
            else
            {
                AttemptsToProcess.Add(attempt);
            }
        }

        public void CheckTask(Attempt attempt)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton("Правильно"),
                new KeyboardButton("Неправильно")
            })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            };

            MessageSender.SendMessage(ChatId, attempt.task.Clause + "\nПравильна відповідь: " + attempt.task.Answer + "\nВідповідь учасника: " + attempt.answer, replyKeyboardMarkup);
            prioriy = Priorities.CheckingTask;
            attemptOnCheck = attempt;
            IsChecking = true;
        }

        private void ProcessMessageWithoutPriority(Message message)
        {
            switch (message.Text)
            {
                case ("/getresults"):
                    {
                        /* Получить резы */
                        break;
                    }
                default:
                    {
                        MessageSender.SendMessage(ChatId, "Зачекайте.");
                        break;
                    }
            }
        }

        public override void StartOlympiad()
        {
            prioriy = Priorities.StartedOlympiad;
        }

        private enum Priorities
        {
            StartOlympiad,
            WaitForStart,
            NoPriority,
            StartedOlympiad,
            CheckingTask
        }
    }
}
