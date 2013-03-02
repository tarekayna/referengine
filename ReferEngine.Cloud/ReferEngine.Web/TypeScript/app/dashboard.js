var NotificationType = (function () {
    function NotificationType() { }
    NotificationType.none = "";
    NotificationType.info = "info";
    NotificationType.success = "success";
    NotificationType.error = "error";
    return NotificationType;
})();
var Notifications = (function () {
    function Notifications() { }
    Notifications.show = function (message, notificationType) {
        $(".top-right").notify({
            message: {
                text: message
            },
            transition: "fade",
            type: notificationType
        }).show();
    };
    return Notifications;
})();
var Map = (function () {
    function Map() { }
    Map.map = null;
    Map.mapData = [];
    Map.heatData = [];
    Map.markers = [];
    Map.how = "location-map";
    Map.who = "launched";
    Map.when = "past-30-days";
    Map.center = new google.maps.LatLng(0, 0);
    Map.options = {
        center: Map.center,
        zoom: 2,
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        streetViewControl: false,
        mapTypeControl: false
    };
    Map.updateHeatmapData = function () {
        Map.heatData = [];
        for(var j = 0; j < Map.mapData.length; j++) {
            Map.heatData.push(Map.mapData[j].latlong);
        }
        Map.heatMap = new google.maps.visualization.HeatmapLayer({
            data: Map.heatData
        });
    };
    Map.showHeatmap = function () {
        Map.updateHeatmapData();
        Map.heatMap.setMap(Map.map);
    };
    Map.hideHeatmap = function () {
        if(Map.heatMap !== undefined) {
            Map.heatMap.setMap(null);
        }
    };
    Map.removeMarkers = function () {
        for(var j = 0; j < Map.markers.length; j++) {
            Map.markers[j].setMap(null);
        }
    };
    Map.updateMarkers = function () {
        Map.removeMarkers();
        for(var i = 0; i < Map.mapData.length; i++) {
            Map.markers.push(new google.maps.Marker({
                position: Map.mapData[i].latlong,
                map: Map.map,
                title: Map.mapData[i].city,
                origin: new google.maps.Point(16, 16),
                anchor: new google.maps.Point(16, 16),
                icon: "https://referenginestorage.blob.core.windows.net/referengine-design/logo-mark-32.png"
            }));
        }
    };
    Map.refresh = function () {
        if(Map.map === null) {
            Map.map = new google.maps.Map(document.getElementById("map-canvas"), Map.options);
        }
        var onSubmitSuccess = function (data, textStatus, jqXhr) {
            Map.mapData = [];
            for(var i = 0; i < data.length; i++) {
                Map.mapData.push({
                    latlong: new google.maps.LatLng(data[i].Latitude, data[i].Longitude),
                    city: data[i].City
                });
            }
            if(Map.how === "location-map") {
                Map.updateMarkers();
                Map.hideHeatmap();
            } else if(Map.how === "heat-map") {
                Map.removeMarkers();
                Map.showHeatmap();
            }
            Notifications.show("Success: map updated", NotificationType.success);
        };
        var onSubmitError = function (e) {
            Notifications.show("Error: could not update map", NotificationType.error);
        };
        $.ajax("../GetAppDashboardMapData", {
            type: "POST",
            data: {
                id: re.appId,
                who: Map.who,
                when: Map.when
            },
            dataType: "json",
            error: onSubmitError,
            success: onSubmitSuccess
        });
        Notifications.show("Refreshing map...", NotificationType.info);
    };
    return Map;
})();
function setTabClickEvents() {
    $('#overviewTab').click(function (e) {
        e.preventDefault();
        $(this).tab('show');
    });
    $('#mapTab').click(function (e) {
        e.preventDefault();
        $(this).tab('show');
    });
}
function setWhenHandler(whn, txt) {
    $("#" + whn).click(function () {
        if(Map.when !== whn) {
            Map.when = whn;
            Map.refresh();
        }
        $("#current-when").text(txt);
    });
}
;
function setWhenHandlers() {
    setWhenHandler("past-year", "Past year");
    setWhenHandler("past-90-days", "Past 90 days");
    setWhenHandler("past-60-days", "Past 60 days");
    setWhenHandler("past-30-days", "Past 30 days");
    setWhenHandler("past-7-days", "Past 7 days");
    setWhenHandler("past-3-days", "Past 3 days");
    setWhenHandler("past-48-hours", "Past 48 hours");
    setWhenHandler("past-24-hours", "Past 24 hours");
}
function setHowHandler(hw, txt) {
    $("#" + hw).click(function () {
        if(Map.how !== hw) {
            Map.how = hw;
            Map.refresh();
        }
        $("#current-how").text(txt);
    });
}
;
function setHowHandlers() {
    setHowHandler("heat-map", "Heat Map");
    setHowHandler("location-map", "Location Map");
}
function setWhoHandler(wh, txt) {
    $("#" + wh).click(function () {
        if(Map.who !== wh) {
            Map.who = wh;
            Map.refresh();
        }
        $("#current-who").text(txt);
    });
}
;
function setWhoHandlers() {
    setWhoHandler("launched", "launched " + re.appName);
    setWhoHandler("intro", "saw the Refer Engine intro page");
    setWhoHandler("recommended", "recommended " + re.appName);
}
function drawChart() {
    var rowData = [
        [
            'Task', 
            'Hours per Day'
        ], 
        [
            'Work', 
            11
        ], 
        [
            'Eat', 
            2
        ], 
        [
            'Commute', 
            2
        ], 
        [
            'Watch TV', 
            2
        ], 
        [
            'Sleep', 
            7
        ]
    ];
    var data = google.visualization.arrayToDataTable(rowData);
    var options = {
    };
    var chart = new google.visualization.LineChart(document.getElementById('chart'));
    chart.draw(data, options);
}
google.load("visualization", "1", {
    packages: [
        "corechart"
    ]
});
$(document).ready(function () {
    setTabClickEvents();
    var mapInitialized = false;
    $('a[data-toggle="tab"]').on('shown', function (e) {
        if(e.target.hash === "#mapTab") {
            Map.refresh();
            if(!mapInitialized) {
                setWhenHandlers();
                setHowHandlers();
                setWhoHandlers();
            }
        } else if(e.target.hash === "#chartsTab") {
            drawChart();
        } else if(e.target.hash === "#overviewTab") {
        }
    });
});
