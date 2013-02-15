///<reference path='globals.ts' static='true' />

declare var mixpanel: any;

import Messaging = module("../common/Messaging");

export class Url {
    static base: string = ReferEngineGlobals.baseUrl;
    static getFriends: string = Url.base + "/recommend/win8/getfriends";
    static postRecommendation: string = Url.base + "/recommend/win8/postrecommendation";
}

export var messenger = new Messaging.Messenger(Messaging.MessengerType.IFrameToClient, "ms-appx://apexa.co.calculi");
messenger.parentLocation = "ms-appx://apexa.co.calculi/Blu.html";

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
    static track(actionName: string, data?: any) {
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
