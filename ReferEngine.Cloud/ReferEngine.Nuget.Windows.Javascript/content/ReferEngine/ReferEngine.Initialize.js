WinJS.Application.addEventListener("activated", function (args) {
    window.ReferEngineClient = { onLoadArray: [] };
    ReferEngineClient.onLoad = function (callback) {
        if (ReferEngine && ReferEngine.isAvailable) callback();
        else ReferEngineClient.onLoadArray.push(callback);
    };
    ReferEngineClient.appId = 21;
    ReferEngineClient.appIsPublished = true;
    ReferEngineClient.appActivationArgs = args;
    var script = document.createElement("script");
    script.setAttribute("type", "text/javascript");
    script.setAttribute("async", false);
    script.src = "/referengine/referengine.js";
    document.querySelector("head").appendChild(script);
});