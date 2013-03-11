///<reference path='..\lib\jquery.d.ts' static='true' />

declare var google;
declare var re; 

enum View {
    Table,
    Map,
    Chart
};

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

class Page {
    static currentView: View = View.Table;
    static who = "launched";
    static timespan = "day";
    static startDate = Date2.today().add({ days: -29 });
    static endDate = Date2.today();
    static initDateRangePicker() {
        var ranges = {
            'Today': ['today', 'today', 'hour'],
            'Yesterday': ['yesterday', 'yesterday', 'hour'],
            'Last 7 Days': [Date2.today().add({ days: -6 }), 'today'],
            'Last 30 Days': [Date2.today().add({ days: -29 }), 'today'],
            'This Month': [Date2.today().moveToFirstDayOfMonth(), Date2.today().moveToLastDayOfMonth()],
            'Last Month': [Date2.today().moveToFirstDayOfMonth().add({ months: -1 }), Date2.today().moveToFirstDayOfMonth().add({ days: -1 })]
        };
        $('#date-range').daterangepicker(
           {
               ranges: ranges,
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
               if (start.compareTo(startDate) !== 0 || end.compareTo(endDate) !== 0) {
                   var isCustomRange = true;
                   for (var p in ranges) {
                       var rangeStart, rangeEnd;
                       if (ranges[p][0] === "today") {
                           rangeStart = Date2.today();
                       }
                       else if (ranges[p][0] === "yesterday") {
                           rangeStart = Date2.today().add({ days: -1 });
                       }
                       else {
                           rangeStart = ranges[p][0];
                       }
                       if (ranges[p][1] === "today") {
                           rangeEnd = Date2.today();
                       }
                       else if (ranges[p][1] === "yesterday") {
                           rangeEnd = Date2.today().add({ days: -1 });
                       }
                       else {
                           rangeEnd = ranges[p][1];
                       }
                       if (Date2.compare(start, rangeStart) === 0
                           && Date2.compare(end, rangeEnd) === 0) {
                           $('#date-range span').html(p);
                           isCustomRange = false;
                           break;
                       }
                   }
                   if (isCustomRange) {
                       $('#date-range span').html(start.toString('MMMM d, yyyy') + ' - ' + end.toString('MMMM d, yyyy'));
                   }
                   if (start.clone().add(1).days().compareTo(end) == -1) {
                       Page.timespan = 'day';
                   }
                   else {
                       Page.timespan = 'hour';
                   }
                   startDate = start;
                   endDate = end;
                   refreshView();
               }
           }
        );

        //Set the initial state of the picker label
        $('#date-range span').html("Last 30 days");
        //$('#date-range span').html(Date2.today().add({ days: -29 }).toString('MMMM d, yyyy') + ' - ' + Date2.today().toString('MMMM d, yyyy'));

    }
    static initWhoSelector() {
        $(".who").click(function () {
            var newWho = $(this).attr("data-who");
            if (Page.who !== newWho) {
                Page.who = newWho;
                refreshView();
            }
            $("#current-who").text($(this).text());
        });
    }
    static initTabs() {
        $('#tableTab').click(function (e) {
            e.preventDefault();
            $(this).tab('show');
        });

        $('#mapTab').click(function (e) {
            e.preventDefault();
            $(this).tab('show');
        });

        $('a[data-toggle="tab"]').on('shown', function (e) {
            if (e.target.hash === "#mapTab") {
                Map.init();
                Page.currentView = View.Map;
            }
            else if (e.target.hash === "#chartTab") {
                Chart.init();
                Page.currentView = View.Chart;
            }
            else if (e.target.hash === "#tableTab") {
                Page.currentView = View.Table;
            }
            refreshView();
        })
    }
    static refreshView() {
        if (currentView === View.Table) {
            Table.refresh();
        }
        else if (currentView === View.Chart) {
            Chart.refresh();
        }
        else if (currentView === View.Map) {
            Map.refresh();
        }
    }
    static initPage() {
        initTabs();
        initDateRangePicker();
        initWhoSelector();
        refreshView();
    }
}

