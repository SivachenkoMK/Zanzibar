using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace ZanzibarBot.Front
{
    public static class ModeratorDisplay
    {
        public static void InformModeratorAboutTools(long chatId)
        {
            MessageSender.SendMessage(chatId,
@"Ви - перевіряючий. 
Після початку олімпіади вам почнуть надходити відповіді учасників. Ваша робота - звіряти ці відповіді та встановлювати результат виконання задачі.");
        }

        public static void InformMainModeratorHowToStartOlympiad(long chatId)
        {
            MessageSender.SendMessage(chatId, 
@"Ви - головний перевіряючий. 
Щоб розпочати олімпіаду, введіть в чат слово 'Розпочати'. 
Перед цим перевірте, чи всі учасники підготувалися до олімпіади.");
        }

        public static void YouCantStartOlympiad(long chatId)
        {
            MessageSender.SendMessage(chatId, "Ви не можете розпочати олімпіаду.");
        }

        public static void WaitForOlympiadStart(long chatId)
        {
            MessageSender.SendMessage(chatId, "Зачекайте початку олімпіади.");
        }

        public static void PleaseGiveCorrectData(long chatId)
        {
            MessageSender.SendMessage(chatId, "Будь ласка, відправляйте коректні дані.");
        }

        public static void GiveInfoAboutCaptainAnswer(long chatId, Tasks.Attempt attempt)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton("Правильно"),
                new KeyboardButton("Неправильно")
            })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            };

            MessageSender.SendMessage(chatId, attempt.task.Clause + "\nПравильна відповідь: " + attempt.task.Answer + "\nВідповідь учасника: " + attempt.answer, replyKeyboardMarkup);
        }

        public static void YouCantDoThat(long ChatId)
        {
            MessageSender.SendMessage(ChatId, "Ви не можете цього зробити");
        }

        public static void InformMainModeratorHowToEndOlympiad(long chatId)
        {
            MessageSender.SendMessage(chatId, "Для учаників олімпіаду закінчено. Коли перевіряючі закінчать, напишіть в чат слово 'Кінець'");
        }
    }
}
