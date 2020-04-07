﻿using System;
using System.ComponentModel;

namespace XI.Portal.Data.Legacy.Models
{
    public class BanFileMonitors
    {
        public Guid BanFileMonitorId { get; set; }
        [DisplayName("File Path")]
        public string FilePath { get; set; }
        public long RemoteFileSize { get; set; }
        [DisplayName("Last Read")]
        public DateTime LastSync { get; set; }
        public string LastError { get; set; }
        public Guid? GameServerServerId { get; set; }

        [DisplayName("Server")]
        public virtual GameServers GameServerServer { get; set; }
    }
}