class Chart {
    static chart = null;
    static chartData = [];
    static timespan = "day";
    static how = "line-chart";
    static isInitialized = false;
    static init() {
        if (!isInitialized) {
            initHowSelector();
            isInitialized = true;
        }
    }
    static initHowSelector() {
        $(".chart-how").click(function () {
            var how = $(this).attr("data-how");
            if (Chart.how !== how) {
                Chart.how = how;
                Chart.refresh();
            }
            $("#current-how-chart").text($(this).text());
        });
    }
    static onDataRequestSuccess(data, textStatus, jqXhr) {
        Chart.chartData = [];
        Chart.chartData.push(['Date',
                              'Count']);
        for (var i = 0; i < data.length; i++) {
            Chart.chartData.push([data[i].Desc,
                                  data[i].Result]);
        }
        var elem = document.getElementById('chart');
        if (Chart.how === "line-chart") {
            Chart.chart = new google.visualization.LineChart(elem);
        }
        else if (Chart.how === "pie-chart") {
            Chart.chart = new google.visualization.PieChart(elem);
        }
        else if (Chart.how === "area-chart") {
            Chart.chart = new google.visualization.AreaChart(elem);
        }
        else if (Chart.how === "column-chart") {
            Chart.chart = new google.visualization.ColumnChart(elem);
        }
        else if (Chart.how === "bar-chart") {
            Chart.chart = new google.visualization.BarChart(elem);
        }

        var dataTable = google.visualization.arrayToDataTable(Chart.chartData);

        var options = {
            //title: 'My Daily Activities'
        };

        chart.draw(dataTable, options);
        Notifications.show("Success: map updated", NotificationType.success);
    }
    static onDataRequestError(e) {
        Notifications.show("Error: could not update chart", NotificationType.error);
    }
    static refresh() {
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
            error: onDataRequestError,
            success: onDataRequestSuccess
        });

        Notifications.show("Refreshing chart...", NotificationType.info);
    }
}

class Table {
    static table = null;
    static tableData = [];
    static timespan = "day";
    static how = "table";
    static isInitialized = false;
    static onDataRequestSuccess(data, textStatus, jqXhr) {
        Table.tableData = [];
        Table.tableData.push(['Date',
                              'Launch Count']);
        for (var i = 0; i < data.length; i++) {
            Table.tableData.push([data[i].Desc,
                                  data[i].Result]);
        }
        var elem = document.getElementById('table');
        if (Table.how === "table") {
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

        table.draw(dataTable, options);
        Notifications.show("Success: table updated", NotificationType.success);
    }
    static onDataRequestError(e) {
        Notifications.show("Error: could not update table", NotificationType.error);
    }
    static refresh() {
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
            error: onDataRequestError,
            success: onDataRequestSuccess
        });

        Notifications.show("Refreshing table...", NotificationType.info);
    }
}

class Map {
    static map = null;
    static mapData = [];
    static dataTable = [];
    static how = "location-map";
    static options = {
        region: null,
        displayMode: 'markers',
        colorAxis: { colors: ['0099e5', 'rgb(140, 34, 222)'] },
        enableRegionInteractivity: true,
        datalessRegionColor: 'CCCCCC'
    };
    static draw = function () {
        Map.map.draw(Map.dataTable, Map.options);
    };
    static refresh = function () {
        var dateFormat = "dd MMMM yyyy";
        var startDate = Page.startDate.toString(dateFormat) + " 00:00:00";
        var endDate = Page.endDate.toString(dateFormat) + " 23:59:59";

        if (Map.map === null) {
            Map.map = new google.visualization.GeoChart(document.getElementById('map-canvas'));
            google.visualization.events.addListener(Map.map, 'regionClick', function (eventData) {
                if (Map.options.region === eventData.region) {
                    Map.options.region = null;
                }
                else {
                    Map.options.region = eventData.region;
                }
                Map.draw();
            });
        }

        var onSubmitSuccess = function (data, textStatus, jqXhr) {
            Map.mapData = [];
            Map.mapData.push(['City', 'Count']);
            for (var i = 0; i < data.length; i++) {
                var l = data[i].City;
                if (data[i].Region) {
                    l += ", " + data[i].Region;
                } 
                l += ", " + data[i].Country;
                Map.mapData.push([l, data[i].Result]);
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
                timeZoneOffset: Date2.today().getUTCOffset(),
            },
            dataType: "json",
            error: onSubmitError,
            success: onSubmitSuccess
        });

        Notifications.show("Refreshing map...", NotificationType.info);
    };
    static initHowSelector() {
        $(".map-how").click(function () {
            var how = $(this).attr("data-how");
            if (Map.how !== how) {
                Map.how = how;
                Map.refresh();
            }
            $("#current-how").text($(this).text());
        });
    }
    static isInitialized = false;
    static init() {
        if (!isInitialized) {
            initHowSelector();
            isInitialized = true;
        }
    }
}

$(document).ready(Page.initPage);

