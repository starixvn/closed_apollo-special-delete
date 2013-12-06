/* File Created: March 28, 2013 */
var validateData = {
    TransId: '',

    checkParamBeforeUdpMsg: function () {
        var _TransId = $('#TransId').val();

        var TransId = _TransId.split(' ').join('').split('\n').join('').replace(/\s/g, "");

        if (TransId == '') {
            var itemNumber = $(".item-number").length;
            if (itemNumber == 0) {
                $('#msg').html(tooltip.show('errorBetId', "BetID(s) is required")).show();
                return false;
            }
        }

        if ($('.value-number-invalid').length > 0) {
            $('#msg').html(tooltip.show('errorBetId', 'Invalid BetID(s) are highlighted in RED.')).show();
            return false;
        }

        $('.value-number').each(function () {
            if (!helper.isNumber($(this).text())) {
                $('#msg').html(tooltip.show('errorBetId', 'BetID(s) is number only.')).show();
                return false;
            }

            if ($(this).text().length > 10) {
                $('#txtTransId').removeClass("text-number");
                $('#txtTransId').addClass("text-number-invalid");
                return false;
            }
        });

        if (!Message.Validator.validateMessageConstraints()) {
            return false;
        }

        validateData.TransId = TransId;
        return true;
    }
};