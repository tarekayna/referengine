/// <reference path="../../lib/require.d.ts" static="true" />
declare var $;
import common = module("common");

$(document).ready(function () {
    var loginButton = $(".login"),
        cancelButton = $("#re-cancel"),
        dontAskCheckbox = $("#dontAsk"),
        mp = common.MixPanel,
        clientMessaging = common.ClientMessaging;

    loginButton.click(function () {
        clientMessaging.postAction("fb-login");
        mp.track("Recommend Intro Start", null);
    });

    cancelButton.click(function () {
        var dontAskAgain = dontAskCheckbox[0].checked;
        clientMessaging.postRecommendCancel(dontAskAgain);
        mp.track("Recommend Intro Cancel", {
            "Dont Ask Again": dontAskAgain
        });
    });

    clientMessaging.hideLoading();
    clientMessaging.postAction("intro-page-loaded");
    mp.track("Recommend Intro", null);
});
