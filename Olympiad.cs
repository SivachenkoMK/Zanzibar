using System;
using System.Collections.Generic;
using System.Text;

namespace ZanzibarBot
{
    public class Olympiad
    {
        private DateTime StartOfOlympiadTime;
        private DateTime EndOfOlympiadTime;

        private void SetTimer()
        {
            StartOfOlympiadTime = DateTime.Now;
            EndOfOlympiadTime = StartOfOlympiadTime.AddHours(2);
            while (DateTime.Now < EndOfOlympiadTime)
            {

            }
        }

        public void StartOlympiad()
        {
            SetTimer();
            Console.WriteLine(1);
        }
    }
}
