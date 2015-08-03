function ShowEbayConnect() {
    $('.divNoSettingFound').dialog({ width: '500px', modal: true, title: 'EBay Settings' }).parent().find('.ui-dialog-titlebar-close').hide();
    $('.ui-dialog').appendTo('form:first');
    $('.ui-dialog-titlebar').remove();
}

function SetTriggers() {

    $("input:file").change(function () {
        var fileName = $(this).val();
        $(".fileName").text(fileName);
    });

    $('.uploadManifest').click(ValidateFile);
    $('#chkSelectAll').change(SetCheckboxes);
    $('.btnProcessSelectedItems').click(ValidateSelection);
    $('.aConnectAgain').live('click', ConnectAgain);
    SetColors();

    MarkFalsePC();

    $('.falseSuburbText').click(ShowPostCodeBox);
    $('#btnChangeSuburb').unbind('click');
    $('#btnChangeSuburb').click(ChangeSuburb);
    $('.messageUnread').live('click', ShowMessageList);


    $('#tblParcelItems').colResizable({
        liveDrag: true
    });


    $("#tblParcelItems").tablesorter({
        // set forced sort on the fourth column and i decending order. 
        sortForce: [[0, 0]],
        headers: {
            1: { sorter: false}      // disable first column
        }
    });


    $("#tblParcelItems").bind("sortStart", function () {
        $('.fullSpan').remove();
    }).bind("sortEnd", function () {
        GroupItemsByAccount();
    });


    GroupItemsByAccount();

    $('.hfIsIncomplete[value="True"]').closest('tr').addClass('incompleteRow').find('.spnIncomplete').show();
}

function GroupItemsByAccount() {
    var newRow = $('<tr class="fullSpan"><td colspan="15" class="groupRow"></td></tr>');

    var currentType = '';
    var currentID = '';
    $('.parcelItem').each(function () {
        var type = $(this).find('.hfAccountType').val();
        var ID = $(this).find('.hfAccountID').val();

        if (ID != currentID) {
            var newRowClone = newRow.clone();
            $(this).before(newRowClone);
            newRowClone.find('td').text(type);
        }
        currentID = ID;
        currentType = type;

        // messages work. utilizing the same loop
        var messages = $(this).find('.hfMessages').val();
        if (messages != '' && messages != null) {
            var messageList = JSON.parse(messages);
            $(this).find('.messageIcon').removeClass('messageRead').addClass('messageUnread');
        }

    });
}

function ConnectAgain() {
    ShowEbayConnect();
    return false;
}

function ValidateFile(event) {
    var fileName = $('.fileName').text();
    if (fileName == '') {
        alert('No File Selected');
        event.preventDefault();
        return false;
    }
}

function SetCheckboxes() {
    $('#tblParcelItems tbody').find('.chkSelect input[type="checkbox"]').prop('checked', $(this).prop('checked'));
}

function ValidateSelection() {
    var count = $('#tblParcelItems tbody').find('input[type="checkbox"]:checked').length;

    if (count == 0) {
        alert('Please select at-least one item to process for e-parcel file');
        return false;
    }
}

function SetColors() {
    $('.shippingMethod:contains("Express")').closest('tr').addClass('backred');
    $('.shippingMethod:contains("AU_Freight")').closest('tr').addClass('backblue');
}

function OpenAuthWindow(event) {
    var url = $('.hfAuthURL').val();
    var windowName = "Ebay Authorization";
    var windowSize = "width=1000,height=600,scrollbars=yes";

    window.open(url, windowName, windowSize);

    event.preventDefault();
}

function MarkFalsePC() {
    $('.imgPostCode[src="/images/delete2.png"]').addClass('falsePC');
    $('.falsePC').closest('tr').find('.suburbText').addClass('falseSuburbText');
}

function ShowPostCodeBox() {
    $('#selectSuburb').dialog({ width: '400px', modal: true, title: 'Update Suburb' });
    $('.ui-dialog').appendTo('form:first');
    var row = $(this).closest('tr');
    var postCode = row.find('.hfPostalCode').val();
    var suburb = $(this).text();
    var address1 = row.find('.hfStreet2').val();
    var address2 = row.find('.hfStreet3').val();

    $('#txtPostCode').val(postCode);
    $('#txtPostCode').prop('disabled', true);
    $('#txtAddress1').val(address1);
    $('#txtAddress2').val(address2);

    $('#hfSelectedRow').val($(this).closest('tr').prevAll().length + 1);

    XHR_GetSuburbList(postCode, suburb, function (result) {
        var postCodes = JSON.parse(result);

        if (postCodes == null || postCodes.length == 0) { // postcode not matching in the list
            $('#txtSuburb').show();
            $('#ddlSuburb').hide();

            $('#txtPostCode').prop('disabled', false);
            $('#noPostCode').text('Postcode lookup failed. Please enter a correct Postcode');
        }
        else {
            $('#noPostCode').text('');
            var ddlSuburb = $('#ddlSuburb');
            ddlSuburb.empty();
            $(postCodes).each(function () {
                ddlSuburb.append('<option>' + this + '</option>');
            });
        }
    }, function (result) {
        alert('some error');
    });
}

function XHR_GetSuburbList(postCode, currentSuburb, OnSuccess, OnError) {
    var service = new GeneralServices.GeneralSvc();
    service.GetSuburbList(postCode, currentSuburb, OnSuccess, OnError, null);
}

function XHR_RemoveAccountToken(type, userAccountCode, OnSuccess, OnError) {
    var service = new GeneralServices.GeneralSvc();
    service.RemoveAccountToken(type, userAccountCode, OnSuccess, OnError, null);
}

