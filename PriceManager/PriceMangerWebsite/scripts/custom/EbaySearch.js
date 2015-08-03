function SetSearchTriggers() {
    $('#btnSearch').click(GetSearchResults);
    $('.myProduct .searchPrice span').live('click', OpenPriceChangeBox);
    $('#btnSavePrice').click(UpdateProductPrice);
    $('#aAdvancedSearch').click(ToggleAdvancedBox);
}

function GetSearchResults(e) {
    var keyword = $('#txtSearch').val();

    if ($.trim(keyword) == '') {
        alert('Keyword cannot be empty');
        e.preventDefault();
        return false;
    }

    $('.loadingContainer').parent().show();
    $('#btnSearch').prop('disabled', true);


    var pageSize = $('#ddlSearchEbay').val();
    var tokenJSON = $('.hfTokenJSON').val();

    var filters = {
        MinQuantity: $('#txtMinQuantity').val(),
        MinPrice: $('#txtMinPrice').val(),
        IsNew: $('#chkNew').prop('checked')
    };

    var filterJSON = JSON.stringify(filters);

    XHR_GetSearchByKeyword(keyword, pageSize, tokenJSON, filterJSON, function (result) {
        var data = JSON.parse(result);

        var body = $('#ulResults');
        body.empty();
        $('#searchTemplate').tmpl(data).appendTo(body);

        $('.hfMyProduct[value="true"]').closest('li').addClass('myProduct');

        BuildSummary();

        $('.loadingContainer').parent().hide();
        $('#btnSearch').prop('disabled', false);
    });

    e.preventDefault();
    return false;
}

function OpenPriceChangeBox() {
    var productID = $(this).closest('li').find('.hfItemID').val();
    var productTitle = $(this).closest('li').find('.searchTitle').text();

    $('#txtNewPrice').val('');
    $('#hfSelectedProductID').val(productID);
    $('#hProductTitle').text(productTitle);
    $('#changePrice').dialog({ modal: true });
}

function BuildSummary() {
    var myProductCount = $('.myProduct').length;

    var matched = true, matchedValue = 0;
    var myPrice = $('.myProduct').find('.searchPrice span').map(function () {
        if ((matchedValue == parseFloat($(this).text()) || matchedValue == 0) && matched == true)
            matched = true;
        else
            matched = false;
        matchedValue = parseFloat($(this).text());
        return matchedValue;
    }).get().join(', ');

    if (matched == true)
        myPrice = matchedValue;

    var price = new Array();
    $('.resultItem').not('.myProduct').find('.searchPrice span').each(function () {
        price.push(parseFloat($(this).text()));
    });
    var a = 1;

    // average
    var sum = 0;
    for (var i = 0; i < price.length; i++) {
        sum += parseInt(price[i]);
    }

    var avg = sum / price.length;
    var max = Math.max.apply(Math, price);
    var min = Math.min.apply(Math, price);

    $('.summaryItems').text(myProductCount + " item(s)");
    $('.summaryYourPrice').text(myPrice);
    $('.summaryAveragePrice').text(avg.toFixed(2));
    $('.summaryHighestPrice').text(max);
    $('.summaryLowestPrice').text(min);

    $('.searchShippingCost:containsExact("Shipping: 0")').hide();

    $('.summaryText').show();
}

function UpdateProductPrice(e) {
    $('.loadingContainer').parent().show();
    $('#btnSavePrice').prop('disabled', true);

    var newPrice = $('#txtNewPrice').val();
    if (newPrice == '') {

        alert('please provide a valid amount');
        e.preventDefault();
        return false;

    }

    var productID = $('#hfSelectedProductID').val();
    var tokenJSON = $('.hfTokenJSON').val();
    var userID = $('.hfItemID[value="' + productID + '"]').closest('li').find('.searchSellerID').text();

    XHR_UpdateProductPrice(productID, newPrice, tokenJSON, userID, function (result) {

        if (result == '' || result == null) {
            $('.hfItemID[value="' + productID + '"]').closest('li').find('.searchPrice span').text(newPrice);
            $('#changePrice').dialog('close');
            ShowInfo('Price Changed for Product ID: ' + productID, 'green');
        }
        else {
            ShowInfo('ERROR: ' + result, 'red');
        }

        $('.loadingContainer').parent().hide();
        $('#btnSavePrice').prop('disabled', false);

    }, null);

    e.preventDefault();
    return false;
}

function XHR_GetSearchByKeyword(searchKeyword, pageSize, tokenJSON, filters, OnSuccess, OnError) {
    var service = new GeneralServices.GeneralSvc();
    service.GetSearchByKeyword(searchKeyword, pageSize, tokenJSON, filters, null, OnSuccess, OnError, null);
}

function XHR_UpdateProductPrice(productID, price, tokenJSON, userID, OnSuccess, OnError) {
    var service = new GeneralServices.GeneralSvc();
    service.UpdateProductPrice(productID, price, tokenJSON, userID, OnSuccess, OnError, null);
}

function GetSellerType(isTopRatedSeller) {
    if (isTopRatedSeller == true)
        return '';
    else
        return 'display:none'
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

function ToggleAdvancedBox() {
    if ($('#divAdvancedSearchBox').is(':visible')) {
        $('#divAdvancedSearchBox').hide();
        $('#aAdvancedSearch').text('Show Filters');
    }
    else {
        $('#divAdvancedSearchBox').show();
        $('#aAdvancedSearch').text('Hide Filters');
    }
}