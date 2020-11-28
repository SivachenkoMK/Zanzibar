using System;
using System.Collections.Generic;
using System.Text;

namespace ZanzibarBot.Tasks
{
    public class Attempt
    {
        public Task task;
        public string answer;

        public long ChatId;
        public long Id;

        public int ControlSum = 0;
        public int ModeratorsWhoGaveResult = 0;
        public int NumberOfModeratorsToCheck;

        public bool Result;
    }
}
