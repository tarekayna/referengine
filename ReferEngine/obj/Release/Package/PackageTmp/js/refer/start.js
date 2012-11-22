$(document).ready(function () {
    var loginButton = $("#login-button");

    loginButton.click(function () {
        console.log("postMessage start");
        var data = {
            action: "fbLogin"
        };
        var str = JSON.stringify(data);
        window.parent.postMessage(str, "ms-appx://apexa.co.calculi/Blu.html");
        console.log("postMessage sent");
        console.log(str);
    });
    
    window.addEventListener("message", function (msg) {
        // TODO: Verify origin

        var data = JSON.parse(msg.data);
        if (data.msg === "loggedIn") {
            var token = data.token;
            var expiresIn = data.expiresIn;

            // test access
            //var url = "https://graph.facebook.com/me?access_token=" + token;

            //var xhr = new window.XMLHttpRequest;
            //var handler = function () {
            //    if (xhr.readyState === 4) {
            //        if (xhr.status === 200) {
            //            var id = JSON.parse(xhr.response).id;

            //        }
            //    }
            //};

            //xhr.open("GET", url, true);
            //xhr.onreadystatechange = handler;
            //xhr.send();

            window.location.href = "http://www.referengine.com/refer/friends?access_token=" + token;
            //window.location.href = "http://localhost:49844/refer/friends?access_token=" + token;
        }
    });
});