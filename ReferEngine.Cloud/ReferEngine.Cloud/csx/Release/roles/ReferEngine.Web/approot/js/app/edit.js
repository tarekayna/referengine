$(document).ready(function () {
    var baseUrl = "http://127.0.0.1:81/";
    var img01 = $(".img01");
    var appId = $("#appId").val();
    var file = $(".hidden-file");
    var button = $(".btn");
    var postUrl = baseUrl + "app/UploadScreenshots/" + appId;

    var uploadFile = function (form) {
        var feedbackDiv = form.getElementsByClassName("upload-feedback")[0];
        
        // Create the iframe...
        var iframe = document.createElement("iframe");
        iframe.setAttribute("id", "upload_iframe");
        iframe.setAttribute("name", "upload_iframe");
        iframe.setAttribute("width", "0");
        iframe.setAttribute("height", "0");
        iframe.setAttribute("border", "0");
        iframe.setAttribute("style", "width: 0; height: 0; border: none;");

        //var iframe = $("<div/>", {
        //    id: "upload_iframe",
        //    name: "upload_iframe",
        //    width: "0",
        //    height: "0",
        //    border: "0",
        //    className: "hidden-iframe"
        //}).appendTo(form);

        // Add to document...
        form.parentNode.appendChild(iframe);
        window.frames['upload_iframe'].name = "upload_iframe";

        iframeId = document.getElementById("upload_iframe");

        // Add event...
        var eventHandler = function() {

            if (iframeId.detachEvent) iframeId.detachEvent("onload", eventHandler);
            else iframeId.removeEventListener("load", eventHandler, false);
            //iframe.off("load", eventHandler);

            // Message from server...
            var content;
            if (iframeId.contentDocument) {
                content = iframeId.contentDocument.body.innerHTML;
            } else if (iframeId.contentWindow) {
                content = iframeId.contentWindow.document.body.innerHTML;
            } else if (iframeId.document) {
                content = iframeId.document.body.innerHTML;
            }
            //var content;
            //if (iframe.contentDocument) {
            //    content = iframe.contentDocument.body.innerHTML;
            //} else if (iframe.contentWindow) {
            //    content = iframe.contentWindow.document.body.innerHTML;
            //} else if (iframe.document) {
            //    content = iframe.document.body.innerHTML;
            //}

            feedbackDiv.innerHTML = "Done.";

            // Del the iframe...
            //setTimeout('iframeId.parentNode.removeChild(iframeId)', 250);
            //iframe.remove();
        };

        if (iframeId.addEventListener) iframeId.addEventListener("load", eventHandler, true);
        else if (iframeId.attachEvent) iframeId.attachEvent("onload", eventHandler);
        //iframe.on("load", eventHandler);

        // Set properties of form...
        form.setAttribute("target", "upload_iframe");
        form.setAttribute("action", postUrl);
        form.setAttribute("method", "post");
        form.setAttribute("enctype", "multipart/form-data");
        form.setAttribute("encoding", "multipart/form-data");

        // Submit the form...
        form.submit();

        feedbackDiv.innerHTML = "Uploading...";
    };

    if (window.RE == undefined) {
        window.RE = {};
    }

    window.RE.App = {
        UploadFile: uploadFile  
    };
});