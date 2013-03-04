///<reference path='..\lib\jquery.d.ts' static='true' />

declare var google;
declare var re; 

var Date2 = <any>Date;

class NotificationType {
    static none = "";
    static info = "info";
    static success = "success";
    static error = "error";
}

class Notifications {
    static show = function (message: string, notificationType: string) {
        $(".top-right").notify({
            message: { text: message },
            transition: "fade",
            type: notificationType
        }).show();
    }
}

class Chart {
    static chart = null;
    static chartData = [];
    static who = "launched";
    static when = "past-30-days";
    static timespan = "day";
    static how = "pie-chart";
    static draw() {
        var rowData: any[][] = [
          ['Task', 'Hours per Day'],
          ['Work', 11],
          ['Eat', 2],
          ['Commute', 2],
          ['Watch TV', 2],
          ['Sleep', 7]
        ];
        var data = google.visualization.arrayToDataTable(rowData);

        var options = {
            //title: 'My Daily Activities'
        };

        chart.draw(data, options);
    }
    static onDataRequestSuccess(data, textStatus, jqXhr) {
        Chart.chartData = [];
        for (var i = 0; i < data.length; i++) {
            Chart.chartData.push({
                //latlong: new google.maps.LatLng(data[i].Latitude, data[i].Longitude),
                //city: data[i].City
            });
        }
        if (Map.how === "line-chart") {
            Chart.chart = new google.visualization.LineChart(document.getElementById('chart'));
        }
        //else if (Map.how === "heat-map") {
        //    Map.removeMarkers();
        //    Map.showHeatmap();
        //}
        Notifications.show("Success: map updated", NotificationType.success);
    }
    static onDataRequestError(e) {
        Notifications.show("Error: could not update chart", NotificationType.error);
    }
    static refresh() {
        $.ajax("../GetAppDashboardChartData", {
            type: "POST",
            data: {
                id: re.appId,
                who: Chart.who,
                when: Chart.when,
                timespan: Chart.timespan
            },
            dataType: "json",
            error: onDataRequestError,
            success: onDataRequestSuccess
        });

        Notifications.show("Refreshing chart...", NotificationType.info);
    }
}

class Map {
    static map = null;
    static mapData = [];
    static heatData = [];
    static markers = [];
    static how = "location-map";
    static who = "launched";
    static startDate = null;
    static endDate = null;
    static heatMap : any;
    static center = new google.maps.LatLng(0, 0);
    static options = {
        center: Map.center,
        zoom: 2,
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        streetViewControl: false, 
        mapTypeControl: false,
    }; 
    static updateHeatmapData = function () {
        Map.heatData = [];
        for (var j = 0; j < Map.mapData.length; j++) { 
            Map.heatData.push(Map.mapData[j].latlong);
        }
        Map.heatMap = new google.maps.visualization.HeatmapLayer({
            data: Map.heatData
        });
    };
    static showHeatmap = function () {
        Map.updateHeatmapData();
        Map.heatMap.setMap(Map.map);
    };
    static hideHeatmap = function () {
        if (Map.heatMap !== undefined) {
            Map.heatMap.setMap(null);
        }
    };
    static removeMarkers = function () {
        for (var j = 0; j < Map.markers.length; j++) {
            Map.markers[j].setMap(null);
        }
    };
    static updateMarkers = function () {
        Map.removeMarkers();
        for (var i = 0; i < Map.mapData.length; i++) {
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
    static refresh = function (start = Map.startDate, end = Map.endDate) {
        
        Map.startDate = start;
        Map.endDate = end;

        if (Map.map === null) {
            Map.map = new google.maps.Map(document.getElementById("map-canvas"), Map.options)
        }

        var onSubmitSuccess = function (data, textStatus, jqXhr) {
            Map.mapData = [];
            for (var i = 0; i < data.length; i++) {
                Map.mapData.push({
                    latlong: new google.maps.LatLng(data[i].Latitude, data[i].Longitude),
                    city: data[i].City
                });
            }
            if (Map.how === "location-map") {
                Map.updateMarkers();
                Map.hideHeatmap();
            }
            else if (Map.how === "heat-map") {
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
                startDate: Map.startDate,
                endDate: Map.endDate
            },
            dataType: "json",
            error: onSubmitError,
            success: onSubmitSuccess
        });

        Notifications.show("Refreshing map...", NotificationType.info);
    };
}

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

//function setMapWhenHandlers() {
//    $(".map-when").click(function () {
//        var when = $(this).attr("data-when");
//        if (Map.when !== when) {
//            Map.when = when;
//            Map.refresh();
//        }
//        $("#current-when").text($(this).text());
//    });
//}
function setMapHowHandlers() {
    $(".map-how").click(function () {
        var how = $(this).attr("data-how");
        if (Map.how !== how) {
            Map.how = how;
            Map.refresh();
        }
        $("#current-how").text($(this).text());
    });
}
function setMapWhoHandlers() {
    $(".map-who").click(function () {
        var who = $(this).attr("data-who");
        if (Map.who !== who) {
            Map.who = who;
            Map.refresh();
        }
        $("#current-who").text($(this).text());
    });
}

function initMapDateRangePicker() {
    $('#map-date-range').daterangepicker(
       {
           ranges: {
               'Today': ['today', 'today'],
               'Yesterday': ['yesterday', 'yesterday'],
               'Last 7 Days': [Date2.today().add({ days: -6 }), 'today'],
               'Last 30 Days': [Date2.today().add({ days: -29 }), 'today'],
               'This Month': [Date2.today().moveToFirstDayOfMonth(), Date2.today().moveToLastDayOfMonth()],
               'Last Month': [Date2.today().moveToFirstDayOfMonth().add({ months: -1 }), Date2.today().moveToFirstDayOfMonth().add({ days: -1 })]
           },
           opens: 'left',
           format: 'MM/dd/yyyy',
           separator: ' to ',
           startDate: Date2.today().add({ days: -29 }),
           endDate: Date2.today(),
           minDate: '01/01/2012',
           maxDate: Date2.today(),
           locale: {
               applyLabel: null,
               fromLabel: 'From',
               toLabel: 'To',
               customRangeLabel: 'Choose Range',
               daysOfWeek: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'],
               monthNames: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
               firstDay: 1
           },
           showWeekNumbers: true,
           buttonClasses: ['btn-danger']
       },
       function (start, end) {
           $('#map-date-range span').html(start.toString('MMMM d, yyyy') + ' - ' + end.toString('MMMM d, yyyy'));
           var dateFormat = "dd MMMM yyyy";
           var startDate = start.toString(dateFormat) + " 00:00:00";
           var endDate = end.toString(dateFormat) + "23:59:59";
           Map.refresh(startDate, endDate);
       }
    );

    //Set the initial state of the picker label
    $('#map-date-range span').html(Date2.today().add({ days: -29 }).toString('MMMM d, yyyy') + ' - ' + Date2.today().toString('MMMM d, yyyy'));

}

$(document).ready(function () {
    setTabClickEvents();

    var mapInitialized = false;
    $('a[data-toggle="tab"]').on('shown', function (e) {
        if (e.target.hash === "#mapTab") {

            Map.refresh();
            if (!mapInitialized) {
                initMapDateRangePicker();
                //setMapWhenHandlers();
                setMapHowHandlers();
                setMapWhoHandlers();
            }
        }
        else if (e.target.hash === "#chartsTab") {
            Chart.refresh();
        }
        else if (e.target.hash === "#overviewTab") {
        }
    })

});

