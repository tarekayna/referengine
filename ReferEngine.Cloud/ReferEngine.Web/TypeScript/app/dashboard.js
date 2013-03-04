var Date = (function () {
    function Date() { }
    Date.today = function today() {
    };
    return Date;
})();
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
var Chart = (function () {
    function Chart() { }
    Chart.chart = null;
    Chart.chartData = [];
    Chart.who = "launched";
    Chart.when = "past-30-days";
    Chart.timespan = "day";
    Chart.how = "pie-chart";
    Chart.draw = function draw() {
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
        Chart.chart.draw(data, options);
    };
    Chart.onDataRequestSuccess = function onDataRequestSuccess(data, textStatus, jqXhr) {
        Chart.chartData = [];
        for(var i = 0; i < data.length; i++) {
            Chart.chartData.push({
            });
        }
        if(Map.how === "line-chart") {
            Chart.chart = new google.visualization.LineChart(document.getElementById('chart'));
        }
        Notifications.show("Success: map updated", NotificationType.success);
    };
    Chart.onDataRequestError = function onDataRequestError(e) {
        Notifications.show("Error: could not update chart", NotificationType.error);
    };
    Chart.refresh = function refresh() {
        $.ajax("../GetAppDashboardChartData", {
            type: "POST",
            data: {
                id: re.appId,
                who: Chart.who,
                when: Chart.when,
                timespan: Chart.timespan
            },
            dataType: "json",
            error: Chart.onDataRequestError,
            success: Chart.onDataRequestSuccess
        });
        Notifications.show("Refreshing chart...", NotificationType.info);
    };
    return Chart;
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
function setMapWhenHandlers() {
    $(".map-when").click(function () {
        var when = $(this).attr("data-when");
        if(Map.when !== when) {
            Map.when = when;
            Map.refresh();
        }
        $("#current-when").text($(this).text());
    });
}
function setMapHowHandlers() {
    $(".map-how").click(function () {
        var how = $(this).attr("data-how");
        if(Map.how !== how) {
            Map.how = how;
            Map.refresh();
        }
        $("#current-how").text($(this).text());
    });
}
function setMapWhoHandlers() {
    $(".map-who").click(function () {
        var who = $(this).attr("data-who");
        if(Map.who !== who) {
            Map.who = who;
            Map.refresh();
        }
        $("#current-who").text($(this).text());
    });
}
function initMapDateRangePicker() {
    $('#map-date-range').daterangepicker({
        ranges: {
            'Today': [
                'today', 
                'today'
            ],
            'Yesterday': [
                'yesterday', 
                'yesterday'
            ],
            'Last 7 Days': [
                Date.today().add({
                    days: -6
                }), 
                'today'
            ],
            'Last 30 Days': [
                Date.today().add({
                    days: -29
                }), 
                'today'
            ],
            'This Month': [
                Date.today().moveToFirstDayOfMonth(), 
                Date.today().moveToLastDayOfMonth()
            ],
            'Last Month': [
                Date.today().moveToFirstDayOfMonth().add({
                    months: -1
                }), 
                Date.today().moveToFirstDayOfMonth().add({
                    days: -1
                })
            ]
        },
        opens: 'left',
        format: 'MM/dd/yyyy',
        separator: ' to ',
        startDate: Date.today().add({
            days: -29
        }),
        endDate: Date.today(),
        minDate: '01/01/2012',
        maxDate: '12/31/2013',
        locale: {
            applyLabel: 'Submit',
            fromLabel: 'From',
            toLabel: 'To',
            customRangeLabel: 'Custom Range',
            daysOfWeek: [
                'Su', 
                'Mo', 
                'Tu', 
                'We', 
                'Th', 
                'Fr', 
                'Sa'
            ],
            monthNames: [
                'January', 
                'February', 
                'March', 
                'April', 
                'May', 
                'June', 
                'July', 
                'August', 
                'September', 
                'October', 
                'November', 
                'December'
            ],
            firstDay: 1
        },
        showWeekNumbers: true,
        buttonClasses: [
            'btn-danger'
        ]
    }, function (start, end) {
        $('#map-date-range span').html(start.toString('MMMM d, yyyy') + ' - ' + end.toString('MMMM d, yyyy'));
    });
    $('#map-date-range span').html(Date.today().add({
        days: -29
    }).toString('MMMM d, yyyy') + ' - ' + Date.today().toString('MMMM d, yyyy'));
}
$(document).ready(function () {
    setTabClickEvents();
    var mapInitialized = false;
    $('a[data-toggle="tab"]').on('shown', function (e) {
        if(e.target.hash === "#mapTab") {
            Map.refresh();
            if(!mapInitialized) {
                setMapWhenHandlers();
                setMapHowHandlers();
                setMapWhoHandlers();
            }
        } else if(e.target.hash === "#chartsTab") {
            Chart.refresh();
        } else if(e.target.hash === "#overviewTab") {
        }
    });
});
