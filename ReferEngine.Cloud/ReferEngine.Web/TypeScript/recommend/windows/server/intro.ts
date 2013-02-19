/// <reference path="../../../lib/require.d.ts" static="true" />
declare var $;
import common = module("common");
import Messaging = module("../common/Messaging");
import Functions = module("../common/Functions");

$(document).ready(function () {
    var loginButton = $(".login"),
        cancelButton = $("#re-cancel"),
        dontAskCheckbox = $("#dontAsk"),
        messenger = common.messenger,
        functionRange: Functions.FunctionRange,
        clientFunction = Functions.ClientFunction,
        severFunction = Functions.ServerFunction,
        facebookAuthorizationCode = null, 
        mp = common.MixPanel;

    function getRecommendUrl(): string {
        var query = "facebookCode=" + facebookAuthorizationCode;
        query = query.replace("#", "%23");
        return common.Url.base + "/recommend/win8/recommend?" + query;
    };

    loginButton.click(function () {
        messenger.call(clientFunction.setLoadingText, { text: "Loading..." });
        messenger.call(clientFunction.showLoading);
        messenger.call(clientFunction.authFacebook);

        mp.track("Rcmnd Intro Start");
    });

    cancelButton.click(function () {
        var dontAskAgain = dontAskCheckbox[0].checked;
       
        messenger.call(clientFunction.setAutoAsk, { askAgain: !dontAskAgain });
        messenger.call(clientFunction.hide);
        mp.track("Rcmnd Intro Cancel", {
            "Dont Ask Again": dontAskAgain
        });
    });

    var closedWhileLoading = false;
    var functions: Functions.Function[] = [
        new Functions.Function(severFunction.closedWhileLoading, function (details) {
            mp.track("Rcmnd Intro Closed While Loading");
            closedWhileLoading = true;
        }),
        new Functions.Function(severFunction.introVisible, function (details) {
            if (!closedWhileLoading) {
                mp.track("Rcmnd Intro");
            }
        }),
        new Functions.Function(severFunction.authFacebookResult, function (details) {
            var authResult: Messaging.AuthResult = details.authResult;
            if (authResult.success) {
                facebookAuthorizationCode = authResult.code;
                messenger.call(clientFunction.navigate, { url: getRecommendUrl() });
            }
            else {
                messenger.call(clientFunction.setLoadingText, { text: "Error: You must authorize Refer Engine to use your Facebook information." });
                messenger.call(clientFunction.showLoading);
            }
        }),
    ];

    functionRange = new Functions.FunctionRange(messenger);
    functionRange.addRange(functions);

    messenger.call(clientFunction.hideLoading);
    messenger.call(clientFunction.setIntroPageLoaded);
});
