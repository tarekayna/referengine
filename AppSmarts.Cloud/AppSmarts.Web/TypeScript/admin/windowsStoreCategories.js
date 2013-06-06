(function () {
    $(document).ready(function () {
        var uploadFile = function (form, categoryId) {
            var feedbackDiv = form.getElementsByClassName("upload-feedback")[0];
            var newIframe = document.createElement("iframe");
            newIframe.setAttribute("id", "upload_iframe");
            newIframe.setAttribute("name", "upload_iframe");
            newIframe.setAttribute("width", "0");
            newIframe.setAttribute("height", "0");
            newIframe.setAttribute("border", "0");
            newIframe.setAttribute("style", "width: 0; height: 0; border: none;");
            form.parentNode.appendChild(newIframe);
            window.frames['upload_iframe'].name = "upload_iframe";
            var iframe = document.getElementById("upload_iframe");
            var eventHandler = function () {
                if(iframe.detachEvent) {
                    iframe.detachEvent("onload", eventHandler);
                } else {
                    iframe.removeEventListener("load", eventHandler, false);
                }
                var content = "";
                if(iframe.contentDocument) {
                    content = iframe.contentDocument.body.innerHTML;
                } else if(iframe.contentWindow) {
                    content = iframe.contentWindow.document.body.innerHTML;
                } else if(iframe.document) {
                    content = iframe.document.body.innerHTML;
                }
                if(content.indexOf("Error") != -1) {
                    feedbackDiv.innerHTML = "Error.";
                } else {
                    feedbackDiv.innerHTML = "Done.";
                }
                setTimeout(function () {
                    iframe.parentNode.removeChild(iframe);
                }, 250);
            };
            if(iframe.addEventListener) {
                iframe.addEventListener("load", eventHandler, true);
            } else if(iframe.attachEvent) {
                iframe.attachEvent("onload", eventHandler);
            }
            form.setAttribute("target", "upload_iframe");
            form.setAttribute("action", "../admin/UploadCategoryImage");
            form.setAttribute("categoryId", "22");
            form.setAttribute("method", "post");
            form.setAttribute("enctype", "multipart/form-data");
            form.setAttribute("encoding", "multipart/form-data");
            form.submit();
            feedbackDiv.innerHTML = "Uploading...";
        };
        window["uploadFile"] = uploadFile;
    });
})();
