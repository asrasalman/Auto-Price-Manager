var isSavetoFile = false;
var ArrayTitle = [];

$(document).ready(function () {
    $("#tabs").tabs({
     select:function(event,ui){                                                       
                            //alert(ui.index);                                                   
                    }
                    });  

$("#ddlAlgorithm").change(function() {

    var algo = $("#ddlAlgorithm option:selected").text();
    if(algo == 'Match Lowest')
    {
        $('#txtLessToLowest option:first-child').attr("selected", "selected");
    }
});


});


function InitializePricing() {

    $("#btnDefaults").bind("click", function () { getDefaultSettings(); });
    $(".btnEdit").live("click", function () { EditItem(this) });
    $(".select-select2").each(function () {
        $(this).select2();
    });
    $("#btnSearch").live("click", function () { GetSearchResults() });
    $("#btnSearchTitle").live("click", function () { GetSearchResultsTitle(); });
    $("#btnRevise").live("click", function () { ReviseFilter() });
    $("#btnReviseTitle").live("click", function () { ReviseFilterTitle() });
    $("#btnSave").live("click", function () { isSavetoFile = false; SavePricingDetails(); });
    $("#btnSaveFile").live("click", function () { isSavetoFile = true; SavePricingDetails(); });
    $("#btnMainSearch").live("click", function () { GetPricingItems(false); });
    $("#btnReloadListing").live("click", function () { GetPricingItems(true); });
    $('#chkSelectAll').change(SetCheckboxes);
    $('#ddlSavedFile').live("change", function () {
        if ($(this).val() > 0) {
            getSavedSettting();
        }
    });
    $("#hrefRefresh").live("click", function () { RefreshPricingItems(true); return false; });
    $("#btnSaveDefaults").live("click", function () { saveDefaultSettings(); });
    $("#btnUseDefaults").live("click", function () { useDefaultSettings(); });
    $("#btnClearForm").live("click", function () { ResetControls("[data-file]"); return false; });
    $("#btnDeactivateAll").live("click", function () { DeactivateAll(); return false; });

    $('#btnLoadTitle').live("click", function () {
        //LoadTitles();
        GetSearchResultsTitle(null, true);
        return false;
    });

    $('#btnSaveTitle').live('click', function () {
        SaveTitles();
        return false;
    });

    GetPricingItems();

}

function editTitle(obj){
    $(obj).removeAttr("disabled").select();
    return false;
}

function SaveTitles() {
    var isValid = true;
    if (ArrayTitle != null) {
        for (var i = 0; i < 5; i++) {
            var txtId = '#txtTitle' + (i + 1);
            if ($(txtId).val() == ArrayTitle[i]) {
                isValid = false;
            }
        }
    }

    if (isValid == false) {
        ShowInfo("Please Chahnge Titles with cross mark.", "red");
    } else {
        
        var TitleJson = {
            ItemTitleId1: $('#hdntitleId1').val(),
            ItemTitleId2: $('#hdntitleId2').val(),
            ItemTitleId3: $('#hdntitleId3').val(),
            ItemTitleId4: $('#hdntitleId4').val(),
            ItemTitleId5: $('#hdntitleId5').val(),
            Title1: $('#txtTitle1').val(),
            Title2: $('#txtTitle2').val(),
            Title3: $('#txtTitle3').val(),
            Title4: $('#txtTitle4').val(),
            Title5: $('#txtTitle5').val(),
            Rotate: $('#ddlRotate').val(),
            Days: $('#ddlDays').val(),
            Sales: $('#ddlSales').val(),
            ItemCode: $('#hfItemCode').val(),
            ItemId: $('#hfItemID').val(),
            IsAutomate: $('#chkTitleAutomate').prop('checked'),
        };

        var mydata = new Object();
        mydata.TitleJson = JSON.stringify(TitleJson);

        XHR_UpdatePricingTitle(JSON.stringify(TitleJson), function (result) {
            if(result)
            {
                var items = JSON.parse(result);
                if (items.login != undefined && items.login == false) {
                    window.location = "/pages/login.aspx?returnurl=" + encodeURIComponent('/pages/seller/pricing.aspx');
                }
                else{
                    ShowInfo("Updated successfully.", "green");
                    MyModalClose(".divFiltersModal");
                }
            }
            else{
                ShowInfo("Something went wrong! Please try later.", "red")
            }
            $(".loadingContainer").parent().hide();

        });
    }
}