function ChangeSuburb(e) {
    var rowIndex = $('#hfSelectedRow').val();
    var postCode = $('#txtPostCode').val();
    var suburb = '';
    if ($('#ddlSuburb').is(':hidden'))
        suburb = $('#txtSuburb').val();
    else
        suburb = $('#ddlSuburb').find('option:selected').text();
    var address1 = $('#txtAddress1').val();
    var address2 = $('#txtAddress2').val();

    var row = $('#tblParcelItems').find('tr:eq(' + rowIndex + ')');
    row.find('.suburbText').removeClass('falseSuburbText').text(suburb);
    row.find('.hfCity').val(suburb);
    row.find('.imgPostCode').attr('src', '/images/tick.png');
    row.find('.hfPostalCode').val(postCode);

    row.find('.hfStreet2').val(address1);
    row.find('.hfStreet3').val(address2);
    row.find('.hfIsPCFixed').val(1);

    $('#selectSuburb').dialog('close');
    e.preventDefault();
    return false;
}


/*-------------- SHOPIFY----------------*/

function OpenShopifyAuthWindow(e) {
    e.preventDefault();
    var shopName = $('#txtShopName').val();
    var isExistingShop = $('.hfSelectedShopifyAccountID').val();
    if (shopName == '') {
        alert('The Shopify URL cannot be empty. Please provide a correct URL');
        return false;
    }
    else {
        window.open("/pages/shopify/ShopifyConnect.aspx?inputShopName=" + shopName + "&isES=" + isExistingShop, "Connect To Shopify", "width=795,height=630,location=0,menubar=0,scrollbars=0,status=1,toolbar=0,resizable=1");
        return false;
    }
}

function SetConnectTriggers() {
    $('.ebayAddMore').click(ShowAddMoreBox);
    $('.refreshConnection').click(SetRefreshConnection);
    $('.refreshShopifyConnection').click(SetShopifyRefreshConnection);
    $('.refreshMagentoConnection').click(SetMagentoRefreshConnection);
    $('.refreshBigcommerceConnection').click(SetBigcommerceRefreshConnection);

    $('.cancelEbayToken').live('click', CancelEbayToken);
    $('.cancelShopifyToken').live('click', CancelShopifyToken);
    $('.cancelMagentoToken').live('click', CancelMagentoToken);
    $('.cancelBigcommerceToken').live('click', CancelBigcommerceToken);
}

function ShowAddMoreBox() {
    $('.ebayConnect').insertAfter($(this)).hide().fadeIn(); ;
}

function SetRefreshConnection(event) {
    var accountID = $(this).prev().val();
    $('.hfSelectedEbayAccountID').val(accountID);

    $('.ebayConnect').insertAfter($(this).closest('.shopItem')).hide().fadeIn();

}

function SetShopifyRefreshConnection() {
    $('.hfSelectedShopifyAccountID').val('1');
    $('.shopifyConnect').show();
}

function SetMagentoRefreshConnection() {
    $('.hfSelectedMagentoAccountID').val('1');
    $('.magentoConnect').show();
}

function SetBigcommerceRefreshConnection() {
    $('.hfSelectedBigcommerceAccountID').val('1');
    $('.bigcommerceConnect').show();
}

function ShowMessageList() {
    var messages = $(this).closest('tr').find('.hfMessages').val();
    var messageList = JSON.parse(messages);

    $('#messageList').find('tbody').empty();
    $('#messageTemplate').tmpl(messageList).appendTo($('#messageList').find('tbody'));

    $('.messageListContainer').dialog({ modal: true, title: 'Messages From Buyer', width: '90%' });
}

function CancelEbayToken() {
    if (confirm('Are you sure you want to cancel this Ebay token?')) {
        var eBayAccountID = $(this).siblings('.hfEbayAccountID').val();
        var row = $(this).closest('.shopItem');
        XHR_RemoveAccountToken("EBAY", eBayAccountID, function (result) {
            if (result) {
                row.hide('slow', function () { row.remove(); });

                var remaining = parseInt($('.hfEbayAccountsRemaining').val());
                $('.ebayAddMore').show();
                $('.ebayAddMore').text(' Add Another Account (' + (remaining + 1) + ' Remaining)');
                ShowInfo("Token cancelled successfully.", "green")
            }
            else {
                ShowInfo("Something went wrong! Please try later.", "red")
            }
        }, null);
    }
}

function CancelShopifyToken() {
    if (confirm('Are you sure you want to cancel this Shopify token?')) {

        var row = $(this).closest('.shopifyConnectedBox');
        XHR_RemoveAccountToken("SHOPIFY", 0, function (result) {

            row.remove();
            $('.hfSelectedBigcommerceAccountID').val('0');
            $('.shopifyConnect').show('slow');

        }, null);
    }
}

function CancelMagentoToken() {
    if (confirm('Are you sure you want to disconnect your Magento account from ParcelSolutions?')) {
        var row = $(this).closest('.magentoConnectedBox');
        XHR_RemoveAccountToken("MAGENTO", 0, function (result) {

            row.remove();
            $('.magentoConnect').show('slow');

        }, null);
    }
}

function CancelBigcommerceToken() {
    if (confirm('Are you sure you want to disconnect your Bigcommerce account from ParcelSolutions?')) {
        var row = $(this).closest('.bigcommerceConnectedBox');
        XHR_RemoveAccountToken("BIGCOMMERCE", 0, function (result) {

            row.remove();
            $('.bigcommerceConnect').show('slow');

        }, null);
    }
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



