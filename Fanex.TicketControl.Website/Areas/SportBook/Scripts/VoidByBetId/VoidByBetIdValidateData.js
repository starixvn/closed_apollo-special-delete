/*global tooltip*/
"use strict"

var validateData = {
    checkParamBeforeGetData: function () {
        $('#msg').hide();

        if ($('#chkBetId').prop('checked') && $('#txtBetId').val() == '') {
            $('#msg').html(tooltip.show('txtBetId', 'Bet Id is required.')).show();
            return false;
        }
        if ($('#chkUsername').prop('checked')) {
            if ($('#txtUsername').val() == '' || $('#txtUsername').val() == 'Input a username') {
                $('#msg').html(tooltip.show('txtUsername', 'Username is required.')).show();
                return false;
            }
            if ($('#FromDate').val() == '') {
                $('#msg').html(tooltip.show('FromDate', 'From is required.')).show();
                return false;
            }
            if ($('#ToDate').val() == '') {
                $('#msg').html(tooltip.show('ToDate', 'To is required.')).show();
                return false;
            }
        }
        return true;
    },
    getSubTicketsChecked: function () {
        var checkedVals = '';
        checkedVals = $('.chk-ticket:checkbox:checked').not('#chk-ticket-all').map(function () {
            return this.value;
        }).get().join(',');

        return checkedVals;
    },
    checkParamBeforeVoidTicket: function () {
        $('#msg').hide();

        if (validateData.getSubTicketsChecked() == '') {
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