/*global DelSingleTicket*/
"use strict";

$(function () {
    var delSingleTicketManager = new DelSingleTicketManager();
    delSingleTicketManager.Init();
});

function DelSingleTicketManager() {
    this.delSingleTicket = new DelSingleTicket();
}

DelSingleTicketManager.prototype.Init = function () {
    var self = this;
    self.delSingleTicket.Init();
}