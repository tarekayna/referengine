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

    util.AddMessageHandler("logged-in", function(data) {
        // unused-data = data.expiresIn;
        var newUrl = util.GetLink("friends", data.appId) + "?userAccessToken=" + data.token;
        util.NavigateTo(newUrl);
    });

    util.HideLoading();
});