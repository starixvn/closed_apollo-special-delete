/* File Created: March 12, 2013 */

$(function () {
    outright.init();
});

var helper = {
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
            return this.value;
        }).get().join(',');

        return checkedVals;
    },

    displayContent: function (isShow) {
        if (isShow) {
            $('#report-content').show();
        }
        else {
            $('#report-content').hide();
        }
    },

    resetInput: function () {
        $('#fromRefno').val('');
        $('#toRefno').val('');
        $('#password').val('');
        $('.msg-void-ticket').val('');
    }
};

var outright = {
    init: function () {
        jQuery("#VoidOutrightBetForm").validationEngine();

        $("#fromRefno").keyup(function () { }).ForceIntegerOnly();
        $("#toRefno").keyup(function () { }).ForceIntegerOnly();

        $('#btnSearch').click(function (e) {
            e.stopPropagation();
            outright.search(e);
            return false;
        });

        $('.toggle-filter-bar').click(function () {
            NapTicketControl.Helper.ToggleFilterBar(this);
        });

        // Event tick option on list.
        $('body').delegate('.chk-ticket', 'click', function () {
            helper.eventSubTicket();
        });

        // Event tick all option.
        $('body').delegate('#chk-ticket-all', 'click', function () {
            helper.setStausAllCheckTicket();
        });

        $('#btnVoidBet').click(function (e) {
            e.stopPropagation();
            outright.voidOutrightBet()
            return false;
        });

        outright.loadLeague();
        outright.loadTeamByLeague($('#ddlLeague').val());
    },

    loadLeague: function () {
        $.ajax({
            type: "POST",
            url: window.rootUrl + "SportBook/VoidOutrightBet/GetOutrightLeague",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            beforeSend: function () {
                NapTicketControl.disableInput();
            },
            success: function (rs) {
                var league = [];

                for (var i = 0; i < rs.length; i++) {
                    league.push('<option value="' + rs[i].Id + '">' + rs[i].Name + '</option>');
                }

                $('#ddlLeague').empty().append(league.join(''));
            },
            error: function () {
                //                console.log('has error :(');
            },
            complete: function () {
                NapTicketControl.enableInput();
            }
        });
    },

    loadTeamByLeague: function (Id) {
        var param = {
            LeagueId: Id
        };

        $.ajax({
            type: "POST",
            url: window.rootUrl + "SportBook/VoidOutrightBet/GetTeamByLeague",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            beforeSend: function () {
                NapTicketControl.disableInput();
            },
            success: function (rs) {
                var league = [];

                for (var i = 0; i < rs.length; i++) {
                    league.push('<option value="' + rs[i].Id + '">' + rs[i].Name + '</option>');
                }

                $('#ddlTeam').empty().append(league.join(''));
            },
            error: function () {
                //                console.log('has error :(');
            },
            complete: function () {
                NapTicketControl.enableInput();
            }
        });
    },

    search: function () {
        if (!validateData.checkParamBeforeGetData()) {
            return;
        }

        outright.getOutrightBet();
    },

    getOutrightBet: function () {
        $('#msg-box').hide();

        var param = {
            OutrightID: $('#ddlTeam').val(),
            FromRefNo: $('#fromRefno').val(),
            ToRefNo: $('#toRefno').val()
        };

        $.ajax({
            type: "POST",
            url: window.rootUrl + "SportBook/VoidOutrightBet/GetOutrightBet",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            // async: false,
            data: JSON.stringify(param),
            beforeSend: function () {
                NapTicketControl.disableInput();
            },
            success: function (rs) {
                //render match info
                helper.displayContent(true);
                outright.loadOutrightBet(rs.Tickets);
                Message.Helper.clearMsgContent();
            },
            error: function (e) { },
            complete: function (e) {
                if (e.status == 403) {
                    NapTicketControl.Helper.showMsgBox(NapTicketControl.Helper.authorizedMsg);
                }
                NapTicketControl.enableInput();
            }
        });
    },

    loadOutrightBet: function (list) {
        var rows = [];
        list = list.Tickets;
        for (var index = 0; index < list.length; index++) {
            var currentRow = list[index];
            rows.push({
                Columns: [
                    { Name: "CustomAdd", Value: currentRow.BetId },
                    { Name: "No", Value: currentRow.No },
                    { Name: "BetId", Value: currentRow.BetId },
                    { Name: "Player", Value: currentRow.Player },
                    { Name: "TransDate", Value: currentRow.TransDate },
                    { Name: "Odds", Value: currentRow.Odds },
                    { Name: "Stake", Value: currentRow.Stake },
                    { Name: "CurrencyName", Value: currentRow.CurrencyName },
                    { Name: "Status", Value: currentRow.Status }

                ],
                CssClass: "DemoCss"
            });
        };

        var gridOptions = {
            columns: [
                { HeaderText: "<div class='option-all'><input class='option' id='chk-ticket-all' type='checkbox' checked='checked'/></div>", Name: "CustomAdd", Width: 30, HeaderAlign: "center", CellAlign: "center", CustomCellRender: outright.optionCustomRender },
                { HeaderText: "No", Name: "No", Width: 20, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "BetID", Name: "BetId", Width: 100, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Member", Name: "Player", Width: 165, HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: "Trans.Date", Name: "TransDate", Width: 135, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Odds", Name: "Odds", Width: 60, HeaderAlign: "center", CellAlign: "center", FormatNumber: "0,0.00" },
                { HeaderText: "Stake", Name: "Stake", Width: 60, HeaderAlign: "center", CellAlign: "center", FormatNumber: "0,0.00" },
                { HeaderText: "Currency", Name: "CurrencyName", Width: 60, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Status", Name: "Status", Width: 60, HeaderAlign: "center", CellAlign: "center" }
            ],
            bodyRows: rows,
            showPager: true,
            hideYBackGroundScroller: true,
            pagerOption: {
                itemsPerPage: 50,
                currentPage: 0,
                itemsPerPageArray: [50, 100, 150, 500],
                afterPagedFunction: function () {
                    $('#chk-ticket-all').attr('checked', 'checked');
                }
            }
        };

        $('#ticketInfo').fanexGrid(gridOptions);

        if (list.length == 0) {
            Message.Helper.displayFormMsg(false);
        }
        else {
            Message.Helper.displayFormMsg(true);
        }
    },

    optionCustomRender: function (cell) {
        return "<input class='chk-ticket' value='" + cell.Value + "' type='checkbox' checked='checked'/>";
    },

    voidOutrightBet: function () {
        if (!validateData.checkParamBeforeVoidTicket()) {
            return;
        }

        var param = {
            MatchId: $('#ddlTeam').val(),
            FromTransId: $('#fromRefno').val(),
            ToTransId: $('#toRefno').val(),
            Bettypes: '10',
            TransIds: helper.getSubTicketsChecked(),
            UserPwd: $('#password').val(),
            UserName: $('#username').val(),
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
            url: window.rootUrl + "SportBook/VoidOutrightBet/VoidOutrightBet",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            beforeSend: function () {
                NapTicketControl.disableInput();
            },
            success: function (rs) {
                NapTicketControl.Helper.showMsgBox(rs);
                if (rs != null && rs.ErrorCode > 0) {
                    helper.displayContent(true);
                }
                else {
                    helper.displayContent(false);
                    helper.resetInput();
                }
            },
            error: function (e) { },
            complete: function (e) {
                if (e.status == 403) {
                    NapTicketControl.Helper.showMsgBox(NapTicketControl.Helper.authorizedMsg);
                }
                NapTicketControl.enableInput();
            }
        });
    }
}