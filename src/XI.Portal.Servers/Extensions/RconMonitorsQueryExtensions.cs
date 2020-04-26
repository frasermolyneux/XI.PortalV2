﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using XI.Portal.Auth.Contract.Extensions;
using XI.Portal.Data.Legacy.Models;

namespace XI.Portal.Servers.Extensions
{
    public static class RconMonitorsQueryExtensions
    {
        public static IQueryable<RconMonitors> ApplyAuth(this IQueryable<RconMonitors> rconMonitors, ClaimsPrincipal claimsPrincipal, IEnumerable<string> requiredClaims)
        {
            if (claimsPrincipal == null || requiredClaims == null)
                return rconMonitors.AsQueryable();

            var (gameTypes, serverIds) = claimsPrincipal.ClaimedGamesAndItems(requiredClaims);
            var query = rconMonitors.Include(monitor => monitor.GameServerServer).AsQueryable();

            return query.Where(server => gameTypes.Contains(server.GameServerServer.GameType)).AsQueryable();
        }
    }
}