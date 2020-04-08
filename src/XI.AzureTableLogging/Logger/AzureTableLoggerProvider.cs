﻿using System;
using Microsoft.Extensions.Logging;
using XI.AzureTableLogging.Configuration;

namespace XI.AzureTableLogging.Logger
{
    public class AzureTableLoggerProvider : ILoggerProvider
    {
        private readonly Action<IAzureTableLoggerOptions> _configureOptions;

        public AzureTableLoggerProvider(Action<IAzureTableLoggerOptions> configureOptions)
        {
            _configureOptions = configureOptions;
        }

        public ILogger CreateLogger(string categoryName)
        {
            var options = new AzureTableLoggerOptions();
            _configureOptions.Invoke(options);

            options.Validate();

            return new AzureTableLogger(options);
        }

        public void Dispose()
        {
        }
    }
}