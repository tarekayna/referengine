/// <reference path='..\lib\jquery.d.ts' static='true' />
/// <reference path='..\lib\ReferEngineGlobals.d.ts' static='true' />

$(document).ready(function () {
    $("#confirm-app-delete").click(function () {
        window.location.href = "../delete/" + re.appId;
    });
});