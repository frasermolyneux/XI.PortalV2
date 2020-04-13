﻿using System.Collections.Generic;

namespace XI.Servers.Models
{
    public interface IGameServerStatus
    {
        string ServerName { get; }
        string Map { get; }
        string Mod { get; }
        int PlayerCount { get; }

        IList<IGameServerPlayer> Players { get; }
    }
}