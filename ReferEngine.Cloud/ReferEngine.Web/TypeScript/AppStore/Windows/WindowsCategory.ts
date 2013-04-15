/// <reference path='../../lib/jquery.d.ts' static='true' />
/// <reference path="../../lib/require.d.ts" static="true" />
/// <reference path='../../lib/ReferEngineGlobals.d.ts' static='true' />

declare var google;
declare var re;

import Notification = module("../../common/notifications"); 

var ko;

class ViewModel {
    constructor() {
        this.Apps = ko.observableArray();
    }
    Apps: any;
}

class Page {
    static viewModel;
    static isInitialized = false;
    static init() {
        if (!isInitialized) {
            viewModel = new ViewModel();
            ko.applyBindings(viewModel);
            isInitialized = true;
        }
    }
    static onGetAppsSuccess(data, textStatus, jqXhr) {
        if (!Page.isInitialized) Page.init();
        viewModel.Apps.removeAll();
        for (var i = 0; i < data.length; i++) {
            viewModel.Apps.push(data[i]);
        }
    }
    static onGetAppsError(e) {
        Notification.show("Error: could not retreive apps", Notification.NotificationType.error);
    }
    static GetApps(searchTerm, category, parentCategory, page, numberOfApps) {
        var link = "/appstore/windows/a/getapps";
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
            error: onGetAppsError,
            success: onGetAppsSuccess
        });

        Notification.show("Retreiving apps...", Notification.NotificationType.info);
    }
}

require(["../../lib/knockout",
         "../../lib/date",
         "../../lib/daterangepicker"], function (_ko) {
             ko = _ko;
             $(document).ready(Page.init);
         }); 