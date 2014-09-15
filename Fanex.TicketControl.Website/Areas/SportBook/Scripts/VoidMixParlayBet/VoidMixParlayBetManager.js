/* File Created: February 6, 2013 */
/*global VoidMixParlayBet*/
"use strict";

$(function () {
    var voidMixParlayBetManager = new VoidMixParlayBetManager();
    voidMixParlayBetManager.Init();
});

function VoidMixParlayBetManager() {
    this.voidMixParlayBet = new VoidMixParlayBet();
}

VoidMixParlayBetManager.prototype.Init = function () {
    var self = this;
    self.voidMixParlayBet.Init();
}