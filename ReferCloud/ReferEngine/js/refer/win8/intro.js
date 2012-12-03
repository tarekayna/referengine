$(document).ready(function () {
    var baseUrl = "http://127.0.0.1/";
    //var baseUrl = "http://www.referengine.com/";
    var loginButton = $("#re-login");
    var notNowButton = $("#re-not-now");
    var dontAskButton = $("#re-dont-ask");
    var message = $("#msg");
    var parentLocation = "ms-appx://apexa.co.calculi/Blu.html";

    var postToParent = function(data) {
        var str = JSON.stringify(data);
        window.parent.postMessage(str, parentLocation);
    };

    var postActionToParent = function(action) {
        postToParent({
            action: action
        });
    };

    loginButton.click(function () {
        postActionToParent("fb-login");
    });

    notNowButton.click(function () {
        postActionToParent("not-now");
    });
    
    dontAskButton.click(function () {
        postActionToParent("dont-ask");
    });

    window.addEventListener("message", function (msg) {
        // TODO: Verify origin
        var data = JSON.parse(msg.data);
        if (data.msg === "logged-in") {
            var token = data.token;
            var expiresIn = data.expiresIn;
            message.text("Logged in successfully. Loading...");

            window.location.href = baseUrl + "refer/win8/friends/"+ data.appId + "?userAccessToken=" + token;
        }
    });

    postActionToParent("hide-loading");
});