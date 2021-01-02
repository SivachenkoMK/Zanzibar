using System;
using System.Collections.Generic;
using System.Text;

namespace ZanzibarBot.Front
{
    public static class PersonDisplay
    {
        public static void OlympiadIsAlreadyInProgress(long chatId)
        {
            MessageSender.SendMessage(chatId, "Ви вже не можете доєднатись - олімпіаду розпочато.");
        }

        public static void Hello(long chatId)
        {
            MessageSender.SendMessage(chatId, "Привіт. Я бот, котрий допоможе з олімпіадою «Занзібар».");
        }

        public static void EnterPassword(long chatId)
        {
            MessageSender.SendMessage(chatId, "Введіть пароль");
        }

        public static void IncorrectPassword(long chatId)
        {
            MessageSender.SendMessage(chatId, "Неправильний пароль. Спробуйте ще раз.");
        }
    }
}
