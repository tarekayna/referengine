(function () {
    "use strict";
    
    //#region base url
    var baseUrl = "http://127.0.0.1:81";
    //var baseUrl = "https://www.referengine-test.com";
    //var baseUrl = "https://www.referengine.com";
    //#endregion base url
    
    //#region variables
    var uiInitialized = false,
        util = WinJS.Utilities,
        container,
        iframe,
        loadingElem,
        progress,
        loadingTextElem,
        facebookAuthorizationCode,
        referEngineAuthorizationToken,
        args = ReferEngine.AppActivationArgs,
        introHasLoaded = false,
        autoOpenTimeout = 15000,
        store = Windows.ApplicationModel.Store,
        currentApp = ReferEngine.AppIsPublished ? store.CurrentApp : store.CurrentAppSimulator,
        applicationData = Windows.Storage.ApplicationData.current,
        roamingSettings = applicationData.roamingSettings,
        referEngineFacebookAppId = "368842109866922",
        logoMarkImageData = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADIAAAAyCAIAAACRXR/mAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAACz1JREFUeNrMmQlwFFUagPv1XJmZzJFzSJgJuQMkJBhlSQIKWpqotcriyiqry3pgleeuoLLr1kqBa+nueiyiVetaIFWLJawogoVgEATUHIIkJCFAQg4yM+SYXDOTmczdb/9+r+cgmYEAYtn89Xjd6ff6m/96/+tGGGPm53ewVzm+tt9d0++Gzj8aR0Gg822fq3nYc5XTiq9s2LpjIydHfb/JUa6qGYbT2w2KvUYnh5lTo94958YDGH94q+7dFltRovSNiuQrmB9dlhE3nbZvPjP2XIn2+bphFjFqKWv3cnAd5oBZOIyBDJgCHG2xn8OvlSVtaLK+dEPiQzPVPzIWPBuAgOOoxXvU4mYRgj5C/HBoMI/FzxIvYQu0knkpUoBTSVhorR5gw4d73SoJylCJNVJ23bykHwfL5PB/0uXY2GIXs0iEGBHLMwEZoYJ/TJpCtDhNfnemYqZWYjs1OG6y205ZQsM1s1MUeo2mMOXEkOftZuuidPni6fJMleSqsN5qsr7dYitOkrXbfAQLiVhGJGgLlSZJ78tVLlCivuqOvq86h+rMfpsHMcKEKNKF1bJpVblplTnQrjs2rJWKni3RXiEW6Gn5gYF+FydhGWDihcfimaYrxX8oUpUpUMfmhs4PGikN4qcTaPg2YmJMrsIFhV6dv7pcdfcsrYy9EqxVNUM7u53JcrHLjyUiJGABH0LLspUP5Stdx3uH6kza2anwJLBR2BFbB+2tlpF6s2Vfh8/uCekNBzkBUVeVW7huMQy8PCzQ04Jd56UskohYqQhJWF4AC37i04WqKr18igFl+bKjZ1PjSK2JCZoWEyxoxRpZyVtVYNOpYu0zjr98fNQVwG4/llIsQpYgY98qS8xRX3aqG601ta6qdpnsISbaxhnUeavLDcsKp4RVsM3oDmAZASLCd66YiR5+u6d97eHe/7VGYnGkLf5X1WSyiX63vcMBcQd+zUdc0J+0V8dEI3H2hqqClxezmKGCaMsw5z8+Be54MW3ZvFz+NqNMhIBLRlRFdbaxIhESVY/TzzIoWyWeqb08PluNyXFyEBA0FQYw6Nm1h0FboCqOqA06Io3slrqVErUs+poIiy5NlbSlWQpywYaTY6NeHLQpmiYX358lL068NNzI3o6uvx72gFchIRJlGWpQEkfNBEyY7wRsnuE6U6T7X2DE7WfHQB+EiQjLww26A4NuLsKmyO7DWzpdPwz7Ls40uK21bcXnPqM9ZDh4mBdOmbARWZI9pCrZuU2NQBYFy+jw7zONI7KeIAonIIaUx+d3EDHJF/t6fRAZsZg8RnvX09WiEFCEIEJDO1QgIEbqTN2bG6Ng2UgtwDMhYcEjiKQlF+h1Yf1hoUZgmq2BWFgD/2mgEKLJTFhYDMJCrkyrzImC9e+TNhRcxxCtDYIJmp4KF0mHqrDDwcXCcjdbRBizF0jYaiEUFIHYvLq6df3hiVg9Dn9oGYP/8jXiRWlx9AIpXUJJWjgFb+W4mEZEzCTzQdhFoGS+UJ5QYQgpLLRwRXF5HCyeoP/LDEWuWozJGS2naJXHkSrPT0o8MYpdjE+yINEKDnFAJtPdNxvh8M8Ax8965LowVmv/99B+0+siBLTOZPK1kjszFJQj1ELN6QcPxdjHYW8A56lFsbA0d+ZEBmCYLChJd+Ym35HLBK0JT4V1/fjKz2k88lij43zVppYgwoSJgfC8FBnUd1AsTNAQiC/AS4oMzdbExEp4oIimKKqwsMsSMawplxnUsFprKwwTirNxs13AWph9F7RzkmQhxaQphFT5TKGqUi+nGhKYiOjkaHlWnFwU04qQuA0fLZFmqCeXhMnLC/VrymlfW6GPHJW1spSuj/zjD7Rtn502HzOyAN0gYJSuDKvhTyXq2/Vxu40uoyPg5XCeSrw4XXajTnrJFB9XnJpZu8L24Unb3s7xFot0hiauOCVxeaFqoSF0j3aBnnkzPKR7UwP4HNSJglbAjgun5dX0uTgOBRC/aYl8QEmSFOQKVmjQWeJT14PERDdoQtEW2eGNeGvB/XKJEgqHADEWKMzq5X6a3TOUXDgiPjNXlhasLhewwIg7TryzJEsJ2yayuWOahjzMT3WA1XAwN3VvbjDtaBWwspOLwIjdwyfvylT6gxFn4rPrtT2sLj4DKItSI7OmnBT4BCupaMW8Pzeav4a9byjivutzX2us0wP1NEqpHaHeqvh4WXK5IZzlP2na2Hj+oFY8uiJfRbPAnh7n1B9gd7WbRveYRvbYXO1TH9V0/iCfqEw2qifvmKfxueoLysCcpKI2S/2u5g1r563f1e1w+vHubofNmwLb80sAudtbTOuh5fhCF1wTJSqvn6N/VivPu5QFBwbGumj+5IIKSyrPuGBNfOCGF++Y9WjPaIvNfuSDW3R8Hufwuy3WSzId637c6W0TsViEOBZhkGHn8W/an7KOn7342GM9u7MSi2DBoaqCyJ/7ZtV1b1ZOXKoPdWyDID1w5v3yZMsfS7RAtqHZavXEzBS+wFiD8fkAZxchnknEl2WYF9jncGO1nS9erLp3DRw17tbIdWOtFhxUVfP6I1HqrfLMuzMT57gDzp1Nf/v7fMXvZ6qHXIFHDg3ELPTsR1zePlKEYVKH4cgSxentM1u/jTX2YPv7cOc0ddZQnZkju4ykckPBqrIoWDfnLdeps+lP2d6w5r2blM+WaD/rcoCrRZ162NGAyWYBFisO845FVnqEg6+7YtnxuGnX2cE6uKEgtWy43kx2i0hXmZP7aGn0LYbb56A/2DLWtfXok38psW+5Rbfqu6ET0bIr6ANo+MWKY/0cC87On/IVCClVMBP15YbF0Xmo/T34k06VrfRpB+tNcDOQCW8romJVzXzstoLHMhKKiTsPfHrihfna+kO/mr7+2MhkMlVcAXAEMA/EC98namMEsih7IUfnjoY19E+/mLHEuKMV4ld3e86ctYsiVTURK06izE8tOzfSQs0ByjvQ9vpp4yv/vRlNfu+TFF8aIEriyXg4lmIRtUF1hPQJN16QPPv3f/TDE26/AyZXy3XF6bd2ftAI/nh+f+eMZYWRe9cor3S1ZMCgs9PvH3N4+6Em7Bqu6Rn9bl7GinT5r6Xi+NCd6Zqb1HH5Nnd7aPXHWAACSVVdl6AQUpfH7/i67fWOoVqy1eDVuTD7t0RVjLowJbnMMIEp5osk8Pr3ax/iw55PSFjEQvxjlsUFqVWFafcmKnND7nWk7RlohYKEjwCeTCPPv23WRqko3ut3NPd+2nx+57hvPGhrVq8tuTdv/b6Fm712z5LmJyczxXwBLpPEp8Rnj7kHFFKF09NHMwA8td1S3TH4pTpOl6aZa0hYmKjIW5T/TvWph70BBxZ2REgpTVuU92qf7UTPSE33cI3L56TRQB1OKlIuLXnp1Kv14ObqiJd1U30b6PHza+LWo084PP1UZyxRGO1AruI3WCi4nRSMKJBxQQnQUA36nFikur/0n9Yd1rNbGiu/eBCGRFXVxb5iyMRKkJzkBTBjumYunZqjhgiaIxB5KggbikrKFEKUECalLbnxlSPpVbkAFItpSi/AwWEbTJ8d7dkKSoIiFtYcoi2GaosWJsTCKKgwwfFxMGWAJCtzq2atdux3O832godLpbGBpvpxRSaOLzUs9Qac4G1nBvb32prgl4AROYT4J6PwvkbI8kEyHFTSXP3SiqwHB743O82WqTBd9seVRvPO7899uCj38YNtrwNUnISPteC2ILzsAJBUpAIy8IHKWc/xW5jTgwmzUq7VN5/Q8dWZN4YcHfMzf/dF6zrQVYn+nhPmnfweafo9nUO1kAJAQxCwsog8dw0/RU0+zNYmaIEj1PlR6mn08/zM+X8BBgD1w2D5qH60pgAAAABJRU5ErkJggg==",
        logoTextImageData = "data:image/png;base64,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";
    //#endregion variables

    //#region initial checks
    if (!ReferEngine.AppId) throw new RangeException("ReferEngine.AppId must be defined.");
    //#endregion initial checks

    //#region urls
    var introUrl = baseUrl + "/recommend/win8/intro/" + ReferEngine.AppId;
    var authUrl = baseUrl + "/api/win8/auth";
    var recommendUri = {
        get: function() {
            var query = "fb_access_code=" + facebookAuthorizationCode +
                        "&re_auth_token=" + referEngineAuthorizationToken;
            query = query.replace("#", "%23");
            return baseUrl + "/recommend/win8/recommend" + query;
        }
    };
    //#endregion urls

    //#region properties
    var isSupportedRegion = function() {
        var code = new Windows.Globalization.GeographicRegion().code;
        return code === "US" || code === "USA" || code === "840";
    };

    var launchCount = {
        get: function() {
            var count = roamingSettings.Values["ReferEngine-LaunchCount"];
            if (count == null) {
                roamingSettings.Values["ReferEngine-LaunchCount"] = 0;
                return 0;
            }
            return count;
        },
        set: function (value) {
            roamingSettings.Values["ReferEngine-LaunchCount"] = value;
        }
    };

    var autoAskAgain = {
        get: function() {
            var autoAsk = roamingSettings.Values["ReferEngine-AutoAskAgain"];
            if (autoAsk == null) {
                roamingSettings.Values["ReferEngine-AutoAskAgain"] = isSupportedRegion;
                return isSupportedRegion;
            }
            return autoAsk;
        },
        set: function (value) {
            roamingSettings.Values["ReferEngine-AutoAskAgain"] = value;
        }
    };
    
    var shouldAutoOpen = {
        get: function() {
            return launchCount % 2 == 0 && autoAskAgain;
        }
    };
    //#endregion properties

    //#region dom creation
    var create = function(e) {
        return document.createElement(e);
    };

    var createDiv = function (className) {
        var elem = create("div");
        if (className != undefined) {
            elem.className = className;
        }
        return elem;
    };

    var createImg = function (src, className) {
        var elem = create("img");
        elem.src = src;
        elem.className = className;
        return elem;
    };
    //#endregion dom creation

    //#region hide/show loading
    var hideLoadingRing = function () {
        progress.style.opacity = 0;
    };

    var showLoadingRing = function () {
        progress.style.opacity = 1;
    };

    var showLoading = function () {
        WinJS.UI.Animation.fadeIn(loadingElem).then(function () {
            util.removeClass(loadingElem, "re-hidden");
        });
    };

    var hideLoading = function () {
        WinJS.UI.Animation.fadeOut(loadingElem).then(function () {
            util.addClass(loadingElem, "re-hidden");
        });
    };

    var setLoadingText = function (text) {
        if (text.substr(0, 5) === "Error") {
            hideLoadingRing();
        }
        loadingTextElem.innerText = text;
    };
    //#endregion hide/show loading

    //#region auth functions
    var authorizeAppAsync = function () {
        return new WinJS.Promise(function (comp, error, prog) {
            currentApp.getAppReceiptAsync().done(function (xml) {
                xml = "<xml>" + xml + "</xml>";
                var xml64 = window.btoa(xml);

                WinJS.xhr({
                    type: "POST",
                    headers: { "Content-type": "text/xml" },
                    url: authUrl,
                    data: xml64
                }).done(
                    function (request) {
                        var data = JSON.parse(request.responseText).Data;
                        referEngineAuthorizationToken = data.token;
                        comp(true);
                    },
                    function () {
                        comp(false);
                    });
            });
        });
    };

    var authorizeFacebookAsync = function() {
        return new WinJS.Promise(function(comp, error, prog) {
            var callback = "https://www.referengine.com/recommend/win8/success";
            var scope = "email,publish_actions";
            var query = "client_id=" + referEngineFacebookAppId +
                "&redirect_uri=" + callback +
                "&scope=" + scope +
                "&display=popup&response_type=code";
            var facebookUrl = "https://www.facebook.com/dialog/oauth?" + query;
            var startUri = new Windows.Foundation.Uri(facebookUrl);
            var web = Windows.Security.Authentication.Web;
            web.WebAuthenticationBroker.authenticateAsync(web.WebAuthenticationOptions.none, startUri).done(
                function(request) {
                    if (request.responseData.indexOf("error_reason=") != -1) {
                        comp(false);
                    }
                    var codeString = "code=";
                    var start = request.responseData.indexOf(codeString) + codeString.Length;
                    facebookAuthorizationCode = request.responseData.substr(start);
                    comp(true);
                },
                function() {
                    comp(false);
                }
            );

        });
    };
    //#endregion auth functions

    //#region onWindowMessage
    var onWindowMessage = function(msg) {
        if (msg.origin === baseUrl) {
            var data = JSON.parse(msg.data);
            if (data.action === "fb-login") {
                showLoading();
                setLoadingText("Loading...");
                authorizeFacebookAsync().then(function (fbAuthorized) {
                    if (fbAuthorized) {
                        setLoadingText("Loading Facebook info...");
                        iframe.src = recommendUri;
                    } else {
                        setLoadingText("Error: You must authorize Refer Engine to use your Facebook information.");
                    }
                });
            } else if (data.action === "cancel") {
                var dontAskAgain = data.dontAskAgain;
                if (dontAskAgain) {
                    autoAskAgain = false;
                }
                hide();
            } else if (data.action === "show-loading") {
                showLoading();
            } else if (data.action === "hide-loading") {
                hideLoading();
            } else if (data.action === "set-loading-text") {
                setLoadingText(data.text);
            } else if (data.action === "done") {
                hide();
                autoAskAgain = false;
            } else if (data.action === "intro-page-loaded") {
                introHasLoaded = true;
                authorizeAppAsync().done(function(result) {
                    if (!result) {
                        authorizeAppAsync();
                    }
                }, authorizeAppAsync);
            }
        }
    };
    //#endregion onWindowMessage

    var load = function() {
        setLoadingText("Loading...");
        iframe.src = introUrl;
        showLoadingRing();

        var timeout = 10000;
        setTimeout(function () {
            if (!introHasLoaded) {
                setLoadingText("This is taking longer than usual...");
                setTimeout(function () {
                    if (!introHasLoaded) {
                        setLoadingText("Almost there...");
                        setTimeout(function () {
                            if (!introHasLoaded) {
                                setLoadingText("Error: The service is not responding. Sorry for that. Please try again in a while.");
                            }
                        }, timeout);
                    }
                }, timeout);
            }
        }, timeout);
    };
    
    var initializeUi = function () {
        var style = create("style");
        style.innerText = ReferEngine.StyleSheetContent;
        style.type = "text/css";
        var firstStyle = document.getElementsByTagName("link")[0];
        firstStyle.parentNode.insertBefore(style, firstStyle);

        container = createDiv("re-container");
        var body = document.querySelector("body");
        body.appendChild(container);

        iframe = create("iframe");
        container.appendChild(iframe);

        loadingElem = createDiv("re-loading-container");
        container.appendChild(loadingElem);

        var closeElem = createDiv("re-close");
        closeElem.innerText = "";
        loadingElem.appendChild(closeElem);
        closeElem.addEventListener("click", hide);

        progress = create("progress");
        progress.className = "win-ring";
            
        var loadingInnerElem = createDiv();
        loadingElem.appendChild(loadingInnerElem);
            
        var logoMarkElem = createImg(logoMarkImageData, "re-logo-mark");
        var logoTextElem = createImg(logoTextImageData, "re-logo-text");
        loadingTextElem = createDiv("re-loading-text");
        loadingInnerElem.appendChild(logoMarkElem);
        loadingInnerElem.appendChild(logoTextElem);
        loadingInnerElem.appendChild(progress);
        loadingInnerElem.appendChild(loadingTextElem);

        window.addEventListener("message", onWindowMessage);

        load();
            
        uiInitialized = true;
    };

    var showMessageDialog = function(content, title) {
        var messageDialog = new Windows.UI.Popups.MessageDialog(content, title);
        messageDialog.showAsync();
    };

    var verifyInternetConnection = function() {
        var internetConnection = Windows.Networking.Connectivity.NetworkInformation.getInternetConnectionProfile();
        if (internetConnection == null)
        {
            var content = "ReferEngine requires a working internet connection.";
            var title = "Not Connected to the Internet";
            showMessageDialog(content, title);
            return false;
        }

        return true;
    };

    var show = function () {
        if (verifyInternetConnection()) {
            if (!uiInitialized) {
                initializeUi();
            } else {
                load();
            }
            WinJS.UI.Animation.fadeIn(container).then(function () {
                util.removeClass(container, "re-hidden");
            });
        }
    };
       
    var hide = function() {
        WinJS.UI.Animation.fadeOut(container).then(function () {
            util.addClass(container, "re-hidden");
        });
    };

    var autoOpenIfNeeded = function() {
        if (args) {
            var prevExecState = args.detail.previousExecutionState;
            var appExecutionState = Windows.ApplicationModel.Activation.ApplicationExecutionState;
            if (prevExecState !== appExecutionState.running && prevExecState !== appExecutionState.suspended) {
                launchCount++;
            }

            if (shouldAutoOpen == true) {
                setTimeout(function() {
                    window.msRequestAnimationFrame(function() {
                        show();
                    });
                }, autoOpenTimeout);
            }
        }
    };

    //#region public methods
    ReferEngine.Show = show;
    ReferEngine.Hide = hide;
    ReferEngine.IsAvailable = true;
    ReferEngine.IsSupportedRegion = isSupportedRegion;
    ReferEngine.IsLoaded = true;
    //#endregion public methods

    if (isSupportedRegion()) {
        if (ReferEngine.OnLoadArray) {
            for (var i = 0; i < ReferEngine.OnLoadArray.length; i++) {
                ReferEngine.OnLoadArray[i]();
            }
        }
        autoOpenIfNeeded();
    }
})();