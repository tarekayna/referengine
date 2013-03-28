define(["require", "exports", "../common/notifications"], function(require, exports, __Notification__) {
    var Notification = __Notification__;

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
                if(email) {
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
})
