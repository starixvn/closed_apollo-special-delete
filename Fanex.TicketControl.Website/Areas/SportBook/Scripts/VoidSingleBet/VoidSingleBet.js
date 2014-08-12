/*global NapTicketControl, tooltip, ContextMenu, dateFormat, validateData, Message, helper*/
"use strict";

var singleDetete = {
    param: {
        MatchId: '',
        FromRefno: '',
        ToRefno: ''
    },

    search: function () {
        $('#msg-box').hide();
        $('#msg').hide();

        //Load Match Info
        if (validateData.checkParamBeforeGetData()) {
            singleDetete.checkMatchIdWithRefno();
        }
    },

    getSingleBet: function () {
        var param = {
            MatchId: singleDetete.param.MatchId,
            FromRefNo: singleDetete.param.FromRefno,
            ToRefNo: singleDetete.param.ToRefno,
            Hdp: $('#hdp').val() == '' ? '9999' : $('#hdp').val(),
            LiveIndicator: $('#EventType').val(),
            LiveHomeScore: helper.GetLiveScore().LiveHomeScore,
            LiveAwayScore: helper.GetLiveScore().LiveAwayScore,
            FromTransdate: helper.GetLiveScore().FromTransdate,
            ToTransdate: helper.GetLiveScore().ToTransdate,
            Bettypes: helper.GetBettypesChecked(),
            Betteam1378: $('#ddlBetTeam1378').val(),
            Betteam515: helper.GetBetteam515(),
            Betteam430: helper.GetBetteam430(),
            Betteam6: NapTicketControl.Helper.GetBetteam6(),
            Betteam14: NapTicketControl.Helper.GetBetteam14(),
            Betteam16: helper.GetBetteam16(),
            SAFilter: $('#TicketType').val()
        };

        $.ajax({
            type: "POST",
            url: window.rootUrl + "SportBook/VoidSingleBet/GetSingleBet",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            beforeSend: function () {
                NapTicketControl.disableInput();
            },
            success: function (rs) {
                // No permission.
                if (rs.Result.ErrorCode == 403) {
                    NapTicketControl.Helper.showMsgBox(NapTicketControl.Helper.authorizedMsg);
                } else {
                    singleDetete.loadMatchInfo(rs.Match);
                    singleDetete.loadSingleTicket(rs.Tickets);

                    Message.Helper.clearMsgContent();
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
    },

    loadMatchInfo: function (matchInfo) {
        if (matchInfo != null) {
            helper.displayContent(true);
        }

        var rows = [];
        rows.push({
            Columns: [{ Name: "code", Value: matchInfo.MatchCode, CssClass: 'bold' },
            { Name: "matchEvent", Value: (matchInfo.HomeName + ' -vs- ' + matchInfo.AwayName), CssClass: 'bold' },
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
            hideYBackGroundScroller: true
        };

        $('#matchInfo').fanexGrid(gridOptions);
    },

    loadSingleTicket: function (list) {
        var rows = [];
        list = list.Tickets;
        if (list.length > 0) {
            helper.displayContent(true);
        }

        for (var index = 0; index < list.length; index++) {
            var currentRow = list[index];
            rows.push({
                Columns: [
                    { Name: "CustomAdd", Value: currentRow.BetId },
                    { Name: "No", Value: currentRow.No },
                    { Name: "BetId", Value: currentRow.BetId },
                    { Name: "Player", Value: currentRow.Player },
                    { Name: "TransDate", Value: currentRow.TransDate },
                    { Name: "BetTypeName", Value: currentRow.BetTypeName },
                    { Name: "Odds", Value: currentRow.Odds },
                    { Name: "Stake", Value: currentRow.Stake },
                    { Name: "CurrencyName", Value: currentRow.CurrencyName },
                    { Name: "Status", Value: currentRow.Status },
                    { Name: "SA", Value: currentRow.SecondAcceptFlag },
                    { Name: "Choice", Value: currentRow.Choice }

                ],
                CssClass: "DemoCss"
            });
        };

        var gridOptions = {
            columns: [
                { HeaderText: "<div class='option-all'><input class='option' id='chk-ticket-all' type='checkbox' checked='checked'/></div>", Name: "CustomAdd", Width: 30, HeaderAlign: "center", CellAlign: "center", CustomCellRender: singleDetete.optionCustomRender },
                { HeaderText: "No", Name: "No", Width: 30, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "BetID", Name: "BetId", Width: 98, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Member", Name: "Player", Width: 160, HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: "Trans.Date", Name: "TransDate", Width: 135, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Bettype", Name: "BetTypeName", Width: 140, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Odds", Name: "Odds", Width: 80, HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: "Stake", Name: "Stake", Width: 60, HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                { HeaderText: "Currency", Name: "CurrencyName", Width: 60, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Status", Name: "Status", Width: 70, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "S.A", Name: "SA", Width: 40, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Choice", Name: "Choice", Width: 250, HeaderAlign: "center", CellAlign: "right" }
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

    voidSingleBet: function () {
        if (!validateData.checkParamBeforeVoidTicket()) {
            return;
        }

        var param = {
            MatchId: $('#txtMatchId').val(),
            FromTransId: $('#fromRefno').val(),
            ToTransId: $('#toRefno').val(),
            Hdp: $('#hdp').val() == '' ? '9999' : $('#hdp').val(),
            LiveIndicator: $('#EventType').val(),
            LiveHomeScore: $('#LiveHomeScore').val(),
            LiveAwayScore: $('#LiveAwayScore').val(),
            FromTransdate: $('#FromTransDate').val(),
            ToTransdate: $('#ToTransDate').val(),
            Bettypes: helper.GetBettypesChecked(),
            Betteam1378: $('#ddlBetTeam1378').val(),
            Betteam515: helper.GetBetteam515(),
            Betteam430: helper.GetBetteam430(),
            Betteam6: NapTicketControl.Helper.GetBetteam6(),
            Betteam14: NapTicketControl.Helper.GetBetteam14(),
            Betteam16: helper.GetBetteam16(),
            SAFilter: $('#TicketType').val(),
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
            url: window.rootUrl + "SportBook/VoidSingleBet/VoidBet",
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
    },

    checkMatchIdWithRefno: function () {
        var param = {
            MatchId: $('#txtMatchId').val(),
            FromRefno: $('#fromRefno').val(),
            ToRefno: $('#toRefno').val()
        };

        $.ajax({
            type: "POST",
            url: window.rootUrl + "SportBook/VoidSingleBet/CheckMatchIdWithRefno",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            data: JSON.stringify(param),
            beforeSend: function () {
                //  NapTicketControl.disableInput();
            },
            success: function (rs) {
                if (rs.ErrorCode == 0) {
                    singleDetete.param = param;
                    singleDetete.getSingleBet();
                    helper.displayContent(true);
                }
                else {
                    switch (rs.ErrorCode) {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            $('#msg').html(tooltip.show('txtMatchId', rs.ErrorMsg)).show();
                            break;
                        case 5:
                            $('#msg').html(tooltip.show('fromRefno', rs.ErrorMsg)).show();
                            break;
                        case 6:
                            $('#msg').html(tooltip.show('toRefno', rs.ErrorMsg)).show();
                            break;
                    }

                    helper.displayContent(false);
                }
            },
            error: function () {
                //                    console.log('has error :(');
            },
            complete: function () {
                // NapTicketControl.enableInput();
            }
        });
    }
};

function DelSingleTicket() {
    this.DOMElement =
    {
        $ddlLiccensee: null,
        $ddlEventType: null,
        $product: null,
        $bettypeWrp: null,
        $chkTransDateRange: null,
        $txtCalendar: null,
        $optFilters: null,
        $eventTypeWrp: null,
        $body: null
    };

    this.OldDataRequest = null;
};

DelSingleTicket.prototype.Mode = {
    Sportsbook: 0
};

DelSingleTicket.prototype.CacheDOMElement = function () {
    var self = this;

    self.DOMElement.$ddlLiccensee = $('.ddlLicensee');
    self.DOMElement.$ddlEventType = $('.ddlEventType');
    self.DOMElement.$product = $('.product');
    self.DOMElement.$bettypeWrp = $('.bettype-wrp');
    self.DOMElement.$chkTransDateRange = $('.chkTransDateRange');
    self.DOMElement.$txtCalendar = $('.txtCalendar');
    self.DOMElement.$optFilters = $('.opt-filters');
    self.DOMElement.$eventTypeWrp = $('.event-type-wrp');
    self.DOMElement.$body = $('#body');
}

DelSingleTicket.prototype.Init = function () {
    jQuery("#DeleteSingleTicketForm").validationEngine();

    var self = this;

    self.CacheDOMElement();
    NapTicketControl.Helper.GetBettypes('Sportsbook');
    self.SetupCalendar('', '.txtCalendar');
    $('.txtCalendar').attr('disabled', 'disabled');

    helper.setStatusAllBettype(false);

    // handle events from DOM element
    // change live type
    self.DOMElement.$ddlEventType.change(function () {
        var eventType = $(this).val(); // live type drop down list
        helper.checkConstraintBettyeByEventTypeForAdvanceMode();
        if (eventType === '1') {
            self.DOMElement.$optFilters.show();
        } else {
            self.DOMElement.$optFilters.hide();
        }
    });

    // enable/disable Trans Date
    self.DOMElement.$chkTransDateRange.click(function () {
        if ($(this).prop('checked')) {
            // fill in current date
            var today = new Date();
            $('.txtCalendar').removeAttr('disabled');
            self.SetupCalendar(dateFormat(today), '.txtCalendar');
        } else {
            // clear text box
            $('.txtCalendar').val('').attr('disabled', 'disabled');
        }
    });

    $('.icon-from-transdate-calendar').click(function () {
        if ($('#TransDate').prop('checked')) {
            $('#FromTransDate').datetimepicker("show");
        } else {
            // clear text box
            $('.txtCalendar').val('').attr('disabled', 'disabled');
            $('#FromTransDate').datetimepicker("hide");
        }
    });

    $('.icon-to-transdate-calendar').click(function () {
        if ($('#TransDate').prop('checked')) {
            $('#ToTransDate').datetimepicker("show");
        } else {
            // clear text box
            $('.txtCalendar').val('').attr('disabled', 'disabled');
            $('#ToTransDate').datetimepicker("hide");
        }
    });

    $('.fieldset-advance-mode legend').click(function () {
        helper.toggleAdvancedMode(this);
    });

    $('.toggle-filter-bar').click(function () {
        NapTicketControl.Helper.ToggleFilterBar(this);
    });

    NapTicketControl.Helper.initAdvancedModeTotalGoal();
    // check all bettypes
    $('body').delegate('#chkallbettype', 'click', function (e) {
        e.stopPropagation();

        if ($('#chkallbettype').prop('checked')) {
            $('.chkbettype').attr('checked', 'checked')
            helper.checkConstraintBettyeByEventTypeForAdvanceMode();
            helper.expandAdvancedMode();
        } else {
            $('.chkbettype').removeAttr('checked');
            helper.setStatusAllAdvanceCS(false, false);
            NapTicketControl.Helper.setStatusAllAdvanceTG(false, false);
            NapTicketControl.Helper.setStatusAllAdvance1HTG(false, false);
            NapTicketControl.Helper.setStatusAllAdvance1HTGAndTG(false, false);
            helper.setStatusAllAdvanceHDP(false);
            helper.setStatusAllAdvance1X2(false);
            helper.setStatusAllAdvanceHTFT(false, false);
            helper.setStatusAllAdvanceFGLG(false, false);
        }
    });

    $('body').delegate('.chkbettype', 'click', function () {
        var selectedItems = $('.tbl-bettype tbody input:checked').length;
        var totalItems = $('.tbl-bettype tbody input:checkbox').length;

        if (selectedItems === totalItems) {
            $('#chkallbettype').attr('checked', 'checked');
        } else {
            $('#chkallbettype').removeAttr('checked');
        }

        //begin
        helper.checkConstraintBettyeByEventTypeForAdvanceMode();
    });

    //Correct Scores check all
    $('body').delegate('#chk-score-all', 'click', function () {
        helper.setStatusAllAdvanceCS(this.checked, true);
    });

    //Correct Scores uncheck any item
    $('body').delegate('.chk-score', 'click', function () {
        helper.eventSubAdvanceCS();
    });

    //HTFT check all
    $('body').delegate('#chk-htft-all', 'click', function () {
        helper.setStatusAllAdvanceHTFT(this.checked, true);
    });

    //HTFT uncheck any item
    $('body').delegate('.chk-htft', 'click', function () {
        helper.eventSubAdvanceHTFT();
    });

    //FGLG check all
    $('body').delegate('#chk-flg-all', 'click', function () {
        helper.setStatusAllAdvanceFGLG(this.checked, true);
    });

    //FGLG uncheck any item
    $('body').delegate('.chk-flg', 'click', function () {
        helper.eventSubAdvanceFGLG();
    });

    // Event tick option on list.
    $('body').delegate('.chk-ticket', 'click', function () {
        helper.eventSubTicket();
    });

    // Event tick all option.
    $('body').delegate('#chk-ticket-all', 'click', function () {
        helper.setStausAllCheckTicket();
    });

    $('#btnSearch').click(function (e) {
        e.stopPropagation();
        singleDetete.search();
        return false;
    });

    $('#btnVoidBet').click(function (e) {
        e.stopPropagation();
        singleDetete.voidSingleBet();
        return false;
    });

    $("#txtMatchId").keyup(function () { }).ForceIntegerOnly();
    $("#fromRefno").keyup(function () { }).ForceIntegerOnly();
    $("#toRefno").keyup(function () { }).ForceIntegerOnly();
    $("#hdp").keyup(function () { }).ForceDecimalOnly();
    $("#LiveHomeScore").keyup(function () { }).ForceIntegerOnly();
    $("#LiveAwayScore").keyup(function () { }).ForceIntegerOnly();
};

DelSingleTicket.prototype.SetupCalendar = function (strdatetime, txtdate) {
    $(txtdate).datetimepicker({
        showSecond: true,
        dateFormat: 'mm/dd/yy',
        timeFormat: 'HH:mm:ss',
        hourGrid: 3,
        minuteGrid: 5,
        secondGrid: 5,
        separator: ' '
    });

    if (strdatetime != null && strdatetime != "") {
        $(txtdate).val(strdatetime).datetimepicker('setDate', (new Date(strdatetime)));
    }
}