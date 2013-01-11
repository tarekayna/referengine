$(document).ready(function () {
    var parentLocation = "ms-appx://apexa.co.calculi/Blu.html",
        baseUrl = "http://127.0.0.1:81/";
        //baseUrl = "https://www.referengine.com/";

    var postToParent = function (data) {
        var jsonData;
        if (typeof data === 'string') {
            jsonData = {
                action: data
            };
        } else {
            jsonData = data;
        }

        window.parent.postMessage(JSON.stringify(jsonData), parentLocation);
    };
    
    var getLink = function(name) {
        return baseUrl + "recommend/win8/" + name;
    };

    var setLoadingText = function(message) {
        postToParent({
            action: "set-loading-text",
            text: message
        });
    };

    var hideLoading = function () {
        postToParent("hide-loading");
    };

    var showLoading = function (message) {
        postToParent("show-loading");
        
        if (message != undefined) {
            setLoadingText(message);
        }
    };

    var navigateTo = function (url) {
        postToParent("show-loading");
        window.location.href = url;
    };

    var messageHandlers = [];
    var addMessageHandler = function (message, handler) {
        messageHandlers.push({
            msg: message,
            handler: handler
        });
    };
    
    window.addEventListener("message", function (event) {
        // TODO: Verify origin
        var data = JSON.parse(event.data);
        for (var i = 0; i < messageHandlers.length; i++) {
            if (messageHandlers[i].msg === data.msg) {
                messageHandlers[i].handler(data);
            }
        }
    });
    
    if (window.RE == undefined) {
        window.RE = {};
    }
    
    window.RE.Utilities = {
        GetLink: getLink,
        PostToParent: postToParent,
        HideLoading: hideLoading,
        ShowLoading: showLoading,
        NavigateTo: navigateTo,
        AddMessageHandler: addMessageHandler
    };
});