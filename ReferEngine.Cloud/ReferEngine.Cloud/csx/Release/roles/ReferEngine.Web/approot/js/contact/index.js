$(document).ready(function () {
    var name = $("#name"),
        email = $("#email"),
        subject = $("#subject"),
        message = $("#message"),
        sendButton = $("#send"),
        form = $("form");

    var onFormSuccess = function () {
        form.text("Thank you for your message. We will be in touch!");
    };

    var onFormError = function () {
        form.append("<div class='text-error'>An error occured while submitting your message! Please try again.</div>");
    };

    var onFormSubmit = function () {
        $.ajax({
            type: 'POST',
            url: "/Contact/SendMessage",
            data: {
                name: name.val(),
                email: email.val(),
                subject: subject.val(),
                message: message.val()
            },
            success: onFormSuccess,
            error: onFormError
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
            },
            message: {
                required: true
            }
        },
        messages: {
            email: {
                required: "Please enter your email",
                email: "Your email address must be in the format of name@domain.com"
            },
            message: {
                required: "Please enter a message"
            }
        }
    });
});