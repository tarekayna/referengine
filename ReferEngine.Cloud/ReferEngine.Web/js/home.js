$(document).ready(function () {

    $(".js-video").click(function() {
        mixpanel.track("Action", {
            Page: "Home",
            Action: "Video Play"
        });
    });
    
    var track = function (selector, actionName, location) {
        $(selector).click(function () {
            mixpanel.track("Action", {
                Page: "Home",
                Action: actionName,
                Location: location
            });
        });
    };

    track("#request-invite", "Request Invite", "Home");

    mixpanel.track("Page View", { Page: "Home" });
});
