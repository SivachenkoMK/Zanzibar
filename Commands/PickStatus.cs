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
            ReplyKeyboardMarkup markup = new ReplyKeyboardMarkup(new[]
            {
                    new KeyboardButton("Captain"),
                    new KeyboardButton("Moderator"),
            });
            markup.ResizeKeyboard = true;
            markup.OneTimeKeyboard = true;
            MessageSender.SendMessage(message.Message.Chat.Id, "Оберіть свій статус. Якщо ви капітан команди - оберіть Captain, якщо перевіряючий - оберіть Moderator", markup);
        }
    }
}
