var _DateFormat = 'MMM dd,yyyy';

function ToggleLoader(show) {
    if (show == true)
        $('.loadingContainer').parent().show();
    else
        $('.loadingContainer').parent().hide();
}

function GetAddressType(type) {
    if (type == "1")
        return "Billing";
    else if (type == "2")
        return "Shipping";
    else
        return "";
}

function GetDate(jsonDate) {
    if (jsonDate != '' && jsonDate != null) {
        var value = new Date(jsonDate);
        date = new Date(value);
        return date.format(_DateFormat);
    }
    else
        return '';
}


function VerifyPagingLinks() {
    if ($('.dropdownPage option:selected').index() == 0 || $('.dropdownPage option:selected').index() == -1)
        $('.aPrev').css('visibility', 'hidden');
    else
        $('.aPrev').css('visibility', 'visible');
    if ($('.dropdownPage option:selected').index() == parseInt($('.dropdownPage option').size()) - 1)
        $('.aNext').css('visibility', 'hidden');
    else
        $('.aNext').css('visibility', 'visible');
}

function ResetPage() {
    $('.dropdownPage').val("1");
}

function PrevPage() {
    $('.dropdownPage').val(parseInt($('.dropdownPage').val()) - 1);
    GetCustomerSearchResult(true, '');
    VerifyPagingLinks();
}

function NextPage() {
    $('.dropdownPage').val(parseInt($('.dropdownPage').val()) + 1);
    GetCustomerSearchResult(true, '');
    VerifyPagingLinks();
}

$.extend($.expr[':'], {
    containsExact: function (a, i, m) {
        return $.trim(a.innerHTML.toLowerCase()) === m[3].toLowerCase();
    },
    containsExactCase: function (a, i, m) {
        return $.trim(a.innerHTML) === m[3];
    },
    containsRegex: function (a, i, m) {
        var regreg = /^\/((?:\\\/|[^\/])+)\/([mig]{0,3})$/,
                reg = regreg.exec(m[3]);
        return RegExp(reg[1], reg[2]).test($.trim(a.innerHTML));
    }
})