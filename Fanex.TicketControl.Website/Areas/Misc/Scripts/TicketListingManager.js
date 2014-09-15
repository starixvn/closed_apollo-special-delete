/*global TicketListing*/
"use strict";

$(function () {
    var ticketListingManager = new TicketListingManager();
    ticketListingManager.Init();
});

function TicketListingManager() {
    this.ticketListing = new TicketListing();
}

TicketListingManager.prototype.Init = function () {
    var self = this;
    self.ticketListing.Init();
}