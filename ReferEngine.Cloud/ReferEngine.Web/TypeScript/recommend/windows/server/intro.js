define(["require", "exports", "common", "../common/Functions"], function(require, exports, __common__, __Functions__) {
    var common = __common__;

    
    var Functions = __Functions__;

    $(document).ready(function () {
        var loginButton = $(".login"), cancelButton = $("#re-cancel"), dontAskCheckbox = $("#dontAsk"), messenger = common.messenger, clientFunction = Functions.ClientFunction, referEngineAuthorizationToken = null, facebookAuthorizationCode = null, mp = common.MixPanel;
        function getRecommendUrl() {
            var query = "fb_access_code=" + facebookAuthorizationCode + "&re_auth_token=" + referEngineAuthorizationToken;
            query = query.replace("#", "%23");
            return common.Url.base + "/recommend/win8/recommend?" + query;
        }
        ;
        loginButton.click(function () {
            messenger.call(clientFunction.setLoadingText, {
                text: "Loading..."
            });
            messenger.call(clientFunction.showLoading);
            messenger.call(clientFunction.authFacebook);
            mp.track("Rcmnd Intro Start");
        });
        cancelButton.click(function () {
            var dontAskAgain = dontAskCheckbox[0].checked;
            messenger.call(clientFunction.setAutoAsk, {
                askAgain: !dontAskAgain
            });
            messenger.call(clientFunction.hide);
            mp.track("Rcmnd Intro Cancel", {
                "Dont Ask Again": dontAskAgain
            });
        });
        messenger.addMessageHandler("loading-closed", function () {
            mp.track("Rcmnd Intro Loading Closed");
        });
        messenger.addMessageHandler("intro-page-visible", function () {
            mp.track("Rcmnd Intro");
        });
        messenger.addMessageHandler("auth-facebook-result", function (details) {
            var authResult = details.authResult;
            if(authResult.success) {
                facebookAuthorizationCode = authResult.code;
                if(referEngineAuthorizationToken) {
                    messenger.call(clientFunction.navigate, {
                        url: getRecommendUrl()
                    });
                }
            } else {
                messenger.call(clientFunction.setLoadingText, {
                    text: "Error: You must authorize Refer Engine to use your Facebook information."
                });
                messenger.call(clientFunction.showLoading);
            }
        });
        messenger.addMessageHandler("auth-app-result", function (details) {
            var authResult = details.authResult;
            if(authResult.success) {
                referEngineAuthorizationToken = authResult.code;
                if(facebookAuthorizationCode) {
                    messenger.call(clientFunction.navigate, {
                        url: getRecommendUrl()
                    });
                }
            } else {
            }
        });
        messenger.call(clientFunction.hideLoading);
        messenger.call(clientFunction.setIntroPageLoaded);
        messenger.call(clientFunction.authApp);
    });
})
