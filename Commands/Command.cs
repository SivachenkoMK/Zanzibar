using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace ZanzibarBot.Commands
{
    public abstract class Command
    {
        public abstract string Name { get; }

        public bool Contains(string Text)
        {
            Text = Text.ToLower();
            if (Text.Contains(this.Name))
                return true;
            return false;
        }

        public abstract void Execute(MessageEventArgs message);
    }
}
