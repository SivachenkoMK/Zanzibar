using System;
using System.Collections.Generic;

namespace ZanzibarBot.Commands
{
    public static class ListOfCommands
    {
        private static List<Command> commands;
        public static List<Command> Commands => GetCommands();

        private static List<Command> GetCommands()
        {
            if (commands != null)
                return commands;
            else
            {
                commands = new List<Command>();
                PassTask passTask = new PassTask();
                commands.Add(passTask);

                Start start = new Start();
                commands.Add(start);

                PickStatus pickStatus = new PickStatus();
                commands.Add(pickStatus);

                GetResults getResults = new GetResults();
                commands.Add(getResults);

                return commands;
            }
        }
    }
}
