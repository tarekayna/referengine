$(document).ready(function () {
    var track = function(selector, actionName, source) {
        $(selector).click(function () {
            mixpanel.track("Page Action", {
                Page: "Pricing",
                Action: actionName,
                Source: source
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
