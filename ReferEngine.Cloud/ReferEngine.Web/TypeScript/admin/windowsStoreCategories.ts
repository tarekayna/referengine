/// <reference path='..\lib\jquery.d.ts' static='true' />
/// <reference path="../lib/require.d.ts" static="true" />

//import Notification = module("../common/notifications");

(function () { 
    //var onError = function () {
    //    Notification.show("Error sending invite", Notification.NotificationType.error);
    //};

    //var onSuccess = function () {
    //    Notification.show("Invite Sent", Notification.NotificationType.success);
    //};

    //$(document).ready(function () {
    //    $("#send-invite").click(function () {
    //        var email = $(":checked").val();
    //        if (email) {
    //            $.ajax("../admin/sendinvite", {
    //                type: "POST",
    //                data: {
    //                    email: email
    //                },
    //                error: onError,
    //                success: onSuccess
    //            });
    //        }
    //    });
    //});

    $(document).ready(function () {
        var uploadFile = function (form, categoryId) {
            var feedbackDiv = form.getElementsByClassName("upload-feedback")[0];

            // Create the iframe...
            var newIframe = document.createElement("iframe");
            newIframe.setAttribute("id", "upload_iframe");
            newIframe.setAttribute("name", "upload_iframe");
            newIframe.setAttribute("width", "0");
            newIframe.setAttribute("height", "0");
            newIframe.setAttribute("border", "0");
            newIframe.setAttribute("style", "width: 0; height: 0; border: none;");

            // Add to document...
            form.parentNode.appendChild(newIframe);
            window.frames['upload_iframe'].name = "upload_iframe";

            var iframe : any = document.getElementById("upload_iframe");

            // Add event...
            var eventHandler = function () {
                if (iframe.detachEvent) iframe.detachEvent("onload", eventHandler);
                else iframe.removeEventListener("load", eventHandler, false);

                //// Message from server...
                var content : string = "";
                if (iframe.contentDocument) {
                    content = iframe.contentDocument.body.innerHTML;
                } else if (iframe.contentWindow) {
                    content = iframe.contentWindow.document.body.innerHTML;
                } else if (iframe.document) {
                    content = iframe.document.body.innerHTML;
                }

                if (content.indexOf("Error") != -1)
                {
                    feedbackDiv.innerHTML = "Error.";
                }
                else {
                    feedbackDiv.innerHTML = "Done.";
                }

                // Del the iframe...
                setTimeout(function () {
                    iframe.parentNode.removeChild(iframe);
                }, 250);
            };

            if (iframe.addEventListener) iframe.addEventListener("load", eventHandler, true);
            else if (iframe.attachEvent) iframe.attachEvent("onload", eventHandler);

            // Set properties of form...
            form.setAttribute("target", "upload_iframe");
            form.setAttribute("action", "../admin/UploadCategoryImage");
            form.setAttribute("categoryId", "22")
            form.setAttribute("method", "post");
            form.setAttribute("enctype", "multipart/form-data");
            form.setAttribute("encoding", "multipart/form-data");

            // Submit the form...
            form.submit();

            feedbackDiv.innerHTML = "Uploading...";
        };

        window["uploadFile"] = uploadFile;
    });

})();