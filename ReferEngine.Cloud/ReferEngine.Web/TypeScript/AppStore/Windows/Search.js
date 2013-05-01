define(["require", "exports", "../../common/notifications"], function(require, exports, __Notification__) {
    var Notification = __Notification__;

    var ko;
    var ViewModel = (function () {
        function ViewModel() {
            this.Apps = ko.observableArray();
            this.SearchTerm = ko.observable();
            this.SearchCompleted = ko.observable(false);
        }
        return ViewModel;
    })();    
    var Page = (function () {
        function Page() { }
        Page.isInitialized = false;
        Page.init = function init() {
            if(!Page.isInitialized) {
                Page.viewModel = new ViewModel();
                ko.applyBindings(Page.viewModel);
                Page.GetApps(re.SearchTerm, re.CategoryName, re.ParentCategoryName, re.PageNumber, re.NumberOfApps);
                Page.isInitialized = true;
            }
        };
        Page.onGetAppsSuccess = function onGetAppsSuccess(data, textStatus, jqXhr) {
            if(!Page.isInitialized) {
                Page.init();
            }
            Page.viewModel.Apps.removeAll();
            for(var i = 0; i < data.length; i++) {
                Page.viewModel.Apps.push(data[i]);
            }
            Page.viewModel.SearchCompleted(true);
        };
        Page.onGetAppsError = function onGetAppsError(e) {
            Page.viewModel.SearchCompleted(true);
            Notification.show("Error: could not retreive apps", Notification.NotificationType.error);
        };
        Page.GetApps = function GetApps(searchTerm, category, parentCategory, page, numberOfApps) {
            var link = "/app-store/windows/a/getapps";
            Page.viewModel.SearchTerm(searchTerm);
            $.ajax(link, {
                type: "POST",
                data: {
                    searchTerm: searchTerm,
                    category: category,
                    parentCategory: parentCategory,
                    page: page,
                    numberOfApps: numberOfApps
                },
                dataType: "json",
                error: Page.onGetAppsError,
                success: Page.onGetAppsSuccess
            });
        };
        return Page;
    })();    
    require([
        "../../lib/knockout", 
        "../../lib/date", 
        "../../lib/daterangepicker"
    ], function (_ko) {
        ko = _ko;
        $(document).ready(Page.init);
    });
})
