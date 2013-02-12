///<reference path='..\..\globals.ts' static='true' />

declare var mixpanel: any;

var parentLocation = "ms-appx://apexa.co.calculi/Blu.html",
baseUrl = ReferEngineGlobals.baseUrl;

export interface IClientMessage {
    action: string;
    text: string;
}

export class ActionMessage implements IClientMessage {
    constructor(public action: string) { }
    text: string = null;
}

class LoadingTextMessage implements IClientMessage {
    constructor(public action: string, public text: string) { }
}

export class Url {
    static getFriends: string = baseUrl + "/recommend/win8/getfriends";
    static postRecommendation: string = baseUrl + "/recommend/win8/postrecommendation";
}

export class ClientMessaging {
    static hideLoading() {
        postAction("hide-loading");
    }
    static showLoading(loadingText: string) {
        postAction("show-loading");
        if (loadingText) {
            postToClient(new LoadingTextMessage("set-loading-text", loadingText));
        }
    }
    static postAction(action: string) {
        postToClient(new ActionMessage(action));
    }
    static postToClient(message: IClientMessage) {
        var data = {
            action: message.action,
            text: message.text
        };
        var str = JSON.stringify(data);
        window.parent.postMessage(str, parentLocation);
    }
    static postRecommendCancel(dontAsk: bool) {
        var data = {
            action: "cancel",
            dontAskAgain: dontAsk
        };
        var str = JSON.stringify(data);
        window.parent.postMessage(str, parentLocation);
    }
}

export class MixPanel {
    private static getDateTime(currentTime: Date) {
        var year = currentTime.getFullYear();
        var month = currentTime.getMonth() + 1;
        var date = currentTime.getDate();
        var hour = currentTime.getHours();
        var minutes = currentTime.getMinutes();
        var seconds = currentTime.getSeconds();
        return year + "-" + month + "-" + date + "T" + hour + ":" + minutes + ":" + seconds;
    };
    private static getDay(currentTime: Date) {
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
    static track(actionName: string, data: any) {
        var currentTime = new Date();
        var properties = {
            AppName: ReferEngineGlobals.appName,
            AppId: ReferEngineGlobals.appId,
            Timestamp: getDateTime(currentTime),
            Day: getDay(currentTime)
        };

        if (data) {
            for (var prop in data) {
                if (data.hasOwnProperty(prop)) {
                    properties[prop] = data[prop];
                }
            }
        }

        mixpanel.track(actionName, properties);
    };
}

var messageHandlers = [];
var addMessageHandler = function (message, handler) {
    messageHandlers.push({
        msg: message,
        handler: handler
    });
};

window.addEventListener("message", function (event: any) {
    // TODO: Verify origin
    var data = JSON.parse(event.data);
    for (var i = 0; i < messageHandlers.length; i++) {
        if (messageHandlers[i].msg === data.msg) {
            messageHandlers[i].handler(data);
        }
    }
});