define(["require", "exports"], function(require, exports) {
    var parentLocation = "ms-appx://apexa.co.calculi/Blu.html", baseUrl = ReferEngineGlobals.baseUrl;
    var ActionMessage = (function () {
        function ActionMessage(action) {
            this.action = action;
            this.text = null;
        }
        return ActionMessage;
    })();
    exports.ActionMessage = ActionMessage;    
    var LoadingTextMessage = (function () {
        function LoadingTextMessage(action, text) {
            this.action = action;
            this.text = text;
        }
        return LoadingTextMessage;
    })();    
    var Url = (function () {
        function Url() { }
        Url.getFriends = baseUrl + "/recommend/win8/getfriends";
        Url.postRecommendation = baseUrl + "/recommend/win8/postrecommendation";
        return Url;
    })();
    exports.Url = Url;    
    var ClientMessaging = (function () {
        function ClientMessaging() { }
        ClientMessaging.hideLoading = function hideLoading() {
            ClientMessaging.postAction("hide-loading");
        };
        ClientMessaging.showLoading = function showLoading(loadingText) {
            ClientMessaging.postAction("show-loading");
            if(loadingText) {
                ClientMessaging.postToClient(new LoadingTextMessage("set-loading-text", loadingText));
            }
        };
        ClientMessaging.postAction = function postAction(action) {
            ClientMessaging.postToClient(new ActionMessage(action));
        };
        ClientMessaging.postToClient = function postToClient(message) {
            var data = {
                action: message.action,
                text: message.text
            };
            var str = JSON.stringify(data);
            window.parent.postMessage(str, parentLocation);
        };
        ClientMessaging.postRecommendCancel = function postRecommendCancel(dontAsk) {
            var data = {
                action: "cancel",
                dontAskAgain: dontAsk
            };
            var str = JSON.stringify(data);
            window.parent.postMessage(str, parentLocation);
        };
        return ClientMessaging;
    })();
    exports.ClientMessaging = ClientMessaging;    
    var MixPanel = (function () {
        function MixPanel() { }
        MixPanel.getDateTime = function getDateTime(currentTime) {
            var year = currentTime.getFullYear();
            var month = currentTime.getMonth() + 1;
            var date = currentTime.getDate();
            var hour = currentTime.getHours();
            var minutes = currentTime.getMinutes();
            var seconds = currentTime.getSeconds();
            return year + "-" + month + "-" + date + "T" + hour + ":" + minutes + ":" + seconds;
        };
        MixPanel.getDay = function getDay(currentTime) {
            var day = currentTime.getDay();
            switch(day) {
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
        MixPanel.track = function track(actionName, data) {
            var currentTime = new Date();
            var properties = {
                AppName: ReferEngineGlobals.appName,
                AppId: ReferEngineGlobals.appId,
                Timestamp: MixPanel.getDateTime(currentTime),
                Day: MixPanel.getDay(currentTime)
            };
            if(data) {
                for(var prop in data) {
                    if(data.hasOwnProperty(prop)) {
                        properties[prop] = data[prop];
                    }
                }
            }
            mixpanel.track(actionName, properties);
        };
        return MixPanel;
    })();
    exports.MixPanel = MixPanel;    
    var messageHandlers = [];
    var addMessageHandler = function (message, handler) {
        messageHandlers.push({
            msg: message,
            handler: handler
        });
    };
    window.addEventListener("message", function (event) {
        var data = JSON.parse(event.data);
        for(var i = 0; i < messageHandlers.length; i++) {
            if(messageHandlers[i].msg === data.msg) {
                messageHandlers[i].handler(data);
            }
        }
    });
})
