/// <reference path='..\lib\jquery.d.ts' static='true' />
/// <reference path="../lib/require.d.ts" static="true" />

import Notification = module("../common/notifications");

(function () { 
    var onError = function () {
        Notification.show("Error sending invite", Notification.NotificationType.error);
    };

    var onSuccess = function () {
        Notification.show("Invite Sent Successfully", Notification.NotificationType.success);
    };

    $(document).ready(function () {
        $("#send-invite").click(function () {
            var email = $("#email").val();
            var msAppId = $("#msAppId").val();
            var name = $("#name").val();
            $.ajax("../admin/SendWindowsAppInvite", {
                type: "POST",
                data: {
                    msAppId: msAppId,
                    email: email,
                    name: name
                },
                error: onError,
                success: onSuccess
            });
        });
    });
})();