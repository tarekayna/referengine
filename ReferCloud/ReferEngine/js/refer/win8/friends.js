$(document).ready(function () {
    var submitButton = $("#submit-button");
    var messageDiv = $("#message");
    var baseUrl = "http://www.referengine.com";
    //var baseUrl = "http://127.0.0.1";
    var appId = $("#appId").val();
    var userAccessToken = $("#userAccessToken").val();
    var parentLocation = "ms-appx://apexa.co.calculi/Blu.html";

    var postToParent = function (data) {
        var str = JSON.stringify(data);
        window.parent.postMessage(str, parentLocation);
    };

    submitButton.click(function () {

    });

    var getFacebookFriends = function() {
        FB.api('/me/friends', function (response) {
            alert('Your name is ' + response.name);
        });
    };

    messageDiv.keyup(function(event) {
        var which = 50;
        getFacebookFriends();
    });
});