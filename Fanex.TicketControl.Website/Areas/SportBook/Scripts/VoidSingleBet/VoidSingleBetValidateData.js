/*exported validateData*/
/*global NapTicketControl, tooltip, Message, helper*/
"use strict";

var validateData = {
    checkParamBeforeGetData: function () {
        $('#msg').hide();

        if ($('#txtMatchId').val() == '') {
            $('#msg').html(tooltip.show('txtMatchId', 'MatchID is required.')).show();
            return false;
        }

        if (isNaN($('#txtMatchId').val()) || $('#txtMatchId').val().length > 10) {
            $('#msg').html(tooltip.show('txtMatchId', 'Invalid MatchID.')).show();
            return false;
        }

        if ($('#fromRefno').val() == '') {
            $('#msg').html(tooltip.show('fromRefno', 'From BetID is required.')).show();
            return false;
        }

        if (isNaN($('#fromRefno').val())) {
            $('#msg').html(tooltip.show('fromRefno', 'Invalid From BetID.')).show();
            return false;
        }

        if ($('#toRefno').val() == '') {
            $('#msg').html(tooltip.show('toRefno', 'To BetID is required.')).show();
            return false;
        }

        if (isNaN($('#toRefno').val())) {
            $('#msg').html(tooltip.show('toRefno', 'Invalid To BetID.')).show();
            return false;
        }

        if (parseFloat($('#toRefno').val()) < parseFloat($('#fromRefno').val())) {
            $('#msg').html(tooltip.show('toRefno', 'Invalid To BetID.')).show();
            return false;
        }

        var eventType = $('#EventType').val();

        if (eventType == '1') {
            if ($('#LiveHomeScore').val() != '' && ($('#LiveHomeScore').val().length > 2 || isNaN($('#LiveHomeScore').val()))) {
                $('#msg').html(tooltip.show('LiveHomeScore', 'Invalid Live Home Score.')).show();
                return false;
            }

            if ($('#LiveAwayScore').val() != '' && ($('#LiveAwayScore').val().length > 2 || isNaN($('#LiveAwayScore').val()))) {
                $('#msg').html(tooltip.show('LiveAwayScore', 'Invalid Live Home Score.')).show();
                return false;
            }
        }

        //cs
        var IsSelectedCS = NapTicketControl.Helper.IsSelectedBettype(4) || NapTicketControl.Helper.IsSelectedBettype(30);
        if (IsSelectedCS) {
            if (NapTicketControl.Helper.GetBetteam430() == '') {
                $('#msg').html(tooltip.show('chk-score-all', 'Please select a score for bettype: Correct score!.')).show();
                return false;
            }
        }

        //htft
        var IsSelectedHTFT = NapTicketControl.Helper.IsSelectedBettype(16);
        if (IsSelectedHTFT) {
            if (helper.GetBetteam16() == '') {
                $('#msg').html(tooltip.show('chk-htft-all', 'Please select a betteam for bettype: HT/FT!.')).show();
                return false;
            }
        }

        //tg
        var IsSelectedTG = NapTicketControl.Helper.IsSelectedBettype(6);
        if (IsSelectedTG) {
            if (NapTicketControl.Helper.GetBetteam6() == '') {
                $('#msg').html(tooltip.show('chk-tg-all', 'Please select a goal for bettype: Total Goal!.')).show();
                return false;
            }
        }

        //fg/lg
        var IsSelectedGFLG = NapTicketControl.Helper.IsSelectedBettype(14);
        if (IsSelectedGFLG) {
            if (NapTicketControl.Helper.GetBetteam14() == '') {
                $('#msg').html(tooltip.show('chk-flg-all', 'Please select a goal for bettype: First Goal/Last Goal!.')).show();
                return false;
            }
        }

        if (!NapTicketControl.Helper.checkNew2BettypeBeforeGetData()) {
            return false;
        }

        if (helper.GetBettypesChecked() == '') {
            $('#msg').html(tooltip.show('chkallbettype', 'At least one bettype must be selected.')).show();
            return false;
        }

        return true;
    },

    checkParamBeforeVoidTicket: function () {
        $('#msg').hide();

        if (helper.getSubTicketsChecked() == '') {
            $('#msg').html(tooltip.show('chk-ticket-all', 'At least one ticket must be selected.')).show();
            return false;
        }

        if (Message.Validator.validateMessageConstraints() === false) {
            return false;
        }

        if ($('#password').val() == '') {
            $('#msg').html(tooltip.show('password', 'Please input password.')).show();
            return false;
        }

        return true;
    }
};