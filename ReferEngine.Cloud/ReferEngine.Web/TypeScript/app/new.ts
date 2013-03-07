///<reference path='..\lib\jquery.d.ts' static='true' />

declare var ko;

class ViewModel {
    static AppSearchTerm = ko.observable("");
    static AppSearchTermComputed = ko.computed(function () {
        return ViewModel.AppSearchTerm();
    }).extend({ throttle: 300 });
    static AppSearchResults = ko.observableArray();
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

class Step {
    name: string;
    beforeShow: Function;
    afterShow: Function;
    beforeHide: Function;
    afterHide: Function;
    constructor(beforeShow?: Function,
                afterShow?,
                beforeHide?,
                afterHide?) {
        this.beforeShow = beforeShow;
        this.beforeHide = beforeHide;
        this.afterHide = afterHide;
        this.afterShow = afterShow;
    }
    hide() {
        if (this.beforeHide) {
            this.beforeHide();
        }
        $("." + this.name).hide();
        if (this.afterHide) {
            this.afterHide();
        }
    }
    show() {
        if (this.beforeShow) {
            this.beforeShow();
        }
        $("." + this.name).show();
        if (this.afterShow) {
            this.afterShow();
        }
    }
}

class StepManager {
    static AddStep(step: Step) {
        step.name = "step" + Steps.length;
        Steps.push(step);
    }
    static NextStep() {
        Steps[CurrentStepIndex].hide();
        CurrentStepIndex++;
        if (CurrentStepIndex < Steps.length) {
            Steps[CurrentStepIndex].show();
        }
    }
    static Start() {
        CurrentStepIndex = 0;
        Steps[CurrentStepIndex].show();
        for (var i = 1; i < Steps.length; i++) {
            Steps[i].hide();
        }
    }
    private static Steps: Step[] = [];
    private static CurrentStepIndex: number = -1;
}

var step0 = new Step();
step0.beforeShow = function () {
    $("#btn-find-app").click(function () {
        //ViewModel.AppSearchTerm($("#find-app-name").val());
    });
};

StepManager.AddStep(step0);

$(document).ready(function () {
    ko.applyBindings(ViewModel);

    StepManager.Start();
});