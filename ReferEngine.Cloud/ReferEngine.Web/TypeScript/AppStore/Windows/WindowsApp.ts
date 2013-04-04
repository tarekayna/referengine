/// <reference path='../../lib/jquery.d.ts' static='true' />
/// <reference path="../../lib/require.d.ts" static="true" />
/// <reference path='../../lib/ReferEngineGlobals.d.ts' static='true' />

declare var google;
declare var re;

//import Notification = module("../../common/notifications"); 

var ko;
var Date2;

class Page {
    static initPage() {
        People.init();
    }
}

class PeopleViewModel {
    constructor() {
        this.PeopleData = ko.observableArray();
    }
    PeopleData: any;
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
        viewModel.PeopleData.removeAll();
        for (var i = 0; i < data.length; i++) {
            viewModel.PeopleData.push(data[i]);
        }
        //Notification.show("Success: people view updated", Notification.NotificationType.success);
    }
    static onDataRequestError(e) {
        //Notification.show("Error: could not update people view", Notification.NotificationType.error);
    }
    static refresh() {
        $.ajax("../GetAppRecommendations", {
            type: "POST",
            data: {
                id: 21,
                count: 10,
                start: 1
            },
            dataType: "json",
            error: onDataRequestError,
            success: onDataRequestSuccess
        });

        //Notification.show("Updating people view...", Notification.NotificationType.info);
    }
}

require(["../../lib/knockout",
         "../../lib/date",
         "../../lib/daterangepicker"], function (_ko) {
             ko = _ko;
             Date2 = <any>Date;
             $(document).ready(Page.initPage);
         }); 