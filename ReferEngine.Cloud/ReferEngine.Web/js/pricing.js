$(document).ready(function () {
    var track = function (selector, actionName, location) {
        $(selector).click(function () {
            mixpanel.track("Action", {
                Page: "Pricing",
                Action: actionName,
                Location: location
            });
        });
    };
    
    track("#request-invite-pricing-free", "Request Invite", "Free");
    track("#request-invite-pricing-startup", "Request Invite", "Startup");
    track("#request-invite-pricing-business", "Request Invite", "Business");
    track("#request-invite-pricing-premium", "Request Invite", "Premium");
    track("#request-invite-pricing-enterprise", "Request Invite", "Enterprise");
    
    mixpanel.track("Page View", { Page: "Pricing" });
});
