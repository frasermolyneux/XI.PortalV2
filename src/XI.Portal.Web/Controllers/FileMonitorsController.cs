﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using XI.CommonTypes;
using XI.Portal.Auth.Contract.Constants;
using XI.Portal.Auth.Contract.Extensions;
using XI.Portal.Data.Legacy.Models;
using XI.Portal.Servers.Interfaces;

namespace XI.Portal.Web.Controllers
{
    [Authorize(Policy = XtremeIdiotsPolicy.ServersManagement)]
    public class FileMonitorsController : Controller
    {
        private readonly IFileMonitorsRepository _fileMonitorsRepository;
        private readonly IGameServersRepository _gameServersRepository;
        private readonly ILogger<FileMonitorsController> _logger;

        private readonly string[] _requiredClaims = {XtremeIdiotsClaimTypes.SeniorAdmin, XtremeIdiotsClaimTypes.HeadAdmin};

        public FileMonitorsController(IFileMonitorsRepository fileMonitorsRepository, IGameServersRepository gameServersRepository, ILogger<FileMonitorsController> logger)
        {
            _fileMonitorsRepository = fileMonitorsRepository ?? throw new ArgumentNullException(nameof(fileMonitorsRepository));
            _gameServersRepository = gameServersRepository ?? throw new ArgumentNullException(nameof(gameServersRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var models = await _fileMonitorsRepository.GetFileMonitors(User, _requiredClaims);
            return View(models);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var model = await _fileMonitorsRepository.GetFileMonitor(id, User, _requiredClaims);

            if (model == null) return NotFound();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["GameServerServerId"] = new SelectList(await _gameServersRepository.GetGameServers(User, _requiredClaims), "ServerId", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("FilePath,GameServerServerId")] FileMonitors model)
        {
            if (!User.HasGameClaim(model.GameServerServer.GameType, _requiredClaims)) return Unauthorized();

            if (ModelState.IsValid)
            {
                await _fileMonitorsRepository.CreateFileMonitor(model);

                _logger.LogInformation(EventIds.Management, "User {User} has created a new file monitor with Id {Id}", User.Username(), model.FileMonitorId);

                TempData["Success"] = "A new File Monitor has been successfully created";
                return RedirectToAction(nameof(Index));
            }

            ViewData["GameServerServerId"] = new SelectList(await _gameServersRepository.GetGameServers(User, _requiredClaims), "ServerId", "Title",
                model.GameServerServerId);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var model = await _fileMonitorsRepository.GetFileMonitor(id, User, _requiredClaims);

            if (model == null) return NotFound();

            ViewData["GameServerServerId"] = new SelectList(await _gameServersRepository.GetGameServers(User, _requiredClaims), "ServerId", "Title",
                model.GameServerServerId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind("FileMonitorId,FilePath,GameServerServerId")]
            FileMonitors model)
        {
            if (id != model.FileMonitorId) return NotFound();

            if (ModelState.IsValid)
                try
                {
                    await _fileMonitorsRepository.UpdateFileMonitor(id, model, User, _requiredClaims);

                    _logger.LogInformation(EventIds.Management, "User {User} has modified a file monitor with Id {Id}", User.Username(), id);

                    TempData["Success"] = "The File Monitor has been successfully updated";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await FileMonitorsExists(model.FileMonitorId))
                        return NotFound();
                    throw;
                }

            ViewData["GameServerServerId"] = new SelectList(await _gameServersRepository.GetGameServers(User, _requiredClaims), "ServerId", "Title",
                model.GameServerServerId);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var model = await _fileMonitorsRepository.GetFileMonitor(id, User, _requiredClaims);

            if (model == null) return NotFound();

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _fileMonitorsRepository.RemoveFileMonitor(id, User, _requiredClaims);

            _logger.LogInformation(EventIds.Management, "User {User} has deleted a file monitor with Id {Id}", User.Username(), id);

            TempData["Success"] = "The File Monitor has been successfully deleted";
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> FileMonitorsExists(Guid id)
        {
            return await _fileMonitorsRepository.FileMonitorExists(id, User, _requiredClaims);
        }
    }
}