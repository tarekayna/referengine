define(["require", "exports", "common", "../common/Functions"], function(require, exports, __common__, __Functions__) {
    var common = __common__;

    
    var Functions = __Functions__;

    $(document).ready(function () {
        var mp = common.MixPanel, messenger = common.messenger, clientFunction = Functions.ClientFunction;
        function postMessageBackCompat(msg) {
            window.parent.postMessage(JSON.stringify({
                action: msg
            }), messenger.parentLocation);
        }
        ;
        $("#update-link").click(function () {
            mp.track("Recommend : Update : Update Now");
        });
        $("#re-cancel").click(function () {
            messenger.call(clientFunction.hide);
            mp.track("Recommend : Update : Cancel");
            postMessageBackCompat("done");
        });
        postMessageBackCompat("hide-loading");
        mp.track("Recommend : Update");
    });
})
