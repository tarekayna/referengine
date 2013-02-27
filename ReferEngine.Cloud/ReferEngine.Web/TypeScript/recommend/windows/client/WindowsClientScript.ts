///<reference path='..\common\winrt.d.ts' static='true' />
///<reference path='ReferEngineClient.ts' static='true' />

declare var WinJS;

import Messaging = module("../common/Messaging");
import Functions = module("../common/Functions");

module ReferEngine {
    var client = ReferEngineClient,
        currentAppData = Windows.Storage.ApplicationData.current,
        roamingSettings = currentAppData.roamingSettings,
        localSettings = currentAppData.localSettings,
        introHasLoaded = false,
        uiInitialized: bool = false,
        currentApp = client.currentApp,
        args = client.appActivationArgs,
        util = WinJS.Utilities,
        anim = WinJS.UI.Animation,
        messenger: Messaging.Messenger,
        functionRange: Functions.FunctionRange,
        clientFunction = Functions.ClientFunction,
        serverFunction = Functions.ServerFunction;

    export var isAvailable = false;

    function initializeUI() {
        if (!uiInitialized) {
            var style: HTMLStyleElement = Dom.createElement("style");
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

    class RemoteOptions {
        private static _styleKey = "ReferEngine-Style";
        static get style(): string {
            return localSettings.values[_styleKey];
        }
        static set style(value: string) {
            localSettings.values[_styleKey] = value;
        }

        private static _loadingMessagesKey = "ReferEngine-LoadingMessages";
        private static _loadingMessagesCached;
        static get loadingMessages() {
            if (_loadingMessagesCached) {
                return _loadingMessagesCached;
            }
            var storeValue = localSettings.values[_loadingMessagesKey];
            _loadingMessagesCached = (JSON.parse(storeValue)).value;
            return _loadingMessagesCached;
        }
        static set loadingMessages(value) {
            _loadingMessagesCached = value;
            var storeValue = JSON.stringify({ value: value });
            localSettings.values[_loadingMessagesKey] = storeValue;
        }

        private static _logoMarkImageDataKey = "ReferEngine-LogoMarkImageData";
        static get logoMarkImageData(): string {
            return localSettings.values[_logoMarkImageDataKey];
        }
        static set logoMarkImageData(value: string) {
            localSettings.values[_logoMarkImageDataKey] = value;
        }

        private static _logoTextImageDataKey = "ReferEngine-LogoTextImageData";
        static get logoTextImageData(): string {
            return localSettings.values[_logoTextImageDataKey];
        }
        static set logoTextImageData(value: string) {
            localSettings.values[_logoTextImageDataKey] = value;
        }

        private static _fbScopeKey = "ReferEngine-FacebookScope";
        static get fbScope(): string {
            return localSettings.values[_fbScopeKey];
        }
        static set fbScope(value: string) {
            localSettings.values[_fbScopeKey] = value;
        }

        private static _appIdKey = "ReferEngine-AppId";
        static get appId(): number {
            return localSettings.values[_appIdKey];
        }
        static set appId(value: number) {
            localSettings.values[_appIdKey] = value;
        }
    }

    class AutoShowOptions {
        private static _timeoutDefault = 15000;
        private static _timeoutKey = "ReferEngine-AutoShowTimeout";
        static get timeout(): number {
            var timeout = roamingSettings.values[_timeoutKey];
            if (!timeout) {
                roamingSettings.values[_timeoutKey] = _timeoutDefault;
                return _timeoutDefault;
            }
            return timeout;
        }
        static set timeout(value: number) {
            roamingSettings.values[_timeoutKey] = value;
        }

        private static _intervalDefault = 2;
        private static _intervalKey = "ReferEngine-AutoShowInterval";
        static get interval(): number {
            var interval = roamingSettings.values[_intervalKey];
            if (!interval) {
                roamingSettings.values[_intervalKey] = _intervalDefault;
                return _intervalDefault;
            }
            return interval;
        }
        static set interval(value: number) {
            roamingSettings.values[_intervalKey] = value;
        }

        private static _enableDefault = true;
        private static _enableKey = "ReferEngine-AutoShowEnable";
        static get enable(): bool {
            var enable = roamingSettings.values[_enableKey];
            if (!enable) {
                roamingSettings.values[_enableKey] = _enableDefault;
                return _enableDefault;
            }
            return enable;
        }
        static set enable(value: bool) {
            roamingSettings.values[_enableKey] = value;
        }
    }

    class AutoShow {
        private static _launchCountKey = "ReferEngine-LaunchCount";
        static get launchCount(): number {
            var count = roamingSettings.values[_launchCountKey];
            if (!count) {
                roamingSettings.values[_launchCountKey] = 0;
                return 0;
            }
            return count;
        }
        static set launchCount(value: number) {
            roamingSettings.values[_launchCountKey] = value;
        }

        private static _autoAskAgainKey = "ReferEngine-AutoAskAgain";
        static get autoAskAgain(): bool {
            var autoAsk = roamingSettings.values[_autoAskAgainKey];
            if (!autoAsk) {
                roamingSettings.values[_autoAskAgainKey] = AutoShowOptions.enable;
                return AutoShowOptions.enable;
            }
            return autoAsk;
        }
        static set autoAskAgain(value: bool) {
            roamingSettings.values[_autoAskAgainKey] = value;
        }

        static private shouldAutoOpen() {
            return launchCount % AutoShowOptions.interval === 0 && autoAskAgain;
        }

        static autoOpenIfNeeded() {
            if (args) {
                var prevExecState = args.detail.previousExecutionState;
                var appExecutionState = Windows.ApplicationModel.Activation.ApplicationExecutionState;
                if (prevExecState !== appExecutionState.running && prevExecState !== appExecutionState.suspended) {
                    launchCount++;
                }
                if (shouldAutoOpen()) {
                    setTimeout(function () {
                        window.msRequestAnimationFrame(function () {
                            ReferEngine.show(true);
                        });
                    }, AutoShowOptions.timeout);
                }
            }
        }
    }
      
    class Url {
        static base: string = "http://127.0.0.1:81";
        //static base: string = "https://www.referengine-test.com";
        //static base: string = "https://www.referengine.com";
        static auth: string = Url.base + "/recommend/win8/authorizeapp";
        static getIntroUrl(isAutoOpen: bool) {
            return Url.base + "/recommend/win8/intro/" + RemoteOptions.appId + "?isAutoOpen=" + (isAutoOpen ? "true" : "false");
        }
    }

    class Dom {
        static createElement (name:string) : any {
            return document.createElement(name);
        }
        static createDiv(className: string) {
            var elem : HTMLDivElement = createElement("div");
            if (className !== null && className !== undefined) {
                elem.className = className;
            }
            return elem;
        }
        static createImg(src: string, className: string) {
            var elem : HTMLImageElement = createElement("img");
            elem.src = src;
            elem.className = className;
            return elem;
        }
        static hideElement(elem: any) {
            util.addClass(elem, "re-hidden");
        }
        static unHideElement(elem: any) {
            util.removeClass(elem, "re-hidden");
        }

        static container: HTMLDivElement;
        static iframe: HTMLIFrameElement;
        static loading: HTMLDivElement;
        static progressSpan: HTMLSpanElement;
        static loadingTextElem: HTMLDivElement;
    }

    class Loading {
        static hideRing() {
            Dom.progressSpan.style.visibility = "hidden";
        };
        static showRing() {
            Dom.progressSpan.style.visibility = "visible";
        };
        static show() {
            anim.enterContent(Dom.loading).then(function () {
                Dom.unHideElement(Dom.loading);
            });
        };
        static hide() {
            anim.fadeOut(Dom.loading).then(function () {
                Dom.hideElement(Dom.loading);
            });
        };
        static setText(text: string) {
            if (text.substr(0, 5) === "Error") {
                hideRing();
            }
            anim.exitContent(Dom.loadingTextElem).done(
                function () {
                    Dom.loadingTextElem.innerText = text;
                    anim.enterContent(Dom.loadingTextElem)
                });
        };
        static loadIntro(isAutoOpen: bool) {
            initializeUI();
            navigate(Url.getIntroUrl(isAutoOpen));

            show();
            showRing();

            var msgIndex = 0;

            var event : any = document.createEvent("CustomEvent");
            event.initCustomEvent("showNextLoadingMessage", true, true, null);

            document.addEventListener("showNextLoadingMessage", function () {
                Loading.setText(RemoteOptions.loadingMessages[msgIndex]);
                msgIndex = (msgIndex === RemoteOptions.loadingMessages.length - 1) ? 0 : msgIndex + 1;
                if (!introHasLoaded) {
                    setTimeout(function () {
                        document.dispatchEvent(event);
                    }, 3000);
                }
            });
            document.dispatchEvent(event);
        }
    }

    class Auth {
        static private _token: string = "ReferEngine-AuthToken";
        static private _expires: string = "ReferEngine-TokenExpiresAt";
        static getStoredToken(): string {
            function resetAndReturnNull() {
                localSettings.values[_expires] = null;
                localSettings.values[_token] = null;
                return null;
            }
            var expiresAt: number = localSettings.values[_expires];
            var token = localSettings.values[_token];
            if (!expiresAt || !token) {
                return resetAndReturnNull();
            }
            var date = new Date();
            var time = date.getTime();
            var threshold = 10 * 60; // 10 minutes
            if (expiresAt - threshold < time) {
                return resetAndReturnNull();
            }
            return token;
        }
        static setStoredToken(value: string, expiresIn: number) {
            var expiresAt = new Date();
            expiresAt.setSeconds(expiresAt.getSeconds() + expiresIn);
            localSettings.values[_token] = value;
            localSettings.values[_expires] = expiresAt;
        }
        static authorizeFacebookAsync() {
            return new WinJS.Promise(function (comp, error, prog) {
                var callback = "https://www.referengine.com/recommend/win8/success";
                var query = "client_id=368842109866922&redirect_uri=" + callback +
                            "&scope=" + RemoteOptions.fbScope + "&display=popup&response_type=code";
                var facebookUrl = "https://www.facebook.com/dialog/oauth?" + query; 
                var startUri = new Windows.Foundation.Uri(facebookUrl);
                var endUri = new Windows.Foundation.Uri(callback);
                var web = Windows.Security.Authentication.Web;
                web.WebAuthenticationBroker.authenticateAsync(web.WebAuthenticationOptions.none, startUri, endUri).done(function (request) {
                    if (request.responseData.indexOf("error_reason=") !== -1 || request.responseErrorDetail === 404) {
                        comp(new Messaging.AuthResult(false));
                    }
                    var codeString = "code=";
                    var start = request.responseData.indexOf(codeString) + codeString.length;
                    var code = request.responseData.substr(start);
                    if (code) {
                        comp(new Messaging.AuthResult(true, code));
                    }
                    else {
                        comp(new Messaging.AuthResult(false));
                    }
                }, function () {
                    comp(new Messaging.AuthResult(false));
                });
            });
        }

        static authorizeAppAsync() {
            return new WinJS.Promise(function (comp, error, prog) {
                var storedToken = getStoredToken();
                if (storedToken) {
                    comp(storedToken);
                }
                else {
                    currentApp.getAppReceiptAsync().done(function (xml) {
                        var xml64 = window.btoa(xml);
                        WinJS.xhr({
                            type: "POST",
                            headers: {
                                "Content-type": "text/xml"
                            },
                            url: Url.auth,
                            data: xml,
                        }).done(function (request) {
                            var data = JSON.parse(request.responseText);
                            if (data.token && data.expiresIn) {
                                setStoredToken(data.token, data.expiresIn);
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
                            }
                            else {
                                comp(null);
                            }
                        }, function (request) {
                            console.error("ReferEngine: could not authorize app with ReferEngine.com.");
                            if (request.statusText) {
                                console.error("ReferEngine - Message from server: " + request.statusText);
                            }
                            comp(null);
                        });
                    });
                }
            });
        }
    }

    function isConnectedToTheInternet() : bool {
        var internetConnection = Windows.Networking.Connectivity.NetworkInformation.getInternetConnectionProfile();
        if (!internetConnection) {
            var content = "ReferEngine requires a working internet connection.";
            var title = "Not Connected to the Internet";
            var messageDialog = new Windows.UI.Popups.MessageDialog(content, title);
            messageDialog.showAsync();
            return false;
        }
        return true;
    }

    var isHidden = true;
    export function show(isAutoOpen: bool) {
        if (isConnectedToTheInternet() && isHidden) {
            Loading.loadIntro(isAutoOpen);
            anim.fadeIn(Dom.container).then(function () {
                Dom.unHideElement(Dom.container);
            });
            isHidden = false;
        }
    };

    export function hide() {
        anim.fadeOut(Dom.container).then(function () {
            Dom.hideElement(Dom.container);
        });
        isHidden = true;
    };

    export function reset() {
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
    };

    function navigate(url) {
        if (url.indexOf("?") == -1) {
            return Auth.authorizeAppAsync().done(function (token) {
                Dom.iframe.src = url + "?authToken=" + token;
            });
        }
        else {
            return Auth.authorizeAppAsync().done(function (token) {
                Dom.iframe.src = url + "&authToken=" + token
            });
        }
    }

    var functions: Functions.Function[] = [
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
            Auth.authorizeFacebookAsync().then(function (authResult: Messaging.AuthResult) {
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

    function onLaunchAsync {
        return Auth.authorizeAppAsync().done(function (token) {
            if (token) {
                if (client.onLoadArray) {
                    for (var i = 0; i < client.onLoadArray.length; i++) {
                        client.onLoadArray[i]();
                    }
                }
                AutoShow.autoOpenIfNeeded();
                isAvailable = true;
            }
        });
    };

    window["ReferEngine"] = ReferEngine;
    onLaunchAsync();
}
