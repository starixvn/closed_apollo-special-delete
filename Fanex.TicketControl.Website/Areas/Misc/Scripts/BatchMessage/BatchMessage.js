/* File Created: March 27, 2013 */
$(function () {
    batchMsg.init();
});

var batchMsg = {
    init: function () {
        $('#fsMessage').show();

        $('body').delegate('#btnSearch', 'click', function (e) {
            e.stopPropagation();
            batchMsg.updateMessage();
        });
        NapTicketControl.Helper.InitNumberGroup();
    },

    highLightInvalidBet: function (invalidTransIds) {
        invalidTransIds = invalidTransIds.split(",");
        $('.value-number').each(function () {
            for (var i = 0; i < invalidTransIds.length; i++) {
                if ($.trim(invalidTransIds[i]) == $.trim($(this).text())) {
                    $(this).removeClass("value-number");
                    $(this).addClass("value-number-invalid");
                    break;
                }
            }
        });
    },

    getTransId: function () {
        var result = "";
        var betId = $('#TransId').val();

        $('.item-number').each(function () {
            result += result == "" ? $.trim($(this).first().text()) : "," + $.trim($(this).first().text());
        });

        if (betId != '') {
            result += result == "" ? betId : "," + betId;
        }

        return result;
    },
    resetValue: function () {
        $(".item-number").remove();
        $('#TransId').val('');
        $('#msg-en').val('');
        $('#msg-ch').val('');
        $('#msg-cs').val('');
        $('#msg-th').val('');
        $('#msg-it').val('');
        $('#msg-jp').val('');
        $('#msg-es').val('');
        $('#msg-ko').val('');
    },
    updateMessage: function () {
        if (!validateData.checkParamBeforeUdpMsg()) {
            return;
        }

        var param = {
            message: {
                Content_en: $('#msg-en').val(),
                Content_ch: $('#msg-ch').val(),
                Content_cs: $('#msg-cs').val(),
                Content_th: $('#msg-th').val(),
                Content_it: $('#msg-it').val(),
                Content_jp: $('#msg-jp').val(),
                Content_es: $('#msg-es').val(),
                Content_ko: $('#msg-ko').val()
            },
            TransIds: batchMsg.getTransId()
        };

        $.ajax({
            type: "POST",
            url: window.rootUrl + "Misc/BatchMessage/UpdateMessage",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            beforeSend: function () {
                NapTicketControl.disableInput();
            },
            success: function (rs) {
                if (rs.ErrorCode > 0) {
                    batchMsg.highLightInvalidBet(rs.ErrorMsg);
                    $('#msg').html(tooltip.show('errorBetId', 'Invalid BetID(s) are highlighted in RED.')).show();
                }
                else {
                    batchMsg.resetValue();
                    NapTicketControl.Helper.showMsgBox(rs);
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
};