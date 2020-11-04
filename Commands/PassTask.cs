using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace ZanzibarBot.Commands
{
    class PassTask : Command
    {
        public override string Name => "passtask";

        public override bool IsEnabled { get; set; } = true;

        public override void Execute(MessageEventArgs message)
        {
            if (IsEnabled)
            {
                /* Add logic for this command */
            }
            else
            {
                MessageSender.SendMessage(message.Message.Chat.Id, "Ви не можете виконати цю команду.");
            }
        }
    }
}
