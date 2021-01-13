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
        private bool IsChecking = false;

        private List<Attempt> AttemptsToProcess = new List<Attempt>();

        private Priorities prioriy = Priorities.NoPriority;

        private Attempt attemptOnCheck = new Attempt();

        public void Start()
        {
            Front.ModeratorDisplay.InformModeratorAboutTools(ChatId);
            prioriy = Priorities.WaitForStart;
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
                        Front.ModeratorDisplay.YouCantDoThat(ChatId);
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
                AttemptsToProcess.Add(attempt);
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
        
        }

        public override void SetNoActionAvailable()
        {
            prioriy = Priorities.NoActionAvailable;
        }

        private enum Priorities
        {
            WaitForStart,
            NoPriority,
            StartedOlympiad,
            CheckingTask,
            NoActionAvailable
        }
    }
}
