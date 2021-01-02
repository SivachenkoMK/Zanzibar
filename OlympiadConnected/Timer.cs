using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ZanzibarBot.OlympiadConnected
{
    public class Timer
    {
        private  DateTime StartOfOlympiadTime;
        private  DateTime EndOfOlympiadTime;

        private Thread thread;

        public Timer()
        {
            
        }

        public void Start()
        {
            StartOfOlympiadTime = DateTime.Now;
            EndOfOlympiadTime = StartOfOlympiadTime.AddHours(2);
            thread = new Thread(CountTime);
            thread.Name = "name";
            thread.IsBackground = true;
            thread.Start();
        }

        private void CountTime(object obj)
        {
            while (DateTime.Now < EndOfOlympiadTime)
            {

            }
            Olympiad.TryEndingOlympiad();
        }
    }
}
