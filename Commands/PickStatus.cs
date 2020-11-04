using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ZanzibarBot.Commands
{

    class PickStatus : Command
    {
        public override string Name => "pickstatus";

        public override bool IsEnabled { get; set; } = true;

        public override void Execute(MessageEventArgs message)
        {
            /*
             Переделать под кнопки 
             InlineKeyboardButton
             */
            MessageSender.SendMessage(message.Message.Chat.Id, "Оберіть свій статус. Якщо ви учасник, напишіть 1, якщо перевіряючий напишіть 2.");
        }
    }
}
