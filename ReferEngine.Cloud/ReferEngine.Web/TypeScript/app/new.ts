/// <reference path='..\lib\jquery.d.ts' static='true' />
/// <reference path="../lib/require.d.ts" static="true" />
import singlePage = module("../lib/SinglePage");

require(["../lib/knockout"], function (ko) {
    var ViewModel :any = {}; 
    ViewModel.AppSearchTerm = ko.observable("");
    ViewModel.AppSearchTermComputed = ko.computed(function () {
        return ViewModel.AppSearchTerm();
    }).extend({ throttle: 300 });
    ViewModel.AppSearchResults = ko.observableArray();
    ViewModel.SelectedApp = ko.observable();
    ViewModel.onClickAppRow = function(app) {
        ViewModel.SelectedApp(app);
        singlePage.goToStep("app-verify");
    }
    ViewModel.onClickMyApp = function () {
        $.ajax("../app/AddNewApp", {
            type: "POST",
            data: {
                msAppId: ViewModel.SelectedApp().MsAppId
            },
            dataType: "json",
            error: function () { },
            success: function () { }
        });
    }

    var onFindAppSuccess = function (data, textStatus, jqXhr) {
        ViewModel.AppSearchResults(data);
    };

    var onFindAppError = function () {
    };

    ViewModel.AppSearchTermComputed.subscribe(function (newValue) {
        if (newValue && newValue !== "") {
            $.ajax("../app/SearchForApp", {
                type: "POST",
                data: {
                    name: ViewModel.AppSearchTerm(),
                    platform: "windows"
                },
                dataType: "json",
                error: onFindAppError,
                success: onFindAppSuccess
            });
        }
        else {
            ViewModel.AppSearchResults([]);
        }
    });

    singlePage.on("app-search", "afterShow", function () {
        $("#refine-app-name").focus();
    }, false);

    singlePage.on("app-verify", "beforeShow", function () {
        $(".btn.back-to-search").click(function () {
            singlePage.goToStep("app-search");
        });
    }, true);

    $(document).ready(function () {
        ko.applyBindings(ViewModel);
        singlePage.start("app-search");
    });
});
