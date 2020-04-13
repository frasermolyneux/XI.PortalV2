﻿using System.Collections.Generic;

namespace XI.Servers.Query.Models
{
    internal class Quake3QueryResponse : IQueryResponse
    {
        public Quake3QueryResponse(Dictionary<string, string> serverParams, List<IQueryPlayer> players)
        {
            ServerParams = serverParams;
            Players = players;
        }

        public string ServerName => ServerParams.ContainsKey("sv_hostname") ? ServerParams["sv_hostname"] : null;

        public string Map => ServerParams.ContainsKey("mapname") ? ServerParams["mapname"] : null;
        public string Mod => ServerParams.ContainsKey("fs_game") ? ServerParams["fs_game"] : null;

        public int PlayerCount => Players.Count;

        public IDictionary<string, string> ServerParams { get; set; }
        public IList<IQueryPlayer> Players { get; set; }
    }
}