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
        public string TeamName;

        private Priorities priority = Priorities.NoPriority;

        private Task FirstTask = ListOfTasks.GetTask(1);
        private Task SecondTask = ListOfTasks.GetTask(2);

        private List<bool> CorrectTasks = new List<bool>();

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
                        if (message.Text == $"Задача №{FirstTask.Number}")
                        {
                            MessageSender.SendMessage(ChatId, $"Введіть відповідь на задачу №{FirstTask.Number}");
                            priority = Priorities.ProcessFirstTask;
                        }
                        else if (message.Text == $"Задача №{SecondTask.Number}")
                        {
                            MessageSender.SendMessage(ChatId, $"Введіть відповідь на задачу №{SecondTask.Number}");
                            priority = Priorities.ProcessSecondTask;
                        }
                        break;
                    }
                case (Priorities.ProcessFirstTask):
                    {
                        if (message.Text == FirstTask.Answer)
                        {
                            CorrectTasks[FirstTask.Number] = true;
                            MessageSender.SendMessage(ChatId, $"Задача №{FirstTask.Number} - правильна відповідь, задачу зараховано!");
                            FirstTask = SecondTask;
                            SecondTask = ListOfTasks.Tasks[FirstTask.Number + 1];
                            MessageSender.SendMessage(ChatId, SecondTask.Clause);
                        }
                        else
                        {
                            /*Moderator - check this answer*/
                        }
                        break;
                    }
                case (Priorities.ProcessSecondTask):
                    {
                        if (message.Text == SecondTask.Answer)
                        {
                            CorrectTasks[SecondTask.Number] = true;
                            MessageSender.SendMessage(ChatId, $"Задача №{SecondTask.Number} - правильна відповідь, задачу зараховано!");
                            int NumeberOfNewTask = SecondTask.Number++;
                            SecondTask = ListOfTasks.Tasks[NumeberOfNewTask];
                            MessageSender.SendMessage(ChatId, SecondTask.Clause);
                        }
                        else
                        {
                            /*Moderator - check this answer*/
                        }
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
                        ReplyKeyboardMarkup markup = new ReplyKeyboardMarkup(new[]
                        {
                            new KeyboardButton($"Задача №{FirstTask.Number}"),
                            new KeyboardButton($"Задача №{SecondTask.Number}"),
                        })
                        {
                            ResizeKeyboard = true,
                            OneTimeKeyboard = true
                        };
                        MessageSender.SendMessage(ChatId, "Оберіть задачу, котру хочете відправити.", markup);
                        priority = Priorities.GetTaskForProcess;
                        break;
                    }
                case (""):
                    {
                        break;
                    }
            }
        }

        private enum Priorities
        {
            GetReady,
            ProcessFirstTask,
            ProcessSecondTask,
            NoPriority,
            GetTaskForProcess
        }
    }
}
