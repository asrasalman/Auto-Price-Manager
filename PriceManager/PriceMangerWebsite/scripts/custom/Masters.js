/* ----------------------- EBAY Items------------------------------ */

function SetTriggersItems() {
    $('.addItems').click(ShowAddItemsBox);
    $('.editItems').click(ShowEditItemsBox);
    $('.adjustItems').click(ShowAdjustItemsBox);
    $('.deleteItemsBulk').click(DeleteSelectedItems);
    $('.deleteItems').click(ConfirmDeleteItems);
    $('.saveItems').click(SetSaveItems);
    $('.btnAdjust').click(SetAdjustItems);
    $('.numeric').numeric();
    SetAccessRights();
    $("input:file").change(function () {
        var fileName = $(this).val();
        $(".fileName").text(fileName);
    });

    $('.uploadManifest').click(ValidateFile);
    $('#chkSelectAll').change(SetCheckboxes);
    $('.btnProcessSelectedItems').click(ValidateSelection);
}

function ShowAddItemsBox() {
    CleanForm();
    ResetValues();
    $('.lblPassword').show();
    $('.chkPassword').hide();
    $('.hfItemID').val('');
    $('.txtItemsName').val('');
    $('.ddlTaxClass option:first').prop('selected', true);
    $('.trInitialQty').show();
    $('.manageItems').dialog({ width: '500px', modal: true, title: 'Add New Item' });
    $('.ui-dialog').insertAfter('.manageItemsReference');
    $('.hfItemsCode').val('');
}

function ShowEditItemsBox() {
    CleanForm();
    
    var row = $(this).closest('.searchItem');
    var ItemID = $.trim($(row).find('.hfListItemID').val());
    var customLabel = $.trim($(row).find('.customLabel').text());
    var description = $.trim($(row).find('.description').text());
    var weight = $.trim($(row).find('.weight').text());
    var length = $.trim($(row).find('.length').text());
    var height = $.trim($(row).find('.height').text());
    var width = $.trim($(row).find('.width').text());
    var initialQty = $.trim($(row).find('.initialQty').text());
    var balanceQty = $.trim($(row).find('.balanceQty').text());
    var minimumThreshold = $.trim($(row).find('.minimumThreshold').text());
    $('.txtCustomLabel').val(customLabel);
    $('.txtDesc').val(description);
    $('.txtWeight').val(weight);
    $('.txtHeight').val(height);
    $('.txtLength').val(length);
    $('.txtWidth').val(width);
    //$('.txtInitialQty').val(initialQty);
    $('.trInitialQty').hide();
    $('.txtMinimumThreshold').val(minimumThreshold);

    $('.manageItems').dialog({ width: '500px', modal: true, title: 'Edit Items' });
    $('.ui-dialog').insertAfter('.manageItemsReference');
    $('.hfItemID').val(ItemID);
}


function SetSaveItems() {
    if (ValidatePage())
        $('.manageItems').dialog('close');
}

function SetAdjustItems() {
    if (ValidatePage())
        $('.adjustItemsmodal').dialog('close');
}

function ConfirmDeleteItems() {
    return confirm('Are you sure you want to delete the selected Items?');
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
    $('.tblItems tbody').find('input[type="checkbox"]').prop('checked', $(this).prop('checked'));
}

function ValidateSelection() {
    var count = $('.tblItems tbody').find('input[type="checkbox"]:checked').length;

    if (count == 0) {
        alert('Please select at-least one item to process for e-parcel file');
        return false;
    }
}

function DeleteSelectedItems(e) {
    var checkboxes = $('.chkSelect input:checked');
    if (checkboxes.length == 0) {
        alert('Please select atleast one item to delete');
        return false;
    }
    else {
        if (confirm('Are you sure you want to delete the selected item(s) ? ')) {

        }
        else {
            e.preventDefault();
            return false;
        }
    }
}

/* ---------------------- CHARGE CODES --------------------------- */

function SetTriggersCodes() {
    $('.addCode').click(ShowAddCodesBox);
    $('.editCode').click(ShowEditCodesBox);
    $('.deleteCode').click(ConfirmDeleteCodes);
    $('.saveCode').click(SetSaveCodes);
    SetAccessRights();
}

