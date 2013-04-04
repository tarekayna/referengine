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
    };
    People.onDataRequestError = function onDataRequestError(e) {
    };
    People.refresh = function refresh() {
        $.ajax("../GetAppRecommendations", {
            type: "POST",
            data: {
                id: 21,
                count: 10,
                start: 1
            },
            dataType: "json",
            error: People.onDataRequestError,
            success: People.onDataRequestSuccess
        });
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