function CheckValues(id, index) {
    if (ArrayTitle != null && ArrayTitle.length > 0) {
        if ($('#' + id).val() == ArrayTitle[index]) {
            $('#' + id).closest('p').find('img.titlereject').show();
            $('#' + id).closest('p').find('img.titleaccept').hide();
        } else {
            $('#' + id).closest('p').find('img.titleaccept').show();
            $('#' + id).closest('p').find('img.titlereject').hide();
        }
    }
}

function CountCharacters(cntrl, spn) {
    var count = $('#' + cntrl).val().length;
    $('#' + spn).text(count);
    if (count > 60) {
        //alert("Max limit for title is 60 chars");
        $('#' + cntrl).css('border-color', 'red');
    } else {
        $('#' + cntrl).css('border-color', '#D5D5D5');
    }
}

function LoadTitles() {

    var itemCode = $("#hfItemCode").val();

    XHR_LoadTitles(itemCode, function (result) {
        $(".loadingContainer").parent().hide();
        alert("hi");
    }, function (error) {
        alert("error");
    });

    //    XHR_GetPricingItems(itemCode, null, null, null, null, null, null, function (result) {
    //        $(".loadingContainer").parent().hide();
    //        if (result) {
    //            var items = JSON.parse(result);
    //            if (items.login != undefined && items.login == false) {
    //                window.location = "/pages/login.aspx?returnurl=" + encodeURIComponent('/pages/seller/pricing.aspx');
    //            }
    //            ResetControls("[data-column]");
    //            BindData(items[0], "[data-column]");
    //        }

    //    }, null); 
    return false;

}


function DeactivateAll() {

    var account = $("#ddlEbayAccount").val();
    var category = $("#ddlEbayCategory").val();
    var searchFeild = $("#ddlSearchFeild").val();
    var searchValue = $("#txtSearchValue").val();
    var country = $("#ddlCountrySearch").val();

    var result = confirm("You want to deactivate all automated items. Are you sure?");
    if (result) {
        XHR_DeactivateAll(account, category, searchFeild, searchValue, country, function (result) {
            $(".loadingContainer").parent().hide();
            if (result) {
                var items = JSON.parse(result);
                if (items.login != undefined && items.login == false) {
                    window.location = "/pages/login.aspx?returnurl=" + encodeURIComponent('/pages/seller/pricing.aspx');
                }
                if (items.length > 0) {
                    $('#tblPricingItems tbody').empty();

                    $('#pricingItemTemplate').tmpl(items).appendTo('#tblPricingItems tbody');

                    setupTablesorter();

                    $('input.numeric').numeric();
                }
                else {
                    var rows = "<tr><td colspan='12'>No items found.</td></tr>";
                    $('#tblPricingItems tbody').empty().append(rows);
                }
            }
            else {
                var rows = "<tr><td colspan='12'>No items found.</td></tr>";
                $('#tblPricingItems tbody').empty().append(rows);

            }


        }, null);

        return false;
    }
}
function RefreshPricingItems() {

    var itemCode = $("#hfItemCode").val();


    XHR_GetPricingItems(itemCode, null, null, null, null, null, null, function (result) {
        $(".loadingContainer").parent().hide();
        if (result) {
            var items = JSON.parse(result);
            if (items.login != undefined && items.login == false) {
                window.location = "/pages/login.aspx?returnurl=" + encodeURIComponent('/pages/seller/pricing.aspx');
            }
            ResetControls("[data-column]");
            BindData(items[0], "[data-column]");
        }

    }, null);
    return false;
}

function useDefaultSettings() {
    XHR_GetDefaultSettings(function (result) {
        $(".loadingContainer").parent().hide();
        if (result) {
            var items = JSON.parse(result);
            if (items.login != undefined && items.login == false) {
                window.location = "/pages/login.aspx?returnurl=" + encodeURIComponent('/pages/seller/pricing.aspx');
            }
            ResetControls("[data-file]");
            BindData(items[0], "[data-file]");


        }
    }, null);
}
function getDefaultSettings() {
    XHR_GetDefaultSettings(function (result) {
        $(".loadingContainer").parent().hide();
        if (result) {
            var items = JSON.parse(result);
            if (items.login != undefined && items.login == false) {
                window.location = "/pages/login.aspx?returnurl=" + encodeURIComponent('/pages/seller/pricing.aspx');
            }
            ResetControls("[data-default]");
            BindData(items[0], "[data-default]");

            $("#txtMinPriceDefault").val(items[0].Minimum_Price);
            $("#txtMaxPriceDefault").val(items[0].Maximum_Price);
            $("#txtMinQuantityDefault").val(items[0].Minimum_Quantity);
            $("#txtMaxQuantityDefault").val(items[0].Maximum_Quantity);

            MyModal(".divDefaultsModal", 'Default Setup', 800, 'auto');
        }
    }, null);
}

