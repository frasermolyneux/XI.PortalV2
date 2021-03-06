﻿using System;
using FM.GeoLocation.Contract.Models;
using XI.CommonTypes;

namespace XI.Portal.Players.Dto
{
    public class PlayerLocationDto
    {
        public GameType GameType { get; set; }

        public Guid ServerId { get; set; }
        public string ServerName { get; set; }
        public string Guid { get; set; }
        public string PlayerName { get; set; }

        public GeoLocationDto GeoLocation { get; set; }
    }
}