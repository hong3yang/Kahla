﻿using Kahla.SDK.Abstract;
using System;
using System.Threading.Tasks;

namespace Kahla.SDK.CommandHandlers
{
    [CommandHandler("reboot")]
    public class RebootCommandHandler : CommandHandlerBase
    {
        public RebootCommandHandler(BotCommander botCommander) : base(botCommander)
        {
        }

        public async override Task Execute(string command)
        {
            await Task.Delay(0);
            Console.Clear();
            var _ = _botCommander._botBase.Connect().ConfigureAwait(false);
        }
    }
}
