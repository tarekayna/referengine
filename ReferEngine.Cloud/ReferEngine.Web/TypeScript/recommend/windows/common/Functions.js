define(["require", "exports"], function(require, exports) {
    
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
