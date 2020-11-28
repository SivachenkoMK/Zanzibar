using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using ZanzibarBot.Tasks;

namespace ZanzibarBot.People
{
    class Captain : Person
    {
        public override string Status => "Captain";

        public string TeamName;

        private Priorities priority = Priorities.NoPriority;

        private Task FirstTask = ListOfTasks.GetTask(1);
        private Task SecondTask = ListOfTasks.GetTask(2);

        private bool[] CorrectTasks = new bool[20];

        public override void ProcessMessage(Message message)
        {
            switch (priority)
            {
                case (Priorities.NoPriority):
                    {
                        ProcessMessageWithNoPriority(message);
                        break;
                    }
                case (Priorities.GetTaskForProcess):
                    {
                        if (message.Text == FirstTask.Number.ToString())
                        {
                            MessageSender.SendMessage(ChatId, $"Введіть відповідь на задачу №{FirstTask.Number}");
                            priority = Priorities.ProcessFirstTask;
                        }
                        else if (message.Text == SecondTask.Number.ToString())
                        {
                            MessageSender.SendMessage(ChatId, $"Введіть відповідь на задачу №{SecondTask.Number}");
                            priority = Priorities.ProcessSecondTask;
                        }
                        else
                        {
                            MessageSender.SendMessage(ChatId, "У Вас нема такої задачі на відправку.");
                        }
                        break;
                    }
                case (Priorities.ProcessFirstTask):
                    {
                        if (message.Text == FirstTask.Answer)
                        {
                            CorrectTasks[FirstTask.Number - 1] = true;
                            MessageSender.SendMessage(ChatId, $"Задача №{FirstTask.Number} - правильна відповідь!");
                        }
                        else
                        {
                            Attempt attempt = new Attempt()
                            {
                                answer = message.Text,
                                ChatId = this.ChatId,
                                Id = long.Parse(ChatId.ToString() + FirstTask.Number.ToString()),
                                task = FirstTask
                            };
                            ModeratorCaptainAdapter.SendAttemptForProcess(attempt);
                        }
                        FirstTask = ListOfTasks.GetTask(SecondTask.Number);
                        SecondTask = ListOfTasks.GetTask(FirstTask.Number + 1);
                        MessageSender.SendMessage(ChatId, SecondTask.Clause);
                        ChooseNextTasks();
                        
                        break;
                    }
                case (Priorities.ProcessSecondTask):
                    {
                        if (message.Text == SecondTask.Answer)
                        {
                            CorrectTasks[SecondTask.Number - 1] = true;
                            MessageSender.SendMessage(ChatId, $"Задача №{SecondTask.Number} - правильна відповідь, задачу зараховано!");
                        }
                        else
                        {
                            Attempt attempt = new Attempt()
                            {
                                answer = message.Text,
                                ChatId = this.ChatId,
                                Id = long.Parse(ChatId.ToString() + SecondTask.Number.ToString()),
                                task = FirstTask
                            };
                            ModeratorCaptainAdapter.SendAttemptForProcess(attempt);
                        }
                        int NumeberOfNewTask = SecondTask.Number + 1;
                        SecondTask = ListOfTasks.GetTask(NumeberOfNewTask);
                        MessageSender.SendMessage(ChatId, SecondTask.Clause);
                        ChooseNextTasks();

                        break;
                    }
                case (Priorities.StartedOlympiad):
                    {
                        break;
                    }
            }
        }

        private void ProcessMessageWithNoPriority(Message message)
        {
            switch (message.Text)
            {
                case ("/passtask"):
                    {
                        ChooseNextTasks();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private void ChooseNextTasks()
        {
            ReplyKeyboardMarkup markup = ReplyKeyboardOfTasksToPass();
            MessageSender.SendMessage(ChatId, "Оберіть задачу, котру хочете відправити.", markup);
            priority = Priorities.GetTaskForProcess;
        }

        public override void StartOlympiad()
        {
            MessageSender.SendMessage(ChatId, "Олімпіаду розпочато!");

            MessageSender.SendMessage(ChatId, FirstTask.Clause);
            MessageSender.SendMessage(ChatId, SecondTask.Clause);

            ReplyKeyboardMarkup replyKeyboardMarkup = ReplyKeyboardOfTasksToPass();
            MessageSender.SendMessage(ChatId, "Щоб відправити відповідь на одну з задач натисніть кнопку з номером задачі.", replyKeyboardMarkup);
            priority = Priorities.GetTaskForProcess;
        }

        private ReplyKeyboardMarkup ReplyKeyboardOfTasksToPass()
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton(FirstTask.Number.ToString()),
                new KeyboardButton(SecondTask.Number.ToString())
            })
            {
                ResizeKeyboard = true
            };
            priority = Priorities.GetTaskForProcess;

            return replyKeyboardMarkup;
        }

        public void SetResultForAttempt(Attempt attempt)
        {
            CorrectTasks[attempt.task.Number] = attempt.Result;
            if (attempt.Result == true)
            {
                MessageSender.SendMessage(ChatId, attempt.task.Number + " - правильно.");
            }
            else
            {
                MessageSender.SendMessage(ChatId, attempt.task.Number + " - неправильно.");
            }
        }

        private enum Priorities
        {
            GetReady,
            ProcessFirstTask,
            ProcessSecondTask,
            NoPriority,
            GetTaskForProcess,
            StartedOlympiad
        }
    }
}
