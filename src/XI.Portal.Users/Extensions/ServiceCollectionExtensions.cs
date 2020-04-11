﻿using System;
using Microsoft.Extensions.DependencyInjection;
using XI.Portal.Users.Configuration;
using XI.Portal.Users.Repository;

namespace XI.Portal.Users.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddUsersModule(this IServiceCollection serviceCollection,
            Action<IUsersModuleOptions> configureOptions)
        {
            if (configureOptions == null) throw new ArgumentNullException(nameof(configureOptions));

            IUsersModuleOptions options = new UsersModuleOptions();
            configureOptions.Invoke(options);

            options.Validate();

            if (options.UsersRepositoryOptions != null)
            {
                IUsersRepositoryOptions subOptions = new UsersRepositoryOptions();
                options.UsersRepositoryOptions.Invoke(subOptions);

                subOptions.Validate();

                serviceCollection.AddSingleton(subOptions);
                serviceCollection.AddScoped<IUsersRepository, UsersRepository>();
            }
        }
    }
}