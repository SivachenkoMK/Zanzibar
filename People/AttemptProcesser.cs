using System;
using System.Collections.Generic;
using System.Text;
using ZanzibarBot.Tasks;

namespace ZanzibarBot.People
{
    public static class AttemptProcesser
    {
        private static List<Attempt> attempts = new List<Attempt>();

        public static void SendAttemptForProcess(Attempt attempt)
        {
            List<Moderator> moderators = new List<Moderator>();
            foreach (Person person in ListOfPeople.People)
            {
                if (person.status == StatusOfPerson.Moderator)
                {
                    moderators.Add((Moderator)person);
                }
            }
            int QuantityOfModerators = moderators.Count;
            if (QuantityOfModerators > 5)
            {
                Random random = new Random();
                List<int> randomNumbers = new List<int>();
                while (randomNumbers.Count < 5)
                {
                    int number = random.Next(0, moderators.Count);
                    if (!randomNumbers.Contains(number))
                    {
                        randomNumbers.Add(number);
                    }
                }
                attempt.NumberOfModeratorsToCheck = 5;
                foreach (int num in randomNumbers)
                {
                    moderators[num].TryCheckingTask(attempt);
                }
            }
            else
            {
                attempt.NumberOfModeratorsToCheck = moderators.Count;
                foreach (Moderator moderator in moderators)
                {
                    moderator.TryCheckingTask(attempt);
                }
            }
        }

        public static void SetPersonalResultOfAttempt(Attempt attempt, bool IsCorrect)
        {
            bool SuchAttemptFound = false;
            foreach (Attempt existingAttempt in attempts)
            {
                if (existingAttempt.Id == attempt.Id)
                {
                    attempt = existingAttempt;
                    SuchAttemptFound = true;
                    break;
                }
            }
            if (!SuchAttemptFound)
            {
                attempts.Add(attempt);
            }
            if (IsCorrect)
            {
                attempt.ControlSum++;
            }
            else
            {
                attempt.ControlSum--;
            }
            attempt.ModeratorsWhoGaveResult++;
            if (attempt.ModeratorsWhoGaveResult == attempt.NumberOfModeratorsToCheck)
            {
                Captain captain = (Captain)ListOfPeople.GetPersonFromList(attempt.ChatId);
                if (attempt.ControlSum >= 0)
                {
                    attempt.Result = true;
                }
                else
                {
                    attempt.Result = false;
                }
                captain.SetResultForAttempt(attempt);
            }
        }
    }
}
