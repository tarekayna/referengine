$(document).ready(function () {
    var loginButton = $("#login-button");
    var message = $("#msg");

    loginButton.click(function () {
        var data = {
            action: "fbLogin"
        };
        var str = JSON.stringify(data);
        window.parent.postMessage(str, "ms-appx://apexa.co.calculi/Blu.html");
    });

    debugger;
    window.addEventListener("message", function (msg) {
        // TODO: Verify origin
        debugger;
        var data = JSON.parse(msg.data);
        if (data.msg === "loggedIn") {
            var token = data.token;
            var expiresIn = data.expiresIn;
            message.text("Logged in successfully. Loading...");

            debugger;
            //window.location.href = "http://www.referengine.com/refer/friends?access_token=" + token;
            window.location.href = "http://127.0.0.1/refer/friends?access_token=" + token;
            //window.location.href = "friends?access_token=" + token;
        }
    });
});