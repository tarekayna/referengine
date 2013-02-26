//var trackSignup = function(status) {
//    mixpanel.track("SignUp", { Status: status });
//};

$(document).ready(function () {
    var form = $("#beta-request-form");
    var formForm = $("#beta-request-form .form");
    var formFeedback = $("#beta-request-form .result");
    var email = $("#beta-request-email");
    var appName = $("#beta-request-app-name");
    var platforms = $("#beta-request-platforms");
    
    formFeedback.hide();

    var onFormSuccess = function () {
        formFeedback.text("Thank you! We will be in touch soon.");
        formFeedback.addClass("text-success");
        formFeedback.show();
        formForm.hide();
        //trackSignup("Success");

        mixpanel.identify(email.val());
        mixpanel.people.set({
            $email: email.val()
        });
    };

    var onFormError = function () {
        formFeedback.addClass("text-error");
        formFeedback.text("Oops! Error submitting to the server. Please try again.");
        formFeedback.show();
        //trackSignup("Error");
    };
    
    var onFormSubmit = function () {
        //trackSignup("Start");
        var platformsVal = platforms.val();
        var platformsString = platformsVal ? platformsVal.toString() : "";
        var token = $("[name='__RequestVerificationToken']").val();

        $.ajax({
           type: 'POST',
           url: "/RequestAnInvite",
           data: {
               email: email.val(),
               appName: appName.val(),
               platforms: platformsString,
               __RequestVerificationToken: token
           },
           success: onFormSuccess,
           error: onFormError
        });

        mixpanel.track("Page Action", {
            Action: "Request Invite Submit"
        });
    };

    form.validate({
        submitHandler: onFormSubmit,
        onfocusout: false,
        onkeyup: false,
        rules: {
            email: {
                required: true,
                email: true
            }
        },
        messages: {
            email: {
                required: "Please enter your email",
                email: "Your email address must be in the format of name@domain.com"
            }
        }
    });
});
