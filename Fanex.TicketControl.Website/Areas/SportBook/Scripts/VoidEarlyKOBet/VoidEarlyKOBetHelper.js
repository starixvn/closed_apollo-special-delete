/* exported helper*/
/*global NapTicketControl*/
"use strict";

var helper = {
    setStatusAllBettype: function (isChecked) {
        if (isChecked) {
            $('.chkbettype').attr('checked', 'checked')
            $('#chkallbettype').attr('checked', 'checked')
            NapTicketControl.Helper.setStatusAllAdvance1HTAndCS(true, true);
            NapTicketControl.Helper.setStatusAllAdvanceCS(true, true);
            NapTicketControl.Helper.setStatusAllAdvance1HTCS(true, true);
            NapTicketControl.Helper.setStatusAllAdvanceTG(true, true);
            NapTicketControl.Helper.setStatusAllAdvance1HTG(true, true);
            NapTicketControl.Helper.setStatusAllAdvance1HTGAndTG(true, true);
            helper.setStatusAllAdvanceHDP(true);
            helper.setStatusAllAdvance1X2(true);
            helper.setStatusAllAdvanceHTFT(true, true);
            helper.setStatusAllAdvanceFGLG(true, true);
        } else {
            $('.chkbettype').removeAttr('checked');
            $('#chkallbettype').removeAttr('checked');
            NapTicketControl.Helper.setStatusAllAdvance1HTAndCS(false, false);
            NapTicketControl.Helper.setStatusAllAdvanceCS(false, false);
            NapTicketControl.Helper.setStatusAllAdvance1HTCS(false, false);
            NapTicketControl.Helper.setStatusAllAdvanceTG(false, false);
            NapTicketControl.Helper.setStatusAllAdvance1HTG(false, false);
            NapTicketControl.Helper.setStatusAllAdvance1HTGAndTG(false, false);
            helper.setStatusAllAdvanceHDP(false);
            helper.setStatusAllAdvance1X2(false);
            helper.setStatusAllAdvanceHTFT(false, false);
            helper.setStatusAllAdvanceFGLG(false, false);
        }
    },

    eventSubAdvanceTG: function () {
        var selectedItems = $('.advance-item-tg .chk-tg:input:checked').not('#chk-tg-all').length;
        var totalItems = $('.advance-item-tg .chk-tg:input:checkbox').not('#chk-tg-all').length;

        if (selectedItems === totalItems) {
            $('#chk-tg-all').attr('checked', 'checked');
        } else {
            $('#chk-tg-all').removeAttr('checked');
        }
    },

    setStatusAllAdvanceHDP: function (isEnable) {
        $('#ddlBetTeam1378').attr('disabled', !isEnable);
        $('#hdp').attr('disabled', !isEnable);
        if (isEnable) {
            //            helper.expandAdvancedMode();
        }
    },

    setStatusAllAdvance1X2: function (isEnable) {
        $('#ddlBetTeam515').attr('disabled', !isEnable);
        if (isEnable) {
            //            helper.expandAdvancedMode();
        }
    },

    setStatusAllAdvanceHTFT: function (isChecked, isEnable) {
        if (isChecked) {
            $('.chk-htft').attr('checked', 'checked');
            //            helper.expandAdvancedMode();
        } else {
            $('.chk-htft').removeAttr('checked');
        }

        $('.chk-htft').attr('disabled', !isEnable);
    },

    eventSubAdvanceHTFT: function () {
        var selectedItems = $('.advance-item-htft .chk-htft:input:checked').not('#chk-htft-all').length;
        var totalItems = $('.advance-item-htft .chk-htft:input:checkbox').not('#chk-htft-all').length;

        if (selectedItems === totalItems) {
            $('#chk-htft-all').attr('checked', 'checked');
        } else {
            $('#chk-htft-all').removeAttr('checked');
        }
    },

    setStatusAllAdvanceFGLG: function (isChecked, isEnable) {
        if (isChecked) {
            $('.chk-flg').attr('checked', 'checked');
            helper.expandAdvancedMode();
        } else {
            $('.chk-flg').removeAttr('checked');
        }

        $('.chk-flg').attr('disabled', !isEnable);
    },

    eventSubAdvanceFGLG: function () {
        var selectedItems = $('.advance-item-flg .chk-flg:input:checked').not('#chk-flg-all').length;
        var totalItems = $('.advance-item-flg .chk-flg:input:checkbox').not('#chk-flg-all').length;

        if (selectedItems === totalItems) {
            $('#chk-flg-all').attr('checked', 'checked');
        } else {
            $('#chk-flg-all').removeAttr('checked');
        }
    },

    GetBettypesChecked: function () {
        var checkedVals = $('.chkbettype:checkbox:checked').map(function () {
            return this.value;
        }).get().join('^');

        return checkedVals;
    },

    GetParlayTypeChecked: function () {
        var checkedVals = $('.chk-parlay-type:radio:checked').map(function () {
            return this.value;
        }).get().join('^');

        return checkedVals;
    },

    GetBetteam16: function () {
        //HT/FT, Liveindicator = 0

        var checkedVals = '';
        checkedVals = $('.chk-htft:checkbox:checked').not('#chk-htft-all').map(function () {
            return this.value;
        }).get().join('^');

        return checkedVals;
    },

    GetBetteam515: function () {
        //FT.1X2 & 1st 1X2, Liveindicator = 1
        var IsSelected = NapTicketControl.Helper.IsSelectedBettype(5) || NapTicketControl.Helper.IsSelectedBettype(15);
        var checkedVals = '';
        if (IsSelected) {
            checkedVals = $('#ddlBetTeam515').val();
        }

        return checkedVals;
    },

    GetBetteam1378: function () {
        var IsSelected = NapTicketControl.Helper.IsSelectedBettype(1) ||
                         NapTicketControl.Helper.IsSelectedBettype(3) ||
                         NapTicketControl.Helper.IsSelectedBettype(7) ||
                         NapTicketControl.Helper.IsSelectedBettype(8);
        var checkedVals = '';
        if (IsSelected) {
            checkedVals = $('#ddlBetTeam1378').val();
        }

        return checkedVals;
    },

    setStausAllCheckTicket: function () {
        var isChecked = $('#chk-ticket-all').prop('checked');
        if (isChecked) {
            $('.chk-ticket').attr('checked', 'checked');
        } else {
            $('.chk-ticket').removeAttr('checked');
        }
    },

    eventSubTicket: function () {
        var selectedItems = $('.chk-ticket:input:checked').length;
        var totalItems = $('.chk-ticket:input:checkbox').length;

        if (selectedItems === totalItems) {
            $('#chk-ticket-all').attr('checked', 'checked');
        } else {
            $('#chk-ticket-all').removeAttr('checked');
        }
    },

    getSubTicketsChecked: function () {
        var checkedVals = '';
        checkedVals = $('.chk-ticket:checkbox:checked').not('#chk-ticket-all').map(function () {
            if (!isNaN(parseFloat(this.value))) {
                return this.value;
            }
        }).get().join(',');

        return checkedVals;
    },

    expandMoreMsg: function () {
        $('.other-languages').show();
        $('#spMoreMsg').html('[-]');
    },

    resetInput: function () {
        $('#txtMatchId').val('');
        $('#fromRefno').val('');
        $('#toRefno').val('');
        $('#toRefno').val('');
        $('#LiveHomeScore').val('');
        $('#LiveAwayScore').val('');
        $('#LiveAwayScore').val('');
        $('#TransDate').removeAttr('checked');

        $('#FromTransDate').val('');
        $('#ToTransDate').val('');
        $('#EventType').val(2);

        $('.opt-filters').hide();

        $('#password').val('');
        $('.msg-void-ticket').val('');
        helper.setStatusAllBettype(false);
        helper.collapseAdvancedMode();
    },

    displayContent: function (isShow) {
        if (isShow) {
            $('#report-content').show();
        }
        else {
            $('#report-content').hide();
        }
    },

    expandAdvancedMode: function () {
        var iEle = $('.fieldset-advance-mode legend').find('i');
        iEle.removeClass('icon-plus-sign');
        iEle.addClass('icon-minus-sign');
        $('.tbl-advance-mode').show();
        $('.fieldset-advance-mode').removeClass('nb-lrb');
        $('.fieldset-advance-mode legend').attr('title', 'Collapse advanced mode');
    },

    collapseAdvancedMode: function () {
        var iEle = $('.fieldset-advance-mode legend').find('i');
        iEle.removeClass('icon-minus-sign');
        iEle.addClass('icon-plus-sign');
        $('.tbl-advance-mode').hide();
        $('.fieldset-advance-mode').addClass('nb-lrb');
        $('.fieldset-advance-mode legend').attr('title', 'Expand advanced mode');
    },

    toggleAdvancedMode: function (self) {
        // here, self is legend tag
        var iEle = $(self).find('i');
        var isExpand = false;

        if (iEle.hasClass('icon-minus-sign')) {
            iEle.removeClass('icon-minus-sign');
            iEle.addClass('icon-plus-sign');
            $('.tbl-advance-mode').hide();
            $('.fieldset-advance-mode').addClass('nb-lrb');
            $(self).attr('title', 'Expand advanced mode');
            isExpand = false;
        } else {
            iEle.removeClass('icon-plus-sign');
            iEle.addClass('icon-minus-sign');
            $('.tbl-advance-mode').show();
            $('.fieldset-advance-mode').removeClass('nb-lrb');
            $(self).attr('title', 'Collapse advanced mode');
            isExpand = true;
        }

        return isExpand;
    },

    checkConstraintBettyeByEventTypeForAdvanceMode: function () {
        var eventType = $('#EventType').val();

        var IsSelectedCS = (NapTicketControl.Helper.IsSelectedBettype(4) || NapTicketControl.Helper.IsSelectedBettype(30));
        if (IsSelectedCS) {
            NapTicketControl.Helper.setAdvancedModeCorrectScore();
        }
        else {
            NapTicketControl.Helper.setStatusAllAdvance1HTAndCS(IsSelectedCS, IsSelectedCS);
            NapTicketControl.Helper.setStatusAllAdvance1HTCS(IsSelectedCS, IsSelectedCS);
            NapTicketControl.Helper.setStatusAllAdvanceCS(IsSelectedCS, IsSelectedCS);
        }

        var IsSelectedTG = NapTicketControl.Helper.IsSelectedBettype(6) ||
                            NapTicketControl.Helper.IsSelectedBettype(126);
        if (IsSelectedTG) {
            NapTicketControl.Helper.setAdvancedModeTotalGoal();
        }
        else {
            NapTicketControl.Helper.setStatusAllAdvance1HTGAndTG(false, false);
            NapTicketControl.Helper.setStatusAllAdvanceTG(false, false);
            NapTicketControl.Helper.setStatusAllAdvance1HTG(false, false);
        }

        var IsSelectedHDP = NapTicketControl.Helper.IsSelectedBettype(1) || NapTicketControl.Helper.IsSelectedBettype(7)
                            || NapTicketControl.Helper.IsSelectedBettype(3) || NapTicketControl.Helper.IsSelectedBettype(8)
                            || NapTicketControl.Helper.IsSelectedBettype(2) || NapTicketControl.Helper.IsSelectedBettype(12);
        helper.setStatusAllAdvanceHDP(IsSelectedHDP);

        var IsSelected1X2 = NapTicketControl.Helper.IsSelectedBettype(5) || NapTicketControl.Helper.IsSelectedBettype(15);
        helper.setStatusAllAdvance1X2(IsSelected1X2);

        var IsSelectedHTFT = NapTicketControl.Helper.IsSelectedBettype(16);
        if (IsSelectedHTFT) {
            if (helper.GetBetteam16() == '') {
                helper.setStatusAllAdvanceHTFT(IsSelectedHTFT, IsSelectedHTFT);
            }
        }
        else {
            helper.setStatusAllAdvanceHTFT(IsSelectedHTFT, IsSelectedHTFT);
        }

        var IsSelectedGFLG = NapTicketControl.Helper.IsSelectedBettype(14) || NapTicketControl.Helper.IsSelectedBettype(127);
        if (IsSelectedGFLG) {
            if (NapTicketControl.Helper.GetBetteam14() == '') {
                helper.setStatusAllAdvanceFGLG(IsSelectedGFLG, IsSelectedGFLG);
            }
        }
        else {
            helper.setStatusAllAdvanceFGLG(IsSelectedGFLG, IsSelectedGFLG);
        }

        if (IsSelectedCS || IsSelectedTG || IsSelectedHDP || IsSelected1X2 || IsSelectedHTFT || IsSelectedGFLG) {
            helper.expandAdvancedMode();
        }
    }
};