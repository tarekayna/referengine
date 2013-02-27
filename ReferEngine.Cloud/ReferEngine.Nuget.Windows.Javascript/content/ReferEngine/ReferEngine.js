/**
 * almond 0.2.5 Copyright (c) 2011-2012, The Dojo Foundation All Rights Reserved.
 * Available via the MIT or new BSD license.
 * see: http://github.com/jrburke/almond for details
 */

(function(){var e,t,n;(function(r){function d(e,t){return h.call(e,t)}function v(e,t){var n,r,i,s,o,u,a,f,c,h,p=t&&t.split("/"),d=l.map,v=d&&d["*"]||{};if(e&&e.charAt(0)===".")if(t){p=p.slice(0,p.length-1),e=p.concat(e.split("/"));for(f=0;f<e.length;f+=1){h=e[f];if(h===".")e.splice(f,1),f-=1;else if(h===".."){if(f===1&&(e[2]===".."||e[0]===".."))break;f>0&&(e.splice(f-1,2),f-=2)}}e=e.join("/")}else e.indexOf("./")===0&&(e=e.substring(2));if((p||v)&&d){n=e.split("/");for(f=n.length;f>0;f-=1){r=n.slice(0,f).join("/");if(p)for(c=p.length;c>0;c-=1){i=d[p.slice(0,c).join("/")];if(i){i=i[r];if(i){s=i,o=f;break}}}if(s)break;!u&&v&&v[r]&&(u=v[r],a=f)}!s&&u&&(s=u,o=a),s&&(n.splice(0,o,s),e=n.join("/"))}return e}function m(e,t){return function(){return s.apply(r,p.call(arguments,0).concat([e,t]))}}function g(e){return function(t){return v(t,e)}}function y(e){return function(t){a[e]=t}}function b(e){if(d(f,e)){var t=f[e];delete f[e],c[e]=!0,i.apply(r,t)}if(!d(a,e)&&!d(c,e))throw new Error("No "+e);return a[e]}function w(e){var t,n=e?e.indexOf("!"):-1;return n>-1&&(t=e.substring(0,n),e=e.substring(n+1,e.length)),[t,e]}function E(e){return function(){return l&&l.config&&l.config[e]||{}}}var i,s,o,u,a={},f={},l={},c={},h=Object.prototype.hasOwnProperty,p=[].slice;o=function(e,t){var n,r=w(e),i=r[0];return e=r[1],i&&(i=v(i,t),n=b(i)),i?n&&n.normalize?e=n.normalize(e,g(t)):e=v(e,t):(e=v(e,t),r=w(e),i=r[0],e=r[1],i&&(n=b(i))),{f:i?i+"!"+e:e,n:e,pr:i,p:n}},u={require:function(e){return m(e)},exports:function(e){var t=a[e];return typeof t!="undefined"?t:a[e]={}},module:function(e){return{id:e,uri:"",exports:a[e],config:E(e)}}},i=function(e,t,n,i){var s,l,h,p,v,g=[],w;i=i||e;if(typeof n=="function"){t=!t.length&&n.length?["require","exports","module"]:t;for(v=0;v<t.length;v+=1){p=o(t[v],i),l=p.f;if(l==="require")g[v]=u.require(e);else if(l==="exports")g[v]=u.exports(e),w=!0;else if(l==="module")s=g[v]=u.module(e);else if(d(a,l)||d(f,l)||d(c,l))g[v]=b(l);else{if(!p.p)throw new Error(e+" missing "+l);p.p.load(p.n,m(i,!0),y(l),{}),g[v]=a[l]}}h=n.apply(a[e],g);if(e)if(s&&s.exports!==r&&s.exports!==a[e])a[e]=s.exports;else if(h!==r||!w)a[e]=h}else e&&(a[e]=n)},e=t=s=function(e,t,n,a,f){return typeof e=="string"?u[e]?u[e](t):b(o(e,t).f):(e.splice||(l=e,t.splice?(e=t,t=n,n=null):e=r),t=t||function(){},typeof n=="function"&&(n=a,a=f),a?i(r,e,t,n):setTimeout(function(){i(r,e,t,n)},4),s)},s.config=function(e){return l=e,l.deps&&s(l.deps,l.callback),s},n=function(e,t,n){t.splice||(n=t,t=[]),!d(a,e)&&!d(f,e)&&(f[e]=[e,t,n])},n.amd={jQuery:!0}})(),n("../../../lib/almond.js",function(){}),n("../common/Messaging",["require","exports"],function(e,t){var n=function(){function e(e,t){this.details=t,this.functionInfo=e}return e.prototype.getString=function(){var e={functionInfo:this.functionInfo};return this.details&&(e.details=this.details),JSON.stringify(e)},e.parse=function(n){var r=JSON.parse(n);return new e(r.functionInfo,r.details)},e}();t.Message=n,function(e){e._map=[],e._map[0]="IFrameToClient",e.IFrameToClient=0,e._map[1]="ClientToIframe",e.ClientToIframe=1}(t.MessengerType||(t.MessengerType={}));var r=t.MessengerType,i=function(){function e(e,t){this.type=e,this.receiveOrigin=t,this.messageHandlers=[];var n=this;window.addEventListener("message",function(e){n.receive(e)})}return e.prototype.send=function(e){this.type==r.IFrameToClient?window.parent.postMessage(e.getString(),this.parentLocation):this.iframe.contentWindow.postMessage(e.getString(),this.iframe.src)},e.prototype.call=function(e,t){var r=new n(e,t);this.send(r)},e.prototype.receive=function(e){if(e.origin===this.receiveOrigin){var t=n.parse(e.data);for(var r=0;r<this.messageHandlers.length;r++)this.messageHandlers[r].msg===t.functionInfo.name&&this.messageHandlers[r].handler(t.details)}},e.prototype.addMessageHandler=function(e,t){this.messageHandlers.push({msg:e,handler:t})},e}();t.Messenger=i;var s=function(){function e(e,t){this.success=e,this.code=t}return e}();t.AuthResult=s}),n("../common/Functions",["require","exports"],function(e,t){var n=function(){function e(e,t){this.info=e,this.handler=t}return e}();t.Function=n;var r=function(){function e(e){this.messenger=e,this.functions=[]}return e.prototype.add=function(e){this.functions.push(e),this.messenger.addMessageHandler(e.info.name,e.handler)},e.prototype.addRange=function(e){for(var t=0;t<e.length;t++)this.add(e[t])},e}();t.FunctionRange=r;var i=function(){function e(e){this.name=e}return e}();t.FunctionInfo=i;var s=function(){function e(){}return e.authFacebook=new i("authFacebook"),e.hide=new i("hide"),e.hideLoading=new i("hideLoading"),e.navigate=new i("navigate"),e.setAutoAsk=new i("setAutoAsk "),e.setIntroPageLoaded=new i("setIntroPageLoaded"),e.setLoadingText=new i("setLoadingText"),e.showLoading=new i("showLoading"),e.hideLoadingCompat=new i("hide-loading"),e}();t.ClientFunction=s;var o=function(){function e(){}return e.closedWhileLoading=new i("closedWhileLoading"),e.introVisible=new i("introVisible"),e.authFacebookResult=new i("authFacebookResult"),e}();t.ServerFunction=o}),n("WindowsClientScript-ready",["require","exports","../common/Messaging","../common/Functions"],function(e,t,n,r){var i=n,s=r,o;(function(e){function g(){if(!a){var t=S.createElement("style");t.innerText=y.style,t.type="text/css";var n=document.getElementsByTagName("link")[0];n.parentNode.insertBefore(t,n);var r=document.querySelector("body");S.container=S.createDiv("re-container"),r.appendChild(S.container),S.iframe=S.createElement("iframe"),S.container.appendChild(S.iframe),S.loading=S.createDiv("re-loading-container"),S.container.appendChild(S.loading);var i=S.createDiv("re-close");i.innerText="",S.loading.appendChild(i),i.addEventListener("click",function(){p.call(m.closedWhileLoading),e.hide()});var s=S.createDiv("meter");S.progressSpan=S.createElement("span"),s.appendChild(S.progressSpan);var o=S.createDiv(null);S.loading.appendChild(o);var u=S.createImg(y.logoMarkImageData,"re-logo-mark"),f=S.createImg(y.logoTextImageData,"re-logo-text");S.loadingTextElem=S.createDiv("re-loading-text"),o.appendChild(u),o.appendChild(f),o.appendChild(s),o.appendChild(S.loadingTextElem),a=!0,p.iframe=S.iframe}}function N(){var e=Windows.Networking.Connectivity.NetworkInformation.getInternetConnectionProfile();if(!e){var t="ReferEngine requires a working internet connection.",n="Not Connected to the Internet",r=new Windows.UI.Popups.MessageDialog(t,n);return r.showAsync(),!1}return!0}function k(e){N()&&C&&(x.loadIntro(e),h.fadeIn(S.container).then(function(){S.unHideElement(S.container)}),C=!1)}function L(){h.fadeOut(S.container).then(function(){S.hideElement(S.container)}),C=!0}function A(){T.setStoredToken(null,0),y.fbScope=null,y.loadingMessages=[],y.logoMarkImageData=null,y.logoTextImageData=null,y.style=null,b.enable=null,b.interval=null,b.timeout=null,w.autoAskAgain=!0,w.launchCount=0,y.appId=null}function O(e){return e.indexOf("?")==-1?T.authorizeAppAsync().done(function(t){S.iframe.src=e+"?authToken="+t}):T.authorizeAppAsync().done(function(t){S.iframe.src=e+"&authToken="+t})}function _(){return T.authorizeAppAsync().done(function(n){if(n){if(t.onLoadArray)for(var r=0;r<t.onLoadArray.length;r++)t.onLoadArray[r]();w.autoOpenIfNeeded(),e.isAvailable=!0}})}var t=ReferEngineClient,n=Windows.Storage.ApplicationData.current,r=n.roamingSettings,o=n.localSettings,u=!1,a=!1,f=t.currentApp,l=t.appActivationArgs,c=WinJS.Utilities,h=WinJS.UI.Animation,p,d,v=s.ClientFunction,m=s.ServerFunction;e.isAvailable=!1;var y=function(){function e(){}return e._styleKey="ReferEngine-Style",Object.defineProperty(e,"style",{get:function(){return o.values[e._styleKey]},set:function(t){o.values[e._styleKey]=t},enumerable:!0,configurable:!0}),e._loadingMessagesKey="ReferEngine-LoadingMessages",Object.defineProperty(e,"loadingMessages",{get:function(){if(e._loadingMessagesCached)return e._loadingMessagesCached;var t=o.values[e._loadingMessagesKey];return e._loadingMessagesCached=JSON.parse(t).value,e._loadingMessagesCached},set:function(t){e._loadingMessagesCached=t;var n=JSON.stringify({value:t});o.values[e._loadingMessagesKey]=n},enumerable:!0,configurable:!0}),e._logoMarkImageDataKey="ReferEngine-LogoMarkImageData",Object.defineProperty(e,"logoMarkImageData",{get:function(){return o.values[e._logoMarkImageDataKey]},set:function(t){o.values[e._logoMarkImageDataKey]=t},enumerable:!0,configurable:!0}),e._logoTextImageDataKey="ReferEngine-LogoTextImageData",Object.defineProperty(e,"logoTextImageData",{get:function(){return o.values[e._logoTextImageDataKey]},set:function(t){o.values[e._logoTextImageDataKey]=t},enumerable:!0,configurable:!0}),e._fbScopeKey="ReferEngine-FacebookScope",Object.defineProperty(e,"fbScope",{get:function(){return o.values[e._fbScopeKey]},set:function(t){o.values[e._fbScopeKey]=t},enumerable:!0,configurable:!0}),e._appIdKey="ReferEngine-AppId",Object.defineProperty(e,"appId",{get:function(){return o.values[e._appIdKey]},set:function(t){o.values[e._appIdKey]=t},enumerable:!0,configurable:!0}),e}(),b=function(){function e(){}return e._timeoutDefault=15e3,e._timeoutKey="ReferEngine-AutoShowTimeout",Object.defineProperty(e,"timeout",{get:function(){var t=r.values[e._timeoutKey];return t?t:(r.values[e._timeoutKey]=e._timeoutDefault,e._timeoutDefault)},set:function(t){r.values[e._timeoutKey]=t},enumerable:!0,configurable:!0}),e._intervalDefault=2,e._intervalKey="ReferEngine-AutoShowInterval",Object.defineProperty(e,"interval",{get:function(){var t=r.values[e._intervalKey];return t?t:(r.values[e._intervalKey]=e._intervalDefault,e._intervalDefault)},set:function(t){r.values[e._intervalKey]=t},enumerable:!0,configurable:!0}),e._enableDefault=!0,e._enableKey="ReferEngine-AutoShowEnable",Object.defineProperty(e,"enable",{get:function(){var t=r.values[e._enableKey];return t?t:(r.values[e._enableKey]=e._enableDefault,e._enableDefault)},set:function(t){r.values[e._enableKey]=t},enumerable:!0,configurable:!0}),e}(),w=function(){function t(){}return t._launchCountKey="ReferEngine-LaunchCount",Object.defineProperty(t,"launchCount",{get:function(){var e=r.values[t._launchCountKey];return e?e:(r.values[t._launchCountKey]=0,0)},set:function(e){r.values[t._launchCountKey]=e},enumerable:!0,configurable:!0}),t._autoAskAgainKey="ReferEngine-AutoAskAgain",Object.defineProperty(t,"autoAskAgain",{get:function(){var e=r.values[t._autoAskAgainKey];return e?e:(r.values[t._autoAskAgainKey]=b.enable,b.enable)},set:function(e){r.values[t._autoAskAgainKey]=e},enumerable:!0,configurable:!0}),t.shouldAutoOpen=function(){return t.launchCount%b.interval===0&&t.autoAskAgain},t.autoOpenIfNeeded=function(){if(l){var r=l.detail.previousExecutionState,i=Windows.ApplicationModel.Activation.ApplicationExecutionState;r!==i.running&&r!==i.suspended&&t.launchCount++,t.shouldAutoOpen()&&setTimeout(function(){window.msRequestAnimationFrame(function(){e.show(!0)})},b.timeout)}},t}(),E=function(){function e(){}return e.base="http://127.0.0.1:81",e.auth=e.base+"/recommend/win8/authorizeapp",e.getIntroUrl=function(n){return e.base+"/recommend/win8/intro/"+y.appId+"?isAutoOpen="+(n?"true":"false")},e}(),S=function(){function e(){}return e.createElement=function(t){return document.createElement(t)},e.createDiv=function(n){var r=e.createElement("div");return n!==null&&n!==undefined&&(r.className=n),r},e.createImg=function(n,r){var i=e.createElement("img");return i.src=n,i.className=r,i},e.hideElement=function(t){c.addClass(t,"re-hidden")},e.unHideElement=function(t){c.removeClass(t,"re-hidden")},e}(),x=function(){function e(){}return e.hideRing=function(){S.progressSpan.style.visibility="hidden"},e.showRing=function(){S.progressSpan.style.visibility="visible"},e.show=function(){h.enterContent(S.loading).then(function(){S.unHideElement(S.loading)})},e.hide=function(){h.fadeOut(S.loading).then(function(){S.hideElement(S.loading)})},e.setText=function(n){n.substr(0,5)==="Error"&&e.hideRing(),h.exitContent(S.loadingTextElem).done(function(){S.loadingTextElem.innerText=n,h.enterContent(S.loadingTextElem)})},e.loadIntro=function(n){g(),O(E.getIntroUrl(n)),e.show(),e.showRing();var r=0,i=document.createEvent("CustomEvent");i.initCustomEvent("showNextLoadingMessage",!0,!0,null),document.addEventListener("showNextLoadingMessage",function(){e.setText(y.loadingMessages[r]),r=r===y.loadingMessages.length-1?0:r+1,u||setTimeout(function(){document.dispatchEvent(i)},3e3)}),document.dispatchEvent(i)},e}(),T=function(){function e(){}return e._token="ReferEngine-AuthToken",e._expires="ReferEngine-TokenExpiresAt",e.getStoredToken=function(){function n(){return o.values[e._expires]=null,o.values[e._token]=null,null}var r=o.values[e._expires],i=o.values[e._token];if(!r||!i)return n();var s=new Date,u=s.getTime(),a=600;return r-a<u?n():i},e.setStoredToken=function(n,r){var i=new Date;i.setSeconds(i.getSeconds()+r),o.values[e._token]=n,o.values[e._expires]=i},e.authorizeFacebookAsync=function(){return new WinJS.Promise(function(e,t,n){var r="https://www.referengine.com/recommend/win8/success",s="client_id=368842109866922&redirect_uri="+r+"&scope="+y.fbScope+"&display=popup&response_type=code",o="https://www.facebook.com/dialog/oauth?"+s,u=new Windows.Foundation.Uri(o),a=new Windows.Foundation.Uri(r),f=Windows.Security.Authentication.Web;f.WebAuthenticationBroker.authenticateAsync(f.WebAuthenticationOptions.none,u,a).done(function(t){(t.responseData.indexOf("error_reason=")!==-1||t.responseErrorDetail===404)&&e(new i.AuthResult(!1));var n="code=",r=t.responseData.indexOf(n)+n.length,s=t.responseData.substr(r);s?e(new i.AuthResult(!0,s)):e(new i.AuthResult(!1))},function(){e(new i.AuthResult(!1))})})},e.authorizeAppAsync=function(){return new WinJS.Promise(function(t,n,r){var i=e.getStoredToken();i?t(i):f.getAppReceiptAsync().done(function(n){var r=window.btoa(n);WinJS.xhr({type:"POST",headers:{"Content-type":"text/xml"},url:E.auth,data:n}).done(function(n){var r=JSON.parse(n.responseText);r.token&&r.expiresIn?(e.setStoredToken(r.token,r.expiresIn),b.enable=r.asoEnabled,b.interval=r.asoInterval,b.timeout=r.asoTimeout,y.style=r.style,y.loadingMessages=r.loadingMessages,y.logoMarkImageData=r.logoMarkImageData,y.logoTextImageData=r.logoTextImageData,y.fbScope=r.fbScope,y.appId=r.appId,t(r.token)):t(null)},function(e){console.error("ReferEngine: could not authorize app with ReferEngine.com."),e.statusText&&console.error("ReferEngine - Message from server: "+e.statusText),t(null)})})})},e}(),C=!0;e.show=k,e.hide=L,e.reset=A;var M=[new s.Function(v.navigate,function(e){O(e.url)}),new s.Function(v.showLoading,function(e){x.show()}),new s.Function(v.hideLoading,function(e){x.hide()}),new s.Function(v.setLoadingText,function(e){x.setText(e.text)}),new s.Function(v.setAutoAsk,function(e){w.autoAskAgain=e.askAgain}),new s.Function(v.hide,function(t){e.hide()}),new s.Function(v.authFacebook,function(e){T.authorizeFacebookAsync().then(function(e){p.call(m.authFacebookResult,{authResult:e})})}),new s.Function(v.setIntroPageLoaded,function(e){u=!0,p.call(m.introVisible)})];p=new i.Messenger(i.MessengerType.ClientToIframe,E.base),d=new s.FunctionRange(p),d.addRange(M),window.ReferEngine=e,_()})(o||(o={}))}),t("WindowsClientScript-ready")})();