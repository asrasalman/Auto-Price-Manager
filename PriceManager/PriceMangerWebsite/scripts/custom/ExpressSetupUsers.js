var statusimages = [
                    '/images/loader.gif',
                    '/images/round_checkmark.png',
                    '/images/round_delete.png'
                   ];
function InitializeExpressSetupUsers() {
    GetExpressSetupUsers();
    $("#btnSearch").live("click", function () { GetExpressSetupUsers() });
    $(".chkAll").live("click", function () { UpdateCheckBoxes(this) });
    $("#btnComplete").live("click", function () { ChangeStatus(2); return false; });
    $("#btnCancel").live("click", function () { ChangeStatus(3); return false; });
}
function GetStatusImage(statusCode) {
    if (statusCode == null)
        statusCode = 1;
    return statusimages[statusCode - 1];
}
function UpdateCheckBoxes(obj) {
    $("#tblUsers tbody tr:visible").each(function () {
        $(this).find("input.chkOne").prop("checked", $(obj).is(":checked"));
    });
}
function ChangeStatus(statusCode) {
    var users = [];
    $("input.chkOne:checked").each(function (index) {
        users[index] = $(this).closest("tr").find(".hfExpressSetupUserCode").val();
    });
    if (users.length > 0) {
        $(".loadingContainer").parent().show();
        XHR_UpdateStatusExpressSetupUsers(users, statusCode, function (result) {
            $(".loadingContainer").parent().hide();
            if (result) {
                for (var i = 0; i < users.length; i++) {
                    var userCode = users[i];
                    $(".hfExpressSetupUserCode[value='" + userCode + "']").closest("tr").find("img.status").attr("src", GetStatusImage(statusCode));
                }
            }
            else {
                OnErrorGeneral();
            }
        }, null);
    }
    else {
        ShowInfo("Please select atleast one express setup request to change status");
    }
}
function GetExpressSetupUsers() {
    $(".loadingContainer").parent().show();
    XHR_GetExpressSetupUsers($("#txtUserNameSearch").val(), $("#txtUserBusinessName").val(), $("#ddlStatus").val(), function (result) {
        $(".loadingContainer").parent().hide();
        if (result) {
            var users = JSON.parse(result);

            $('#tblUsers tbody').empty();

            $('#tblUsersTemplate').tmpl(users).appendTo('#tblUsers tbody');

            setupTablesorter();
        }
        else {
            OnErrorGeneral();
        }

    }, null)
}
function XHR_GetExpressSetupUsers(Name, BusinessName, StatusCode, OnSuccess, OnError) {
    var service = new GeneralServices.GeneralSvc();
    service.GetExpressSetupUsers(Name, BusinessName, StatusCode, OnSuccess, OnErrorGeneral, null);
}
function XHR_UpdateStatusExpressSetupUsers(userCodes, statusCode, OnSuccess, OnError) {
    var service = new GeneralServices.GeneralSvc();
    service.UpdateStatusExpressSetupUsers(userCodes, statusCode, OnSuccess, OnErrorGeneral, null);
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
        $("#tablePagination").find("a").removeClass("button").addClass("button1");
        $("#tablePagination_currPage").addClass("numeric");
    });

}
function OnErrorGeneral(alert) {
    $(".loadingContainer").parent().hide();
    ShowInfo("Something went wrong! Please try later.", "red")
}
function ShowInfo(information, color) {
    $('.infoContainer').text(information);

    if (color == 'green')
        $('.infoContainer').addClass('infoContainerGreen');
    else if (color == 'red')
        $('.infoContainer').addClass('infoContainerRed');

    $('.infoContainer').show('slide', { direction: 'right' }, 500);
    setTimeout(function () {
        $('.infoContainer').hide('slide', { direction: 'right' }, 700, function () {
            $('.infoContainer').removeClass('infoContainerGreen').removeClass('infoContainerRed');
        });
    }, 5000);
}


