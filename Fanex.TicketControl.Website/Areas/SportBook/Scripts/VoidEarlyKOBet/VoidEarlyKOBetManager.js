/*global VoidEarlyKickOffBet*/
"use strict";

$(function () {
    var voidEarlyKOBetManager = new VoidEarlyKOBetManager();
    voidEarlyKOBetManager.Init();
});

function VoidEarlyKOBetManager() {
    this.voidEarlyKOBet = new VoidEarlyKOBet();
}

VoidEarlyKOBetManager.prototype.Init = function () {
    var self = this;
    self.voidEarlyKOBet.Init();
}