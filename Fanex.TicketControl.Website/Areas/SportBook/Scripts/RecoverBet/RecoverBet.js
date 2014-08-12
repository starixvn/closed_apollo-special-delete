/*global NapTicketControl, tooltip, ContextMenu*/

"use strict";

$(function () {
    recoverBet.init();
});

var helper = {
    displayContent: function (isShow) {
        if (isShow) {
            $('#report-content').show();
        }
        else {
            $('#report-content').hide();
        }
    },

    resetInput: function(){
        $('#txtBetId').val('');
        $('.msg-void-ticket').val('');
    },

    checkParamBeforRecoverBet: function () {
        $('#msg').hide();

        return Message.Validator.validateMessageConstraints();
    }
}

var recoverBet = {
    init: function () {
        jQuery("#RecoverBetForm").validationEngine();

        $("#txtBetId").keyup(function () { }).ForceIntegerOnly();

        $('body').delegate('#btnSearch', 'click', function (e) {
            e.stopPropagation();
            recoverBet.search(e);
        });

        $('body').delegate('.undo', 'click', function (e) {
            e.stopPropagation();
            recoverBet.actionRecover($(this).attr('betId'));
        });
    },

    validData: function () {
        $('#msg-box').hide();
        if ($('#txtBetId').val() == '') {
            $('#msg').html(tooltip.show('txtBetId', 'Bet ID is required.')).show();
            return false;
        }

        if (isNaN($('#txtBetId').val())) {
            $('#msg').html(tooltip.show('txtBetId', 'Invalid Bet ID.')).show();
            return false;
        }
        return true;
    },

    search: function (e) {
        if (!recoverBet.validData(e)) {
            return;
        }

        var param = {
            TransId: $('#txtBetId').val()
        };

        $.ajax({
            type: "POST",
            url: window.rootUrl + "SportBook/RecoverBet/GetTicketRecover",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            beforeSend: function () {
                NapTicketControl.disableInput();
            },
            success: function (rs) {
                if (rs != null && rs.ErrorCode > 0) {
                    $('#msg').html(tooltip.show('txtBetId', rs.ErrorMsg)).show();
                    helper.displayContent(false);
                }
                else {
                    recoverBet.loadMatchInfo(rs.Match);
                    recoverBet.loadSingleTicket(rs.Tickets);
                }
            },
            error: function (e) {},
            complete: function (e) {
                if(e.status == 403){
                    NapTicketControl.Helper.showMsgBox(NapTicketControl.Helper.authorizedMsg);
                }
                NapTicketControl.enableInput();
            }
        });
    },

    loadMatchInfo: function (matchInfo) {
         if (matchInfo != null) {
            helper.displayContent(true);
        }

        var rows = [];
        rows.push({
            Columns: [{ Name: "code", Value: matchInfo.MatchCode, CssClass: 'bold' },
            { Name: "matchEvent", Value: matchInfo.MatchEvent, CssClass: 'bold' },
            { Name: "eventDate", Value: matchInfo.EventDate, CssClass: 'bold' }
            ]
        });

        var gridOptions = {
            columns: [
                { HeaderText: "Code", Name: "code", Width: 100, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Match Event", Name: "matchEvent", Width: 300, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Event Date", Name: "eventDate", Width: 100, HeaderAlign: "center", CellAlign: "center" }
            ],
            bodyRows: rows,
            hideYBackGroundScroller: true,
        };
        $('#matchInfo').fanexGrid(gridOptions);
    },

    loadSingleTicket: function (list) {
        var rows = [];
        list = list.Tickets;
        var count = list.length;
        for (var index = 0; index < count; index++) {
            var currentRow = list[index];
            rows.push({
                Columns: [
                    { Name: "No", Value: currentRow.No },
                    { Name: "BetId", Value: currentRow.BetId },
                    { Name: "Player", Value: currentRow.Player },
                    { Name: "TransDate", Value: currentRow.TransDate },
                    { Name: "BetTypeName", Value: currentRow.BetTypeName },
                    { Name: "Odds", Value: currentRow.Odds },
                    { Name: "Stake", Value: currentRow.Stake },
                    { Name: "CurrencyName", Value: currentRow.CurrencyName },
                    { Name: "Status", Value: currentRow.Status },
                    { Name: "Choice", Value: currentRow.Choice },
                    { Name: "CustomAdd", Value: currentRow.BetId, TdClass: "Demo", CssClass: "demo2" }
                ],
                CssClass: "DemoCss"
            });
        };

        var gridOptions = {
            columns: [
                { HeaderText: "No", Name: "No", Width: 20, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "BetID", Name: "BetId", Width: 100, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Member", Name: "Player", Width: 165, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Trans.Date", Name: "TransDate", Width: 135, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Bettype", Name: "BetTypeName", Width: 140, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Odds", Name: "Odds", Width: 80, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Stake", Name: "Stake", Width: 60, HeaderAlign: "center", CellAlign: "center", FormatNumber: "0,0.00" },
                { HeaderText: "Currency", Name: "CurrencyName", Width: 60, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Status", Name: "Status", Width: 60, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Choice", Name: "Choice", Width: 200, HeaderAlign: "center", CellAlign: "right" },
             { HeaderText: "<div><span>Recover</span></div>", Name: "CustomAdd", Width: 80, HeaderAlign: "center", CellAlign: "center", CustomCellRender: recoverBet.optionCustomRender }
            ],
            bodyRows: rows,
            hideYBackGroundScroller: true,
        };

         if (count >0) {
            Message.Helper.displayFormMsg(true);
            Message.Helper.clearMsgContent();
        }
        else
        {
            helper.displayContent(true);
            Message.Helper.displayFormMsg(false);
        }

        $('#ticketInfo').fanexGrid(gridOptions);
    },

    optionCustomRender: function (cell) {
        return "<div betid='" + cell.Value + "' class='undo'></div>";
    },

    actionRecover: function (betId) {
        if (!helper.checkParamBeforRecoverBet()) {
            return;
        }

        if (!confirm('Are you sure you want to recover transaction ' + betId + ' ?')) {
            return;
        }

        var param = {
            TransId: betId,
            Message: {
                Content_en: $('#msg-en').val(),
                Content_ch: $('#msg-ch').val(),
                Content_cs: $('#msg-cs').val(),
                Content_th: $('#msg-th').val(),
                Content_it: $('#msg-it').val(),
                Content_jp: $('#msg-jp').val(),
                Content_es: $('#msg-es').val(),
                Content_ko: $('#msg-ko').val()
            }
        };
        $.ajax({
            type: "POST",
            url: window.rootUrl + "SportBook/RecoverBet/RecoverBet",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            beforeSend: function () {
                NapTicketControl.disableInput();
            },
            success: function (rs) {
                if(rs.ErrorCode < 0) // Warning message
                {
                    helper.displayContent(false);
                    helper.resetInput();
                    NapTicketControl.Helper.showWarningBox(rs);
                }
                else
                {
                    if (rs.ErrorCode > 0) {
                        helper.displayContent(true);
                    }
                    else
                    {
                        helper.displayContent(false);
                        helper.resetInput();
                    }
                    NapTicketControl.Helper.showMsgBox(rs);
                }
            },
            error: function (e) {},
            complete: function (e) {
                if(e.status == 403){
                    NapTicketControl.Helper.showMsgBox(NapTicketControl.Helper.authorizedMsg);
                }
                NapTicketControl.enableInput();
            }
        });
    }
};