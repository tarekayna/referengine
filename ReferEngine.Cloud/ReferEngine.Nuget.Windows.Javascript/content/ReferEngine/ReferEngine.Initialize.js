WinJS.Application.addEventListener("activated", function (args) {
    window.ReferEngineClient = { onLoadArray: [] };
    ReferEngineClient.onLoad = function (callback) {
        if (window.ReferEngine && ReferEngine.isAvailable) callback();
        else ReferEngineClient.onLoadArray.push(callback);
    };
    
    // Note:      Change this line to use:
    //               'Windows.ApplicationModel.Store.CurrentAppSimulator'
    //            instead of:
    //               'Windows.ApplicationModel.Store.CurrentApp'
    //            only in these cases:
    //               1- Your app has never been published in the store
    //               2- You are testing specific app licencings with Refer 
    //                  Engine (whether your app is in trial or full license mode)
    //           
    // MSDN Documentation:
    //            http://msdn.microsoft.com/en-us/library/windows/apps/windows.applicationmodel.store.currentappsimulator.aspx
    //
    // IMPORTANT: Don't forget to change this back to 'CurrentApp' before you 
    //            submit your app to store certification. If you don't, your app 
    //            will fail certification
    ReferEngineClient.currentApp = Windows.ApplicationModel.Store.CurrentAppSimulator;
    
    ReferEngineClient.appActivationArgs = args;
    var script = document.createElement("script");
    script.setAttribute("type", "text/javascript");
    script.setAttribute("async", false);
    script.src = "/referengine/referengine.js";
    document.querySelector("head").appendChild(script);
});