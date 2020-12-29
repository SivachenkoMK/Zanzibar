using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using ZanzibarBot.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.InputFiles;

namespace ZanzibarBot.People
{
    class Captain : Person
    {
        public override string Status => "Captain";

        public string TeamName;

        public int NumberOfRowInExcelWorksheet
        {
            get
            {
                return OlympiadConnected.TeamsInfo.GetNumberByTeamName(TeamName);
            }
        }
        

        private Priorities priority = Priorities.NoPriority;

        private Task FirstTask = ListOfTasks.GetTask(1);
        private Task SecondTask = ListOfTasks.GetTask(2);

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
                        else if (message.Text == SecondTask.Number.ToString() && message.Text != "-1")
                        {
                            MessageSender.SendMessage(ChatId, $"Введіть відповідь на задачу №{SecondTask.Number}");
                            priority = Priorities.ProcessSecondTask;
                        }
                        else if (message.Text == "/getresults")
                        {
                            OlympiadConnected.Results.workbook.Close();
                            OlympiadConnected.Results.SendCurrentResults(ChatId);
                            ChooseNextTasks();
                        }
                        else 
                        {
                            MessageSender.SendMessage(ChatId, "У Вас нема такої задачі на відправку.");
                            ChooseNextTasks();
                        }
                        break;
                    }
                case (Priorities.ProcessFirstTask):
                    {
                        if (FirstTask.Number == -1)
                        {
                            OlympiadConnected.Olympiad.TryEndingOlympiad();
                        }
                        else if (message.Text == FirstTask.Answer)
                        {
                            OlympiadConnected.Results.WriteCorrectInWorksheet(NumberOfRowInExcelWorksheet, FirstTask.Number);
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
                        if (SecondTask.Number < 20 && SecondTask.Number != -1)
                        {
                            FirstTask = ListOfTasks.GetTask(SecondTask.Number);
                            SecondTask = ListOfTasks.GetTask(SecondTask.Number + 1);
                            MessageSender.SendMessage(ChatId, SecondTask.Clause);
                            ChooseNextTasks();
                        }
                        else if (SecondTask.Number == 20)
                        {
                            FirstTask = ListOfTasks.GetTask(20);
                            SecondTask = new Task()
                            {
                                Number = -1
                            };
                            MessageSender.SendMessage(ChatId, FirstTask.Clause);
                            ChooseNextTasks();
                        }
                        else if (SecondTask.Number == -1)
                        {
                            FirstTask = new Task()
                            {
                                Number = -1
                            };
                            OlympiadConnected.Olympiad.TryEndingOlympiad();
                        }
                        else
                        {
                            OlympiadConnected.Olympiad.TryEndingOlympiad();
                        }
                        break;
                    }
                case (Priorities.ProcessSecondTask):
                    {
                        if (SecondTask.Number == -1)
                        {
                            
                        }
                        else if (message.Text == SecondTask.Answer)
                        {
                            OlympiadConnected.Results.WriteCorrectInWorksheet(NumberOfRowInExcelWorksheet, SecondTask.Number);
                            MessageSender.SendMessage(ChatId, $"Задача №{SecondTask.Number} - правильна відповідь, задачу зараховано!");
                        }
                        else
                        {
                            Attempt attempt = new Attempt()
                            {
                                answer = message.Text,
                                ChatId = this.ChatId,
                                Id = long.Parse(ChatId.ToString() + SecondTask.Number.ToString()),
                                task = SecondTask
                            };
                            ModeratorCaptainAdapter.SendAttemptForProcess(attempt);
                        }
                        if (SecondTask.Number != 20)
                        {
                            int NumeberOfNewTask = SecondTask.Number + 1;
                            SecondTask = ListOfTasks.GetTask(NumeberOfNewTask);
                            MessageSender.SendMessage(ChatId, SecondTask.Clause);
                            ChooseNextTasks();
                        }
                        else
                        {
                            SecondTask = new Task()
                            {
                                Number = -1
                            };
                            MessageSender.SendMessage(ChatId, FirstTask.Clause);
                            ChooseNextTasks();
                        }

                        break;
                    }
                case (Priorities.StartedOlympiad):
                    {
                        break;
                    }
                case (Priorities.EndedOlympiad):
                    {
                        if (message.Text == "/getresults")
                        {
                            OlympiadConnected.Results.workbook.Close();
                            OlympiadConnected.Results.SendCurrentResults(ChatId);
                            MessageSender.SendMessage(ChatId, "Перевіряючі, можливло, ще не закінчили перевірку.");
                        }
                        else
                        {
                            MessageSender.SendMessage(ChatId, "Олімпіаду вже закінчено, але Ви можете перевірити результати.");
                        }
                        break;
                    }
            }
        }

        private void ProcessMessageWithNoPriority(Message message)
        {
            switch (message.Text)
            {
               case ("/getresults"):
                    {
                        OlympiadConnected.Results.workbook.Close();
                        OlympiadConnected.Results.SendCurrentResults(ChatId);
                        ChooseNextTasks();
                        break;
                    }
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

            MessageSender.SendMessage(ChatId,
@"Вітаю! 
Внизу є дві кнопки з номерами задач, котрі ви можете відправити. 
Натискаючи на одну з них, ви маете написати відповідь на обрану задачу.
Команда /getresults - корисна функція для перегляду інформації про стан олімпіади на даний момент.
Успіхів!");

            MessageSender.SendMessage(ChatId, FirstTask.Clause);
            MessageSender.SendMessage(ChatId, SecondTask.Clause);

            ReplyKeyboardMarkup replyKeyboardMarkup = ReplyKeyboardOfTasksToPass();
            MessageSender.SendMessage(ChatId, "Щоб відправити відповідь на одну з задач натисніть кнопку з номером задачі.", replyKeyboardMarkup);
            priority = Priorities.GetTaskForProcess;
        }

        private ReplyKeyboardMarkup ReplyKeyboardOfTasksToPass()
        {
            if (SecondTask.Number != -1 && FirstTask.Number != -1)
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
            else if (SecondTask.Number == -1 && FirstTask.Number != -1)
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
                {
                    new KeyboardButton(FirstTask.Number.ToString()),
                })
                {
                    ResizeKeyboard = true
                };
                priority = Priorities.GetTaskForProcess;

                return replyKeyboardMarkup;
            }
            else
            {
                OlympiadConnected.Olympiad.TryEndingOlympiad();
                return new ReplyKeyboardMarkup();
            }

        }

        public void SetResultForAttempt(Attempt attempt)
        {
            if (attempt.Result)
            {
                OlympiadConnected.Results.WriteCorrectInWorksheet(NumberOfRowInExcelWorksheet, attempt.task.Number);
                MessageSender.SendMessage(ChatId, attempt.task.Number + " - правильно.");
            }
            else
            {
                OlympiadConnected.Results.WriteIncorrectInWorksheet(NumberOfRowInExcelWorksheet, attempt.task.Number);
                MessageSender.SendMessage(ChatId, attempt.task.Number + " - неправильно.");
            }
        }

        public override void EndOlympiad()
        {
            priority = Priorities.EndedOlympiad;
        }

        private enum Priorities
        {
            GetReady,
            ProcessFirstTask,
            ProcessSecondTask,
            NoPriority,
            GetTaskForProcess,
            StartedOlympiad,
            EndedOlympiad
        }
    }
}
