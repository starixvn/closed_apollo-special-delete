/*global NapTicketControl: true, ContextMenu, tooltip, helper*/
"use strict";

if (NapTicketControl === undefined) {
    NapTicketControl = {};
}

NapTicketControl.Helper = (function ($) {
    // Config constant batch message.
    var MaxTransIdPerBatch = 100;

    // define some private variables
    var _productTypes = {
        Sportsbook: 0,
        NumberGame: 1,
        LiveCasino: 6,
        TicketListing: 3,
        EarlyKO: 4,
        MixParlay: 2
    };

    // used to store local bettypes,
    // and avoid request to server when they are avaible
    var _betTypes = {
        Sportsbook: null,
        NumberGame: null,
        LiveCasino: null,
        TicketListing: null,
        EarlyKO: null,
        MixParlay: null
    }

    return {
        authorizedMsg: 'You are not authorized on this function.',

        GetLicensees: function () {
            $.ajax({
                type: "POST",
                url: window.rootUrl + "Core/Common/GetLicensees",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: null,
                beforeSend: function () {
                    NapTicketControl.disableInput();
                },
                success: function (licensees) {
                    if (licensees) {
                        NapTicketControl.Helper.RenderLicenseeDropDownList(licensees);
                    }
                },
                error: function () {
                },
                complete: function () {
                    NapTicketControl.enableInput();
                }
            });
        },

        RenderLicenseeDropDownList: function (licensees) {
            var self = this;
            var licenseeLength = licensees.length;
            var $licensee = $('.ddlLicensee');
            var licenseeHtml = '';

            for (var index = 0; index < licenseeLength; index++) {
                var currentLicensee = licensees[index];
                licenseeHtml += '<option value="' + currentLicensee.Id + '">' + currentLicensee.Name + '</option>';
            }

            $licensee.append("<optgroup label='Licensee'></optgroup>");
            $licensee.find('optgroup').append(licenseeHtml);
        },

        GetBettypes: function (product, eventType, isTicketListing) {
            var self = this;
            var mode = product; // determine Mode to transfer to sp on db

            if (isTicketListing === true) {
                mode = "TicketListing";
            }
            // get the list of bet types belong selected product
            if (_betTypes[mode] !== null) {
                var html = self.RenderBettypes(product, eventType, isTicketListing);
                $('.bettype-wrp').html(html);
            }
            else {
                var param = {
                    mode: mode
                };

                $.ajax({
                    type: "POST",
                    url: window.rootUrl + "Core/Common/GetBettypes",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    data: JSON.stringify(param),
                    beforeSend: function () {
                        NapTicketControl.disableInput();
                    },
                    success: function (bettypes) {
                        if (bettypes) {
                            _betTypes[mode] = bettypes;
                            var html = self.RenderBettypes(product, eventType, isTicketListing);
                            $('.bettype-wrp').html(html);
                        }
                    },
                    error: function () {
                    },
                    complete: function () {
                        NapTicketControl.enableInput();
                    }
                });
            }
        },

        // Render Bettypes:
        // product: selected product
        // eventType: only affect when product is Sportsbook (0: dead ball, 1: live ball, 2: all)
        // Note: temperaroly, don't cn
        RenderBettypes: function (product, eventType, isTicketListing) {
            var self = this;

            // NOTE: Warning: If it is tickects listing, will include bettype: Mix Paraly, System Parlay
            if (isTicketListing === true) {
                if (product === "Sportsbook") {
                    var bettypes = $.grep(_betTypes["TicketListing"], function (value) {
                        return value.Type === 0;
                    });
                }

                if (product === "NumberGame") {
                    var bettypes = $.grep(_betTypes["TicketListing"], function (value) {
                        return value.Type === 1;
                    });
                }
            }
            else {
                // get the bettype list from the storage object
                var bettypes = _betTypes[product];
            }

            var deadBallBettypes = $.grep(bettypes, function (value) {
                return value.LiveIndicator === false;
            });
            var liveBallBettypes = $.grep(bettypes, function (value) {
                return value.LiveIndicator === true;
            });

            var html = '<table class="tbl-bettype">';
            html += '<thead><tr><th colspan="5">'
                + '<input type="checkbox" id="chkallbettype" checked="checked" />'
                + '<label for="chkallbettype"><b>All Bet types</b>'
                + '</th></tr></thead>';

            switch (product) {
                //                case 'Sportsbook':
                //                    switch (eventType) {
                //                        case '0':
                //                            // render only dead ball bettypes
                //                            html += this.RenderBettypesDetails(deadBallBettypes, 'BetTypes');
                //                            break;
                //                        case '1':
                //                            // render only live ball bettypes
                //                            html += this.RenderBettypesDetails(liveBallBettypes, 'BetTypes');
                //                            break;
                //                        default:
                //                            // render all bettypes
                //                            html += this.RenderBettypesDetails(bettypes, 'BetTypes');
                //                            break;
                //                    }
                //                    break;
                case 'NumberGame':
                    // render dead ball bettypes
                    html += '<tr class="bettype-group-title"> <td colspan="5">&nbsp;' + 'Dead ball' + '</td></tr>';
                    html += this.RenderBettypesDetails(deadBallBettypes, 'BetTypes', 0);

                    // render live ball bettype
                    html += '<tr class="bettype-group-title"> <td colspan="5">&nbsp;' + 'Live ball' + '</td></tr>';
                    html += this.RenderBettypesDetails(liveBallBettypes, 'LiveBetTypes', deadBallBettypes.length);
                    break;
                default:
                    html += this.RenderBettypesDetails(bettypes, 'BetTypes');
                    break;
            }

            html += '</table>';

            return html;
        },

        RenderBettypesDetails: function (bettypes, nameValue, startPosition) {
            var length = bettypes.length;
            var step = 5;
            var repeatTime = length / step;
            var html = '';

            if (startPosition === undefined) {
                startPosition = 0;
            }

            for (var i = 0; i < repeatTime; i++) {
                var offset = i * step;
                html += '<tr>';
                for (var j = 0; j < step; j++) {
                    if (offset + j < length) {
                        var bt = bettypes[offset + j];
                        var id = 'chk-bettype-' + startPosition;
                        var betname = bt.Name;

                        switch (bt.Id) {
                            case '85':
                                betname = "Next Over/Under";
                                break;
                            case '86':
                                if (bt.LiveIndicator === true) {
                                    betname = "Next Odd/Even";
                                } else {
                                    betname = "FT. Odd/Even";
                                }
                                break;
                            case '90':
                                if (bt.LiveIndicator === true) {
                                    betname += " (Live Game)";
                                } else {
                                    betname += " (Pre Game)";
                                }
                                break;
                        }
                        html += '<td>';
                        html += '<input type="checkbox" checked="checked" class="chkbettype"'
                            + ' id=' + id + ' value=' + bt.Id + ' name=' + nameValue + ' />';
                        html += '<label for="' + id + '">' + betname + '</label>';
                        html += '</td>';
                    } else {
                        html += '<td>&nbsp;</td>';
                    }

                    startPosition++;
                }
                html += '</tr>';
            }

            return html;
        },

        showMsgBox: function (msgs) {
            var msgbox = $('#msg-box');

            if (msgbox.length == 0) {
                msgbox = $(
                    '<div class="alert alert-error" id="msg-box">' +
                        '<button type="button" class="close" data-dismiss="alert">&times;</button>' +
                        '<div class="alert-content">' +
                        '</div>' +
                    '</div>').prependTo('body');
            }

            var html = [];

            if ($.isPlainObject(msgs)) {
                var msg = msgs;
                if (msg.ErrorCode > 0) {//Fail
                    html.push('<span class="fail">' + msg.ErrorMsg + '<i class="icon-warning-sign"></i></span><br/>');
                }
                else {
                    html.push('<span>' + msg.ErrorMsg + '&nbsp;<i class="icon-ok"></i></span><br/>');
                }
            }
            else {
                if ($.isArray(msgs)) {
                    for (var i = 0; i < msgs.length; i++) {
                        var msg = msgs[i];
                        if (msg.ErrorCode > 0) {
                            //Fail
                            html.push('<span class="fail">' + msg.ErrorMsg + '&nbsp;<i class="icon-warning-sign"></i></span><br/>');
                        }
                        else {
                            html.push('<span>' + msg.ErrorMsg + '&nbsp;<i class="icon-ok"></i></span><br/>');
                        }
                    }
                } else {
                    html.push('<span class="fail">' + msgs + '<i class="icon-warning-sign"></i></span><br/>');
                }
            }

            msgbox.attr('class', 'alert alert-success').find('.alert-content').html(html.join(''));
            msgbox.fadeIn();
            msgbox.show();
        },

        showWarningBox: function (msg) {
            var msgbox = $('#msg-box');
            var html = [];

            if (msgbox.length == 0) {
                msgbox = $(
                    '<div class="alert alert-error" id="msg-box">' +
                        '<button type="button" class="close" data-dismiss="alert">&times;</button>' +
                        '<div class="alert-content">' +
                        '</div>' +
                    '</div>').prependTo('body');
            }

            html.push('<span><b>Warning!</b> ' + msg.ErrorMsg + '</span><br/>');
            msgbox.attr('class', 'alert').find('.alert-content').html(html.join(''));
            msgbox.fadeIn();
            msgbox.show();
        },

        displayContent: function (isShow) {
            if (isShow) {
                $('#report-content').show();
            }
            else {
                $('#report-content').hide();
            }
        },

        ToggleFilterBar: function () {
            if ($('.filter-bar-inner').css('display') == 'none') {
                $('.filter-bar-inner').show();
            }
            else {
                $('.filter-bar-inner').hide();
            }
        },

        ChangeNumberItem: function (e) {
            var itemNumberCount = $('.item-number').length;
            if (itemNumberCount >= MaxTransIdPerBatch) {
                return false;
            }

            var code = "";
            var value = $(".text-number").val();
            var KEY_COMMA = 188;
            var KEY_BACKSPACE = 8;
            if (e.which) {
                code = e.which;
            } else if (e.keyCode) {
                code = e.keyCode;
            }

            switch (code) {
                case KEY_COMMA:
                    if (value == "") {
                        return;
                    }
                    NapTicketControl.Helper.AddNumberItem(value);
                    break;
                case KEY_BACKSPACE:
                    if (value == "") {
                        var $this = this;
                        NapTicketControl.Helper.RemoveNumberItemByKeyBackspace($this);
                    }

                    break;
                default:
                    break;
            }
        },

        RederNumberItem: function (value, isNumber) {
            var item = "";
            if (isNumber && value.length < 20) {
                item += "<div class='item-number'><span class='value-number'>";
            }
            else {
                item += "<div class='item-number'><span class='value-number-invalid'>";
            }
            item += value;
            item += "</span><span class='icon-delete'>&nbsp</span></div>";
            return item;
        },

        AddNumberItem: function (number) {
            var isTransIdContainNumberOnly = /^\+?(0|[1-9]\d*)$/.test(number);
            $(".text-number").before(NapTicketControl.Helper.RederNumberItem(number, isTransIdContainNumberOnly));
            $(".text-number").val("");
            $(".icon-delete").mousedown(NapTicketControl.Helper.RemoveNumberItem);
            $(".value-number").disableSelection();
            $(".icon-delete").disableSelection();
        },

        RemoveNumberItem: function () {
            $(this).parent().remove();
        },

        RemoveNumberItemByKeyBackspace: function (number) {
            number.previousSibling.remove();
        },

        InitNumberGroup: function () {
            $(".text-number").keydown(NapTicketControl.Helper.ChangeNumberItem
            ).ForceIntegerOnly();

            $(".number-group").mouseup(function () {
                $(".text-number").focus();
            });

            $(".number-group-disabled").mouseup(function () {
                $(".text-number").focus();
            });

            $("#TransId").bind("paste blur", function (e) {
                setTimeout(function () {
                    var value = $("#TransId").val();
                    var isNumber = /^\+?(0|[1-9]\d*)$/.test(value);
                    var containComma = value.indexOf(',');
                    var transIds = value.split(',');
                    for (index = 0; index < transIds.length; index++) {
                        var transId = $.trim(transIds[index]);
                        var itemNumberCount = $('.item-number').length;
                        if (itemNumberCount >= MaxTransIdPerBatch) {
                            return false;
                        }
                        else {
                            if (transId.length > 0) {
                                NapTicketControl.Helper.AddNumberItem(transId);
                            }
                        }
                    }
                }, 0);
            });
        },

        IsSelectedBettype: function (bettypeId) {
            var checkedVals = $('.chkbettype:checkbox:checked').map(function () {
                return this.value;
            }).get();

            var isSelected = false;

            for (var i = 0; i < checkedVals.length; i++) {
                if (checkedVals[i] == bettypeId) {
                    isSelected = true;
                    break;
                }
            }

            return isSelected;
        },

        initAdvancedModeTotalGoal: function () {
            $('body').delegate('#chk-tg-all', 'click', function () {
                if (NapTicketControl.Helper.IsSelectedBettype(6)) {
                    NapTicketControl.Helper.setStatusAllAdvanceTG(this.checked, true);
                }
                else {
                    NapTicketControl.Helper.setStatusAllAdvanceTG(false, false);
                }

                if (NapTicketControl.Helper.IsSelectedBettype(126)) {
                    NapTicketControl.Helper.setStatusAllAdvance1HTG(this.checked, true);
                }
                else {
                    NapTicketControl.Helper.setStatusAllAdvance1HTG(false, false);
                }

                NapTicketControl.Helper.setStatusAllAdvance1HTGAndTG(this.checked, true);
            });

            $('body').delegate('.chk-tg', 'click', function () {
                NapTicketControl.Helper.eventSubAdvanceTG();
            });

            $('body').delegate('.chk-1h-tg', 'click', function () {
                NapTicketControl.Helper.eventSubAdvanceTG();
            });

            $('body').delegate('.chk-ft-tg', 'click', function () {
                NapTicketControl.Helper.eventSubAdvanceTG();
            });
        },

        setStatusAllAdvanceTG: function (isChecked, isEnable) {
            if (isChecked) {
                $('.chk-ft-tg').attr('checked', 'checked');
            } else {
                $('.chk-ft-tg').removeAttr('checked');
            }

            $('.chk-ft-tg').attr('disabled', !isEnable);
        },

        setStatusAllAdvance1HTG: function (isChecked, isEnable) {
            if (isChecked) {
                $('.chk-1h-tg').attr('checked', 'checked');
            } else {
                $('.chk-1h-tg').removeAttr('checked');
            }

            $('.chk-1h-tg').attr('disabled', !isEnable);
        },

        setStatusAllAdvance1HTGAndTG: function (isChecked, isEnable) {
            if (isChecked) {
                $('.chk-tg').attr('checked', 'checked');
            } else {
                $('.chk-tg').removeAttr('checked');
            }

            $('.chk-tg').attr('disabled', !isEnable);
        },

        eventSubAdvanceTG: function () {
            var selectedItems = $('.chk-tg:input:checked').not('#chk-tg-all').length;
            var totalItems = 2;

            if (NapTicketControl.Helper.IsSelectedBettype(6)) {
                totalItems += 2;
                selectedItems += $('.chk-ft-tg:input:checked').length;
            }

            if (NapTicketControl.Helper.IsSelectedBettype(126) ||
                NapTicketControl.Helper.IsSelectedBettype(131) ||
                NapTicketControl.Helper.IsSelectedBettype(132)) {
                totalItems += 1;
                selectedItems += $('.chk-1h-tg:input:checked').length;
            }

            if (selectedItems === totalItems) {
                $('#chk-tg-all').attr('checked', 'checked');
            } else {
                $('#chk-tg-all').removeAttr('checked');
            }
        },

        setAdvancedModeTotalGoal: function () {
            var IsSelectedTotalGoal = NapTicketControl.Helper.IsSelectedBettype(6);
            var IsSelected1HTotalGoal = NapTicketControl.Helper.IsSelectedBettype(126);

            $('#chk-bettype-5').click(function () {
                NapTicketControl.Helper.IsClickTotalGoal = true;
                NapTicketControl.Helper.IsClick1HTotalGoal = false;
            });

            $('#chk-bettype-26').click(function () {
                NapTicketControl.Helper.IsClickTotalGoal = false;
                NapTicketControl.Helper.IsClick1HTotalGoal = true;
            });

            $('#EventType').click(function () {
                NapTicketControl.Helper.IsClickTotalGoal = false;
                NapTicketControl.Helper.IsClick1HTotalGoal = false;
            });

            if (NapTicketControl.Helper.GetBetteamFTTG() !== '' ||
                NapTicketControl.Helper.GetBetteamTG() !== '' ||
                NapTicketControl.Helper.GetBetteam1HTG() !== '') {
                if (NapTicketControl.Helper.IsClickTotalGoal) {
                    NapTicketControl.Helper.setStatusAllAdvanceTG(IsSelectedTotalGoal, IsSelectedTotalGoal);
                }

                if (NapTicketControl.Helper.IsClick1HTotalGoal) {
                    NapTicketControl.Helper.setStatusAllAdvance1HTG(IsSelected1HTotalGoal, IsSelected1HTotalGoal);
                }
            }
            else {
                NapTicketControl.Helper.setStatusAllAdvanceTG(IsSelectedTotalGoal, IsSelectedTotalGoal);
                NapTicketControl.Helper.setStatusAllAdvance1HTG(IsSelected1HTotalGoal, IsSelected1HTotalGoal);
                NapTicketControl.Helper.setStatusAllAdvance1HTGAndTG(true, true);
            }
        },

        GetBetteam6: function () {
            //Total Goal, Liveindicator = 0
            var checkedVals = '';
            var allChecked = [];
            var isCheckAll = false;

            var checkedTG = $('.chk-tg:checkbox:checked').not('#chk-tg-all').map(function () {
                return this.value;
            }).get().join('^');
            var checkedFTTG = $('.chk-ft-tg:checkbox:checked').not('#chk-tg-all').map(function () {
                return this.value;
            }).get().join('^');
            var checked1HTG = $('.chk-1h-tg:checkbox:checked').not('#chk-tg-all').map(function () {
                return this.value;
            }).get().join('^');

            if (checkedTG !== '') {
                allChecked.push(checkedTG);
            }

            if (checkedFTTG !== '') {
                allChecked.push(checkedFTTG);
            }

            if (checked1HTG !== '') {
                allChecked.push(checked1HTG);
            }

            var totalChecked = $('.chk-tg:checkbox:checked').not('#chk-tg-all').length +
                                $('.chk-ft-tg:checkbox:checked').not('#chk-tg-all').length +
                                $('.chk-1h-tg:checkbox:checked').not('#chk-tg-all').length;

            if (totalChecked === 5) {
                isCheckAll = true;
            }

            checkedVals = isCheckAll ? "All" : allChecked.join('^');

            return checkedVals;
        },

        GetBetteamFTTG: function () {
            var checkedVals = $('.chk-ft-tg:checkbox:checked').not('#chk-tg-all').map(function () {
                return this.value;
            }).get().join('^');

            return checkedVals;
        },

        GetBetteamTG: function () {
            var checkedVals = $('.chk-tg:checkbox:checked').not('#chk-tg-all').map(function () {
                return this.value;
            }).get().join('^');

            return checkedVals;
        },

        GetBetteam1HTG: function () {
            var checkedVals = '';
            var allChecked = [];

            var checkedTG = $('.chk-tg:checkbox:checked').not('#chk-tg-all').map(function () {
                return this.value;
            }).get().join('^');

            var checked1HTG = $('.chk-1h-tg:checkbox:checked').not('#chk-tg-all').map(function () {
                return this.value;
            }).get().join('^');

            if (checkedTG !== '') {
                allChecked.push(checkedTG);
            }

            if (checked1HTG !== '') {
                allChecked.push(checked1HTG);
            }

            checkedVals = allChecked.join('^');

            return checkedVals;
        },

        GetBetteam14: function () {
            //First Goal/Last Goal, Liveindicator = 0

            var checkedVals = '';
            checkedVals = $('.chk-flg:checkbox:checked').not('#chk-flg-all').map(function () {
                return this.value;
            }).get().join('^');

            return checkedVals;
        },

        checkNew2BettypeBeforeGetData: function () {
            var IsSelected1HFGLG = NapTicketControl.Helper.IsSelectedBettype(127);
            if (IsSelected1HFGLG) {
                if (NapTicketControl.Helper.GetBetteam14() === '') {
                    $('#msg').html(tooltip.show('chk-flg-all', 'Please select a goal for bettype: 1H First Goal/Last Goal!.')).show();
                    return false;
                }
            }

            var IsSelected1HTG = NapTicketControl.Helper.IsSelectedBettype(126);
            if (IsSelected1HTG) {
                if (NapTicketControl.Helper.GetBetteam1HTG() === '') {
                    $('#msg').html(tooltip.show('chk-tg-all', 'Please select a goal for bettype: 1H Total Goal!.')).show();
                    return false;
                }
            }

            return true;
        },

        isSettledTicket: function (statusTicket) {
            var result = false;
            var status = statusTicket.toLowerCase();
            switch (status) {
                case 'won':
                case 'lose':
                case 'draw':
                    result = true;
                    break;
                default:
                    break;
            }

            return result;
        },

        getTicketEnable: function (list) {
            if (list.length === 0) {
                return false;
            }

            for (var index = 0; index < list.length; index++) {
                var currentRow = list[index];
                if (!NapTicketControl.Helper.isSettledTicket(currentRow.Status)) {
                    return true;
                }
            };

            return false;
        },

        renderCheckBoxAll: function (list) {
            var isDisabled = !NapTicketControl.Helper.getTicketEnable(list) ? true : false;
            var checkBoxAll = "<div class='option-all'><input class='option' id='chk-ticket-all' type='checkbox' ";

            checkBoxAll += isDisabled ? "disabled=disabled /></div>" : "checked='checked' /></div>";

            return checkBoxAll;
        },

        setStatusAllAdvance1HTAndCS: function (isChecked, isEnable) {
            if (isChecked) {
                $('.chk-score').attr('checked', 'checked');
            } else {
                $('.chk-score').removeAttr('checked');
            }

            $('.chk-score').attr('disabled', !isEnable);
        },

        setStatusAllAdvance1HTCS: function (isChecked, isEnable) {
            if (isChecked) {
                $('.chk-1h-score').attr('checked', 'checked');
            } else {
                $('.chk-1h-score').removeAttr('checked');
            }

            $('.chk-1h-score').attr('disabled', !isEnable);
        },

        setStatusAllAdvanceCS: function (isChecked, isEnable) {
            if (isChecked) {
                $('.chk-ft-score').attr('checked', 'checked');
            } else {
                $('.chk-ft-score').removeAttr('checked');
            }

            $('.chk-ft-score').attr('disabled', !isEnable);
        },

        setAdvancedModeCorrectScore: function () {
            var IsSelectedCorrectScore = NapTicketControl.Helper.IsSelectedBettype(4);
            var IsSelected1HCorrectScore = NapTicketControl.Helper.IsSelectedBettype(30);

            $('#chk-bettype-3').click(function () {
                NapTicketControl.Helper.IsClickCorrectScore = true;
                NapTicketControl.Helper.IsClick1HCorrectScore = false;
            });

            $('#chk-bettype-20').click(function () {
                NapTicketControl.Helper.IsClickCorrectScore = false;
                NapTicketControl.Helper.IsClick1HCorrectScore = true;
            });

            $('#EventType').click(function () {
                NapTicketControl.Helper.IsClickCorrectScore = false;
                NapTicketControl.Helper.IsClick1HCorrectScore = false;
            });

            if (NapTicketControl.Helper.GetBetteamFTCS() !== '' ||
                NapTicketControl.Helper.GetBetteamCS() !== '' ||
                NapTicketControl.Helper.GetBetteam1HTCS() !== '') {
                if (NapTicketControl.Helper.IsClickCorrectScore) {
                    NapTicketControl.Helper.setStatusAllAdvanceCS(IsSelectedCorrectScore, IsSelectedCorrectScore);
                }

                if (NapTicketControl.Helper.IsClick1HCorrectScore) {
                    NapTicketControl.Helper.setStatusAllAdvance1HTCS(IsSelected1HCorrectScore, IsSelected1HCorrectScore);
                }
            }
            else {
                NapTicketControl.Helper.setStatusAllAdvanceCS(IsSelectedCorrectScore, IsSelectedCorrectScore);
                NapTicketControl.Helper.setStatusAllAdvance1HTCS(IsSelected1HCorrectScore, IsSelected1HCorrectScore);
                NapTicketControl.Helper.setStatusAllAdvance1HTAndCS(true, true);
            }
        },

        eventSubAdvanceCS: function () {
            var selectedItems = $('.all-chk-score .chk-score:input:checked').length;
            var totalItems = 16;

            if (NapTicketControl.Helper.IsSelectedBettype(4)) {
                totalItems += 9;
                selectedItems += $('.chk-ft-score:input:checked').length;
            }

            if (NapTicketControl.Helper.IsSelectedBettype(30)) {
                totalItems += 2;
                selectedItems += $('.chk-1h-score:input:checked').length;
            }

            if (selectedItems === totalItems) {
                $('#chk-score-all').attr('checked', 'checked');
            } else {
                $('#chk-score-all').removeAttr('checked');
            }
        },

        GetBetteam430: function () {
            var checkedVals = '';
            var allChecked = [];
            var isCheckAll = $('#chk-score-all').is(':checked');

            if (isCheckAll) {
                return "All";
            }

            var checkedCS = $('.chk-score:checkbox:checked').not('#chk-score-all').map(function () {
                return this.value;
            }).get().join('^');
            var checkedFTCS = $('.chk-ft-score:checkbox:checked').not('#chk-score-all').map(function () {
                return this.value;
            }).get().join('^');
            var checked1HTCS = $('.chk-1h-score:checkbox:checked').not('#chk-score-all').map(function () {
                return this.value;
            }).get().join('^');

            if (checkedCS !== '') {
                allChecked.push(checkedCS);
            }

            if (checkedFTCS !== '') {
                allChecked.push(checkedFTCS);
            }

            if (checked1HTCS !== '') {
                allChecked.push(checked1HTCS);
            }

            checkedVals = allChecked.join('^');

            return checkedVals;
        },

        GetBetteamFTCS: function () {
            var checkedVals = $('.chk-ft-score:checkbox:checked').not('#chk-score-all').map(function () {
                return this.value;
            }).get().join('^');

            return checkedVals;
        },

        GetBetteamCS: function () {
            var checkedVals = $('.chk-score:checkbox:checked').not('#chk-score-all').map(function () {
                return this.value;
            }).get().join('^');

            return checkedVals;
        },

        GetBetteam1HTCS: function () {
            var checkedVals = '';
            var allChecked = [];

            var checkedCS = $('.chk-score:checkbox:checked').not('#chk-score-all').map(function () {
                return this.value;
            }).get().join('^');

            var checked1HTCS = $('.chk-1h-score:checkbox:checked').not('#chk-score-all').map(function () {
                return this.value;
            }).get().join('^');

            if (checkedCS !== '') {
                allChecked.push(checkedCS);
            }

            if (checked1HTCS !== '') {
                allChecked.push(checked1HTCS);
            }

            checkedVals = allChecked.join('^');

            return checkedVals;
        },

        initAdvancedModeCorrectScores: function () {
            $('body').delegate('#chk-score-all', 'click', function () {
                if (NapTicketControl.Helper.IsSelectedBettype(4)) {
                    NapTicketControl.Helper.setStatusAllAdvanceCS(this.checked, true);
                }
                else {
                    NapTicketControl.Helper.setStatusAllAdvanceCS(false, false);
                }

                if (NapTicketControl.Helper.IsSelectedBettype(30)) {
                    NapTicketControl.Helper.setStatusAllAdvance1HTCS(this.checked, true);
                }
                else {
                    NapTicketControl.Helper.setStatusAllAdvance1HTCS(false, false);
                }

                NapTicketControl.Helper.setStatusAllAdvance1HTAndCS(this.checked, true);
            });

            $('body').delegate('.chk-score', 'click', function () {
                NapTicketControl.Helper.eventSubAdvanceCS();
            });

            $('body').delegate('.chk-1h-score', 'click', function () {
                NapTicketControl.Helper.eventSubAdvanceCS();
            });

            $('body').delegate('.chk-ft-score', 'click', function () {
                NapTicketControl.Helper.eventSubAdvanceCS();
            });
        }
    };
} (jQuery));