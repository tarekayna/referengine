$(document).ready(function () {
    var accessToken,
        uid;

    var result = $("#result");
    result.hide();
    
    function testAPI() {
        console.log('Welcome!  Fetching your information.... ');
        FB.api('/me', function(response) {
            console.log('Good to see you, ' + response.name + '.');
            FB.api('/' + response.id + '/accounts', function(res2) {
                console.log(res2);
                FB.api(res2.paging.next, function(res3) {
                    console.log(res3);
                });
            });
        });
    }

    function login() {
        FB.login(function(response) {
            if (response.authResponse) {
                // connected
                testAPI();
            } else {
                // cancelled
            }
        });
    }

    var onFormSuccess = function(data) {
        var iphonenames = $("#iPhoneNames");
        var iPadNames = $("#iPadNames");
        var AndroidNames = $("#AndroidNames");

        for (var i = 0; i < data.iPhone.length; i++) {
            iphonenames.append("<p>" + data.iPhone[i].Name + "</p>");
        }

        for (i = 0; i < data.iPad.length; i++) {
            iPadNames.append("<p>" + data.iPad[i].Name + "</p>");
        }

        for (i = 0; i < data.Android.length; i++) {
            AndroidNames.append("<p>" + data.Android[i].Name + "</p>");
        }

        result.show();
    };

    var onFormError = function() {

    };

    window.fbAsyncInit = function() {
        FB.init({
            appId: '368842109866922', // App ID
            channelUrl: 'www.ReferEngine.com/facebook_channel.html', // Channel File
            status: true, // check login status
            cookie: true, // enable cookies to allow the server to access the session
            xfbml: true  // parse XFBML
        });

        FB.getLoginStatus(function(response) {
            if (response.status === 'connected') {
                uid = response.authResponse.userID;
                accessToken = response.authResponse.accessToken;                
            } else if (response.status === 'not_authorized') {
            } else {
                // not_logged_in
            }
        });

    };

    // Load the SDK Asynchronously
    (function(d) {
        var js, id = 'facebook-jssdk', ref = d.getElementsByTagName('script')[0];
        if (d.getElementById(id)) {
            return;
        }
        js = d.createElement('script');
        js.id = id;
        js.async = true;
        js.src = "//connect.facebook.net/en_US/all.js";
        ref.parentNode.insertBefore(js, ref);
    }(document));

    $("#get-devices").click(function() {
        var data = {
            UserID: uid,
            AccessToken: accessToken
        };
        var xhr = $.post("/home/facebook", data, onFormSuccess);
        xhr.error(onFormError);
    });

});