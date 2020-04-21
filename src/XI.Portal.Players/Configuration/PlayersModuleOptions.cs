﻿using System;
using XI.Portal.Players.Interfaces;

namespace XI.Portal.Players.Configuration
{
    internal class PlayersModuleOptions : IPlayersModuleOptions
    {
        public Action<IPlayersRepositoryOptions> PlayersRepositoryOptions { get; set; }
        public Action<IAdminActionsRepositoryOptions> AdminActionsRepositoryOptions { get; set; }
        public Action<IPlayerLocationsRepositoryOptions> PlayerLocationsRepositoryOptions { get; set; }

        public void Validate()
        {
            if (PlayersRepositoryOptions == null)
                throw new NullReferenceException(nameof(PlayersRepositoryOptions));

            if (AdminActionsRepositoryOptions == null)
                throw new NullReferenceException(nameof(AdminActionsRepositoryOptions));

            if (PlayerLocationsRepositoryOptions == null)
                throw new NullReferenceException(nameof(PlayerLocationsRepositoryOptions));
        }
    }
}