function XHR_GetDefaultSettings(OnSuccess, OnError) {
    $(".loadingContainer").parent().show();
    var service = new GeneralServices.GeneralSvc();
    service.GetDefaultSettings(OnSuccess, OnErrorGeneral, null);
}


function saveDefaultSettings() {
    var editedData = CreateObject("[data-default]");
    if (editedData) {
        XHR_SaveDefaultSettings(JSON.stringify(editedData), function (result) {
            $(".loadingContainer").parent().hide();
            if (result == true) {
                ShowInfo("Default sttings updated successfully.", "green");
                MyModalClose(".divDefaultsModal");
            }
        }, null);
    }

}

function XHR_SaveDefaultSettings(updatedData, OnSuccess, OnError) {

    var itemCodeFile = Number($("#ddlSavedFile").val());
    var service = new GeneralServices.GeneralSvc();
    service.SaveDefaultSettings(updatedData, OnSuccess, OnErrorGeneral, null);
}



function SetCheckboxes() {
    $('#tblPricingItems tbody').find('.chkSelect input[type="checkbox"]').prop('checked', $(this).prop('checked'));
}

function EditItem(obj) {
    obj = $(obj);
    var itemID = obj.closest("tr").find(".spnItemID").text();
    var itemCode = obj.closest("tr").find(".hfItemCode").val();
    $("#hfItemID").val(itemID);
    $("#hfItemCode").val(itemCode);

    XHR_GetPricingItems(itemCode, null, null, null, null, null, null, function (result) {
        $(".loadingContainer").parent().hide();
        if (result) {
            var items = JSON.parse(result);
            if (items.login != undefined && items.login == false) {
                window.location = "/pages/login.aspx?returnurl=" + encodeURIComponent('/pages/seller/pricing.aspx');
            }
            ResetControls("[data-column]");
            BindData(items[0], "[data-column]");
            
            if (items[0].ItemTitles.length > 0) {
                        for (var i = 0; i < items[0].ItemTitles.length; i++) {
                            var txtId = '#txtTitle' + (i + 1);
                            var hdnTitleId = '#hdntitleId' + (i + 1);
                            var spnTotalSales = '#spnTotalSales' + (i + 1);
                            $(txtId).val(items[0].ItemTitles[i].Title);
                            //$(txtId).attr("disabled", "disabled");
                            $(hdnTitleId).val(items[0].ItemTitles[i].ItemTitleId);
                            $(spnTotalSales).text(items[0].ItemTitles[i].TotalSales);

                            $("span.totalSales").show();
                        }
            }

           // alert(items[0].Is_Automated);

            if (items[0].Is_Automated == false && items[0].Keywords == null) {

                $("#txtKeywords").val(items[0].Item_Name);
            }
            else {
                $("#txtKeywords").val(items[0].Keywords);
            }
            
            $("#txtExcludeSellers").val(items[0].Exclued_Sellers);
            $("#txtIncludeSellers").val(items[0].Inclued_Sellers);
            var href = $("#linkPricingHistory").attr('href');
            $("#linkPricingHistory").attr('href', href.replace("{itemcode}", items[0].Item_Code));
            MyModal(".divFiltersModal", 'Price Automation : ' + itemID, 900, 'auto');
        }
    }, null);
    return false;

}
function ReviseFilter() {
    var body = $('#ulResults');
    body.empty();
    $(".divSearchResults, #btnRevise").hide();
    $(".divPricingInputs, #btnSearch").fadeIn();
}
function ReviseFilterTitle() {
    var body = $('#ulResults');
    body.empty();
    $("#tabs").tabs("select", 0);  
    $(".divSearchResults, #btnRevise").hide();
    $(".divPricingInputs, #btnSearch").fadeIn();
}

