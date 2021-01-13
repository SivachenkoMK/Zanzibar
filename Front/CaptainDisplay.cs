using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using ZanzibarBot.Tasks;

namespace ZanzibarBot.Front
{
    public static class CaptainDisplay
    {
        public static void YouCantDoThat(long chatId)
        {
            MessageSender.SendMessage(chatId, "Ви не можете цього зробити");
        }

        public static void DisplayReady(long chatId)
        {
            MessageSender.SendMessage(chatId, "Вітаю! Ви підготувались до олімпіади.");
        }
        public static void DisplayRequestForEnteringAnswer(long chatId, int taskNumber)
        {
            MessageSender.SendMessage(chatId, $"Введіть відповідь на задачу №{taskNumber}");
        }

        public static void NoSuchTaskToChoose(long chatId)
        {
            MessageSender.SendMessage(chatId, "У Вас нема такої задачі на відправку.");
        }

        public static void InformAboutTaskCorrectness(long chatId, int taskNumber)
        {
            MessageSender.SendMessage(chatId, $"Задача №{taskNumber} - правильна відповідь, задачу зараховано!");
        }

        public static void InformAboutTaskIncorrectness(long chatId, int taskNumber)
        {
            MessageSender.SendMessage(chatId, $"Задача №{taskNumber} - неправильна відповідь, задачу не зараховано!");
        }

        public static void SendClauseOfTask(long chatId, Task task)
        {
            MessageSender.SendMessage(chatId, task.Clause);
        }

        public static void TasksAreStillOnCheck(long chatId)
        {
            MessageSender.SendMessage(chatId, "Ще не всі задачі перевірені - зачекайте, Вам надішлють фінальні результати.");
        }

        public static void OlympiadIsEnded(long chatId)
        {
            MessageSender.SendMessage(chatId, "Олімпіаду вже закінчено, але Ви можете перевірити результати.");
        }

        public static ReplyKeyboardMarkup GetKeyboardOfTasksToPass(Task firstTask, Task secondTask)
        {
            if (secondTask.Number != -1 && firstTask.Number != -1)
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
                {
                    new KeyboardButton(firstTask.Number.ToString()),
                    new KeyboardButton(secondTask.Number.ToString())
                })
                {
                    ResizeKeyboard = true
                };

                return replyKeyboardMarkup;
            }
            else if (secondTask.Number == -1 && firstTask.Number != -1)
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
                {
                    new KeyboardButton(firstTask.Number.ToString()),
                })
                {
                    ResizeKeyboard = true
                };

                return replyKeyboardMarkup;
            }
            else
            {
                return new ReplyKeyboardMarkup();
            }
        }

        public static void ChooseTaskToAnswer(long ChatId, Task firstTask, Task secondTask)
        {
            ReplyKeyboardMarkup markup = GetKeyboardOfTasksToPass(firstTask, secondTask);
            MessageSender.SendMessage(ChatId, "Оберіть задачу, котру хочете відправити.", markup);
        }
        
        public static void DisplayReadme(long chatId)
        {
            MessageSender.SendMessage(chatId,
@"Вітаю! 
Олімпіаду розпочато.
Внизу є дві кнопки з номерами задач, котрі ви можете відправити. 
Натискаючи на одну з них, ви маете написати відповідь на обрану задачу.
Команда /getresults - корисна функція для перегляду інформації про стан олімпіади на даний момент.
Успіхів!");
        }
    }
}
