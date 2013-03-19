define(["require", "exports", "../common/Messaging", "../common/Functions"], function(require, exports, __Messaging__, __Functions__) {
    var Messaging = __Messaging__;

    var Functions = __Functions__;

    var ReferEngine;
    (function (ReferEngine) {
        var client = ReferEngineClient, currentAppData = Windows.Storage.ApplicationData.current, roamingSettings = currentAppData.roamingSettings, localSettings = currentAppData.localSettings, introHasLoaded = false, uiInitialized = false, currentApp = client.currentApp, args = client.appActivationArgs, util = WinJS.Utilities, anim = WinJS.UI.Animation, messenger, functionRange, clientFunction = Functions.ClientFunction, serverFunction = Functions.ServerFunction;
        ReferEngine.isAvailable = false;
        function initializeUI() {
            if(!uiInitialized) {
                var style = Dom.createElement("style");
                style.innerText = RemoteOptions.style;
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
                    messenger.call(serverFunction.closedWhileLoading);
                    ReferEngine.hide();
                });
                var meter = Dom.createDiv("meter");
                Dom.progressSpan = Dom.createElement("span");
                meter.appendChild(Dom.progressSpan);
                var loadingInnerElem = Dom.createDiv(null);
                Dom.loading.appendChild(loadingInnerElem);
                var logoMarkElem = Dom.createImg(RemoteOptions.logoMarkImageData, "re-logo-mark");
                var logoTextElem = Dom.createImg(RemoteOptions.logoTextImageData, "re-logo-text");
                Dom.loadingTextElem = Dom.createDiv("re-loading-text");
                loadingInnerElem.appendChild(logoMarkElem);
                loadingInnerElem.appendChild(logoTextElem);
                loadingInnerElem.appendChild(meter);
                loadingInnerElem.appendChild(Dom.loadingTextElem);
                uiInitialized = true;
                messenger.iframe = Dom.iframe;
            }
        }
        var RemoteOptions = (function () {
            function RemoteOptions() { }
            RemoteOptions._styleKey = "ReferEngine-Style";
            Object.defineProperty(RemoteOptions, "style", {
                get: function () {
                    return localSettings.values[RemoteOptions._styleKey];
                },
                set: function (value) {
                    localSettings.values[RemoteOptions._styleKey] = value;
                },
                enumerable: true,
                configurable: true
            });
            RemoteOptions._loadingMessagesKey = "ReferEngine-LoadingMessages";
            Object.defineProperty(RemoteOptions, "loadingMessages", {
                get: function () {
                    if(RemoteOptions._loadingMessagesCached) {
                        return RemoteOptions._loadingMessagesCached;
                    }
                    var storeValue = localSettings.values[RemoteOptions._loadingMessagesKey];
                    RemoteOptions._loadingMessagesCached = (JSON.parse(storeValue)).value;
                    return RemoteOptions._loadingMessagesCached;
                },
                set: function (value) {
                    RemoteOptions._loadingMessagesCached = value;
                    var storeValue = JSON.stringify({
                        value: value
                    });
                    localSettings.values[RemoteOptions._loadingMessagesKey] = storeValue;
                },
                enumerable: true,
                configurable: true
            });
            RemoteOptions._logoMarkImageDataKey = "ReferEngine-LogoMarkImageData";
            Object.defineProperty(RemoteOptions, "logoMarkImageData", {
                get: function () {
                    return localSettings.values[RemoteOptions._logoMarkImageDataKey];
                },
                set: function (value) {
                    localSettings.values[RemoteOptions._logoMarkImageDataKey] = value;
                },
                enumerable: true,
                configurable: true
            });
            RemoteOptions._logoTextImageDataKey = "ReferEngine-LogoTextImageData";
            Object.defineProperty(RemoteOptions, "logoTextImageData", {
                get: function () {
                    return localSettings.values[RemoteOptions._logoTextImageDataKey];
                },
                set: function (value) {
                    localSettings.values[RemoteOptions._logoTextImageDataKey] = value;
                },
                enumerable: true,
                configurable: true
            });
            RemoteOptions._fbScopeKey = "ReferEngine-FacebookScope";
            Object.defineProperty(RemoteOptions, "fbScope", {
                get: function () {
                    return localSettings.values[RemoteOptions._fbScopeKey];
                },
                set: function (value) {
                    localSettings.values[RemoteOptions._fbScopeKey] = value;
                },
                enumerable: true,
                configurable: true
            });
            RemoteOptions._appIdKey = "ReferEngine-AppId";
            Object.defineProperty(RemoteOptions, "appId", {
                get: function () {
                    return localSettings.values[RemoteOptions._appIdKey];
                },
                set: function (value) {
                    localSettings.values[RemoteOptions._appIdKey] = value;
                },
                enumerable: true,
                configurable: true
            });
            return RemoteOptions;
        })();        
        var AutoShowOptions = (function () {
            function AutoShowOptions() { }
            AutoShowOptions._timeoutDefault = 15000;
            AutoShowOptions._timeoutKey = "ReferEngine-AutoShowTimeout";
            Object.defineProperty(AutoShowOptions, "timeout", {
                get: function () {
                    var timeout = roamingSettings.values[AutoShowOptions._timeoutKey];
                    if(!timeout) {
                        roamingSettings.values[AutoShowOptions._timeoutKey] = AutoShowOptions._timeoutDefault;
                        return AutoShowOptions._timeoutDefault;
                    }
                    return timeout;
                },
                set: function (value) {
                    roamingSettings.values[AutoShowOptions._timeoutKey] = value;
                },
                enumerable: true,
                configurable: true
            });
            AutoShowOptions._intervalDefault = 2;
            AutoShowOptions._intervalKey = "ReferEngine-AutoShowInterval";
            Object.defineProperty(AutoShowOptions, "interval", {
                get: function () {
                    var interval = roamingSettings.values[AutoShowOptions._intervalKey];
                    if(!interval) {
                        roamingSettings.values[AutoShowOptions._intervalKey] = AutoShowOptions._intervalDefault;
                        return AutoShowOptions._intervalDefault;
                    }
                    return interval;
                },
                set: function (value) {
                    roamingSettings.values[AutoShowOptions._intervalKey] = value;
                },
                enumerable: true,
                configurable: true
            });
            AutoShowOptions._enableDefault = true;
            AutoShowOptions._enableKey = "ReferEngine-AutoShowEnable";
            Object.defineProperty(AutoShowOptions, "enable", {
                get: function () {
                    var enable = roamingSettings.values[AutoShowOptions._enableKey];
                    if(!enable) {
                        roamingSettings.values[AutoShowOptions._enableKey] = AutoShowOptions._enableDefault;
                        return AutoShowOptions._enableDefault;
                    }
                    return enable;
                },
                set: function (value) {
                    roamingSettings.values[AutoShowOptions._enableKey] = value;
                },
                enumerable: true,
                configurable: true
            });
            return AutoShowOptions;
        })();        
        var AutoShow = (function () {
            function AutoShow() { }
            AutoShow._launchCountKey = "ReferEngine-LaunchCount";
            Object.defineProperty(AutoShow, "launchCount", {
                get: function () {
                    var count = roamingSettings.values[AutoShow._launchCountKey];
                    if(!count) {
                        roamingSettings.values[AutoShow._launchCountKey] = 0;
                        return 0;
                    }
                    return count;
                },
                set: function (value) {
                    roamingSettings.values[AutoShow._launchCountKey] = value;
                },
                enumerable: true,
                configurable: true
            });
            AutoShow._autoAskAgainKey = "ReferEngine-AutoAskAgain";
            Object.defineProperty(AutoShow, "autoAskAgain", {
                get: function () {
                    var autoAsk = roamingSettings.values[AutoShow._autoAskAgainKey];
                    if(!autoAsk) {
                        roamingSettings.values[AutoShow._autoAskAgainKey] = AutoShowOptions.enable;
                        return AutoShowOptions.enable;
                    }
                    return autoAsk;
                },
                set: function (value) {
                    roamingSettings.values[AutoShow._autoAskAgainKey] = value;
                },
                enumerable: true,
                configurable: true
            });
            AutoShow.shouldAutoOpen = function shouldAutoOpen() {
                return AutoShow.launchCount % AutoShowOptions.interval === 0 && AutoShow.autoAskAgain;
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
                                ReferEngine.show(true);
                            });
                        }, AutoShowOptions.timeout);
                    }
                }
            };
            return AutoShow;
        })();        
        var Url = (function () {
            function Url() { }
            Url.base = "https://www.ReferEngine.com";
            Url.auth = Url.base + "/recommend/win8/authorizeappcode";
            Url.getIntroUrl = function getIntroUrl(isAutoOpen) {
                return Url.base + "/recommend/win8/intro/" + RemoteOptions.appId + "?isAutoOpen=" + (isAutoOpen ? "true" : "false");
            };
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
                Dom.progressSpan.style.visibility = "hidden";
            };
            Loading.showRing = function showRing() {
                Dom.progressSpan.style.visibility = "visible";
            };
            Loading.show = function show() {
                anim.enterContent(Dom.loading).then(function () {
                    Dom.unHideElement(Dom.loading);
                });
            };
            Loading.hide = function hide() {
                anim.fadeOut(Dom.loading).then(function () {
                    Dom.hideElement(Dom.loading);
                });
            };
            Loading.setText = function setText(text) {
                if(text.substr(0, 5) === "Error") {
                    Loading.hideRing();
                }
                anim.exitContent(Dom.loadingTextElem).done(function () {
                    Dom.loadingTextElem.innerText = text;
                    anim.enterContent(Dom.loadingTextElem);
                });
            };
            Loading.loadIntro = function loadIntro(isAutoOpen) {
                initializeUI();
                navigate(Url.getIntroUrl(isAutoOpen));
                Loading.show();
                Loading.showRing();
                var msgIndex = 0;
                var event = document.createEvent("CustomEvent");
                event.initCustomEvent("showNextLoadingMessage", true, true, null);
                document.addEventListener("showNextLoadingMessage", function () {
                    Loading.setText(RemoteOptions.loadingMessages[msgIndex]);
                    msgIndex = (msgIndex === RemoteOptions.loadingMessages.length - 1) ? 0 : msgIndex + 1;
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
            Auth._token = "ReferEngine-AuthToken";
            Auth._expires = "ReferEngine-TokenExpiresAt";
            Auth.getStoredToken = function getStoredToken() {
                function resetAndReturnNull() {
                    localSettings.values[Auth._expires] = null;
                    localSettings.values[Auth._token] = null;
                    return null;
                }
                var expiresAt = localSettings.values[Auth._expires];
                var token = localSettings.values[Auth._token];
                if(!expiresAt || !token) {
                    return resetAndReturnNull();
                }
                var date = new Date();
                var time = date.getTime();
                var threshold = 10 * 60;
                if(expiresAt - threshold < time) {
                    return resetAndReturnNull();
                }
                return token;
            };
            Auth.setStoredToken = function setStoredToken(value, expiresIn) {
                var expiresAt = new Date();
                expiresAt.setSeconds(expiresAt.getSeconds() + expiresIn);
                localSettings.values[Auth._token] = value;
                localSettings.values[Auth._expires] = expiresAt;
            };
            Auth.authorizeFacebookAsync = function authorizeFacebookAsync() {
                return new WinJS.Promise(function (comp, error, prog) {
                    var callback = "https://www.referengine.com/recommend/win8/success";
                    var query = "client_id=368842109866922&redirect_uri=" + callback + "&scope=" + RemoteOptions.fbScope + "&display=popup&response_type=code";
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
                        if(code) {
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
                    var storedToken = Auth.getStoredToken();
                    if(storedToken) {
                        comp(storedToken);
                    } else {
                        currentApp.getAppReceiptAsync().done(function (xml) {
                            WinJS.xhr({
                                type: "POST",
                                url: Url.auth,
                                headers: {
                                    "Content-type": "application/x-www-form-urlencoded"
                                },
                                data: "appReceiptXml=" + xml + "&appVerificationCode=" + ReferEngineClient.appVerificationCode
                            }).done(function (request) {
                                var data = JSON.parse(request.responseText);
                                if(data.token && data.expiresIn) {
                                    Auth.setStoredToken(data.token, data.expiresIn);
                                    AutoShowOptions.enable = data.asoEnabled;
                                    AutoShowOptions.interval = data.asoInterval;
                                    AutoShowOptions.timeout = data.asoTimeout;
                                    RemoteOptions.style = data.style;
                                    RemoteOptions.loadingMessages = data.loadingMessages;
                                    RemoteOptions.logoMarkImageData = data.logoMarkImageData;
                                    RemoteOptions.logoTextImageData = data.logoTextImageData;
                                    RemoteOptions.fbScope = data.fbScope;
                                    RemoteOptions.appId = data.appId;
                                    comp(data.token);
                                } else {
                                    comp(null);
                                }
                            }, function (request) {
                                console.error("ReferEngine: could not authorize app with ReferEngine.com.");
                                if(request.statusText) {
                                    console.error("ReferEngine - Message from server: " + request.statusText);
                                }
                                comp(null);
                            });
                        });
                    }
                });
            };
            return Auth;
        })();        
        function isConnectedToTheInternet() {
            var internetConnection = Windows.Networking.Connectivity.NetworkInformation.getInternetConnectionProfile();
            if(!internetConnection) {
                var content = "ReferEngine requires a working internet connection.";
                var title = "Not Connected to the Internet";
                var messageDialog = new Windows.UI.Popups.MessageDialog(content, title);
                messageDialog.showAsync();
                return false;
            }
            return true;
        }
        var isHidden = true;
        function show(isAutoOpen) {
            if(isConnectedToTheInternet() && isHidden) {
                Loading.loadIntro(isAutoOpen);
                anim.fadeIn(Dom.container).then(function () {
                    Dom.unHideElement(Dom.container);
                });
                isHidden = false;
            }
        }
        ReferEngine.show = show;
        ;
        function hide() {
            anim.fadeOut(Dom.container).then(function () {
                Dom.hideElement(Dom.container);
            });
            isHidden = true;
        }
        ReferEngine.hide = hide;
        ;
        function reset() {
            Auth.setStoredToken(null, 0);
            RemoteOptions.fbScope = null;
            RemoteOptions.loadingMessages = [];
            RemoteOptions.logoMarkImageData = null;
            RemoteOptions.logoTextImageData = null;
            RemoteOptions.style = null;
            AutoShowOptions.enable = null;
            AutoShowOptions.interval = null;
            AutoShowOptions.timeout = null;
            AutoShow.autoAskAgain = true;
            AutoShow.launchCount = 0;
            RemoteOptions.appId = null;
        }
        ReferEngine.reset = reset;
        ;
        function navigate(url) {
            if(url.indexOf("?") == -1) {
                return Auth.authorizeAppAsync().done(function (token) {
                    Dom.iframe.src = url + "?authToken=" + token;
                });
            } else {
                return Auth.authorizeAppAsync().done(function (token) {
                    Dom.iframe.src = url + "&authToken=" + token;
                });
            }
        }
        var functions = [
            new Functions.Function(clientFunction.navigate, function (details) {
                navigate(details.url);
            }), 
            new Functions.Function(clientFunction.showLoading, function (details) {
                Loading.show();
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
            new Functions.Function(clientFunction.authFacebook, function (details) {
                Auth.authorizeFacebookAsync().then(function (authResult) {
                    messenger.call(serverFunction.authFacebookResult, {
                        authResult: authResult
                    });
                });
            }), 
            new Functions.Function(clientFunction.setIntroPageLoaded, function (details) {
                introHasLoaded = true;
                messenger.call(serverFunction.introVisible);
            })
        ];
        messenger = new Messaging.Messenger(Messaging.MessengerType.ClientToIframe, Url.base);
        functionRange = new Functions.FunctionRange(messenger);
        functionRange.addRange(functions);
        function onLaunchAsync() {
            return Auth.authorizeAppAsync().done(function (token) {
                if(token) {
                    if(client.onLoadArray) {
                        for(var i = 0; i < client.onLoadArray.length; i++) {
                            client.onLoadArray[i]();
                        }
                    }
                    AutoShow.autoOpenIfNeeded();
                    ReferEngine.isAvailable = true;
                }
            });
        }
        ;
        window["ReferEngine"] = ReferEngine;
        onLaunchAsync();
    })(ReferEngine || (ReferEngine = {}));
})
