/* File Created: February 5, 2013 */

var voidByBetId = {
    search: function() {
        $('#msg-box').hide();
        $('#msg').hide();

        //Load Match Info
        if (validateData.checkParamBeforeGetData()) {
            voidByBetId.getTicket();
        }
    },
    limitDate : function (target, limit) { // limit: mindate/maxdate
        var _limit = target.attr(limit);

        if (_limit) { // if limit attribute exists
            _limit = _limit.split("/");

            var year = _limit[2],
					month = _limit[0],
					day = _limit[1];

            return new Date(year, month - 1, day);
        }

        return null;
    },
    displayContent: function(isShow) {
        if (isShow) {
            $('#report-content').show();
        } else {
            $('#report-content').hide();
        }
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
            return this.value;
        }).get().join(',');

        return checkedVals;
    },
    getTicket: function() {
        var param = {
            TransID: $('#txtBetId').val(),
            Username: $('#txtUsername').val(),
            FromDate: $('#FromDate').val(),
            ToDate: $('#ToDate').val()
        };

        $.ajax({
            type: "POST",
            url: window.rootUrl + "SportBook/VoidByBetId/GetTicket",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            beforeSend: function() {
                NapTicketControl.disableInput();
            },
            success: function(rs) {
                // No permission.
                if (rs.Result.ErrorCode == 403) {
                    NapTicketControl.Helper.showMsgBox(NapTicketControl.Helper.authorizedMsg);
                } else {
                    voidByBetId.loadSingleTicket(rs.Tickets);
                    Message.Helper.clearMsgContent();
                }
            },
            error: function(e) {
            },
            complete: function(e) {
                if (e.status == 403) {
                    NapTicketControl.Helper.showMsgBox(NapTicketControl.Helper.authorizedMsg);
                }
                NapTicketControl.enableInput();
            }
        });
    },

    loadSingleTicket: function(list) {
        var rows = [];
        list = list.Tickets;
        voidByBetId.displayContent(true);

        for (var index = 0; index < list.length; index++) {
            var currentRow = list[index];
            rows.push({
                Columns: [
                    { Name: "CustomAdd", Value: currentRow.BetId},
                    { Name: "No", Value: currentRow.No },
                    { Name: "BetId", Value: currentRow.BetId },
                    { Name: "Player", Value: currentRow.Player },
                    { Name: "TransDate", Value: currentRow.TransDate },
                    { Name: "Odds", Value: currentRow.Odds },
                    { Name: "Stake", Value: currentRow.Stake },
                    { Name: "Winloss", Value: currentRow.WinLost },
                    { Name: "Status", Value: currentRow.Status },
                    { Name: "Choice", Value: currentRow.Choice }
                ],
                CssClass: "DemoCss"
            });
        }
        ;

        var gridOptions = {
            columns: [
                { HeaderText: "<div class='option-all'><input class='option' id='chk-ticket-all' type='checkbox' checked='checked'/></div>", Name: "CustomAdd", Width: 30, HeaderAlign: "center", CellAlign: "center", CustomCellRender: voidByBetId.optionCustomRender },
                { HeaderText: "No", Name: "No", Width: 40, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "BetID", Name: "BetId", Width: 98, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Member", Name: "Player", Width: 165, HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: "Trans.Date", Name: "TransDate", Width: 135, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Odds", Name: "Odds", Width: 80, HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: "Stake", Name: "Stake", Width: 60, HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                { HeaderText: "Winloss", Name: "Winloss", Width: 80, HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: "Status", Name: "Status", Width: 80, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Choice", Name: "Choice", Width: 350, HeaderAlign: "center", CellAlign: "right" }
            ],
            bodyRows: rows,
            showPager: false,
            hideYBackGroundScroller: true
        };

        $('#ticketInfo').fanexGrid(gridOptions);

        if (list.length == 0) {
            Message.Helper.displayFormMsg(false);
        } else {
            Message.Helper.displayFormMsg(true);
        }
    },

    optionCustomRender: function(cell) {
        return "<input class='chk-ticket' value='" + cell.Value + "' type='checkbox' checked='checked'/>";
    },

    voidTicket: function() {
        if (!validateData.checkParamBeforeVoidTicket()) {
            return;
        }

        var param = {
            Username :$('#txtUsername').val(),
            FromDate: $('#FromDate').val(),
            ToDate: $('#ToDate').val(),
            TransIds: voidByBetId.getSubTicketsChecked(),
            UserPwd: $('#password').val(),
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
            url: window.rootUrl + "SportBook/VoidByBetId/VoidTicket",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            beforeSend: function() {
                NapTicketControl.disableInput();
            },
            success: function(rs) {
                NapTicketControl.Helper.showMsgBox(rs);
                if (rs != null && rs.ErrorCode > 0) {
                    voidByBetId.displayContent(true);
                } else {
                    voidByBetId.displayContent(false);
                    $('#txtBetId').val('');
                    $('#txtUsername').val('');
                }
            },
            error: function(e) {
            },
            complete: function(e) {
                if (e.status == 403) {
                    NapTicketControl.Helper.showMsgBox(NapTicketControl.Helper.authorizedMsg);
                }
                NapTicketControl.enableInput();
            }
        });
    },
};

