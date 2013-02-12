﻿var ReferEngine;
(function (ReferEngine) {
    var referEngineAuthorizationToken;
    var logoMarkImageData = "data:image/png;base64,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", logoTextImageData = "data:image/png;base64,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", clientVariables = ReferEngineClient, roamingSettings = Windows.Storage.ApplicationData.current.roamingSettings, hiddenClassName = "re-hidden", introHasLoaded = false, store = Windows.ApplicationModel.Store, currentApp, args = clientVariables.appActivationArgs, util = WinJS.Utilities;
    if(!clientVariables.appId) {
        throw "ReferEngine.ClientVariables.appId must be defined.";
    }
    if(clientVariables.appIsPublished) {
        currentApp = store.CurrentApp;
    } else {
        currentApp = store.CurrentAppSimulator;
    }
    var isSupportedRegion = function () {
        var code = new Windows.Globalization.GeographicRegion().code;
        return code === "US" || code === "USA" || code === "840";
    };
    var AutoShow = (function () {
        function AutoShow() { }
        AutoShow.autoOpenTimeout = 15000;
        AutoShow._launchCount = 0;
        Object.defineProperty(AutoShow, "launchCount", {
            get: function () {
                var count = roamingSettings.values["ReferEngine-LaunchCount"];
                if(count === null) {
                    roamingSettings.values["ReferEngine-LaunchCount"] = 0;
                    return 0;
                }
                return count;
            },
            set: function (value) {
                roamingSettings.values["ReferEngine-LaunchCount"] = value;
            },
            enumerable: true,
            configurable: true
        });
        AutoShow._autoAskAgain = true;
        Object.defineProperty(AutoShow, "autoAskAgain", {
            get: function () {
                var autoAsk = roamingSettings.values["ReferEngine-AutoAskAgain"];
                if(autoAsk === null) {
                    roamingSettings.values["ReferEngine-AutoAskAgain"] = isSupportedRegion();
                    return isSupportedRegion();
                }
                return autoAsk;
            },
            set: function (value) {
                roamingSettings.values["ReferEngine-AutoAskAgain"] = value;
            },
            enumerable: true,
            configurable: true
        });
        AutoShow.shouldAutoOpen = function shouldAutoOpen() {
            return AutoShow.launchCount % 2 === 0 && AutoShow.autoAskAgain;
        };
        AutoShow.autoOpenIfNeeded = function autoOpenIfNeeded() {
            if(args) {
                var prevExecState = args.detail.previousExecutionState;
                var appExecutionState = Windows.ApplicationModel.Activation.ApplicationExecutionState;
                if(prevExecState !== appExecutionState.running && prevExecState !== appExecutionState.suspended) {
                    AutoShow.launchCount++;
                }
                if(AutoShow.shouldAutoOpen()) {
                    setTimeout(function () {
                        window.msRequestAnimationFrame(function () {
                            ReferEngine.show();
                        });
                    }, AutoShow.autoOpenTimeout);
                }
            }
        };
        return AutoShow;
    })();    
    var Initialization = (function () {
        function Initialization() { }
        Initialization.uiInitialized = false;
        Initialization.initializeUI = function initializeUI() {
            if(!Initialization.uiInitialized) {
                var style = Dom.CreateElement("style");
                style.innerText = clientVariables.style;
                style.type = "text/css";
                var firstStyle = document.getElementsByTagName("link")[0];
                firstStyle.parentNode.insertBefore(style, firstStyle);
                var body = document.querySelector("body");
                DomElements.container = Dom.CreateDiv("re-container");
                body.appendChild(DomElements.container);
                DomElements.iframe = Dom.CreateElement("iframe");
                DomElements.container.appendChild(DomElements.iframe);
                DomElements.loading = Dom.CreateDiv("re-loading-container");
                DomElements.container.appendChild(DomElements.loading);
                var closeElem = Dom.CreateDiv("re-close");
                closeElem.innerText = "";
                DomElements.loading.appendChild(closeElem);
                closeElem.addEventListener("click", ReferEngine.hide);
                DomElements.progress = Dom.CreateElement("progress");
                DomElements.progress.className = "win-ring";
                var loadingInnerElem = Dom.CreateDiv(null);
                DomElements.loading.appendChild(loadingInnerElem);
                var logoMarkElem = Dom.CreateImg(logoMarkImageData, "re-logo-mark");
                var logoTextElem = Dom.CreateImg(logoTextImageData, "re-logo-text");
                DomElements.loadingTextElem = Dom.CreateDiv("re-loading-text");
                loadingInnerElem.appendChild(logoMarkElem);
                loadingInnerElem.appendChild(logoTextElem);
                loadingInnerElem.appendChild(DomElements.progress);
                loadingInnerElem.appendChild(DomElements.loadingTextElem);
                window.addEventListener("message", Listener.onWindowMessage);
                Loading.loadIntro();
                Initialization.uiInitialized = true;
            }
        };
        return Initialization;
    })();    
    var Dom = (function () {
        function Dom() { }
        Dom.CreateElement = function CreateElement(name) {
            return document.createElement(name);
        };
        Dom.CreateDiv = function CreateDiv(className) {
            var elem = Dom.CreateElement("div");
            if(className !== null && className !== undefined) {
                elem.className = className;
            }
            return elem;
        };
        Dom.CreateImg = function CreateImg(src, className) {
            var elem = Dom.CreateElement("img");
            elem.src = src;
            elem.className = className;
            return elem;
        };
        return Dom;
    })();    
    var Loading = (function () {
        function Loading() { }
        Loading.hideRing = function hideRing() {
            DomElements.progress.style.opacity = "0";
        };
        Loading.showRing = function showRing() {
            DomElements.progress.style.opacity = "1";
        };
        Loading.show = function show() {
            WinJS.UI.Animation.fadeIn(DomElements.loading).then(function () {
                util.removeClass(DomElements.loading, hiddenClassName);
            });
        };
        Loading.hide = function hide() {
            WinJS.UI.Animation.fadeOut(DomElements.loading).then(function () {
                util.addClass(DomElements.loading, hiddenClassName);
            });
        };
        Loading.setText = function setText(text) {
            if(text.substr(0, 5) === "Error") {
                Loading.hideRing();
            }
            DomElements.loadingTextElem.innerText = text;
        };
        Loading.loadIntro = function loadIntro() {
            Initialization.initializeUI();
            DomElements.iframe.src = Url.intro;
            Loading.setText("Loading...");
            Loading.showRing();
            var timeout = 10000;
            setTimeout(function () {
                if(!introHasLoaded) {
                    Loading.setText("This is taking longer than usual...");
                    setTimeout(function () {
                        if(!introHasLoaded) {
                            Loading.setText("Almost there...");
                            setTimeout(function () {
                                if(!introHasLoaded) {
                                    Loading.setText("Error: The service is not responding. Sorry for that. Please try again in a while.");
                                }
                            }, timeout);
                        }
                    }, timeout);
                }
            }, timeout);
        };
        return Loading;
    })();    
    var Listener = (function () {
        function Listener() { }
        Listener.onWindowMessage = function onWindowMessage(msg) {
            if(msg.origin === Url.base) {
                var data = JSON.parse(msg.data);
                if(data.action === "fb-login") {
                    Loading.show();
                    Loading.setText("Loading...");
                    Auth.authFacebookAsync().then(function (fbAuthorized) {
                        if(fbAuthorized) {
                            Loading.setText("Loading Facebook info...");
                            DomElements.iframe.src = Url.recommend;
                        } else {
                            Loading.setText("Error: You must authorize Refer Engine to use your Facebook information.");
                        }
                    });
                } else if(data.action === "cancel") {
                    var dontAskAgain = data.dontAskAgain;
                    if(dontAskAgain) {
                        AutoShow.autoAskAgain = false;
                    }
                    ReferEngine.hide();
                } else if(data.action === "show-loading") {
                    Loading.show();
                } else if(data.action === "hide-loading") {
                    Loading.hide();
                } else if(data.action === "set-loading-text") {
                    Loading.setText(data.text);
                } else if(data.action === "done") {
                    ReferEngine.hide();
                    AutoShow.autoAskAgain = false;
                } else if(data.action === "intro-page-loaded") {
                    introHasLoaded = true;
                    Auth.authorizeAppAsync().done(function (result) {
                        if(!result) {
                            Auth.authorizeAppAsync();
                        }
                    }, Auth.authorizeAppAsync);
                    Loading.hide();
                }
            }
        };
        return Listener;
    })();    
})(ReferEngine || (ReferEngine = {}));
