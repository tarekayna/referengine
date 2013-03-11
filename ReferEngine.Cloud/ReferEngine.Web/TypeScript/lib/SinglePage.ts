﻿declare var $;

export function goToStep(stepName: string) {
    if (stepName !== _currentStepName) {
        var e = $("[data-sp-step='" + stepName + "']");
        if (e.length > 0) {
            if (_currentStepElem && _currentStepName) {
                if (_eventHandlers.beforeHide[_currentStepName]) {
                    var handler = _eventHandlers.beforeHide[_currentStepName];
                    if (!handler.runOnce || !handler.ranOnce) {
                        handler.callback();
                        handler.ranOnce = true;
                    }
                }
                _currentStepElem.hide();
                if (_eventHandlers.afterHide[_currentStepName]) {
                    var handler = _eventHandlers.afterHide[_currentStepName];
                    if (!handler.runOnce || !handler.ranOnce) {
                        handler.callback();
                        handler.ranOnce = true;
                    }
                }

                _previousSteps.push(_currentStepName);
            }

            _currentStepName = stepName;
            _currentStepElem = e;

            if (_eventHandlers.beforeShow[_currentStepName]) {
                var handler = _eventHandlers.beforeShow[_currentStepName];
                if (!handler.runOnce || !handler.ranOnce) {
                    handler.callback();
                    handler.ranOnce = true;
                }
            }
            _currentStepElem.show();
            if (_eventHandlers.afterShow[_currentStepName]) {
                var handler = _eventHandlers.afterShow[_currentStepName];
                if (!handler.runOnce || !handler.ranOnce) {
                    handler.callback();
                    handler.ranOnce = true;
                }
            }
        }
    }
}
export function start(stepName: string) {
    $("[data-sp-step]").each(function () {
        $(this).hide();
        return 0;
    });
    goToStep(stepName);
}
export function on(stepName: string,
            eventName: string,
            callback: Function,
            runOnce?: bool = true) {
    if (eventName !== "beforeShow" &&
        eventName !== "afterShow" &&
        eventName !== "beforeHide" &&
        eventName !== "afterHide") {
        throw "Invalid event name";
    }
    _eventHandlers[eventName][stepName] = {
        callback: callback,
        runOnce: runOnce,
        ranOnce: false
    };
}
var _eventHandlers = {
    beforeShow: {},
    afterShow: {},
    beforeHide: {},
    afterHide: {}
}
var _currentStepName;
var _currentStepElem;
var _previousSteps = [];