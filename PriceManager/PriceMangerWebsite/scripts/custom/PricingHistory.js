
function InitializePricingHistory() {
    $(".loadingContainer").parent().show();
    priceHistoryDatewise();
    getPricingHistory();
    getTitleHistory();
}

function setupTablesorter() {
    $('table.tablesorter').each(function (i, e) {
        var myHeaders = {}
        $(this).find('th.nosort').each(function (i, e) {
            myHeaders[$(this).index()] = { sorter: false };
        });
        $(this).tablesorter({ widgets: ['zebra'], headers: myHeaders });
        $("#tablePagination").remove();
        $(this).oneSimpleTablePagination({ topNav: false });
        $("#tablePagination").find("a").removeClass("button").addClass("button");
        $("#tablePagination_currPage").addClass("numeric inputbox").attr("style", "width:50px");
    });

}

function getPricingHistory() {
    //    var dateRangeType, fromDate, toDate;

    //    dateRangeType = $("#ddlSearchRange").val();
    //    fromDate = $("#txtFromDate").val();
    //    toDate = $("#txtToDate").val();


    //    if (dateRangeType == 8) {
    //        if (fromDate == "" || fromDate == null || toDate == "" || toDate == null) {
    //            alert("Enter proper date range!");
    //            return;
    //        }
    //    }

    var fromDate = new Date();
    var toDate = new Date();
    var itemCode = $("#hfItemCode").val();
    var service = new GeneralServices.GeneralSvc();
    service.GetPricingHistory(itemCode, fromDate, toDate, getPricingHistorySuccess, getPricingHistoryError, null);

}

function getPricingHistorySuccess(result) {
    $(".loadingContainer").parent().hide();
    if (result) {
        var items = JSON.parse(result);
        if (items.length > 0) {
            $('#tblPricingItems tbody').empty();

            $('#pricingItemTemplate').tmpl(items).appendTo('#tblPricingItems tbody');

           // setupTablesorter();

        }
        else {
            var rows = "<tr><td colspan='5' style='color: Red;font-weight: bold;text-align: center;'>No Data Found.</td></tr>";
            $('#tblPricingItems tbody').empty().append(rows);
        }
    }
    else {
        var rows = "<tr><td colspan='5' style='color: Red;font-weight: bold;text-align: center;'>No Data Found.</td></tr>";
        $('#tblPricingItems tbody').empty().append(rows);

    }
}
function getPricingHistoryError(error) {
    $(".loadingContainer").parent().hide();
    ShowInfo("Something went wrong! Please try later.", "red");
    return false;
}


function getTitleHistory() {
    //    var dateRangeType, fromDate, toDate;

    //    dateRangeType = $("#ddlSearchRange").val();
    //    fromDate = $("#txtFromDate").val();
    //    toDate = $("#txtToDate").val();


    //    if (dateRangeType == 8) {
    //        if (fromDate == "" || fromDate == null || toDate == "" || toDate == null) {
    //            alert("Enter proper date range!");
    //            return;
    //        }
    //    }

    var fromDate = new Date();
    var toDate = new Date();
    var itemCode = $("#hfItemCode").val();
    var service = new GeneralServices.GeneralSvc();
    service.GetTitleHistory(itemCode, fromDate, toDate, getTitleHistorySuccess, getTitleHistoryError, null);

}

function getTitleHistorySuccess(result) {
    $(".loadingContainer").parent().hide();
    if (result) {
        var items = JSON.parse(result);
        if (items.length > 0) {
            $('#tblTitlesItems tbody').empty();

            $('#titleItemTemplate').tmpl(items).appendTo('#tblTitlesItems tbody');

            setupTablesorter();

        }
        else {
            var rows = "<tr><td colspan='5' style='color: Red;font-weight: bold;text-align: center;'>No Data Found.</td></tr>";
            $('#tblTitlesItems tbody').empty().append(rows);
        }
    }
    else {
        var rows = "<tr><td colspan='5' style='color: Red;font-weight: bold;text-align: center;'>No Data Found.</td></tr>";
        $('#tblTitlesItems tbody').empty().append(rows);

    }
}
function getTitleHistoryError(error) {
    $(".loadingContainer").parent().hide();
    ShowInfo("Something went wrong! Please try later.", "red");
    return false;
}


function priceHistoryDatewise() {

//    var dateRangeType, fromDate, toDate;

//    dateRangeType = $("#ddlSearchRange").val();
//    fromDate = $("#txtFromDate").val();
//    toDate = $("#txtToDate").val();


//    if (dateRangeType == 8) {
//        if (fromDate == "" || fromDate == null || toDate == "" || toDate == null) {
//            alert("Enter proper date range!");
//            return;
//        }
//    }

    var fromDate = new Date();
    var toDate = new Date();
    var itemCode = $("#hfItemCode").val();
    var service = new GeneralServices.GeneralSvc();
    service.PriceHistoryDatewise(itemCode, fromDate, toDate, priceHistoryDatewiseSuccess, priceHistoryDatewiseError, null);
    
}
function priceHistoryDatewiseSuccess(data) {
    if (data) {
        var jdata = JSON.parse(data);
        if (jdata[0].length > 0) {
            var line1 = jdata[0];
            $('#chartHitRateDateWise').empty();
            var plot1 = $.jqplot('chartHitRateDateWise', [line1], {
                title: 'Last 15 Price Revised',
                gridPadding: { right: 35 },
                animate: true,
                animateReplot: true,
                axes: {
                    xaxis: {
                        renderer: $.jqplot.DateAxisRenderer,
                        tickOptions: { formatString: '%b %#d' },
                        tickInterval: '5 day',
                        label: "Revise Dates",
                       
                    }
                },
                highlighter: {
                    show: true,
                    showLabel: true,
                    tooltipAxes: 'y',
                    sizeAdjust: 7.5, tooltipLocation: 'ne'
                },

                cursor: {
                    show: true,
                    zoom: true,
                    looseZoom: true,
                    showTooltip: false
                },
                series: [{ lineWidth: 4, markerOptions: { style: 'square'}}]
            });
        }
        else {
            var h = "<div class='no-data' style='color: Red;font-weight: bold;text-align: center;'><h1>No Data Found</h1></div>";
            $('#chartHitRateDateWise').empty().html(h);

        }
    } else {
        var h = "<div class='no-data'><h1>No Data Found</h1></div>";
        $('#chartHitRateDateWise').empty().html(h);
    }
}
function priceHistoryDatewiseError(error) {
    $(".loadingContainer").parent().hide();
    ShowInfo("Something went wrong! Please try later.", "red");
    return false;
}
