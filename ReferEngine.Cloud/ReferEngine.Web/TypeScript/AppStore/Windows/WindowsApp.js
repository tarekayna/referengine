define(["require", "exports", "../../common/notifications"], function(require, exports, __Notification__) {
    var Notification = __Notification__;

    var ko;
    var Date2;
    var Page = (function () {
        function Page() { }
        Page.initPage = function initPage() {
            People.init();
        };
        return Page;
    })();    
    var PeopleViewModel = (function () {
        function PeopleViewModel() {
            this.PeopleData = ko.observableArray();
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
            People.viewModel.PeopleData.removeAll();
            for(var i = 0; i < data.length; i++) {
                People.viewModel.PeopleData.push(data[i]);
            }
            Notification.show("Success: people view updated", Notification.NotificationType.success);
        };
        People.onDataRequestError = function onDataRequestError(e) {
            Notification.show("Error: could not update people view", Notification.NotificationType.error);
        };
        People.refresh = function refresh() {
            $.ajax("../GetAppRecommendations", {
                type: "POST",
                data: {
                    appId: re.appId,
                    page: 1
                },
                dataType: "json",
                error: People.onDataRequestError,
                success: People.onDataRequestSuccess
            });
            Notification.show("Updating people view...", Notification.NotificationType.info);
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
