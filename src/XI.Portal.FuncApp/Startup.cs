﻿using System;
using System.IO;
using System.Reflection;
using FM.GeoLocation.Client.Extensions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XI.Forums.Extensions;
using XI.Portal.Bus.Client;
using XI.Portal.Bus.Extensions;
using XI.Portal.Data.Legacy;
using XI.Portal.FuncApp;
using XI.Portal.Maps.Extensions;
using XI.Portal.Players.Extensions;
using XI.Portal.Repository.Config;
using XI.Portal.Repository.Extensions;
using XI.Portal.Servers.Extensions;
using XI.Utilities.FtpHelper;

[assembly: FunctionsStartup(typeof(Startup))]

namespace XI.Portal.FuncApp
{
    public class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var context = builder.GetContext();

            builder.ConfigurationBuilder
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.json"), true, false)
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, $"appsettings.{context.EnvironmentName}.json"),
                    true, false)
                .AddJsonFile("local.settings.json", true, false)
                .AddUserSecrets(Assembly.GetExecutingAssembly(), false)
                .AddEnvironmentVariables();

            base.ConfigureAppConfiguration(builder);
        }


        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = builder.GetContext().Configuration;

            builder.Services.AddDbContext<LegacyPortalContext>(options =>
                options.UseSqlServer(config["LegacyPortalContext:ConnectionString"]));

            builder.Services.AddGeoLocationClient(options =>
            {
                options.BaseUrl = config["GeoLocation:BaseUrl"];
                options.ApiKey = config["GeoLocation:ApiKey"];
                options.UseMemoryCache = true;
                options.BubbleExceptions = false;
                options.CacheEntryLifeInMinutes = 60;
                options.RetryTimespans = new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(3),
                    TimeSpan.FromSeconds(5)
                };
            });

            builder.Services.AddServersModule(options =>
            {
                options.ConfigureGameServerStatusRepository(repositoryOptions =>
                {
                    repositoryOptions.StorageConnectionString = config["AppDataContainer:StorageConnectionString"];
                    repositoryOptions.StorageTableName = config["GameServerStatusRepository:StorageTableName"];
                });

                options.ConfigureGameServerStatusStatsRepository(repositoryOptions =>
                {
                    repositoryOptions.StorageConnectionString = config["AppDataContainer:StorageConnectionString"];
                    repositoryOptions.StorageTableName = config["GameServerStatusStatsRepository:StorageTableName"];
                });
            });

            builder.Services.AddPlayersModule(options =>
            {
                options.ConfigurePlayerLocationsRepository(repositoryOptions =>
                {
                    repositoryOptions.StorageConnectionString = config["AppDataContainer:StorageConnectionString"];
                    repositoryOptions.StorageTableName = config["PlayerLocationsRepository:StorageTableName"];
                });

                options.ConfigurePlayersCacheRepository(repositoryOptions =>
                {
                    repositoryOptions.StorageConnectionString = config["AppDataContainer:StorageConnectionString"];
                    repositoryOptions.StorageTableName = config["PlayerCacheRepository:StorageTableName"];
                });

                options.ConfigureBanFilesRepositoryOptions(repositoryOptions =>
                {
                    repositoryOptions.StorageConnectionString = config["AppDataContainer:StorageConnectionString"];
                    repositoryOptions.StorageContainerName = config["BanFilesRepository:StorageContainerName"];
                });
            });

            builder.Services.AddMapsModule(options =>
            {
                options.ConfigureMapRedirectRepository(repositoryOptions =>
                {
                    repositoryOptions.MapRedirectBaseUrl = config["MapsRedirect:BaseUrl"];
                    repositoryOptions.ApiKey = config["MapsRedirect:ApiKey"];
                });
            });

            builder.Services.AddForumsClient(options =>
            {
                options.BaseUrl = config["XtremeIdiotsForums:BaseUrl"];
                options.ApiKey = config["XtremeIdiotsForums:ApiKey"];
            });

            builder.Services.AddSingleton<IFtpHelper, FtpHelper>();

            builder.Services.Configure<PortalServiceBusOptions>(config.GetSection("ServiceBus"));
            builder.Services.AddServiceBus();

            builder.Services.Configure<AppDataOptions>(config.GetSection("AppData"));
            builder.Services.AddAppData();
        }
    }
}