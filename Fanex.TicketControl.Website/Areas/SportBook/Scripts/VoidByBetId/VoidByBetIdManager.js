/* File Created: February 6, 2013 */

$(function () {
    var voidByBetIdManager = new VoidByBetIdManager();
    voidByBetIdManager.Init();
});

function VoidByBetIdManager() {
    this.voidTicket = new VoidByBetIdTicket();
}

VoidByBetIdManager.prototype.Init = function () {
    var self = this;
    self.voidTicket.Init();
}