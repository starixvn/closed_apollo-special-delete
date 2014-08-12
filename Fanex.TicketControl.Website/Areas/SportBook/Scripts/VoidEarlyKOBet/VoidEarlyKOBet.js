/*global NapTicketControl, tooltip, ContextMenu, dateFormat, validateData, Message, helper*/

"use strict";

var parlayDelete = {
    param:{
        MatchId: ''
    },

    search: function(){
        $('#msg-box').hide();
        $('#msg').hide();

        //Load Match Info
        if(validateData.checkParamBeforeGetData()){
            parlayDelete.getTicket();
        }
    },

    getTicket: function (){
         var param = {
            MatchId: $('#txtMatchId').val(),
            Hdp: $('#hdp').val() == '' ? '9999' :  $('#hdp').val(),
            SAFilter: 0,
            KickOffDate: $('#KickOffDate').val(),
            Bettype: helper.GetParlayTypeChecked(),
            Bettypes: helper.GetBettypesChecked(),
            Betteam1378: helper.GetBetteam1378(),
            Betteam515: helper.GetBetteam515(),
            Betteam430: helper.GetBetteam430(),
            Betteam6: NapTicketControl.Helper.GetBetteam6(),
            Betteam14: NapTicketControl.Helper.GetBetteam14(),
            Betteam16: helper.GetBetteam16()
         };

         $.ajax({
                type: "POST",
                url: window.rootUrl + "SportBook/VoidEarlyKOBet/GetEarlyKOTicket",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(param),
                beforeSend: function () {
                    NapTicketControl.disableInput();
                },
                success: function (rs) {
                    // Render match info
                    parlayDelete.loadMatchInfo(rs.Match) ;
                    parlayDelete.loadSingleTicket(rs.Tickets);
                    Message.Helper.clearMsgContent();
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

    loadMatchInfo: function(matchInfo){
        if(matchInfo == null)
        {
            $('#matchInfo').hide();
            return;
        }
        NapTicketControl.Helper.displayContent(true);

        var rows = [];
        rows.push({
        Columns: [{ Name: "code", Value: matchInfo.MatchCode, CssClass: 'bold' },
            { Name: "matchEvent", Value: (matchInfo.HomeName + ' -vs- ' + matchInfo.AwayName), CssClass: 'bold'},
            { Name: "eventDate", Value:  matchInfo.EventDate, CssClass: 'bold'}
        ]});

        var gridOptions = {
            columns: [
                { HeaderText: "Code", Name: "code", Width: 100, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Match Event", Name: "matchEvent", Width: 300, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Event Date", Name: "eventDate", Width: 100, HeaderAlign: "center", CellAlign: "center" }
            ],
            hideYBackGroundScroller: true,
            bodyRows: rows
        };

        $('#matchInfo').fanexGrid(gridOptions);
    },

    loadSingleTicket : function(list)
    {
        var rows = [];
       list = list.Tickets;
        for (var index = 0; index < list.length; index++) {
            var currentRow = list[index];
            rows.push({
                Columns:[
                    { Name: "CustomAdd", Value: currentRow.SubTransId},
                    { Name: "No", Value: currentRow.No  },
                    { Name: "BetId", Value: currentRow.BetId },
                    { Name: "Player", Value: currentRow.Player },
                    { Name: "TransDate", Value:currentRow.TransDate  },
                    { Name: "BetTypeName", Value:currentRow.BetTypeName },
                    { Name: "Odds", Value:currentRow.Odds},
                    { Name: "Stake", Value: currentRow.Stake},
                    { Name: "CurrencyName", Value:currentRow.CurrencyName  },
                    { Name: "Status", Value: currentRow.Status },
                    { Name: "Choice", Value:currentRow.Choice }

                ],
                CssClass: "DemoCss"
            });
        };

        var gridOptions = {
            columns: [
                { HeaderText: "<div class='option-all'><input class='option' id='chk-ticket-all' type='checkbox' checked='checked'/></div>", Name: "CustomAdd", Width: 30, HeaderAlign: "center", CellAlign: "center", CustomCellRender: parlayDelete.optionCustomRender },
                { HeaderText: "No", Name: "No", Width: 40, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "BetID", Name: "BetId", Width: 90, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Member", Name: "Player", Width: 165, HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: "Trans.Date", Name: "TransDate", Width: 135, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Bettype", Name: "BetTypeName", Width: 140, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Odds", Name: "Odds", Width: 80, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Stake", Name: "Stake", Width: 60, HeaderAlign: "center", CellAlign: "center", FormatNumber: "0,0.00" },
                { HeaderText: "Currency", Name: "CurrencyName", Width: 60, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Status", Name: "Status", Width: 60, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Choice", Name: "Choice", Width: 250, HeaderAlign: "center", CellAlign: "right" }
            ],
            bodyRows: rows,
            showPager: false,
            isResetScroller: false,
            hideYBackGroundScroller: true,
            pagerOption: {
                itemsPerPage: 50,
                currentPage: 0,
                itemsPerPageArray: [50, 100, 150, 500],
                afterPagedFunction: function()
                {
                    $('#chk-ticket-all').attr('checked', 'checked');
                }
            }
        };

        $('#ticketInfo').fanexGrid(gridOptions);

       if (list.length == 0) {
            Message.Helper.displayFormMsg(false);
        }
        else{
            Message.Helper.displayFormMsg(true);
        }
    },

    optionCustomRender: function (cell)
    {
        return "<input class='chk-ticket' value='"+ cell.Value +"' type='checkbox' checked='checked'/>";
    },

    voidSingleBet: function (){
        if(!validateData.checkParamBeforVoidTicket()){
            return;
        }

        var param = {
            MatchId: $('#txtMatchId').val(),
            SAFilter: 0,
            Hdp: $('#hdp').val() == '' ? '9999' :  $('#hdp').val(),
            KickOffDate: $('#KickOffDate').val(),
            Bettypes: helper.GetBettypesChecked(),
            Betteam1378: helper.GetBetteam1378(),
            Betteam515: helper.GetBetteam515(),
            Betteam430: helper.GetBetteam430(),
            Betteam6: NapTicketControl.Helper.GetBetteam6(),
            Betteam14: NapTicketControl.Helper.GetBetteam14(),
            Betteam16: helper.GetBetteam16(),
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
                url: window.rootUrl + "SportBook/VoidEarlyKOBet/VoidEarlyKOBet",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(param),
                beforeSend: function () {
                    NapTicketControl.disableInput();
                },
                success: function (rs) {
                    // Render match info
                    NapTicketControl.Helper.showMsgBox(rs);
                    if(rs.ErrorCode==0)
                    {
                        NapTicketControl.Helper.displayContent(false);
                        helper.resetInput();
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

function VoidEarlyKOBet() {
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
        $body: null,
    };

    this.OldDataRequest = null;
};

VoidEarlyKOBet.prototype.Mode = {
    EarlyKO: 4
};

VoidEarlyKOBet.prototype.CacheDOMElement = function () {
    var self = this;

    self.DOMElement.$ddlLiccensee = $('.ddlLicensee');
    self.DOMElement.$ddlEventType = $('.ddlEventType');
    self.DOMElement.$product = $('.product');
    self.DOMElement.$bettypeWrp = $('.bettype-wrp');
    self.DOMElement.$txtCalendar = $('.txtCalendar');
    self.DOMElement.$optFilters = $('.opt-filters');
    self.DOMElement.$eventTypeWrp = $('.event-type-wrp');
    self.DOMElement.$body = $('#body');
}

VoidEarlyKOBet.prototype.Init = function () {
    jQuery("#VoidEarlyKOBetForm").validationEngine();

    var self = this;

    self.CacheDOMElement();
    NapTicketControl.Helper.GetBettypes('EarlyKO');

    var today = new Date();
    $('.txtCalendar').removeAttr('disabled');
    $('.txtCalendar').css('cursor', 'default');
    self.SetupCalendar(dateFormat(today), '.txtCalendar');

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

   $('.icon-from-transdate-calendar').click(function () {
        $('.txtCalendar').removeAttr('disabled');
        $('.txtCalendar').css('cursor', 'default');
        $('#KickOffDate').datetimepicker("show");
    });

    $('.fieldset-advance-mode legend').click(function(){
        helper.toggleAdvancedMode(this);
    });

    $('.toggle-filter-bar').click(function(){
        NapTicketControl.Helper.ToggleFilterBar(this);
    });

    NapTicketControl.Helper.initAdvancedModeTotalGoal();

    // check all bettypes
    $('body').delegate('#chkallbettype', 'click', function (e) {
        e.stopPropagation();

        if ($('#chkallbettype').prop('checked')) {
            $('.chkbettype').attr('checked', 'checked')
            helper.checkConstraintBettyeByEventTypeForAdvanceMode();
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

    $('#btnSearch').click(function(e){
        e.stopPropagation();
        parlayDelete.search();
        return false;
    });

    $('#btnVoidBet').click(function(e){
        e.stopPropagation();
        parlayDelete.voidSingleBet();
        return false;
    });

    $("#txtMatchId").keyup(function () {}).ForceIntegerOnly();
    $("#hdp").keyup(function () {}).ForceDecimalOnly();
};

VoidEarlyKOBet.prototype.SetupCalendar = function (strdatetime, txtdate) {
    $(txtdate).datetimepicker({
        showSecond: true,
        dateFormat: 'mm/dd/yy',
        timeFormat: 'HH:mm:ss',
        hourGrid: 3,
        minuteGrid: 5,
        secondGrid: 5,
        separator: ' ',
        showButtonPanel: true
    });

    if (strdatetime != null && strdatetime != "") {
        $(txtdate).val(strdatetime).datetimepicker('setDate', (new Date(strdatetime)));
    }
}