function VoidByBetIdTicket() {
    this.DOMElement =
    {
        $username: null,
        $betId: null,
        $body: null
    };

    this.OldDataRequest = null;
};

VoidByBetIdTicket.prototype.CacheDOMElement = function() {
    var self = this;
    self.DOMElement.$body = $('#body');
    self.DOMElement.$username = $('#chkUsername');
    self.DOMElement.$betId = $('#chkBetId');
};

VoidByBetIdTicket.prototype.Init = function () {
    jQuery("#VoidByBetIdForm").validationEngine();

    var self = this;

    self.CacheDOMElement();
    self.SetupCalendar('', '#FromDate');
    self.SetupCalendar('', '#ToDate');
    $('.txtCalendar').attr('disabled', 'disabled');

    // handle events from DOM element

    // enable/disable Trans Date
    self.DOMElement.$username.click(function () {
        if ($(this).prop('checked')) {
            // fill in current date
            var today = new Date();
            $('#txtUsername').removeAttr('disabled');
            $('.txtCalendar').removeAttr('disabled');
            $('#txtBetId').val('').attr('disabled', 'disabled');
            self.SetupCalendar(dateFormat(today, "mm/dd/yyyy"), '#FromDate');
            self.SetupCalendar(dateFormat(today, "mm/dd/yyyy"), '#ToDate');
        }
    });
    self.DOMElement.$betId.click(function() {
        if ($(this).prop('checked')) {
            // clear text box
            if ($('#txtUsername').val() != "Input a username") {
                $('#txtUsername').val('').attr('disabled', 'disabled');
            } else {
                $('#txtUsername').attr('disabled', 'disabled');
            }
            $('.txtCalendar').val('').attr('disabled', 'disabled');
            $('#txtBetId').removeAttr('disabled');
        }
    });

    $('.icon-from-transdate-calendar').click(function () {
        if ($('#chkUsername').prop('checked')) {
            $('#FromDate').datetimepicker("show");
        } else {
            // clear text box
            $('.txtCalendar').val('').attr('disabled', 'disabled');
            $('#FromDate').datetimepicker("hide");
        }
    });

    $('.icon-to-transdate-calendar').click(function () {
        if ($('#chkUsername').prop('checked')) {
            $('#ToDate').datetimepicker("show");
        } else {
            // clear text box
            $('.txtCalendar').val('').attr('disabled', 'disabled');
            $('#ToDate').datetimepicker("hide");
        }
    });

     // Event tick option on list.
    $('body').delegate('.chk-ticket', 'click', function () {
        voidByBetId.eventSubTicket();
    });

    // Event tick all option.
    $('body').delegate('#chk-ticket-all', 'click', function () {
        voidByBetId.setStausAllCheckTicket();
    });
    $('#btnSearch').click(function (e) {
        e.stopPropagation();
        voidByBetId.search();
        return false;
    });

    $('#btnVoidBet').click(function (e) {
        e.stopPropagation();
        voidByBetId.voidTicket();
        return false;
    });

    $("#txtBetId").keyup(function () { }).ForceIntegerOnly();
};

VoidByBetIdTicket.prototype.SetupCalendar = function(strdatetime, txtdate) {
    $(txtdate).datepicker({
        dateFormat: 'mm/dd/yy',
        minDate: voidByBetId.limitDate($(txtdate), "mindate"),
        maxDate: voidByBetId.limitDate($(txtdate), "maxdate"),
    });

    if (strdatetime != null && strdatetime != "") {
        $(txtdate).val(strdatetime);
    }
};
// A jQuery based placeholder polyfill
$(document).ready(function(){
  function add() {
    if($(this).val() === ''){
      $(this).val($(this).attr('placeholder')).addClass('placeholder');
    }
  }

  function remove() {
    if($(this).val() === $(this).attr('placeholder')){
      $(this).val('').removeClass('placeholder');
    }
  }

  // Create a dummy element for feature detection
  if (!('placeholder' in $('<input>')[0])) {
    // Select the elements that have a placeholder attribute
    $('input[placeholder], textarea[placeholder]').blur(add).focus(remove).each(add);

    // Remove the placeholder text before the form is submitted
    $('form').submit(function(){
      $(this).find('input[placeholder], textarea[placeholder]').each(remove);
    });
  }
});