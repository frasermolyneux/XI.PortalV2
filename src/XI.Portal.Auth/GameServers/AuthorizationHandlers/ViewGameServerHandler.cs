﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using XI.Portal.Auth.Contract.Constants;
using XI.Portal.Auth.GameServers.AuthorizationRequirements;
using XI.Portal.Servers.Dto;

namespace XI.Portal.Auth.GameServers.AuthorizationHandlers
{
    public class ViewGameServerHandler : AuthorizationHandler<ViewGameServer, GameServerDto>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ViewGameServer requirement, GameServerDto resource)
        {
            if (context.User.HasClaim(claim => claim.Type == XtremeIdiotsClaimTypes.SeniorAdmin))
                context.Succeed(requirement);

            if (context.User.HasClaim(XtremeIdiotsClaimTypes.HeadAdmin, resource.GameType.ToString()))
                context.Succeed(requirement);

            if (context.User.HasClaim(PortalClaimTypes.GameServer, resource.GameType.ToString()))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}