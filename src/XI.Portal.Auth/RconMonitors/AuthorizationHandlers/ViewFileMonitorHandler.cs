﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using XI.Portal.Auth.Contract.Constants;
using XI.Portal.Auth.RconMonitors.AuthorizationRequirements;
using XI.Portal.Servers.Dto;

namespace XI.Portal.Auth.RconMonitors.AuthorizationHandlers
{
    public class ViewRconMonitorHandler : AuthorizationHandler<ViewRconMonitor, RconMonitorDto>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ViewRconMonitor requirement, RconMonitorDto resource)
        {
            if (context.User.HasClaim(claim => claim.Type == XtremeIdiotsClaimTypes.SeniorAdmin))
                context.Succeed(requirement);

            if (context.User.HasClaim(XtremeIdiotsClaimTypes.HeadAdmin, resource.GameServer.GameType.ToString()))
                context.Succeed(requirement);

            if (context.User.HasClaim(PortalClaimTypes.RconMonitor, resource.GameServer.ServerId.ToString()))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}