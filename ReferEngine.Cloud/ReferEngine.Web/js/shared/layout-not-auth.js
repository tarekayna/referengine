$(document).ready(function () {

    var track = function (selector, actionName, source) {
        $(selector).click(function () {
            mixpanel.track("Page Action", {
                Action: actionName,
                Source: source
            });
        });
    };

    track("#request-invite-hdr", "Request Invite", "Header");


    $("#log-in-hdr").click(function () {
        mixpanel.track("Page Action", {
            Action: "Log In Header"
        });
    });
});
