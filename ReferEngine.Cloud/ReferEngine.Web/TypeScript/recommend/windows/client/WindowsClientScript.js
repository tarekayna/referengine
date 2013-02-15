define(["require", "exports", "../common/Messaging", "../common/Functions"], function(require, exports, __Messaging__, __Functions__) {
    var Messaging = __Messaging__;

    var Functions = __Functions__;

    (function (ReferEngine) {
        var logoMarkImageData = "data:image/png;base64,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", logoTextImageData = "data:image/png;base64,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", styleText = ".re-container{opacity:0;height:100%;width:100%;z-index:9999999;position:absolute;top:0;left:0;background-color:rgba(0,0,0,.6)}.re-logo-text{margin-left:5px}.re-container iframe,.re-container .re-loading-container{position:absolute;height:600px;width:900px;top:calc((100% - 600px)/2);left:calc((100% - 900px)/2)}.re-loading-container{display:-ms-flexbox;-ms-flex-pack:center;-ms-flex-align:center;background-color:#fff;color:#000}.re-loading-container>div:not(.re-close){width:300px;height:auto;display:-ms-flexbox;-ms-flex-wrap:wrap;-ms-flex-pack:center}.re-loading-container img.re-logo-mark{height:50px;width:50px}.re-loading-container .re-loading-text{text-align:center;width:300px}.re-loading-container progress.win-ring{height:40px;width:40px;margin:15px 0;color:#38b0e6}.re-hidden{display:none!important}.re-close{position:absolute;width:50px;height:50px;padding-top:9px;top:0;right:0;font-size:x-large;font-family:'Segoe UI Symbol';vertical-align:middle;text-align:center;color:#ccc;cursor:pointer}", client = ReferEngineClient, roamingSettings = Windows.Storage.ApplicationData.current.roamingSettings, introHasLoaded = false, referEngineFacebookAppId = "368842109866922", store = Windows.ApplicationModel.Store, currentApp, uiInitialized = false, args = client.appActivationArgs, util = WinJS.Utilities, messenger, functionRange, clientFunction = Functions.ClientFunction;
        if(!client.appId) {
            throw "ReferEngineClient.appId must be defined.";
        }
        if(client.appIsPublished) {
            currentApp = store.CurrentApp;
        } else {
            currentApp = store.CurrentAppSimulator;
        }
        function initializeUI() {
            if(!uiInitialized) {
                var style = Dom.createElement("style");
                style.innerText = styleText;
                style.type = "text/css";
                var firstStyle = document.getElementsByTagName("link")[0];
                firstStyle.parentNode.insertBefore(style, firstStyle);
                var body = document.querySelector("body");
                Dom.container = Dom.createDiv("re-container");
                body.appendChild(Dom.container);
                Dom.iframe = Dom.createElement("iframe");
                Dom.container.appendChild(Dom.iframe);
                Dom.loading = Dom.createDiv("re-loading-container");
                Dom.container.appendChild(Dom.loading);
                var closeElem = Dom.createDiv("re-close");
                closeElem.innerText = "";
                Dom.loading.appendChild(closeElem);
                closeElem.addEventListener("click", function () {
                    messenger.sendTextMessage("loading-closed");
                    ReferEngine.hide();
                });
                Dom.progress = Dom.createElement("progress");
                Dom.progress.className = "win-ring";
                var loadingInnerElem = Dom.createDiv(null);
                Dom.loading.appendChild(loadingInnerElem);
                var logoMarkElem = Dom.createImg(logoMarkImageData, "re-logo-mark");
                var logoTextElem = Dom.createImg(logoTextImageData, "re-logo-text");
                Dom.loadingTextElem = Dom.createDiv("re-loading-text");
                loadingInnerElem.appendChild(logoMarkElem);
                loadingInnerElem.appendChild(logoTextElem);
                loadingInnerElem.appendChild(Dom.progress);
                loadingInnerElem.appendChild(Dom.loadingTextElem);
                uiInitialized = true;
                messenger.iframe = Dom.iframe;
            }
        }
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
                        roamingSettings.values["ReferEngine-AutoAskAgain"] = true;
                        return true;
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
        var Url = (function () {
            function Url() { }
            Url.base = "http://127.0.0.1:81";
            Url.intro = Url.base + "/recommend/win8/intro/" + client.appId;
            Url.auth = Url.base + "/api/win8/auth";
            return Url;
        })();        
        var Dom = (function () {
            function Dom() { }
            Dom.createElement = function createElement(name) {
                return document.createElement(name);
            };
            Dom.createDiv = function createDiv(className) {
                var elem = Dom.createElement("div");
                if(className !== null && className !== undefined) {
                    elem.className = className;
                }
                return elem;
            };
            Dom.createImg = function createImg(src, className) {
                var elem = Dom.createElement("img");
                elem.src = src;
                elem.className = className;
                return elem;
            };
            Dom.hideElement = function hideElement(elem) {
                util.addClass(elem, "re-hidden");
            };
            Dom.unHideElement = function unHideElement(elem) {
                util.removeClass(elem, "re-hidden");
            };
            return Dom;
        })();        
        var Loading = (function () {
            function Loading() { }
            Loading.hideRing = function hideRing() {
                Dom.progress.style.opacity = "0";
            };
            Loading.showRing = function showRing() {
                Dom.progress.style.opacity = "1";
            };
            Loading.show = function show() {
                WinJS.UI.Animation.enterContent(Dom.loading).then(function () {
                    Dom.unHideElement(Dom.loading);
                });
            };
            Loading.hide = function hide() {
                WinJS.UI.Animation.fadeOut(Dom.loading).then(function () {
                    Dom.hideElement(Dom.loading);
                });
            };
            Loading.setText = function setText(text) {
                if(text.substr(0, 5) === "Error") {
                    Loading.hideRing();
                }
                Dom.loadingTextElem.innerText = text;
            };
            Loading.loadIntro = function loadIntro() {
                initializeUI();
                Dom.iframe.src = Url.intro;
                Loading.showRing();
                var messages = [
                    "Recommend to your friends", 
                    "Win cash", 
                    "Share the love", 
                    "Earn a reward"
                ];
                var msgIndex = 0;
                var event = document.createEvent("CustomEvent");
                event.initCustomEvent("showNextLoadingMessage", true, true, null);
                document.addEventListener("showNextLoadingMessage", function () {
                    Loading.setText(messages[msgIndex]);
                    msgIndex = (msgIndex === messages.length - 1) ? 0 : msgIndex + 1;
                    if(!introHasLoaded) {
                        setTimeout(function () {
                            document.dispatchEvent(event);
                        }, 3000);
                    }
                });
                document.dispatchEvent(event);
            };
            return Loading;
        })();        
        var Auth = (function () {
            function Auth() { }
            Auth.authorizeFacebookAsync = function authorizeFacebookAsync() {
                return new WinJS.Promise(function (comp, error, prog) {
                    var callback = "https://www.referengine.com/recommend/win8/success";
                    var query = "client_id=" + referEngineFacebookAppId + "&redirect_uri=" + callback + "&scope=email,publish_actions&display=popup&response_type=code";
                    var facebookUrl = "https://www.facebook.com/dialog/oauth?" + query;
                    var startUri = new Windows.Foundation.Uri(facebookUrl);
                    var endUri = new Windows.Foundation.Uri(callback);
                    var web = Windows.Security.Authentication.Web;
                    web.WebAuthenticationBroker.authenticateAsync(web.WebAuthenticationOptions.none, startUri, endUri).done(function (request) {
                        if(request.responseData.indexOf("error_reason=") !== -1 || request.responseErrorDetail === 404) {
                            comp(new Messaging.AuthResult(false));
                        }
                        var codeString = "code=";
                        var start = request.responseData.indexOf(codeString) + codeString.length;
                        var code = request.responseData.substr(start);
                        if(code && code !== null && code !== "") {
                            comp(new Messaging.AuthResult(true, code));
                        } else {
                            comp(new Messaging.AuthResult(false));
                        }
                    }, function () {
                        comp(new Messaging.AuthResult(false));
                    });
                });
            };
            Auth.authorizeAppAsync = function authorizeAppAsync() {
                return new WinJS.Promise(function (comp, error, prog) {
                    currentApp.getAppReceiptAsync().done(function (xml) {
                        xml = "<xml>" + xml + "</xml>";
                        var xml64 = window.btoa(xml);
                        WinJS.xhr({
                            type: "POST",
                            headers: {
                                "Content-type": "text/xml"
                            },
                            url: Url.auth,
                            data: xml64
                        }).done(function (request) {
                            var data = JSON.parse(request.responseText).Data;
                            if(data.token && data.token !== null && data.token !== "") {
                                comp(new Messaging.AuthResult(true, data.token));
                            } else {
                                comp(new Messaging.AuthResult(false));
                            }
                        }, function () {
                            comp(new Messaging.AuthResult(false));
                        });
                    });
                });
            };
            return Auth;
        })();        
        function verifyInternetConnection() {
            var internetConnection = Windows.Networking.Connectivity.NetworkInformation.getInternetConnectionProfile();
            if(internetConnection === null) {
                var content = "ReferEngine requires a working internet connection.";
                var title = "Not Connected to the Internet";
                var messageDialog = new Windows.UI.Popups.MessageDialog(content, title);
                messageDialog.showAsync();
                return false;
            }
            return true;
        }
        function show() {
            if(verifyInternetConnection()) {
                Loading.loadIntro();
                WinJS.UI.Animation.fadeIn(Dom.container).then(function () {
                    Dom.unHideElement(Dom.container);
                });
            }
        }
        ReferEngine.show = show;
        ;
        function hide() {
            WinJS.UI.Animation.fadeOut(Dom.container).then(function () {
                Dom.hideElement(Dom.container);
            });
        }
        ReferEngine.hide = hide;
        ;
        ReferEngine.isAvailable = true;
        var functions = [
            new Functions.Function(clientFunction.navigate, function (details) {
                Dom.iframe.src = details.url;
            }), 
            new Functions.Function(clientFunction.showLoading, function (details) {
                Dom.iframe.src = details.url;
            }), 
            new Functions.Function(clientFunction.hideLoading, function (details) {
                Loading.hide();
            }), 
            new Functions.Function(clientFunction.setLoadingText, function (details) {
                Loading.setText(details.text);
            }), 
            new Functions.Function(clientFunction.setAutoAsk, function (details) {
                AutoShow.autoAskAgain = details.askAgain;
            }), 
            new Functions.Function(clientFunction.hide, function (details) {
                ReferEngine.hide();
            }), 
            new Functions.Function(clientFunction.authApp, function (details) {
                Auth.authorizeAppAsync().done(function (authResult) {
                    messenger.send(new Messaging.Message("auth-app-result", {
                        authResult: authResult
                    }));
                });
            }), 
            new Functions.Function(clientFunction.authFacebook, function (details) {
                Auth.authorizeFacebookAsync().then(function (authResult) {
                    messenger.send(new Messaging.Message("auth-facebook-result", {
                        authResult: authResult
                    }));
                });
            }), 
            new Functions.Function(clientFunction.setIntroPageLoaded, function (details) {
                introHasLoaded = true;
            })
        ];
        messenger = new Messaging.Messenger(Messaging.MessengerType.ClientToIframe, Url.base);
        functionRange = new Functions.FunctionRange(messenger);
        functionRange.addRange(functions);
        function onLaunch() {
            WinJS.xhr({
                type: "POST",
                headers: {
                    "Content-type": "text/xml"
                },
                url: "http://127.0.0.1:81/recommend/win8/connect",
                data: ""
            }).done(function (request) {
            }, function () {
            });
        }
        ;
        if(client.onLoadArray) {
            for(var i = 0; i < client.onLoadArray.length; i++) {
                client.onLoadArray[i]();
            }
        }
        AutoShow.autoOpenIfNeeded();
        window["ReferEngine"] = ReferEngine;
    })(exports.ReferEngine || (exports.ReferEngine = {}));
    var ReferEngine = exports.ReferEngine;
})
