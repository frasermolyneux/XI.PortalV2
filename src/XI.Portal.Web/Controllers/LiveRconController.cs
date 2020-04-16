﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using XI.Portal.Auth.Contract.Constants;
using XI.Portal.Servers.Repository;
using XI.Servers.Helpers;
using XI.Servers.Rcon.Factories;

namespace XI.Portal.Web.Controllers
{
    [Authorize(Policy = XtremeIdiotsPolicy.CanAccessLiveRcon)]
    public class LiveRconController : Controller
    {
        private readonly IGameServersRepository _gameServersRepository;
        private readonly IGameServerStatusHelper _gameServerStatusHelper;
        private readonly ILogger<LiveRconController> _logger;
        private readonly IRconClientFactory _rconClientFactory;

        private readonly string[] _requiredClaims = {XtremeIdiotsClaimTypes.SeniorAdmin, XtremeIdiotsClaimTypes.HeadAdmin, XtremeIdiotsClaimTypes.GameAdmin};

        public LiveRconController(ILogger<LiveRconController> logger,
            IGameServersRepository gameServersRepository,
            IRconClientFactory rconClientFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _gameServersRepository = gameServersRepository ?? throw new ArgumentNullException(nameof(gameServersRepository));
            _rconClientFactory = rconClientFactory ?? throw new ArgumentNullException(nameof(rconClientFactory));
        }

        public async Task<IActionResult> Index()
        {
            var servers = (await _gameServersRepository.GetGameServers(User, _requiredClaims))
                .Where(server => !string.IsNullOrWhiteSpace(server.RconPassword));

            return View(servers);
        }

        public async Task<IActionResult> ViewRcon(Guid? id)
        {
            if (id == null) return NotFound();

            var model = await _gameServersRepository.GetGameServer(id, User, _requiredClaims);

            return View(model);
        }

        public async Task<IActionResult> GetRconPlayers(Guid? id)
        {
            if (id == null) return NotFound();

            var model = await _gameServersRepository.GetServerStatus(id, User, _requiredClaims);

            return Json(new
            {
                data = model.Players
            });
        }

        public async Task<IActionResult> KickPlayer(Guid? id, string num)
        {
            if (id == null) return NotFound();

            if (string.IsNullOrWhiteSpace(num))
                return NotFound();

            var model = await _gameServersRepository.GetGameServer(id, User, _requiredClaims);

            var rconClient = _rconClientFactory.CreateInstance(model.GameType, model.Title, model.Hostname, model.QueryPort, model.RconPassword);

            rconClient.KickPlayer(num);

            TempData["Success"] = $"Player in slot {num} has been kicked";
            return RedirectToAction(nameof(ViewRcon), new {id});
        }
    }
}