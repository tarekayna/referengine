define(["require", "exports", "../common/Messaging"], function(require, exports, __Messaging__) {
    var Messaging = __Messaging__;

    var Url = (function () {
        function Url() { }
        Url.base = ReferEngineGlobals.baseUrl;
        Url.getFriends = Url.base + "/recommend/win8/getfriends";
        Url.postRecommendation = Url.base + "/recommend/win8/postrecommendation";
        return Url;
    })();
    exports.Url = Url;    
    exports.messenger = new Messaging.Messenger(Messaging.MessengerType.IFrameToClient, "ms-appx://apexa.co.calculi");
    exports.messenger.parentLocation = "ms-appx://apexa.co.calculi/Blu.html";
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
})
