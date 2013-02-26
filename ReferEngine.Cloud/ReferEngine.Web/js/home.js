$(document).ready(function () {

    $(".js-video").click(function() {
        mixpanel.track("Page Action", {
            Page: "Home",
            Action: "Video Play"
        });
    });
    
    var track = function (selector, actionName, source) {
        $(selector).click(function () {
            mixpanel.track("Page Action", {
                Page: "Home",
                Action: actionName,
                Source: source
            });
        });
    };

    track("#request-invite", "Request Invite", "Home");

    mixpanel.track("Page View", { Page: "Home" });
});
