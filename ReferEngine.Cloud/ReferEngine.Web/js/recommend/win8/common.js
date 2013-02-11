$(document).ready(function () {
    var parentLocation = "ms-appx://apexa.co.calculi/Blu.html",
        baseUrl = window.ReferEngine.BaseUrl;

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
        return baseUrl + "/recommend/win8/" + name;
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

    var getMixPanelDateTime = function (currentTime) {
        var year = currentTime.getFullYear();
        var month = currentTime.getMonth() + 1;
        var date = currentTime.getDate();
        var hour = currentTime.getHours();
        var minutes = currentTime.getMinutes();
        var seconds = currentTime.getSeconds();
        return year + "-" + month + "-" + date + "T" + hour + ":" + minutes + ":" + seconds;
    };

    var getMixPanelDay = function (currentTime) {
        var day = currentTime.getDay();
        switch (day) {
            case 0:
                return "Sun";
            case 1:
                return "Mon";
            case 2:
                return "Tue";
            case 3:
                return "Wed";
            case 4:
                return "Thu";
            case 5:
                return "Fri";
            case 6:
                return "Sat";
        }
    };
    
    var mixPanelTrack = function (actionName, data) {
        var currentTime = new Date();
        var properties = {
            AppName: re.appName,
            AppId: re.appId,
            Timestamp: getMixPanelDateTime(currentTime),
            Day: getMixPanelDay(currentTime)
        };
        
        if (data) {
            for (prop in data) {
                if (data.hasOwnProperty(prop)) {
                    properties[prop] = data[prop];
                }
            }
        }

        mixpanel.track(actionName, properties);
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
        AddMessageHandler: addMessageHandler,
        MixPanelTrack: mixPanelTrack
    };
});