function ShowAddCodesBox() {
    $('.hfSelectedChargeCode').val('');
    $('.txtEbayCode, .txtChargeCode').val('');

    $('.manageCodes').dialog({ width: '500px', modal: true, title: 'Add New Code' });
    $('.ui-dialog').insertAfter('.manageCodesReference');
}

function ShowEditCodesBox() {
    var row = $(this).closest('.searchItem');
    var chargeCode = $.trim($(row).find('.hfChargeCode').val());
    var userCode = $.trim($(row).find('.hfUserCode').val());
    var chargeCodeName = $.trim($(row).find('.lblChargeCodeName').text());
    var ebayCode = $.trim($(row).find('.lblEbayCode').text());

    $('.ddlUser').val(userCode);
    $('.txtEbayCode').val(ebayCode);
    $('.txtChargeCode').val(chargeCodeName);

    $('.manageCodes').dialog({ width: '500px', modal: true, title: 'Edit Codes' });
    $('.ui-dialog').insertAfter('.manageCodesReference');
    $('.hfSelectedChargeCode').val(chargeCode);
}


function SetSaveCodes() {
    if (ValidatePage())
        $('.manageCodes').dialog('close');
}

function ConfirmDeleteCodes() {
    return confirm('Are you sure you want to delete the selected Charge Code?');
}


/* --------------------- COMMON ------------------------- */


function ValidatePage() {
    if (typeof (Page_ClientValidate) == 'function') { Page_ClientValidate(); }
    if (Page_IsValid) { return true; }
    else { return false; }
}

function CleanForm() {
    document.forms[0].reset();
    for (i = 0; i < Page_Validators.length; i++) {
        Page_Validators[i].style.visibility = 'hidden';
    }
    return false;
}

function SetDropdownOtherChoice() {
    if ($(this).find('option:contains("-OTHER-")').length == 1)
        $(this).next('input[type="text"]').show();
    else
        $(this).next('input[type="text"]').hide();
}

function CreateInsertTemplate() {
    var object = $(this).attr('obj');
    var template = $('.inlineTemplateMaster').clone().removeClass('inlineTemplateMaster');
    $(template).find('.textInline').focus().addClass('txt' + object);
    $(template).find('.saveSmall').addClass('save' + object);
    $(template).insertAfter(this).show();
    $(this).hide();
}

function CancelInsertTemplate() {
    HideTemplate(this);
}

function HideTemplate(obj) {
    var template = $(obj).closest('.inlineTemplate');
    $(template).siblings('.addSmallInline').show();
    $(template).remove();
}

function SetAccessRights() {
//    if ($('#hfIsAdmin').val() == 'false' && $('#hfCanAddUpdate').val() == 'false') {
//        $('.editFunction').remove();
//    }
}

function ResetValues() {
    $('.txtCustomLabel').val('');
    $('.txtDesc').val('');
    $('.txtWeight').val('');
    $('.txtHeight').val('');
    $('.txtLength').val('');
    $('.txtWidth').val('');
    $('.txtInitialQty').val('');
    $('.txtMinimumThreshold').val('');
    
}

function ShowAdjustItemsBox() {
    CleanForm();
    var row = $(this).closest('.searchItem');
    var ItemID = $.trim($(row).find('.hfListItemID').val());
    var balanceQty = $.trim($(row).find('.balanceQty').text());
    if (!balanceQty)
        balanceQty = 0;
    $("#rdbAddition, #rdbRemoval").removeAttr("checked");
    $("#rdbAddition").attr("checked", "checked");
    $('#spanCurrentQty').text("Current Qty : " + balanceQty);
    $("#spanNewQty").text("").hide();
    $("#errortxtAdjustQty").text("");
    $("#errortxtAdjustNarration").text("");
    $('.txtAdjustNarration').val('');
    $('.txtAdjustQty').val('');
    $('.hfAdjustItemID').val(ItemID);
    $('.adjustItemsmodal').dialog({ width: '500px', modal: true, title: 'Adjust Item' });
    //$('.adjustItemsmodal').parent().appendTo($("form:first"));
    $('.ui-dialog').insertAfter('.manageItemsReference');
   
}
