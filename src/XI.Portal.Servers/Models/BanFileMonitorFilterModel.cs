﻿using System;
using System.Collections.Generic;
using XI.CommonTypes;

namespace XI.Portal.Servers.Models
{
    public class BanFileMonitorFilterModel
    {
        public enum OrderBy
        {
            None,
            BannerServerListPosition,
            GameType
        }

        public List<GameType> GameTypes { get; set; }
        public List<Guid> BanFileMonitorIds { get; set; }
        public Guid ServerId { get; set; } = Guid.Empty;
        public OrderBy Order { get; set; } = OrderBy.None;
        public int SkipEntries { get; set; } = 0;
        public int TakeEntries { get; set; } = 0;
    }
}