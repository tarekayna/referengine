define(["require", "exports", "../common/Messaging"], function(require, exports, __Messaging__) {
    var Messaging = __Messaging__;

    var Url = (function () {
        function Url() { }
        Url.base = re.baseUrl;
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
        MixPanel.getHour = function getHour(currentTime) {
            return currentTime.getHours();
        };
        MixPanel.getMonth = function getMonth(currentTime) {
            var month = currentTime.getMonth();
            switch(month) {
                case 0:
                    return "Jan";
                case 1:
                    return "Feb";
                case 2:
                    return "Mar";
                case 3:
                    return "Apr";
                case 4:
                    return "May";
                case 5:
                    return "Jun";
                case 6:
                    return "Jul";
                case 7:
                    return "Aug";
                case 8:
                    return "Sep";
                case 9:
                    return "Oct";
                case 10:
                    return "Nov";
                case 11:
                    return "Dec";
            }
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
                AppName: re.appName,
                AppId: re.appId,
                Hour: MixPanel.getHour(currentTime),
                Day: MixPanel.getDay(currentTime),
                Month: MixPanel.getMonth(currentTime)
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
