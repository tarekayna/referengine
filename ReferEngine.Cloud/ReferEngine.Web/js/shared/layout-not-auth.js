$(document).ready(function () {

    var track = function (selector, actionName, location) {
        $(selector).click(function () {
            mixpanel.track("Action", {
                Action: actionName,
                Location: location
            });
        });
    };

    track("#request-invite-hdr", "Request Invite", "Header");

    $("#log-in-hdr").click(function () {
        mixpanel.track("Action", {
            Action: "Log In",
            Location: "Header"
        });
    });
});
