$(document).ready(function () {
    var util = RE.Utilities,
        submitButton = $("#submit-button"),
        msgDiv = $("#message"),
        asContainer = $("#asContainer"),
        searchStringStart = 0,
        searchStringEnd = 0,
        msgPrev = "",
        searchString = "";

    var keys = {
        enter: 13,
        space: 32,
        up: 38,
        down: 40
    };

    var autoSuggestViewModelClass = function () {
        var thisViewModel = this;
        thisViewModel.msgText = ko.observable("");
        thisViewModel.msgHTML = ko.observable("");
        thisViewModel.searchString = ko.observable("");
        thisViewModel.friends = RE.Friends;
        thisViewModel.selectedIndex = ko.observable(-1);
        thisViewModel.searchResult = ko.computed(function () {
            var str = thisViewModel.searchString().toLowerCase();
            var firstNameResult = [];
            var lastNameResult = [];
            if (str != undefined && str !== "") {
                for (var i = 0; i < thisViewModel.friends.length; i++) {
                    var friend = thisViewModel.friends[i];
                    var firstName = friend.FirstName.toLowerCase();
                    var lastName = friend.LastName.toLowerCase();
                    if (firstName.indexOf(str) === 0) {
                        firstNameResult.push(friend);
                    }
                    if (lastName.indexOf(str) === 0) {
                        lastNameResult.push(friend);
                    }
                }
            }
            return firstNameResult.concat(lastNameResult);
        });

        thisViewModel.onClick = function (friend) {
            tagFriend(friend);
        };
    };

    var viewModel = new autoSuggestViewModelClass();
    ko.applyBindings(viewModel);

    viewModel.searchResult.subscribe(function(newSearchResult) {
        if (newSearchResult.length > 0) {
            var searchStrElem = msgDiv.find(".search-str");
            if (searchStrElem.length > 0) {
                var offset = searchStrElem.offset();
                //offset.left += searchStrElem.outerWidth();
                offset.top += searchStrElem.outerHeight();
                asContainer.offset(offset);

                searchStrElem.addClass("highlighted");
            }
        } else {
            asContainer.css("display", "none");
        }
    });

    var removeSearchStringClass = function() {
        var elem = msgDiv.find(".search-str");
        if (elem.length > 0) {
            elem.replaceWith(elem.text());
        }
    };

    var tagFriend = function(friend) {
        msgDiv.find(".search-str")
            .replaceWith("<span class='friendTag' data-friend-id='" + friend.FacebookId + "'>" + friend.Name + "</span>");
        msgDiv.focus();
        viewModel.searchString("");
    };

    var latestTimestamp;
    var verifyTimestamp = function(event) {
        return latestTimestamp === event.timeStamp;
    };

    var onDownArrow = function () {
        if (viewModel.searchResult() !== []) {
            viewModel.selectedIndex(viewModel.selectedIndex() + 1);
        }
    };

    var onUpArrow = function () {
        if (viewModel.searchResult() !== []) {
            viewModel.selectedIndex(viewModel.selectedIndex() - 1);
        }
    };

    msgDiv.focusout(function() {
        removeSearchStringClass();
        viewModel.searchString("");
    });

    msgDiv.focusin(function() {
        $(".msg-placeholder").css("display", "none");
    });

    msgDiv.keydown(function(event) {
        if (event.which === keys.enter) {
            var selectedIndex = viewModel.selectedIndex();
            if (viewModel.searchResult() !== [] && selectedIndex != -1) {
                tagFriend(viewModel.searchResult()[selectedIndex]);
                msgDiv.find("p").last().remove();
            }
            
            event.stopImmediatePropagation();
            event.preventDefault();
        }
    });

    msgDiv.keyup(function (event) {
        if (event.which === keys.up) {
            return onUpArrow();
        }
        else if (event.which === keys.down) {
            return onDownArrow(event);
        }
        else {
            viewModel.selectedIndex(-1);
        }
        
        latestTimestamp = event.timeStamp;
        removeSearchStringClass();

        viewModel.msgText(msgDiv.text());
        
        searchString = "";
        var msg = viewModel.msgText();
        var msgLength = msg.length;
        if (msgLength === msgPrev.length + 1) {
            for (var i = msgLength - 1; i >= 0 && verifyTimestamp(event); i--) {
                if (msg.charAt(i) !== msgPrev.charAt(i - 1)) {
                    // new char is at i
                    searchStringEnd = i;
                    searchString = msg.substring(0, i + 1);
                    searchStringStart = searchString.lastIndexOf(" ") + 1;
                    searchString = searchString.substring(searchStringStart);
                    if (searchString !== "") {
                        var msgHtml = msgDiv.html();
                        var htmlStart = searchStringStart;
                        var htmlEnd = searchStringEnd;
                        var textIndex = -1;
                        var increment = true;
                        if (msgHtml.indexOf("<") !== -1) {
                            for (var j = 0; j < msgHtml.length; j++) {
                                increment = (msgHtml.charAt(j) === '<') ? false : increment;
                                textIndex = increment ? textIndex + 1 : textIndex;
                                increment = (msgHtml.charAt(j) === '>') ? true : increment;
                                if (textIndex === searchStringStart) {
                                    htmlStart = j;
                                }
                                if (textIndex === searchStringEnd) {
                                    htmlEnd = j;
                                    break;
                                }
                            }
                        }
                        var pre = msgHtml.substring(0, htmlStart);
                        var post = msgHtml.substring(htmlEnd + 1);
                        var newMsg = pre + "<span class='search-str'>" + searchString + "</span>" + post;

                        if (verifyTimestamp(event)) {
                            viewModel.msgHTML(newMsg);
                        }
                    }

                    break;
                }
            }
        }
        
        if (verifyTimestamp(event)) {
            viewModel.searchString(searchString);
            msgPrev = viewModel.msgText();
            viewModel.msgHTML(msgDiv.html());
        }
    });

    var onSubmitError = function (jqXhr, textStatus, errorThrown) {

    };

    var onSubmitSuccess = function (data, textStatus, jqXhr) {

    };

    submitButton.click(function () {
        util.ShowLoading("Posting to Facebook...");
        
        var msg = msgDiv.clone();
        var tags = msg.find(".friendTag");
        if (tags.length > 0) {
            tags.replaceWith(function() {
                var id = $(this).attr("data-friend-id");
                $(this).replaceWith("@[" + id + "]");
            });
        }
        var msgText = msg.text();

        var postUri = util.GetLink("PostRecommendation");

        $.ajax({
            type: "POST",
            url: postUri,
            data: {
                message: msgText,
                re_auth_token: RE.ReferEngineAuthToken
            },
            dataType: "json",
            error: onSubmitError,
            success: onSubmitSuccess
        });
    });

    asContainer.css("display", "none");
    util.HideLoading();
});