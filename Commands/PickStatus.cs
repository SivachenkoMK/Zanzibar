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

        public override void Execute(MessageEventArgs message, TelegramBotClient bot)
        {
            InlineKeyboardButton Participant = new InlineKeyboardButton();
            Participant.Text = "Учасник.";
            InlineKeyboardButton Administrator = new InlineKeyboardButton();
            Participant.Text = "Администратор.";
            List<InlineKeyboardButton> Buttons = new List<InlineKeyboardButton> { Administrator, Participant };
            InlineKeyboardMarkup keyboardMarkup = new InlineKeyboardMarkup(Buttons);
            bot.SendTextMessageAsync(message.Message.Chat.Id, "Оберіть свій статус.", replyMarkup: keyboardMarkup);
        }
    }
}
