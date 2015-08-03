
$(function () {

    $("#txtFromDate").datepicker({
        defaultDate: "-1w",
        dateFormat: "dd/m/yy",
        changeMonth: true,
        numberOfMonths: 1,
        onClose: function (selectedDate) {
            $("#txtToDate").datepicker("option", "minDate", selectedDate);
        }
    });

    $("#txtToDate").datepicker({
        maxDate: new Date(),
        dateFormat: "dd/m/yy",
        changeMonth: true,
        numberOfMonths: 1,
        onClose: function (selectedDate) {
            $("#txtFromDate").datepicker("option", "maxDate", selectedDate);
        }
    });

    if ($("#ddlDates").val() == 'Custom') {
        $("#txtFromDate").show();
        $("#txtToDate").show();
    }
    else { $("#txtFromDate").hide(); $("#txtToDate").hide(); }

    
});


function DefaultDateFunctions() {

    $(".datepicker").blur(function () {
        if ($(this).val().trim() == "") {
            var d = new Date();
            $(this).val(new Date().format("dd/MM/yyyy"));
        }
    });
}

function setCustomDefaultDate() {
    var d = new Date();
    $("#txtToDate").val(new Date().format("dd/MM/yyyy"));
    d.setDate(d.getDate() - 7);
    $("#txtFromDate").val(d.format("dd/MM/yyyy"));
}


$("#ddlDates").bind("change", function () {
    if ($(this).val() == 'Custom') {
        $('#txtFromDate').fadeIn('100');
        $('#txtToDate').fadeIn('500');
        setCustomDefaultDate();
        //SearchHistory();
    }
    else if ($(this).val() != 'Custom') {
        $('#txtFromDate').fadeOut('100');
        $('#txtToDate').fadeOut('100');
        //$(".datepicker").hide();
        $("#txtFromDate").val('');
        $("#txtToDate").val('');



        //SearchHistory();
    }
});
