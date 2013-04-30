/// <reference path='..\lib\jquery.d.ts' static='true' />

export class NotificationType {
    static none = "";
    static info = "info";
    static success = "success";
    static error = "error";
}

export function show(message: string, notificationType: string) {
    $(".top-right").notify({
        message: { text: message },
        transition: "fade",
        type: notificationType
    }).show();
} 