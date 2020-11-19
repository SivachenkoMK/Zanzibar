using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace ZanzibarBot.People
{
    class Moderator : Person
    {
        public bool IsMain;

        private Priorities prioriy = Priorities.NoPriority;

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
                        if (message.Text.Contains("Розпочати"))
                        {
                            if (IsMain)
                            {
                                /* Следующий шаг - создание олимпиады, работа с ней и ее начало. */
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
            }
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

        private enum Priorities
        {
            StartOlympiad,
            WaitForStart,
            NoPriority,

        }
    }
}
