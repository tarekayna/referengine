(function () {
/**
 * almond 0.2.5 Copyright (c) 2011-2012, The Dojo Foundation All Rights Reserved.
 * Available via the MIT or new BSD license.
 * see: http://github.com/jrburke/almond for details
 */
//Going sloppy to avoid 'use strict' string cost, but strict practices should
//be followed.
/*jslint sloppy: true */
/*global setTimeout: false */

var requirejs, require, define;
(function (undef) {
    var main, req, makeMap, handlers,
        defined = {},
        waiting = {},
        config = {},
        defining = {},
        hasOwn = Object.prototype.hasOwnProperty,
        aps = [].slice;

    function hasProp(obj, prop) {
        return hasOwn.call(obj, prop);
    }

    /**
     * Given a relative module name, like ./something, normalize it to
     * a real name that can be mapped to a path.
     * @param {String} name the relative name
     * @param {String} baseName a real name that the name arg is relative
     * to.
     * @returns {String} normalized name
     */
    function normalize(name, baseName) {
        var nameParts, nameSegment, mapValue, foundMap,
            foundI, foundStarMap, starI, i, j, part,
            baseParts = baseName && baseName.split("/"),
            map = config.map,
            starMap = (map && map['*']) || {};

        //Adjust any relative paths.
        if (name && name.charAt(0) === ".") {
            //If have a base name, try to normalize against it,
            //otherwise, assume it is a top-level require that will
            //be relative to baseUrl in the end.
            if (baseName) {
                //Convert baseName to array, and lop off the last part,
                //so that . matches that "directory" and not name of the baseName's
                //module. For instance, baseName of "one/two/three", maps to
                //"one/two/three.js", but we want the directory, "one/two" for
                //this normalization.
                baseParts = baseParts.slice(0, baseParts.length - 1);

                name = baseParts.concat(name.split("/"));

                //start trimDots
                for (i = 0; i < name.length; i += 1) {
                    part = name[i];
                    if (part === ".") {
                        name.splice(i, 1);
                        i -= 1;
                    } else if (part === "..") {
                        if (i === 1 && (name[2] === '..' || name[0] === '..')) {
                            //End of the line. Keep at least one non-dot
                            //path segment at the front so it can be mapped
                            //correctly to disk. Otherwise, there is likely
                            //no path mapping for a path starting with '..'.
                            //This can still fail, but catches the most reasonable
                            //uses of ..
                            break;
                        } else if (i > 0) {
                            name.splice(i - 1, 2);
                            i -= 2;
                        }
                    }
                }
                //end trimDots

                name = name.join("/");
            } else if (name.indexOf('./') === 0) {
                // No baseName, so this is ID is resolved relative
                // to baseUrl, pull off the leading dot.
                name = name.substring(2);
            }
        }

        //Apply map config if available.
        if ((baseParts || starMap) && map) {
            nameParts = name.split('/');

            for (i = nameParts.length; i > 0; i -= 1) {
                nameSegment = nameParts.slice(0, i).join("/");

                if (baseParts) {
                    //Find the longest baseName segment match in the config.
                    //So, do joins on the biggest to smallest lengths of baseParts.
                    for (j = baseParts.length; j > 0; j -= 1) {
                        mapValue = map[baseParts.slice(0, j).join('/')];

                        //baseName segment has  config, find if it has one for
                        //this name.
                        if (mapValue) {
                            mapValue = mapValue[nameSegment];
                            if (mapValue) {
                                //Match, update name to the new value.
                                foundMap = mapValue;
                                foundI = i;
                                break;
                            }
                        }
                    }
                }

                if (foundMap) {
                    break;
                }

                //Check for a star map match, but just hold on to it,
                //if there is a shorter segment match later in a matching
                //config, then favor over this star map.
                if (!foundStarMap && starMap && starMap[nameSegment]) {
                    foundStarMap = starMap[nameSegment];
                    starI = i;
                }
            }

            if (!foundMap && foundStarMap) {
                foundMap = foundStarMap;
                foundI = starI;
            }

            if (foundMap) {
                nameParts.splice(0, foundI, foundMap);
                name = nameParts.join('/');
            }
        }

        return name;
    }

    function makeRequire(relName, forceSync) {
        return function () {
            //A version of a require function that passes a moduleName
            //value for items that may need to
            //look up paths relative to the moduleName
            return req.apply(undef, aps.call(arguments, 0).concat([relName, forceSync]));
        };
    }

    function makeNormalize(relName) {
        return function (name) {
            return normalize(name, relName);
        };
    }

    function makeLoad(depName) {
        return function (value) {
            defined[depName] = value;
        };
    }

    function callDep(name) {
        if (hasProp(waiting, name)) {
            var args = waiting[name];
            delete waiting[name];
            defining[name] = true;
            main.apply(undef, args);
        }

        if (!hasProp(defined, name) && !hasProp(defining, name)) {
            throw new Error('No ' + name);
        }
        return defined[name];
    }

    //Turns a plugin!resource to [plugin, resource]
    //with the plugin being undefined if the name
    //did not have a plugin prefix.
    function splitPrefix(name) {
        var prefix,
            index = name ? name.indexOf('!') : -1;
        if (index > -1) {
            prefix = name.substring(0, index);
            name = name.substring(index + 1, name.length);
        }
        return [prefix, name];
    }

    /**
     * Makes a name map, normalizing the name, and using a plugin
     * for normalization if necessary. Grabs a ref to plugin
     * too, as an optimization.
     */
    makeMap = function (name, relName) {
        var plugin,
            parts = splitPrefix(name),
            prefix = parts[0];

        name = parts[1];

        if (prefix) {
            prefix = normalize(prefix, relName);
            plugin = callDep(prefix);
        }

        //Normalize according
        if (prefix) {
            if (plugin && plugin.normalize) {
                name = plugin.normalize(name, makeNormalize(relName));
            } else {
                name = normalize(name, relName);
            }
        } else {
            name = normalize(name, relName);
            parts = splitPrefix(name);
            prefix = parts[0];
            name = parts[1];
            if (prefix) {
                plugin = callDep(prefix);
            }
        }

        //Using ridiculous property names for space reasons
        return {
            f: prefix ? prefix + '!' + name : name, //fullName
            n: name,
            pr: prefix,
            p: plugin
        };
    };

    function makeConfig(name) {
        return function () {
            return (config && config.config && config.config[name]) || {};
        };
    }

    handlers = {
        require: function (name) {
            return makeRequire(name);
        },
        exports: function (name) {
            var e = defined[name];
            if (typeof e !== 'undefined') {
                return e;
            } else {
                return (defined[name] = {});
            }
        },
        module: function (name) {
            return {
                id: name,
                uri: '',
                exports: defined[name],
                config: makeConfig(name)
            };
        }
    };

    main = function (name, deps, callback, relName) {
        var cjsModule, depName, ret, map, i,
            args = [],
            usingExports;

        //Use name if no relName
        relName = relName || name;

        //Call the callback to define the module, if necessary.
        if (typeof callback === 'function') {

            //Pull out the defined dependencies and pass the ordered
            //values to the callback.
            //Default to [require, exports, module] if no deps
            deps = !deps.length && callback.length ? ['require', 'exports', 'module'] : deps;
            for (i = 0; i < deps.length; i += 1) {
                map = makeMap(deps[i], relName);
                depName = map.f;

                //Fast path CommonJS standard dependencies.
                if (depName === "require") {
                    args[i] = handlers.require(name);
                } else if (depName === "exports") {
                    //CommonJS module spec 1.1
                    args[i] = handlers.exports(name);
                    usingExports = true;
                } else if (depName === "module") {
                    //CommonJS module spec 1.1
                    cjsModule = args[i] = handlers.module(name);
                } else if (hasProp(defined, depName) ||
                           hasProp(waiting, depName) ||
                           hasProp(defining, depName)) {
                    args[i] = callDep(depName);
                } else if (map.p) {
                    map.p.load(map.n, makeRequire(relName, true), makeLoad(depName), {});
                    args[i] = defined[depName];
                } else {
                    throw new Error(name + ' missing ' + depName);
                }
            }

            ret = callback.apply(defined[name], args);

            if (name) {
                //If setting exports via "module" is in play,
                //favor that over return value and exports. After that,
                //favor a non-undefined return value over exports use.
                if (cjsModule && cjsModule.exports !== undef &&
                        cjsModule.exports !== defined[name]) {
                    defined[name] = cjsModule.exports;
                } else if (ret !== undef || !usingExports) {
                    //Use the return value from the function.
                    defined[name] = ret;
                }
            }
        } else if (name) {
            //May just be an object definition for the module. Only
            //worry about defining if have a module name.
            defined[name] = callback;
        }
    };

    requirejs = require = req = function (deps, callback, relName, forceSync, alt) {
        if (typeof deps === "string") {
            if (handlers[deps]) {
                //callback in this case is really relName
                return handlers[deps](callback);
            }
            //Just return the module wanted. In this scenario, the
            //deps arg is the module name, and second arg (if passed)
            //is just the relName.
            //Normalize module name, if it contains . or ..
            return callDep(makeMap(deps, callback).f);
        } else if (!deps.splice) {
            //deps is a config object, not an array.
            config = deps;
            if (callback.splice) {
                //callback is an array, which means it is a dependency list.
                //Adjust args if there are dependencies
                deps = callback;
                callback = relName;
                relName = null;
            } else {
                deps = undef;
            }
        }

        //Support require(['a'])
        callback = callback || function () {};

        //If relName is a function, it is an errback handler,
        //so remove it.
        if (typeof relName === 'function') {
            relName = forceSync;
            forceSync = alt;
        }

        //Simulate async callback;
        if (forceSync) {
            main(undef, deps, callback, relName);
        } else {
            //Using a non-zero value because of concern for what old browsers
            //do, and latest browsers "upgrade" to 4 if lower value is used:
            //http://www.whatwg.org/specs/web-apps/current-work/multipage/timers.html#dom-windowtimers-settimeout:
            //If want a value immediately, use require('id') instead -- something
            //that works in almond on the global level, but not guaranteed and
            //unlikely to work in other AMD implementations.
            setTimeout(function () {
                main(undef, deps, callback, relName);
            }, 4);
        }

        return req;
    };

    /**
     * Just drops the config on the floor, but returns req in case
     * the config return value is used.
     */
    req.config = function (cfg) {
        config = cfg;
        if (config.deps) {
            req(config.deps, config.callback);
        }
        return req;
    };

    define = function (name, deps, callback) {

        //This module may not have dependencies
        if (!deps.splice) {
            //deps is not an array, so probably means
            //an object literal or factory function for
            //the value. Adjust args.
            callback = deps;
            deps = [];
        }

        if (!hasProp(defined, name) && !hasProp(waiting, name)) {
            waiting[name] = [name, deps, callback];
        }
    };

    define.amd = {
        jQuery: true
    };
}());

define("../../../lib/almond.js", function(){});

define('../common/Messaging',["require", "exports"], function(require, exports) {
    
    var Message = (function () {
        function Message(funcInfo, details) {
            this.details = details;
            this.functionInfo = funcInfo;
        }
        Message.prototype.getString = function () {
            var data = {
                functionInfo: this.functionInfo
            };
            if(this.details) {
                data.details = this.details;
            }
            return JSON.stringify(data);
        };
        Message.parse = function parse(msgString) {
            var msgJson = JSON.parse(msgString);
            return new Message(msgJson.functionInfo, msgJson.details);
        };
        return Message;
    })();
    exports.Message = Message;    
    (function (MessengerType) {
        MessengerType._map = [];
        MessengerType._map[0] = "IFrameToClient";
        MessengerType.IFrameToClient = 0;
        MessengerType._map[1] = "ClientToIframe";
        MessengerType.ClientToIframe = 1;
    })(exports.MessengerType || (exports.MessengerType = {}));
    var MessengerType = exports.MessengerType;
    ;
    var Messenger = (function () {
        function Messenger(type, receiveOrigin) {
            this.type = type;
            this.receiveOrigin = receiveOrigin;
            this.messageHandlers = [];
            var thisMessenger = this;
            window.addEventListener("message", function (event) {
                thisMessenger.receive(event);
            });
        }
        Messenger.prototype.send = function (message) {
            if(this.type == MessengerType.IFrameToClient) {
                window.parent.postMessage(message.getString(), this.parentLocation);
            } else {
                this.iframe.contentWindow.postMessage(message.getString(), this.iframe.src);
            }
        };
        Messenger.prototype.call = function (functionInfo, details) {
            var msg = new Message(functionInfo, details);
            this.send(msg);
        };
        Messenger.prototype.receive = function (event) {
            if(event.origin === this.receiveOrigin) {
                var message = Message.parse(event.data);
                for(var i = 0; i < this.messageHandlers.length; i++) {
                    if(this.messageHandlers[i].msg === message.functionInfo.name) {
                        this.messageHandlers[i].handler(message.details);
                    }
                }
            }
        };
        Messenger.prototype.addMessageHandler = function (msg, handler) {
            this.messageHandlers.push({
                msg: msg,
                handler: handler
            });
        };
        return Messenger;
    })();
    exports.Messenger = Messenger;    
    var AuthResult = (function () {
        function AuthResult(success, code) {
            this.success = success;
            this.code = code;
        }
        return AuthResult;
    })();
    exports.AuthResult = AuthResult;    
})
;
define('../common/Functions',["require", "exports"], function(require, exports) {
    
    var Function = (function () {
        function Function(info, handler) {
            this.info = info;
            this.handler = handler;
        }
        return Function;
    })();
    exports.Function = Function;    
    var FunctionRange = (function () {
        function FunctionRange(messenger) {
            this.messenger = messenger;
            this.functions = [];
        }
        FunctionRange.prototype.add = function (func) {
            this.functions.push(func);
            this.messenger.addMessageHandler(func.info.name, func.handler);
        };
        FunctionRange.prototype.addRange = function (functions) {
            for(var i = 0; i < functions.length; i++) {
                this.add(functions[i]);
            }
        };
        return FunctionRange;
    })();
    exports.FunctionRange = FunctionRange;    
    var FunctionInfo = (function () {
        function FunctionInfo(name) {
            this.name = name;
        }
        return FunctionInfo;
    })();
    exports.FunctionInfo = FunctionInfo;    
    var ClientFunction = (function () {
        function ClientFunction() { }
        ClientFunction.authFacebook = new FunctionInfo("authFacebook");
        ClientFunction.hide = new FunctionInfo("hide");
        ClientFunction.hideLoading = new FunctionInfo("hideLoading");
        ClientFunction.navigate = new FunctionInfo("navigate");
        ClientFunction.setAutoAsk = new FunctionInfo("setAutoAsk ");
        ClientFunction.setIntroPageLoaded = new FunctionInfo("setIntroPageLoaded");
        ClientFunction.setLoadingText = new FunctionInfo("setLoadingText");
        ClientFunction.showLoading = new FunctionInfo("showLoading");
        ClientFunction.hideLoadingCompat = new FunctionInfo("hide-loading");
        return ClientFunction;
    })();
    exports.ClientFunction = ClientFunction;    
    var ServerFunction = (function () {
        function ServerFunction() { }
        ServerFunction.closedWhileLoading = new FunctionInfo("closedWhileLoading");
        ServerFunction.introVisible = new FunctionInfo("introVisible");
        ServerFunction.authFacebookResult = new FunctionInfo("authFacebookResult");
        return ServerFunction;
    })();
    exports.ServerFunction = ServerFunction;    
})
;
define('WindowsClientScript-ready',["require", "exports", "../common/Messaging", "../common/Functions"], function(require, exports, __Messaging__, __Functions__) {
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
require('WindowsClientScript-ready');}());