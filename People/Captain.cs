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
        public string TeamName;

        public void Start()
        {
            Front.CaptainDisplay.DisplayReady(ChatId);
        }

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
                case (Priorities.NoActionAvailable):
                    {
                        break;
                    }
                case (Priorities.NoPriority):
                    {
                        ProcessMessageWithNoPriority(message);
                        break;
                    }
                case (Priorities.GetTaskForProcess):
                    {
                        if (message.Text == FirstTask.Number.ToString())
                        {
                            Front.CaptainDisplay.DisplayRequestForEnteringAnswer(ChatId, FirstTask.Number);
                            priority = Priorities.ProcessFirstTask;
                        }
                        else if (message.Text == SecondTask.Number.ToString() && message.Text != "-1")
                        { 
                            Front.CaptainDisplay.DisplayRequestForEnteringAnswer(ChatId, SecondTask.Number);
                            priority = Priorities.ProcessSecondTask;
                        }
                        else if (message.Text == "/getresults")
                        {
                            OlympiadConnected.Results.SendCurrentResults(ChatId);
                            ChooseTaskToAnswer();
                        }
                        else 
                        {
                            Front.CaptainDisplay.NoSuchTaskToChoose(ChatId);
                            ChooseTaskToAnswer();
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
                            Front.CaptainDisplay.InformAboutTaskCorrectness(ChatId, FirstTask.Number);
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
                            AttemptProcesser.SendAttemptForProcess(attempt);
                        }
                        if (SecondTask.Number < 20 && SecondTask.Number != -1)
                        {
                            FirstTask = ListOfTasks.GetTask(SecondTask.Number);
                            SecondTask = ListOfTasks.GetTask(SecondTask.Number + 1);
                            Front.CaptainDisplay.SendClauseOfTask(ChatId, SecondTask);
                            ChooseTaskToAnswer();
                        }
                        else if (SecondTask.Number == 20)
                        {
                            FirstTask = ListOfTasks.GetTask(20);
                            SecondTask = new Task()
                            {
                                Number = -1
                            };
                            Front.CaptainDisplay.SendClauseOfTask(ChatId, FirstTask);
                            ChooseTaskToAnswer();
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
                            Front.CaptainDisplay.InformAboutTaskCorrectness(ChatId, SecondTask.Number);
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
                            AttemptProcesser.SendAttemptForProcess(attempt);
                        }
                        if (SecondTask.Number != 20)
                        {
                            int NumeberOfNewTask = SecondTask.Number + 1;
                            SecondTask = ListOfTasks.GetTask(NumeberOfNewTask);
                            Front.CaptainDisplay.SendClauseOfTask(ChatId, SecondTask);
                            ChooseTaskToAnswer();
                        }
                        else
                        {
                            SecondTask = new Task()
                            {
                                Number = -1
                            };
                            Front.CaptainDisplay.SendClauseOfTask(ChatId, FirstTask);
                            ChooseTaskToAnswer();
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
                            OlympiadConnected.Results.SendCurrentResults(ChatId);
                            Front.CaptainDisplay.TasksAreStillOnCheck(ChatId);
                        }
                        else
                        {
                            Front.CaptainDisplay.OlympiadIsEnded(ChatId);
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
                        OlympiadConnected.Results.SendCurrentResults(ChatId);
                        ChooseTaskToAnswer();
                        break;
                    }
               default:
                    {
                        if (OlympiadConnected.Olympiad.IsStarted && !OlympiadConnected.Olympiad.IsEnded)
                            ChooseTaskToAnswer();
                        else
                            Front.CaptainDisplay.YouCantDoThat(ChatId);
                        break;
                    }
            }
        }

        private void ChooseTaskToAnswer()
        {
            Front.CaptainDisplay.ChooseTaskToAnswer(ChatId, FirstTask, SecondTask);
            priority = Priorities.GetTaskForProcess;
        }

        public override void StartOlympiad()
        {
            Front.CaptainDisplay.DisplayReadme(ChatId);

            Front.CaptainDisplay.SendClauseOfTask(ChatId, FirstTask);
            Front.CaptainDisplay.SendClauseOfTask(ChatId, SecondTask);

            Front.CaptainDisplay.ChooseTaskToAnswer(ChatId, FirstTask, SecondTask);
            priority = Priorities.GetTaskForProcess;
        }

        public void SetResultForAttempt(Attempt attempt)
        {
            if (attempt.Result)
            {
                OlympiadConnected.Results.WriteCorrectInWorksheet(NumberOfRowInExcelWorksheet, attempt.task.Number);
                Front.CaptainDisplay.InformAboutTaskCorrectness(ChatId, attempt.task.Number);
            }
            else
            {
                OlympiadConnected.Results.WriteIncorrectInWorksheet(NumberOfRowInExcelWorksheet, attempt.task.Number);
                Front.CaptainDisplay.InformAboutTaskIncorrectness(ChatId, attempt.task.Number);
            }
        }

        public override void EndOlympiad()
        {
            priority = Priorities.EndedOlympiad;
        }

        public override void SetNoActionAvailable()
        {
            priority = Priorities.NoActionAvailable;
        }

        private enum Priorities
        {
            GetReady,
            ProcessFirstTask,
            ProcessSecondTask,
            NoPriority,
            GetTaskForProcess,
            StartedOlympiad,
            EndedOlympiad,
            NoActionAvailable
        }
    }
}
