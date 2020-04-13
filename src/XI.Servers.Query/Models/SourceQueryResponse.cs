﻿using System.Collections.Generic;

namespace XI.Servers.Query.Models
{
    internal class SourceQueryResponse : IQueryResponse
    {
        public SourceQueryResponse(Dictionary<string, string> serverParams, List<IQueryPlayer> players)
        {
            ServerParams = serverParams;
            Players = players;
        }

        public string ServerName => ServerParams.ContainsKey("hostname") ? ServerParams["hostname"] : null;

        public string Map => ServerParams.ContainsKey("mapname") ? ServerParams["mapname"] : null;
        public string Mod => ServerParams.ContainsKey("modname") ? ServerParams["modname"] : null;

        public int PlayerCount => Players.Count;

        public IDictionary<string, string> ServerParams { get; set; }
        public IList<IQueryPlayer> Players { get; set; }
    }
}