/* File Created: February 6, 2013 */

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