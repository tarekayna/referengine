var trackSignup = function(status) {
    mixpanel.track("SignUp", { Status: status });
};

$(document).ready(function () {
    var form = $("#email-form");
    var formFeedback = $("#form-feedback p");
    var email = $("#email");
    
    formFeedback.hide();

    var onFormSuccess = function () {
        formFeedback.text("Thank you! We will be in touch soon.");
        formFeedback.addClass("text-success");
        formFeedback.show();
        form.hide();
        trackSignup("Success");
    };

    var onFormError = function () {
        formFeedback.addClass("text-error");
        formFeedback.text("Error sending feedback. Please try again.");
        formFeedback.show();
        trackSignup("Error");
    };
    
    var onFormSubmit = function () {
        trackSignup("Start");
        var data = {
            email: email.val()
        };
        var xhr = $.post("/", data, onFormSuccess);
        xhr.error(onFormError);
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