$(document).ready(function () {
    var util = RE.Utilities,
        loginButton = $("#re-login"),
        notNowButton = $("#re-not-now"),
        dontAskButton = $("#re-dont-ask");

    loginButton.click(function () {
        util.PostToParent("fb-login");
    });

    notNowButton.click(function () {
        util.PostToParent("not-now");
    });
    
    dontAskButton.click(function () {
        util.PostToParent("dont-ask");
    });

    util.HideLoading();
});