using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace ZanzibarBot.Commands
{
    class GetResults : Command
    {
        public override bool IsEnabled { get; set; } = true;

        public override string Name => "getresults";

        public override void Execute(MessageEventArgs message)
        {
            
        }
    }
}
