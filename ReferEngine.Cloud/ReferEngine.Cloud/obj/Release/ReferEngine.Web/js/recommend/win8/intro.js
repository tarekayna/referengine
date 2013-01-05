$(document).ready(function () {
    var util = RE.Utilities,
        loginButton = $("#re-login"),
        cancelButton = $("#re-cancel"),
        dontAskCheckbox = $("#dontAsk");

    loginButton.click(function () {
        util.PostToParent("fb-login");
    });

    cancelButton.click(function () {
        util.PostToParent({
            action: "cancel",
            dontAskAgain: dontAskCheckbox[0].checked
        });
    });
    
    util.HideLoading();
});