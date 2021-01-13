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

        public static void OlympiadIsEnded(long chatId)
        {
            MessageSender.SendMessage(chatId, "Олімпіаду закінчено");
        }

        public static void InformAboutEndOfChecking(long chatId)
        {
            MessageSender.SendMessage(chatId, "Фінальні результати:");
        }

        public static void TheseAreFinalResults(long chatId)
        {
            MessageSender.SendMessage(chatId, "Це - кінцеві результати.");
        }
    }
}