function GetSearchResults(obj) {
    obj = $(obj);
    var keyword = $("#txtKeywords").val();
    var ItemID = $("#hfItemID").val();
    var tokenJSON = $(".hfTokenJSON").val();

    if ($.trim(keyword) == '') {
        ShowInfo("Search query can not be empty.")
        return false;
    }
    var pageSize = 20;
    var filter = CreateObject("[data-filter]");
    if (filter) {
        $(".divPricingInputs, #btnSearch").hide();
        $(".divSearchResults, #btnRevise").fadeIn();
        $(".divSearchResults").addClass("loading");
        var body = $('#ulResults');
        body.empty();
        XHR_GetProductRank(JSON.stringify(filter), pageSize, tokenJSON, function (result) {
            $(".divSearchResults").removeClass("loading");
            if (result) {
                var data = JSON.parse(result);
                var li = $('#searchTemplate').tmpl(data);
                body.append(li).hide().fadeIn();
                $('.hfMyProduct[value="true"]').closest('li').addClass('myProduct');
                $(obj).prop('disabled', false);
            }
            else {

                var notFound = "<li class='resultItem'><strong>No item(s) Found. Please revise your search filters.</strong></li>"
                body.append(notFound).hide().fadeIn();
            }

        }, null);
    }
    return false;
}
function GetSellerType(isTopRatedSeller) {
    if (isTopRatedSeller == true)
        return '';
    else
        return 'display:none'
}

function GetSearchResultsTitle(obj, isLoadTitle) {
    obj = $(obj);
    var keyword = $("#txtKeywords").val();
    var ItemID = $("#hfItemID").val();
    var tokenJSON = $(".hfTokenJSON").val();

    if ($.trim(keyword) == '') {
        ShowInfo("Search query can not be empty.");
        return false;
    }
    var pageSize = 20;
    var filter = CreateObject("[data-filter]");
    if (filter) {
        $(".divPricingInputsTitle, #btnSearchTitle").hide();
        $(".divSearchResultsTitle, #btnReviseTitle").fadeIn();
        $(".divSearchResultsTitle").addClass("loading");
        var body = $('#ulResultTitle');
        body.empty();
        XHR_GetProductRankTitle(JSON.stringify(filter), pageSize, tokenJSON, function (result) {
            
            if (result) {
                
                var data = JSON.parse(result);
                //var li = $('#searchTemplateTitle').tmpl(data);
                //body.append(li).hide().fadeIn();
                $('.hfMyProductTitle[value="true"]').closest('li').addClass('myProduct');
                $(obj).prop('disabled', false);
                $('.divSearchResultsTitle').show();

                if (isLoadTitle === true) {

                    var returnedData = $.grep(data, function (element, index) {
                        return element.IsMyProduct == false;
                    });
                    if(returnedData.length > 0 )
                    {
                        $("span.totalSales").hide();
                        $("span.salesCount").text('');
                        for (var i = 0; i < 5; i++) {
                            if(returnedData[i])
                            {
                                var txtId = '#txtTitle' + (i + 1);
                                var spnId = '#spnCount' + (i + 1);
                                $(txtId).val(returnedData[i].Title);
                                $(txtId).closest('p').find('img.searchImg').attr("src", returnedData[i].ImageURL).show();
                                $(txtId).closest('p').find('span.searchSellerID').text(returnedData[i].SellerID);
                                $(txtId).closest('p').find('span.searchTimeRemaining').text("Time: " + returnedData[i].TimeRemaining);
                                $(txtId).closest('p').find('img.topRatedSeller').attr("style", GetSellerType(returnedData[i].TopRatedSeller));
                                $(txtId).closest('p').find('img.titlereject').show();
                                //$(txtId).attr("disabled", "disabled");
                                $(spnId).text($(txtId).val().length);
                                ArrayTitle.push(returnedData[i].Title);

                                if ($(txtId).val().length > 60) {
                                    $(txtId).css('border-color', 'red');
                                }
                            }
                        }
                    }
                    else{
                        ShowInfo("No item(s) Found. Please revise your search filters.", "red");
                    }
                }
            }
            else {
                ShowInfo("No item(s) Found. Please revise your search filters.", "red");
            }
            $(".loadingContainer").parent().hide();
        }, null);
    }
    return false;
}

