var ViewModel = (function () {
    function ViewModel() { }
    ViewModel.AppSearchTerm = ko.observable("");
    ViewModel.AppSearchTermComputed = ko.computed(function () {
        return ViewModel.AppSearchTerm();
    }).extend({
        throttle: 300
    });
    ViewModel.AppSearchResults = ko.observableArray();
    return ViewModel;
})();
var onFindAppSuccess = function (data, textStatus, jqXhr) {
    ViewModel.AppSearchResults(data);
};
var onFindAppError = function () {
};
ViewModel.AppSearchTermComputed.subscribe(function (newValue) {
    if(newValue && newValue !== "") {
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
    } else {
        ViewModel.AppSearchResults([]);
    }
});
var Step = (function () {
    function Step(beforeShow, afterShow, beforeHide, afterHide) {
        this.beforeShow = beforeShow;
        this.beforeHide = beforeHide;
        this.afterHide = afterHide;
        this.afterShow = afterShow;
    }
    Step.prototype.hide = function () {
        if(this.beforeHide) {
            this.beforeHide();
        }
        $("." + this.name).hide();
        if(this.afterHide) {
            this.afterHide();
        }
    };
    Step.prototype.show = function () {
        if(this.beforeShow) {
            this.beforeShow();
        }
        $("." + this.name).show();
        if(this.afterShow) {
            this.afterShow();
        }
    };
    return Step;
})();
var StepManager = (function () {
    function StepManager() { }
    StepManager.AddStep = function AddStep(step) {
        step.name = "step" + StepManager.Steps.length;
        StepManager.Steps.push(step);
    };
    StepManager.NextStep = function NextStep() {
        StepManager.Steps[StepManager.CurrentStepIndex].hide();
        StepManager.CurrentStepIndex++;
        if(StepManager.CurrentStepIndex < StepManager.Steps.length) {
            StepManager.Steps[StepManager.CurrentStepIndex].show();
        }
    };
    StepManager.Start = function Start() {
        StepManager.CurrentStepIndex = 0;
        StepManager.Steps[StepManager.CurrentStepIndex].show();
        for(var i = 1; i < StepManager.Steps.length; i++) {
            StepManager.Steps[i].hide();
        }
    };
    StepManager.Steps = [];
    StepManager.CurrentStepIndex = -1;
    return StepManager;
})();
var step0 = new Step();
step0.beforeShow = function () {
    $("#btn-find-app").click(function () {
    });
};
StepManager.AddStep(step0);
$(document).ready(function () {
    ko.applyBindings(ViewModel);
    StepManager.Start();
});
