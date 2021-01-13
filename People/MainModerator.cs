using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace ZanzibarBot.People
{
    class MainModerator : Person
    {
        public override void ProcessMessage(Message message)
        {
            if (message.Text == "Розпочати" && !OlympiadConnected.Olympiad.IsStarted)
            {
                OlympiadConnected.Olympiad.TryStartingOlympiad();
            }    
            else if (message.Text == "Закінчити" && OlympiadConnected.Olympiad.IsEnded)
            {
                OlympiadConnected.Results.EndForModeratorsAndSendFinalResults();
            }
        }

        public void Start()
        {
            Front.ModeratorDisplay.InformMainModeratorHowToStartOlympiad(ChatId);
        }

        public override void StartOlympiad()
        {
            Front.ModeratorDisplay.OlympiadStartedSuccessfuly(ChatId);
        }

        public override void SetNoActionAvailable()
        {
            
        }

        public override void EndOlympiad()
        {
            Front.ModeratorDisplay.InformMainModeratorHowToEndOlympiad(ChatId);
        }
    }
}
