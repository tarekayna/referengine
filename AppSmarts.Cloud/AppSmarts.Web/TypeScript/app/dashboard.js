define(["require", "exports", "../common/notifications"], function(require, exports, __Notification__) {
    var Notification = __Notification__;

    var ko;
    var Date2;
    var View;
    (function (View) {
        View._map = [];
        View._map[0] = "Table";
        View.Table = 0;
        View._map[1] = "Map";
        View.Map = 1;
        View._map[2] = "Chart";
        View.Chart = 2;
        View._map[3] = "People";
        View.People = 3;
    })(View || (View = {}));
    ;
    var Page = (function () {
        function Page() { }
        Page.currentView = View.Chart;
        Page.who = "recommended";
        Page.timespan = "day";
        Page.getUnitName = function getUnitName() {
            if(Page.who === "launched") {
                return "App Launches";
            } else if(Page.who === "intro") {
                return "Intro Views";
            } else if(Page.who === "recommended") {
                return "Recommendations";
            }
        };
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
                startDate: Page.startDate,
                endDate: Page.endDate,
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
            $('#date-range span').html("Last 7 Days");
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
            $('#peopleTab').click(function (e) {
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
                } else if(e.target.hash === "#peopleTab") {
                    People.init();
                    Page.currentView = View.People;
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
            } else if(Page.currentView === View.People) {
                People.refresh();
            }
        };
        Page.initPage = function initPage() {
            Page.startDate = Date2.today().add({
                days: -6
            });
            Page.endDate = Date2.today();
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
        Chart.how = "column-chart";
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
                '# of ' + Page.getUnitName()
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
                legend: {
                    position: 'bottom'
                }
            };
            Chart.chart.draw(dataTable, options);
            Notification.show("Success: chart updated", Notification.NotificationType.success);
        };
        Chart.onDataRequestError = function onDataRequestError(e) {
            Notification.show("Error: could not update chart", Notification.NotificationType.error);
        };
        Chart.refresh = function refresh() {
            var dateFormat = "dd MMMM yyyy";
            var startDate = Page.startDate.toString(dateFormat) + " 00:00:00";
            var endDate = Page.endDate.clone().add({
                days: 1
            }).toString(dateFormat) + " 00:00:01";
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
            Notification.show("Refreshing chart...", Notification.NotificationType.info);
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
                '# of ' + Page.getUnitName()
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
            Notification.show("Success: table updated", Notification.NotificationType.success);
        };
        Table.onDataRequestError = function onDataRequestError(e) {
            Notification.show("Error: could not update table", Notification.NotificationType.error);
        };
        Table.refresh = function refresh() {
            var dateFormat = "dd MMMM yyyy";
            var startDate = Page.startDate.toString(dateFormat) + " 00:00:00";
            var endDate = Page.endDate.clone().add({
                days: 1
            }).toString(dateFormat) + " 00:00:01";
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
            Notification.show("Refreshing table...", Notification.NotificationType.info);
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
            var endDate = Page.endDate.clone().add({
                days: 1
            }).toString(dateFormat) + " 00:00:01";
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
                    '# of ' + Page.getUnitName()
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
                Notification.show("Success: map updated", Notification.NotificationType.success);
            };
            var onSubmitError = function (e) {
                Notification.show("Error: could not update map", Notification.NotificationType.error);
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
            Notification.show("Refreshing map...", Notification.NotificationType.info);
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
    var PeopleViewModel = (function () {
        function PeopleViewModel() {
            this.PeopleData = ko.observableArray();
        }
        return PeopleViewModel;
    })();    
    var People = (function () {
        function People() { }
        People.isInitialized = false;
        People.init = function init() {
            if(!People.isInitialized) {
                People.viewModel = new PeopleViewModel();
                ko.applyBindings(People.viewModel);
                People.isInitialized = true;
            }
        };
        People.onDataRequestSuccess = function onDataRequestSuccess(data, textStatus, jqXhr) {
            if(!People.isInitialized) {
                People.init();
            }
            People.viewModel.PeopleData.removeAll();
            for(var i = 0; i < data.length; i++) {
                People.viewModel.PeopleData.push(data[i]);
            }
            Notification.show("Success: people view updated", Notification.NotificationType.success);
        };
        People.onDataRequestError = function onDataRequestError(e) {
            Notification.show("Error: could not update people view", Notification.NotificationType.error);
        };
        People.refresh = function refresh() {
            var dateFormat = "dd MMMM yyyy";
            var startDate = Page.startDate.toString(dateFormat) + " 00:00:00";
            var endDate = Page.endDate.clone().add({
                days: 1
            }).toString(dateFormat) + " 00:00:01";
            $.ajax("../GetAppDashboardPeopleData", {
                type: "POST",
                data: {
                    id: re.appId,
                    who: "recommended",
                    startDate: startDate,
                    endDate: endDate,
                    timeZoneOffset: Date2.today().getUTCOffset()
                },
                dataType: "json",
                error: People.onDataRequestError,
                success: People.onDataRequestSuccess
            });
            Notification.show("Updating people view...", Notification.NotificationType.info);
        };
        return People;
    })();    
    require([
        "../lib/knockout", 
        "../lib/date", 
        "../lib/daterangepicker"
    ], function (_ko) {
        ko = _ko;
        Date2 = Date;
        $(document).ready(Page.initPage);
    });
})
