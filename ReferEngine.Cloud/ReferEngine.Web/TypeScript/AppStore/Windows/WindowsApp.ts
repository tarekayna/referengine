/// <reference path='../../lib/jquery.d.ts' static='true' />
/// <reference path="../../lib/require.d.ts" static="true" />
/// <reference path='../../lib/ReferEngineGlobals.d.ts' static='true' />

declare var google;
declare var re;

import Notification = module("../../common/notifications"); 
 
var ko;
var Date2;

class Page {
    static initPage() {
        People.init();

        var openStore = $("#open-win8-store");
        if (openStore.length > 0) {
            var link = openStore.first().attr("data-store-link");
            openStore.click(function (event) {
                event.stopPropagation();
                $('#open-store-frame').attr('src', link);
            });
        }
    }
}

class PeopleViewModel {
    constructor() {
        this.Recommenders = ko.observableArray();
        this.RecommendersLoaded = ko.observable(false);
    }
    Recommenders: any;
    RecommendersLoaded: any;
}

class People {
    static viewModel;
    static isInitialized = false;
    static init() {
        if (!isInitialized) {
            viewModel = new PeopleViewModel();
            ko.applyBindings(viewModel);
            isInitialized = true;
        }
        refresh();
    }
    static onDataRequestSuccess(data, textStatus, jqXhr) {
        if (!People.isInitialized) People.init();
        viewModel.Recommenders.removeAll();
        for (var i = 0; i < data.length; i++) {
            viewModel.Recommenders.push(data[i]);
        }
        Notification.show("Success: recommendations updated", Notification.NotificationType.success);
        viewModel.RecommendersLoaded(true);
    }
    static onDataRequestError(e) {
        Notification.show("Error: could not update recommendations", Notification.NotificationType.error);
    }
    static refresh() { 
        if (re.appId != -1) {
            $.ajax("../a/GetAppRecommendations", {
                type: "POST",
                data: {
                    appId: re.appId,
                    page: 1
                },
                dataType: "json",
                error: onDataRequestError,
                success: onDataRequestSuccess
            });

            Notification.show("Updating recommendations...", Notification.NotificationType.info);
        }
        else {
            viewModel.RecommendersLoaded(true); 
        }
    }
}

require(["../../lib/knockout",
         "../../lib/date",
         "../../lib/daterangepicker"], function (_ko) {
             ko = _ko;
             Date2 = <any>Date;
             $(document).ready(Page.initPage);
         }); 