function GetPricingItems(reload) {
    var account = $("#ddlEbayAccount").val();
    var category = $("#ddlEbayCategory").val();
    var searchFeild = $("#ddlSearchFeild").val();
    var searchValue = $("#txtSearchValue").val();
    var country = $("#ddlCountrySearch").val();
    // alert(country);

    if(reload == true)
    {
        $(".loadingContainer").css('width', '109px');
        $(".loadingContainer").css('height', '33px');
        $(".loadingContainer").css('padding-top', '3%');
        $(".loadingContainer").css('text-align', 'center');
        $(".loadingContainer").css('padding-left', '1%');
        $(".loadingContainer").css('font-size', '11px');
        $(".loadingContainer").text('Please be patient this may take some time');
    }

    $(".loadingContainer").parent().show();

    XHR_GetPricingItems(null, account, category, searchFeild, searchValue, reload, country, function (result) {

        
        if (result) {
            var items = JSON.parse(result);
            if (items.login != undefined && items.login == false) {
                window.location = "/pages/login.aspx?returnurl=" + encodeURIComponent('/pages/seller/pricing.aspx');
            }
            if (items.length > 0) {
                $('#tblPricingItems tbody').empty();

                $('#pricingItemTemplate').tmpl(items).appendTo('#tblPricingItems tbody');

                setupTablesorter();

                $('input.numeric').numeric();
            }
            else {
                var rows = "<tr><td colspan='12'>No items found.</td></tr>";
                $('#tblPricingItems tbody').empty().append(rows);
            }
            $(".loadingContainer").parent().hide();
            $(".loadingContainer").text('');
            $(".loadingContainer").removeAttr('style');

        }
        else {
            var rows = "<tr><td colspan='12'>No items found.</td></tr>";
            $('#tblPricingItems tbody').empty().append(rows);
            $(".loadingContainer").parent().hide();
            $(".loadingContainer").text('');
            $(".loadingContainer").removeAttr('style');
        }


    }, null)

 
  
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

function SavePricingDetails() {
    var editedData = CreateObject("[data-column]");
    if (editedData) {
        var errorHtml = "";
        var isValidated = true;
        if (editedData.Is_Automated == true) {
            if (editedData.Keywords == null) {
                errorHtml += "<li>Please enter search query</li>";
                isValidated = false;
            }
            if (editedData.Algo == null) {
                errorHtml += "<li>Please select algorithm</li>";
                isValidated = false;
            }
            if (editedData.Floor_Price == null) {
                errorHtml += "<li>Please enter floor price</li>";
                isValidated = false;
            }
            if (editedData.Ceiling_Price == null) {
                errorHtml += "<li>Please enter ceiling price</li>";
                isValidated = false;
            }
              if (editedData.Algo != '3' && editedData.Less_To_Lowest_Price == null) {
                errorHtml += "<li>Please enter less to lowest price</li>";
                isValidated = false;
            }
//            if (editedData.Less_To_Lowest_Price == null) {
//                errorHtml += "<li>Please enter less to lowest price</li>";
//                isValidated = false;
//            }
            if (isValidated) {
                if (parseFloat(editedData.Floor_Price) > parseFloat(editedData.Ceiling_Price)) {
                    errorHtml += "<li>Floor price should be less than equals to ceiling price</li>";
                    isValidated = false;
                }
            }
        }

        if (isValidated) {
            $("#ulErrors").empty();
            $(".errorbox").hide();
            $(".loadingContainer").parent().show();
            XHR_UpdatePricingItems(JSON.stringify(editedData), function (result) {
                if (result) {
                    var items = JSON.parse(result);
                    var tr = $(".hfItemCode[value=" + items[0].Item_Code + "]").closest("tr");
                    var itemRow = $('#pricingItemTemplate').tmpl(items);
                    tr.html(itemRow.html());
                    $(".loadingContainer").parent().hide();
                    ShowInfo("Updated successfully.", "green");
                   // MyModalClose(".divFiltersModal");
                }
            }, null);
        }
        else {
            $("#ulErrors").html(errorHtml)
            $(".errorbox").fadeIn();
        }
    }
    else
        ShowInfo("There is no records found to be updated.");
}



function XHR_GetPricingItems(itemCode, account, category, searchFeild, searchValue, isReload, country, OnSuccess, OnError) {
    $(".loadingContainer").parent().show();

    var service = new GeneralServices.GeneralSvc();  
    service.GetPricingItems(itemCode, account, category, searchFeild, searchValue, isReload, country, OnSuccess, OnErrorGeneral, null);

}

function XHR_LoadTitles(itemCode, OnSuccess, OnError) {
    $(".loadingContainer").parent().show();
    var service = new GeneralServices.GeneralSvc();
    service.LoadTitle(itemCode, OnSuccess, OnError);
}

function XHR_UpdatePricingItems(updatedData, OnSuccess, OnError) {
    var itemCodeFile = Number($("#ddlSavedFile").val());
    var service = new GeneralServices.GeneralSvc();
    service.UpdatePricingItems(updatedData, isSavetoFile, itemCodeFile, OnSuccess, OnErrorGeneral, null);
}

function XHR_UpdatePricingTitle(updatedData, OnSuccess, OnError) {
    $(".loadingContainer").parent().show();
    var service = new GeneralServices.GeneralSvc();
    service.UpdatePricingTitle(updatedData, OnSuccess, OnErrorGeneral, null);
}

function XHR_GetProductRank(filter, pageSize, tokenJSON, OnSuccess, OnError) {
    var service = new GeneralServices.GeneralSvc();
    service.GetProductRank(filter, pageSize, tokenJSON, null, OnSuccess, OnErrorGeneral, null);
}

function XHR_GetProductRankTitle(filter, pageSize, tokenJSON, OnSuccess, OnError) {
    $(".loadingContainer").parent().show();
    var service = new GeneralServices.GeneralSvc();
    service.GetProductRankTitle(filter, pageSize, tokenJSON, null, OnSuccess, OnErrorGeneral, null);
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

function CreateObject(dataAttr) {
    var elements = $("input" + dataAttr + ", textarea" + dataAttr + ", select" + dataAttr + " ,a" + dataAttr + " ,span" + dataAttr + " ,img" + dataAttr);
    var param = new Object();
    elements.each(function () {
        var column = this.getAttribute(dataAttr.replace("[", "").replace("]", ""));
        if (this.getAttribute('type') == 'checkbox') {
            param[column] = $(this).is(":checked");
        }
        else if ($(this).is(".select-select2")) {
            var selectValues = $(this).select2().val();
            var csv = "";
            if (selectValues) {
                for (var i = 0; i < selectValues.length; i++) {
                    csv += "," + selectValues[i];
                }
                if (csv != "")
                    csv = csv.substring(1, csv.length);
            }
            param[column] = csv;
        }
        else if ($(this).is(".anchor")) {
            param[column] = $(this).attr("href");
        }
        else if ($(this).is(".image")) {
            param[column] = $(this).attr("src");
        }
        else if ($(this).is(".span")) {
            param[column] = $(this).text();
        }
        else {
            var value = $(this).val();
            if (value != "")
                param[column] = value;
            else
                param[column] = null;
        }
    });
    return param;
}
function BindData(jdata, dataAttr) {
    var elements = $("input" + dataAttr + ", textarea" + dataAttr + ", select" + dataAttr + " ,a" + dataAttr + " ,span" + dataAttr + " ,img" + dataAttr);
    elements.each(function () {
        var column = this.getAttribute(dataAttr.replace("[", "").replace("]", ""));
        if (jdata[column]) {
            if (this.getAttribute('type') == 'checkbox') {
                $(this).attr("checked", jdata[column]);
            }
            else if ($(this).is(".select-select2")) {
                var selectValues = jdata[column].split(',');
                $(this).val(selectValues).select2();
            }
            else if ($(this).is(".select-normal")) {
                $(this).val(jdata[column]);
            }
            else if ($(this).is(".anchor")) {
                $(this).attr("href", jdata[column]);
            }
            else if ($(this).is(".image")) {
                $(this).attr("src", jdata[column]);
            }
            else if ($(this).is(".span")) {
                $(this).text(jdata[column]);
            }
            else {
                $(this).val(jdata[column]);
            }
        }
    });

}
function ResetControls(dataAttr) {

    ReviseFilter();
    var elements = $("input" + dataAttr + ", textarea" + dataAttr + ", select" + dataAttr + " ,a" + dataAttr + " ,span" + dataAttr + " ,img" + dataAttr);
    $("#ulErrors").empty();
    $(".errorbox").hide();
    elements.each(function () {
        if (this.getAttribute('type') == 'checkbox') {
            $(this).attr("checked", false);
        }
        else if ($(this).is(".select-select2")) {
            $(this).val([]).select2();
        }
        else if ($(this).is(".select-normal")) {
            $(this).val("");
        }
        else if ($(this).is(".anchor")) {
            $(this).attr("href", "");
        }
        else if ($(this).is(".image")) {
            $(this).attr("src", "");
        }
        else if ($(this).is(".span")) {
            $(this).text("");
        }
        else {
            $(this).val("");
        }
    });
    $("#chkFixedPrice").prop("checked", true);
    $("#txtExcludeSellers").val($("#hfSeller").val());
    //$("#ddlCountry").val($("#hfCountryCode").val());
    $("#linkPricingHistory").attr('href', "/pages/seller/pricinghistory.aspx?itemcode={itemcode}");

}
function formatPrice(value) {
    if (isNaN(parseFloat(value)))
        return "";
    else
        return parseFloat(value).toFixed(2);
}
function MyModal(id, title, width, height) {
    var mydiv = $(id);
    mydiv.dialog(
             {
                 autoOpen: false,
                 modal: true,
                 resizable: false,
                 title: title,
                 width: width,
                 height: height,
                 closeOnEscape: true
             });
    $('.ui-dialog').appendTo($('form:first'));
    mydiv.dialog('open');
    return false;
}
function MyModalClose(id) {
    var mydiv = $(id);
    mydiv.dialog("close");
}

function setSavedSettigns(obj) {

    obj = $(obj);
    var itemID = obj.closest("tr").find(".spnItemID").text();
    var itemCode = obj.closest("tr").find(".hfItemCode").val();
    $("#hfItemID").val(itemID);
    $("#hfItemCode").val(itemCode);

    XHR_GetPricingItemsFile(itemCode, null, function (result) {
        if (result) {
            var items = JSON.parse(result);
            var options = $("#selectTemplate").tmpl(items);
            $("#ddlSavedFile").html(options);
            var none = new Option("None", "0");
            $("#ddlSavedFile").prepend(none);
            $("#ddlSavedFile optionp[value='0']").prop("selected", true);

            XHR_GetPricingItems(itemCode, null, null, null, null, null, null, function (result) {
                if (result) {
                    var items = JSON.parse(result);
                    ResetControls("[data-column]");
                    BindData(items[0], "[data-column]");
                    
                    if (items.ItemTitles.length > 0) {
                        for (var i = 0; i < items.ItemTitles.length; i++) {
                            var txtId = '#txtTitle' + (i + 1);
                            $(txtId).val(items.ItemTitles[i].Title);
                        }
                    }
                    
                    if ($("#txtExcludeSellers").val()) {
                        $("#txtExcludeSellers").val($("#hfSeller").val());
                    }
                    $("#txtExcludeSellers").val($("#hfSeller").val());
                    MyModal(".divFiltersModal", 'Price Automation : ' + itemID, 800, 'auto');
                }

            }, null)

        }

    }, null);
}

function XHR_GetPricingItemsFile(itemCode, itemCodeFile, OnSuccess, OnError) {
    $(".loadingContainer").parent().show();
    var service = new GeneralServices.GeneralSvc();
    service.GetPricingItemsFile(itemCode, itemCodeFile, OnSuccess, OnErrorGeneral, null);
}

function getSavedSettting() {
    XHR_GetPricingItemsFile(null, $("#ddlSavedFile").val(), function (result) {
        $(".loadingContainer").parent().hide();
        if (result) {
            var items = JSON.parse(result);
            ResetControls("[data-file]");
            BindData(items[0], "[data-file]");
        }

    }, null);
}

function XHR_GetPricingItemsFile(itemCode, itemCodeFile, OnSuccess, OnError) {
    $(".loadingContainer").parent().show();
    var service = new GeneralServices.GeneralSvc();
    service.GetPricingItemsFile(itemCode, itemCodeFile, OnSuccess, OnErrorGeneral, null);
}

function XHR_DeactivateAll(account, category, searchFeild, searchValue, country, OnSuccess, OnError) {
    $(".loadingContainer").parent().show();
    var service = new GeneralServices.GeneralSvc();
    service.DeactivateAll(account, category, searchFeild, searchValue, country, OnSuccess, OnErrorGeneral, null);
}

