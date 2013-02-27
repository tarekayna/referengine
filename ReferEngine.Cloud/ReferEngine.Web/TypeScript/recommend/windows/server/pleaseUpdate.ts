/// <reference path="../../../lib/require.d.ts" static="true" />
declare var $;
import common = module("common");
import Messaging = module("../common/Messaging");
import Functions = module("../common/Functions");
 
$(document).ready(function () {
    var mp = common.MixPanel,
        messenger = common.messenger,
        clientFunction = Functions.ClientFunction;

    function postMessageBackCompat(msg: string) {
        window.parent.postMessage(JSON.stringify({
            action: msg
        }), messenger.parentLocation);
    };

    $("#update-link").click(function () {
        mp.track("Action", {
            Action: "Update App Now"
        });
    });

    $("#re-cancel").click(function () {
        messenger.call(clientFunction.hide);
        mp.track("Action", {
            Action: "Update App Cancel"
        });

        postMessageBackCompat("done");
    });

    postMessageBackCompat("hide-loading");
    mp.track("Page View", {
        Page: "Recommend Update"
    });
});
