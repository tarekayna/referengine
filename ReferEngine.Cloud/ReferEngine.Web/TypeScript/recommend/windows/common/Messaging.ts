import Functions = module("Functions");

export class Message {
    constructor(funcInfo: Functions.FunctionInfo, public details?: any) {
        this.functionName = funcInfo.name;
    }

    public functionName: string;
    
    public getString() {
        var data: any = { functionName: this.functionName };
        if (this.details) {
            data.details = this.details;
        }
        return JSON.stringify(data);
    }

    public static parse(msgString: string) {
        var msgJson = JSON.parse(msgString);
        var msg: Message = new Message(msgJson.msg);
        msg.details = msgJson.details;
        return msg;
    }
}

export enum MessengerType {
    IFrameToClient,
    ClientToIframe
};

export class Messenger {

    constructor(public type: MessengerType, public receiveOrigin: string) {
        var thisMessenger = this;
        window.addEventListener("message", function (event) {
            thisMessenger.receive(event);
        });
    }

    private messageHandlers = [];

    public iframe: HTMLIFrameElement;
    public parentLocation: string;

    public send(message: Message) {
        if (this.type == MessengerType.IFrameToClient) {
            window.parent.postMessage(message.getString(), this.parentLocation);
        }
        else {
            this.iframe.contentWindow.postMessage(message.getString(), this.iframe.src);
        }
    }

    public call(functionInfo: Functions.FunctionInfo, details? : any) {
        var msg: Message = new Message(functionInfo, details);
        this.send(msg);
    }

    public receive(event: any) {
        if (event.origin === this.receiveOrigin) {
            var message = Message.parse(event.data);
            for (var i = 0; i < this.messageHandlers.length; i++) {
                if (this.messageHandlers[i].msg === message.functionName) {
                    this.messageHandlers[i].handler(message.details);
                }
            }
        }
    }

    public addMessageHandler(msg, handler) {
        this.messageHandlers.push({
            msg: msg,
            handler: handler
        });
    };
}

export class AuthResult {
    constructor(success: bool, code?: string) {
        this.success = success;
        this.code = code;
    }
    success: bool;
    code: string;
}