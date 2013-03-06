//$(document).ready(function () {

//    $('#overviewTab').click(function (e) {
//        e.preventDefault();
//        $(this).tab('show');
//    });
//    $('#mapTab').click(function (e) {
//        e.preventDefault();
//        $(this).tab('show');
//    });
    
//    var center = new google.maps.LatLng(0, 0);
//    var mapOptions = {
//        center: center,
//        zoom: 2,
//        mapTypeId: google.maps.MapTypeId.ROADMAP,
//        streetViewControl: false,
//        mapTypeControl: false,
//    };
//    var map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);

//    var heatmapData = [];
//    var heatmap;
//    var updateHeatmapData = function () {
//        for (var j = 0; j < window.mapData.length; j++) {
//            heatmapData.push(window.mapData[j].latlong);
//        }
//        heatmap = new google.maps.visualization.HeatmapLayer({
//            data: heatmapData
//        });
//    };
//    var showHeatmap = function () {
//        updateHeatmapData();
//        heatmap.setMap(map);
//    };
//    var hideHeatmap = function () {
//        heatmap.setMap(null);
//    };

//    var markers = [];
//    var removeMarkers = function() {
//        for (var j = 0; j < markers.length; j++) {
//            markers[j].setMap(null);
//        }
//    };
//    var updateMarkers = function () {
//        removeMarkers();
//        for (var i = 0; i < window.mapData.length; i++) {
//            markers.push(new google.maps.Marker({
//                position: window.mapData[i].latlong,
//                map: map,
//                title: window.mapData[i].city,
//                icon: "https://referenginestorage.blob.core.windows.net/referengine-design/logo-mark-32.png"
//            }));
//        }
//    };

//    var how = "location-map";
//    var who = "launched";
//    var when = "past-30-days";

//    var updateMap = function () {
//        var onSubmitSuccess = function (data, textStatus, jqXhr) {
//            window.mapData = [];
//            for (var i = 0; i < data.length; i++) {
//                window.mapData.push({
//                    latlong: new google.maps.LatLng(data[i].Latitude, data[i].Longitude),
//                    city: data[i].City
//                });
//            }
//            updateMarkers();
//        };
        
//        var onSubmitError = function (e) {

//        };

//        $.ajax({
//            type: "POST",
//            url: "../GetAppDashboardMapData",
//            data: {
//                id: window.appId,
//                who: who,
//                when: when
//            },
//            dataType: "json",
//            error: onSubmitError,
//            success: onSubmitSuccess
//        });
//    };

//    var setWhenHandler = function(whn, txt) {
//        $("#" + whn).click(function() {
//            if (when !== whn) {
//                when = whn;
//                updateMap();
//            }
//        });
//        $("#current-when").text(txt);
//    };

//    setWhenHandler("past-year", "Past year");
//    setWhenHandler("past-90-days", "Past 90 days");
//    setWhenHandler("past-60-days", "Past 60 days");
//    setWhenHandler("past-30-days", "Past 30 days");
//    setWhenHandler("past-7-days", "Past 7 days");
//    setWhenHandler("past-3-days", "Past 3 days");
//    setWhenHandler("past-48-hours", "Past 48 hours");
//    setWhenHandler("past-24-hours", "Past 24 hours");

//    $("#heat-map").click(function () {
//        removeMarkers();
//        showHeatmap();
//    });

//    updateMap();
//});