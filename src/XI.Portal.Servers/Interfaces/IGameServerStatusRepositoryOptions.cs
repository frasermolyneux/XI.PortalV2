﻿using XI.Portal.Servers.Configuration;

namespace XI.Portal.Servers.Interfaces
{
    public interface IGameServerStatusRepositoryOptions
    {
        string StorageConnectionString { get; set; }
        string StorageTableName { get; set; }

        GeoLocationClientConfig GeoLocationClientConfiguration { get; set; }

        void Validate();
    }
}