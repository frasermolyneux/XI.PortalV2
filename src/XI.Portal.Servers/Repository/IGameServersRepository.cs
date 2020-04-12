﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using XI.Portal.Data.Legacy.Models;

namespace XI.Portal.Servers.Repository
{
    public interface IGameServersRepository
    {
        Task<List<GameServers>> GetGameServers(ClaimsPrincipal user, IEnumerable<string> requiredClaims);
        Task<GameServers> GetGameServer(Guid? id, ClaimsPrincipal user, IEnumerable<string> requiredClaims);
        Task CreateGameServer(GameServers model);
        Task UpdateGameServer(Guid? id, GameServers model, ClaimsPrincipal user, IEnumerable<string> requiredClaims);
        Task<bool> GameServerExists(Guid id, ClaimsPrincipal user, IEnumerable<string> requiredClaims);

        Task RemoveGameServer(Guid id, ClaimsPrincipal user, IEnumerable<string> requiredClaims);
    }
}