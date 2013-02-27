import Messaging = module("Messaging");

export class Function {
    constructor(public info: FunctionInfo, public handler: any) { }
}

export class FunctionRange {
    constructor(public messenger: Messaging.Messenger) { }

    public add(func: Function) {
        this.functions.push(func);
        this.messenger.addMessageHandler(func.info.name, func.handler);
    }

    public addRange(functions: Function[]) {
        for (var i = 0; i < functions.length; i++) {
            this.add(functions[i]);
        }
    }

    private functions = [];
}

export class FunctionInfo {
    constructor(public name: string) { }
}

export class ClientFunction {
    static authFacebook = new FunctionInfo("authFacebook");
    static hide = new FunctionInfo("hide");
    static hideLoading = new FunctionInfo("hideLoading");
    static navigate = new FunctionInfo("navigate");
    static setAutoAsk = new FunctionInfo("setAutoAsk ");
    static setIntroPageLoaded = new FunctionInfo("setIntroPageLoaded");
    static setLoadingText = new FunctionInfo("setLoadingText");
    static showLoading = new FunctionInfo("showLoading");
    static hideLoadingCompat = new FunctionInfo("hide-loading");
}

export class ServerFunction {
    static closedWhileLoading = new FunctionInfo("closedWhileLoading");
    static introVisible = new FunctionInfo("introVisible");
    static authFacebookResult = new FunctionInfo("authFacebookResult");
}