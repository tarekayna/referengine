var View;
(function (View) {
    View._map = [];
    View._map[0] = "Table";
    View.Table = 0;
    View._map[1] = "Map";
    View.Map = 1;
    View._map[2] = "Chart";
    View.Chart = 2;
})(View || (View = {}));
;
var Date2 = Date;
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
var Page = (function () {
    function Page() { }
    Page.currentView = View.Table;
    Page.who = "launched";
    Page.timespan = "day";
    Page.startDate = Date2.today().add({
        days: -29
    });
    Page.endDate = Date2.today();
    Page.initDateRangePicker = function initDateRangePicker() {
        var ranges = {
            'Today': [
                'today', 
                'today', 
                'hour'
            ],
            'Yesterday': [
                'yesterday', 
                'yesterday', 
                'hour'
            ],
            'Last 7 Days': [
                Date2.today().add({
                    days: -6
                }), 
                'today'
            ],
            'Last 30 Days': [
                Date2.today().add({
                    days: -29
                }), 
                'today'
            ],
            'This Month': [
                Date2.today().moveToFirstDayOfMonth(), 
                Date2.today().moveToLastDayOfMonth()
            ],
            'Last Month': [
                Date2.today().moveToFirstDayOfMonth().add({
                    months: -1
                }), 
                Date2.today().moveToFirstDayOfMonth().add({
                    days: -1
                })
            ]
        };
        $('#date-range').daterangepicker({
            ranges: ranges,
            opens: 'left',
            format: 'MM/dd/yyyy',
            separator: ' to ',
            startDate: Date2.today().add({
                days: -29
            }),
            endDate: Date2.today(),
            minDate: '01/01/2012',
            maxDate: Date2.today(),
            locale: {
                applyLabel: null,
                fromLabel: 'From',
                toLabel: 'To',
                customRangeLabel: 'Choose Range',
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
            if(start.compareTo(Page.startDate) !== 0 || end.compareTo(Page.endDate) !== 0) {
                var isCustomRange = true;
                for(var p in ranges) {
                    var rangeStart, rangeEnd;
                    if(ranges[p][0] === "today") {
                        rangeStart = Date2.today();
                    } else if(ranges[p][0] === "yesterday") {
                        rangeStart = Date2.today().add({
                            days: -1
                        });
                    } else {
                        rangeStart = ranges[p][0];
                    }
                    if(ranges[p][1] === "today") {
                        rangeEnd = Date2.today();
                    } else if(ranges[p][1] === "yesterday") {
                        rangeEnd = Date2.today().add({
                            days: -1
                        });
                    } else {
                        rangeEnd = ranges[p][1];
                    }
                    if(Date2.compare(start, rangeStart) === 0 && Date2.compare(end, rangeEnd) === 0) {
                        $('#date-range span').html(p);
                        isCustomRange = false;
                        break;
                    }
                }
                if(isCustomRange) {
                    $('#date-range span').html(start.toString('MMMM d, yyyy') + ' - ' + end.toString('MMMM d, yyyy'));
                }
                if(start.clone().add(1).days().compareTo(end) == -1) {
                    Page.timespan = 'day';
                } else {
                    Page.timespan = 'hour';
                }
                Page.startDate = start;
                Page.endDate = end;
                Page.refreshView();
            }
        });
        $('#date-range span').html("Last 30 days");
    };
    Page.initWhoSelector = function initWhoSelector() {
        $(".who").click(function () {
            var newWho = $(this).attr("data-who");
            if(Page.who !== newWho) {
                Page.who = newWho;
                Page.refreshView();
            }
            $("#current-who").text($(this).text());
        });
    };
    Page.initTabs = function initTabs() {
        $('#tableTab').click(function (e) {
            e.preventDefault();
            $(this).tab('show');
        });
        $('#mapTab').click(function (e) {
            e.preventDefault();
            $(this).tab('show');
        });
        $('a[data-toggle="tab"]').on('shown', function (e) {
            if(e.target.hash === "#mapTab") {
                Map.init();
                Page.currentView = View.Map;
            } else if(e.target.hash === "#chartTab") {
                Chart.init();
                Page.currentView = View.Chart;
            } else if(e.target.hash === "#tableTab") {
                Page.currentView = View.Table;
            }
            Page.refreshView();
        });
    };
    Page.refreshView = function refreshView() {
        if(Page.currentView === View.Table) {
            Table.refresh();
        } else if(Page.currentView === View.Chart) {
            Chart.refresh();
        } else if(Page.currentView === View.Map) {
            Map.refresh();
        }
    };
    Page.initPage = function initPage() {
        Page.initTabs();
        Page.initDateRangePicker();
        Page.initWhoSelector();
        Page.refreshView();
    };
    return Page;
})();
var Chart = (function () {
    function Chart() { }
    Chart.chart = null;
    Chart.chartData = [];
    Chart.timespan = "day";
    Chart.how = "line-chart";
    Chart.isInitialized = false;
    Chart.init = function init() {
        if(!Chart.isInitialized) {
            Chart.initHowSelector();
            Chart.isInitialized = true;
        }
    };
    Chart.initHowSelector = function initHowSelector() {
        $(".chart-how").click(function () {
            var how = $(this).attr("data-how");
            if(Chart.how !== how) {
                Chart.how = how;
                Chart.refresh();
            }
            $("#current-how-chart").text($(this).text());
        });
    };
    Chart.onDataRequestSuccess = function onDataRequestSuccess(data, textStatus, jqXhr) {
        Chart.chartData = [];
        Chart.chartData.push([
            'Date', 
            'Count'
        ]);
        for(var i = 0; i < data.length; i++) {
            Chart.chartData.push([
                data[i].Desc, 
                data[i].Result
            ]);
        }
        var elem = document.getElementById('chart');
        if(Chart.how === "line-chart") {
            Chart.chart = new google.visualization.LineChart(elem);
        } else if(Chart.how === "pie-chart") {
            Chart.chart = new google.visualization.PieChart(elem);
        } else if(Chart.how === "area-chart") {
            Chart.chart = new google.visualization.AreaChart(elem);
        } else if(Chart.how === "column-chart") {
            Chart.chart = new google.visualization.ColumnChart(elem);
        } else if(Chart.how === "bar-chart") {
            Chart.chart = new google.visualization.BarChart(elem);
        }
        var dataTable = google.visualization.arrayToDataTable(Chart.chartData);
        var options = {
        };
        Chart.chart.draw(dataTable, options);
        Notifications.show("Success: map updated", NotificationType.success);
    };
    Chart.onDataRequestError = function onDataRequestError(e) {
        Notifications.show("Error: could not update chart", NotificationType.error);
    };
    Chart.refresh = function refresh() {
        var dateFormat = "dd MMMM yyyy";
        var startDate = Page.startDate.toString(dateFormat) + " 00:00:00";
        var endDate = Page.endDate.toString(dateFormat) + " 23:59:59";
        $.ajax("../GetAppDashboardChartData", {
            type: "POST",
            data: {
                id: re.appId,
                who: Page.who,
                startDate: startDate,
                endDate: endDate,
                timeZoneOffset: Date2.today().getUTCOffset(),
                timespan: Page.timespan
            },
            dataType: "json",
            error: Chart.onDataRequestError,
            success: Chart.onDataRequestSuccess
        });
        Notifications.show("Refreshing chart...", NotificationType.info);
    };
    return Chart;
})();
var Table = (function () {
    function Table() { }
    Table.table = null;
    Table.tableData = [];
    Table.timespan = "day";
    Table.how = "table";
    Table.isInitialized = false;
    Table.onDataRequestSuccess = function onDataRequestSuccess(data, textStatus, jqXhr) {
        Table.tableData = [];
        Table.tableData.push([
            'Date', 
            'Launch Count'
        ]);
        for(var i = 0; i < data.length; i++) {
            Table.tableData.push([
                data[i].Desc, 
                data[i].Result
            ]);
        }
        var elem = document.getElementById('table');
        if(Table.how === "table") {
            Table.table = new google.visualization.Table(elem);
        }
        var dataTable = google.visualization.arrayToDataTable(Table.tableData);
        var options = {
            cssClassNames: {
                tableCell: 'txt-middle'
            },
            page: "enable",
            pageSize: 15
        };
        Table.table.draw(dataTable, options);
        Notifications.show("Success: table updated", NotificationType.success);
    };
    Table.onDataRequestError = function onDataRequestError(e) {
        Notifications.show("Error: could not update table", NotificationType.error);
    };
    Table.refresh = function refresh() {
        var dateFormat = "dd MMMM yyyy";
        var startDate = Page.startDate.toString(dateFormat) + " 00:00:00";
        var endDate = Page.endDate.toString(dateFormat) + " 23:59:59";
        $.ajax("../GetAppDashboardChartData", {
            type: "POST",
            data: {
                id: re.appId,
                who: Page.who,
                startDate: startDate,
                endDate: endDate,
                timeZoneOffset: Date2.today().getUTCOffset(),
                timespan: Page.timespan
            },
            dataType: "json",
            error: Table.onDataRequestError,
            success: Table.onDataRequestSuccess
        });
        Notifications.show("Refreshing table...", NotificationType.info);
    };
    return Table;
})();
var Map = (function () {
    function Map() { }
    Map.map = null;
    Map.mapData = [];
    Map.dataTable = [];
    Map.how = "location-map";
    Map.options = {
        region: null,
        displayMode: 'markers',
        colorAxis: {
            colors: [
                '0099e5', 
                'rgb(140, 34, 222)'
            ]
        },
        enableRegionInteractivity: true,
        datalessRegionColor: 'CCCCCC'
    };
    Map.draw = function () {
        Map.map.draw(Map.dataTable, Map.options);
    };
    Map.refresh = function () {
        var dateFormat = "dd MMMM yyyy";
        var startDate = Page.startDate.toString(dateFormat) + " 00:00:00";
        var endDate = Page.endDate.toString(dateFormat) + " 23:59:59";
        if(Map.map === null) {
            Map.map = new google.visualization.GeoChart(document.getElementById('map-canvas'));
            google.visualization.events.addListener(Map.map, 'regionClick', function (eventData) {
                if(Map.options.region === eventData.region) {
                    Map.options.region = null;
                } else {
                    Map.options.region = eventData.region;
                }
                Map.draw();
            });
        }
        var onSubmitSuccess = function (data, textStatus, jqXhr) {
            Map.mapData = [];
            Map.mapData.push([
                'City', 
                'Count'
            ]);
            for(var i = 0; i < data.length; i++) {
                var l = data[i].City;
                if(data[i].Region) {
                    l += ", " + data[i].Region;
                }
                l += ", " + data[i].Country;
                Map.mapData.push([
                    l, 
                    data[i].Result
                ]);
            }
            Map.dataTable = google.visualization.arrayToDataTable(Map.mapData);
            Map.draw();
            Notifications.show("Success: map updated", NotificationType.success);
        };
        var onSubmitError = function (e) {
            Notifications.show("Error: could not update map", NotificationType.error);
        };
        $.ajax("../GetAppDashboardMapData", {
            type: "POST",
            data: {
                id: re.appId,
                who: Page.who,
                startDate: startDate,
                endDate: endDate,
                timeZoneOffset: Date2.today().getUTCOffset()
            },
            dataType: "json",
            error: onSubmitError,
            success: onSubmitSuccess
        });
        Notifications.show("Refreshing map...", NotificationType.info);
    };
    Map.initHowSelector = function initHowSelector() {
        $(".map-how").click(function () {
            var how = $(this).attr("data-how");
            if(Map.how !== how) {
                Map.how = how;
                Map.refresh();
            }
            $("#current-how").text($(this).text());
        });
    };
    Map.isInitialized = false;
    Map.init = function init() {
        if(!Map.isInitialized) {
            Map.initHowSelector();
            Map.isInitialized = true;
        }
    };
    return Map;
})();
$(document).ready(Page.initPage);
