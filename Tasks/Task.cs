using System;
using System.Collections.Generic;
using System.Text;

namespace ZanzibarBot.Tasks
{
    public class Task
    {
        public int Number;

        public string Clause;

        public string Answer;

        public Task()
        {

        }

        public Task(int number, string clause, string answer)
        {
            Number = number;
            Clause = clause;
            Answer = answer;
        }
            
    }
}
