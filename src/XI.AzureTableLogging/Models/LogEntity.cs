﻿using System;
using FM.AzureTableExtensions.Library;
using FM.AzureTableExtensions.Library.Attributes;

namespace XI.AzureTableLogging.Models
{
    public class LogEntity : TableEntityExtended
    {
        public string HourKey { get; set; }

        public string LogLevel { get; set; }
        public string EventId { get; set; }
        public string Message { get; set; }

        [EntityJsonPropertyConverter] public Exception Exception { get; set; }
    }
}