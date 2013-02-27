define(["require", "exports"], function(require, exports) {
    
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
