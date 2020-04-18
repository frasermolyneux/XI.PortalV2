﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using XI.Portal.Auth.Contract.Extensions;
using XI.Portal.Data.Legacy.Models;

namespace XI.Portal.Servers.Extensions
{
    public static class BanFileMonitorDbSetAuthExtensions
    {
        public static IQueryable<BanFileMonitors> ApplyAuthPolicies(this DbSet<BanFileMonitors> banFileMonitors, ClaimsPrincipal claimsPrincipal, IEnumerable<string> requiredClaims)
        {
            if (claimsPrincipal == null || requiredClaims == null)
                return banFileMonitors.AsQueryable();

            var (gameTypes, serverIds) = claimsPrincipal.ClaimedGamesAndServers(requiredClaims);
            var query = banFileMonitors.Include(monitor => monitor.GameServerServer).AsQueryable();

            return query.Where(server => gameTypes.Contains(server.GameServerServer.GameType)).AsQueryable();
        }
    }
}