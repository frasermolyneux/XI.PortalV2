﻿function escapeHtml(unsafe) {
    return unsafe
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");
}

function handleAjaxError(xhr, textStatus, error) {
    console.log(textStatus);
}

function renderPlayerName(gameType, username) {
    var safeUsername = escapeHtml(username);
    return gameTypeIcon(gameType) + " " + safeUsername;
}

function renderPlayerName(gameType, username, playerId) {
    var safeUsername = escapeHtml(username);
    return gameTypeIcon(gameType) + " <a href='/Players/Details/" + playerId + "'>" + safeUsername + "</a>";
}

function chatLogUrl(chatLogId) {
    return "<a href='/ServerAdmin/ChatLogPermaLink/" + chatLogId + "'>PermLink</a>";
}

function gameTypeIcon(gameType) {
    return "<img src='/images/game-icons/" +
        gameType +
        ".png' alt='" +
        gameType +
        "' width='16' height='16' />";
}

function downloadDemoLink(demoName, demoId) {
    return "<a href='/Demos/Download/" + demoId + "'>" + demoName + "</a>";
}

function deleteDemoLink(demoId) {
    return "<a href='/Demos/Delete/" + demoId + "'>Delete Demo</a>";
}

function geoLocationIpLink(ipAddress) {
    return "<a href='https://geo-location.net/Home/LookupAddress/" + ipAddress + "'>" + ipAddress + "</a>";
}