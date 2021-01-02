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

        public void Start()
        {
            Front.ModeratorDisplay.InformModeratorAboutTools(ChatId);
            prioriy = Priorities.StartOlympiad;
        }

        public void StartMain()
        {
            Front.ModeratorDisplay.InformMainModeratorHowToStartOlympiad(ChatId);
            prioriy = Priorities.StartOlympiad;
        }

        public override void ProcessMessage(Message message)
        {
            switch (prioriy)
            {
                case (Priorities.NoActionAvailable):
                    {
                        break;
                    }
                case (Priorities.NoPriority):
                    {
                        if (OlympiadConnected.Olympiad.IsEnded && IsMain && message.Text == "Кінець")
                        {
                            foreach (Person person in ListOfPeople.People)
                            {
                                OlympiadConnected.Results.SendCurrentResults(person.ChatId);
                                person.SetNoActionAvailable();
                            }
                        }
                        else
                            Front.ModeratorDisplay.YouCantDoThat(ChatId);
                        break;
                    }
                case (Priorities.StartOlympiad):
                    {
                        if (message.Text == "Розпочати")
                        {
                            if (IsMain)
                            {
                                OlympiadConnected.Olympiad.TryStartingOlympiad();
                                OlympiadConnected.Timer timer = new OlympiadConnected.Timer();
                                timer.Start();
                            }
                            else
                            {
                                Front.ModeratorDisplay.YouCantStartOlympiad(ChatId);
                            }
                        }
                        break;
                    }
                case (Priorities.WaitForStart):
                    {
                        Front.ModeratorDisplay.WaitForOlympiadStart(ChatId);
                        break;
                    }
                case (Priorities.CheckingTask):
                    {
                        if (message.Text == "Правильно")
                        {
                            prioriy = Priorities.StartedOlympiad;
                            AttemptProcesser.SetPersonalResultOfAttempt(attemptOnCheck, true);
                            TryCheckingNextTask();
                        }
                        else if (message.Text == "Неправильно")
                        {
                            prioriy = Priorities.StartedOlympiad;
                            AttemptProcesser.SetPersonalResultOfAttempt(attemptOnCheck, false);
                            TryCheckingNextTask();
                        }
                        else
                        {
                            Front.ModeratorDisplay.PleaseGiveCorrectData(ChatId);
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
            Front.ModeratorDisplay.GiveInfoAboutCaptainAnswer(ChatId, attempt);
            prioriy = Priorities.CheckingTask;
            attemptOnCheck = attempt;
            IsChecking = true;
        }

        public override void StartOlympiad()
        {
            prioriy = Priorities.StartedOlympiad;
        }

        public override void EndOlympiad()
        {
            if (IsMain)
            {
                Front.ModeratorDisplay.InformMainModeratorHowToEndOlympiad(ChatId);
            }
        }

        public override void SetNoActionAvailable()
        {
            prioriy = Priorities.NoActionAvailable;
        }

        private enum Priorities
        {
            StartOlympiad,
            WaitForStart,
            NoPriority,
            StartedOlympiad,
            CheckingTask,
            NoActionAvailable
        }
    }
}
