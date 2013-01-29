$(document).ready(function () {
    var util = RE.Utilities,
        loginButton = $(".login"),
        cancelButton = $("#re-cancel"),
        dontAskCheckbox = $("#dontAsk");

    loginButton.click(function () {
        util.PostToParent("fb-login");
        util.MixPanelTrack("Recommend Intro Start");
    });

    cancelButton.click(function () {
        var dontAskAgain = dontAskCheckbox[0].checked;
        util.PostToParent({
            action: "cancel",
            dontAskAgain: dontAskAgain
        });

        util.MixPanelTrack("Recommend Intro Cancel", {
            "Dont Ask Again": dontAskAgain
        });
    });
    
    util.HideLoading();
    util.PostToParent("intro-page-loaded");
    util.MixPanelTrack("Recommend Intro");
});
