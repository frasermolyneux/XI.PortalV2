﻿using XI.Portal.Players.Configuration;

namespace XI.Portal.Players.Interfaces
{
    public interface IPlayerLocationsRepositoryOptions
    {
        string StorageConnectionString { get; set; }
        string StorageTableName { get; set; }

        GeoLocationClientConfig GeoLocationClientConfiguration { get; set; }

        void Validate();
    }
}