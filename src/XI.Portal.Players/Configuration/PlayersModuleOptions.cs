﻿using System;

namespace XI.Portal.Players.Configuration
{
    internal class PlayersModuleOptions : IPlayersModuleOptions
    {
        public Action<IPlayersRepositoryOptions> PlayersRepositoryOptions { get; set; }

        public void Validate()
        {
            if (PlayersRepositoryOptions == null)
                throw new NullReferenceException(nameof(PlayersRepositoryOptions));
        }
    }
}