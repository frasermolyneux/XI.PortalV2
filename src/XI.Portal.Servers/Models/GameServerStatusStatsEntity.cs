﻿using FM.AzureTableExtensions.Library;
using FM.AzureTableExtensions.Library.Attributes;
using XI.CommonTypes;

namespace XI.Portal.Servers.Models
{
    internal class GameServerStatusStatsEntity : TableEntityExtended
    {
        public int PlayerCount { get; set; }
        public string MapName { get; set; }
        [EntityEnumPropertyConverter] public GameType GameType { get; set; } = GameType.Unknown;
    }
}