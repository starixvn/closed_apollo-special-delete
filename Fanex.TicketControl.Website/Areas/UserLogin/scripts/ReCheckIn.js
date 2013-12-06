var Recheckin = function ($) {
    return {
        init: function () {
            Recheckin.time = 10000;
            Recheckin.kickedOutMsg = 'You have logged in from another session or your login session has expired!';
        },

        reCheckin: function () {
            var urlRecheck, urlLogout, urlOffline;

            if (window.rootWebPath.indexOf('localhost') > 0) {
                urlRecheck = window.rootWebPath + "UserLogin/Login/ReCheckLogin";
                urlLogout = window.rootWebPath + "UserLogin/Login/LogOut";
                urlOffline = window.rootWebPath + "UserLogin/Common/Offline";
            }
            else {
                urlRecheck = window.rootWebPath + "/UserLogin/Login/ReCheckLogin";
                urlLogout = window.rootWebPath + "/UserLogin/Login/LogOut";
                urlOffline = window.rootWebPath + "/UserLogin/Common/Offline";
            }

            $.ajax({
                url: urlRecheck,
                type: "POST",
                dataType: "json",
                success: function (result) {
                    if (result == "1") {
                        alert(Recheckin.kickedOutMsg);
                        window.location.href = urlLogout;
                    } else if (result == "-1") {
                        window.location.href = urlOffline;
                    }
                },
                error: function (request, status, error) {
                    if (status == "abort") return;
                }
            });
        }
    }
} (jQuery);

$(document).ready(function () {
    Recheckin.init();
    setInterval("Recheckin.reCheckin()", Recheckin.time);
});