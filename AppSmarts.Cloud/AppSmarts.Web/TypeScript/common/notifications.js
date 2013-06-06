define(["require", "exports"], function(require, exports) {
    var NotificationType = (function () {
        function NotificationType() { }
        NotificationType.none = "";
        NotificationType.info = "info";
        NotificationType.success = "success";
        NotificationType.error = "error";
        return NotificationType;
    })();
    exports.NotificationType = NotificationType;    
    function show(message, notificationType) {
        $(".top-right").notify({
            message: {
                text: message
            },
            transition: "fade",
            type: notificationType
        }).show();
    }
    exports.show = show;
})
