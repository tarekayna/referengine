$(document).ready(function () {
    var submitButton = $("#submit-button");
    //var baseUrl = "http://www.referengine.com";
    var baseUrl = "http://127.0.0.1";
    var appId = $("#appId").val();
    var userAccessToken = $("#userAccessToken").val();

    submitButton.click(function () {
        var checked = $("input[type='checkbox']:checked");
        for (var i = 0; i < checked.length; i++) {
            var friendId = checked[i].name;
            var link = baseUrl + "/refer/win8/postToTimeline/" +
                appId + "?userAccessToken=" + userAccessToken +
                "&friendId=" + friendId;
            $.ajax({
                url: link,
                type: "POST",
                beforeSend: function() {
                    
                },
                success: function(data) {
                    
                },
                error: function() {
                    
                },
                complete: function() {
                    
                }
            });
        }
    });
});