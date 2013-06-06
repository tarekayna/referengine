/// <reference path='..\lib\jquery.d.ts' static='true' />
/// <reference path="../lib/require.d.ts" static="true" />

import Notification = module("../common/notifications");

(function () { 
    var onError = function () {
        Notification.show("Error sending invite", Notification.NotificationType.error);
    };

    var onSuccess = function () {
        Notification.show("Invite Sent", Notification.NotificationType.success);
    };

    $(document).ready(function () {
        $("#send-invite").click(function () {
            var email = $(":checked").val();
            if (email) {
                $.ajax("../admin/sendinvite", {
                    type: "POST",
                    data: {
                        email: email
                    },
                    error: onError,
                    success: onSuccess
                });
            }
        });
    });
})();