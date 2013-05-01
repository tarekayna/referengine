define(["require", "exports", "../../common/notifications"], function(require, exports, __Notification__) {
    var Notification = __Notification__;

    var ko;
    var Date2;
    var Page = (function () {
        function Page() { }
        Page.initPage = function initPage() {
            People.init();
            var openStore = $("#open-win8-store");
            if(openStore.length > 0) {
                var link = openStore.first().attr("data-store-link");
                openStore.click(function (event) {
                    event.stopPropagation();
                    $('#open-store-frame').attr('src', link);
                });
            }
            $(".popover-button").popover({
            });
        };
        return Page;
    })();    
    var PeopleViewModel = (function () {
        function PeopleViewModel() {
            this.Recommenders = ko.observableArray();
            this.RecommendersLoaded = ko.observable(false);
        }
        return PeopleViewModel;
    })();    
    var People = (function () {
        function People() { }
        People.isInitialized = false;
        People.init = function init() {
            if(!People.isInitialized) {
                People.viewModel = new PeopleViewModel();
                ko.applyBindings(People.viewModel);
                People.isInitialized = true;
            }
            People.refresh();
        };
        People.onDataRequestSuccess = function onDataRequestSuccess(data, textStatus, jqXhr) {
            if(!People.isInitialized) {
                People.init();
            }
            People.viewModel.Recommenders.removeAll();
            for(var i = 0; i < data.length; i++) {
                People.viewModel.Recommenders.push(data[i]);
            }
            Notification.show("Success: recommendations updated", Notification.NotificationType.success);
            People.viewModel.RecommendersLoaded(true);
        };
        People.onDataRequestError = function onDataRequestError(e) {
            Notification.show("Error: could not update recommendations", Notification.NotificationType.error);
        };
        People.refresh = function refresh() {
            if(re.appId != -1) {
                $.ajax("../a/GetAppRecommendations", {
                    type: "POST",
                    data: {
                        appId: re.appId,
                        page: 1
                    },
                    dataType: "json",
                    error: People.onDataRequestError,
                    success: People.onDataRequestSuccess
                });
                Notification.show("Updating recommendations...", Notification.NotificationType.info);
            } else {
                People.viewModel.RecommendersLoaded(true);
            }
        };
        return People;
    })();    
    require([
        "../../lib/knockout", 
        "../../lib/date", 
        "../../lib/daterangepicker"
    ], function (_ko) {
        ko = _ko;
        Date2 = Date;
        $(document).ready(Page.initPage);
    });
})
