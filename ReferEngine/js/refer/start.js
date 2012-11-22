$(document).ready(function () {
    var loginButton = $("#login-button");

    loginButton.click(function () {
        var data = {
            action: "fbLogin"
        };
        var str = JSON.stringify(data);
        window.parent.postMessage(str, "ms-appx://apexa.co.calculi/Blu.html");
    });
    
    window.addEventListener("message", function (msg) {
        // TODO: Verify origin

        var data = JSON.parse(msg.data);
        if (data.msg === "loggedIn") {
            var token = data.token;
            var expiresIn = data.expiresIn;

            //window.location.href = "http://www.referengine.com/refer/friends?access_token=" + token;
            window.location.href = "http://localhost:49844/refer/friends?access_token=" + token;
        }
    });
});