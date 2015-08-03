function SetTriggersUser() {
    $('.addUser').click(ShowAddUserBox);
    $('.editUser').live('click', ShowEditUserBox);
    $('.deleteUser').live('click', ConfirmDeleteUser);
    $('.saveUser').live('click', SetSaveUser);
    $('.chkPassword input').change(TogglePasswordEnable);
    SetAccessRights();
}

function ShowAddUserBox() {
    CleanForm();
    $('.lblPassword').show();
    $('.chkPassword').hide();

    $('.txtUserName').val('');
    $('.ddlRole option:first').prop('selected', true);
    $('.txtEmailAddress').val('');

    validator1 = document.getElementById($('.rfvPassword').attr('id'));
    validator2 = document.getElementById($('.rfvPasswordConfirm').attr('id'));
    ValidatorEnable(validator1, true);
    ValidatorEnable(validator2, true);

    $('.manageUser').dialog({ width: '500px', modal: true, title: 'Add New User' });
    $('.ui-dialog').insertAfter('.manageUserReference');
    $('.hfUserCode').val('');
}

function ShowEditUserBox() {
    CleanForm();
    var row = $(this).closest('.searchItem');
    var fullName = $.trim($(row).find('.fullName').text());
    var emailAddress = $.trim($(row).find('.emailAddress').text());
    var roleName = $.trim($(row).find('.roleName').text());
    var userCode = $.trim($(row).find('.hfListUserCode').val());
    var packageId = $.trim($(row).find('.hfPackageId').val());

    $('.txtUserName').val(fullName);
    $('.ddlRole option:contains("' + roleName + '")').prop('selected', true);
    $('.ddlPackage option[value="' + packageId + '"]').prop('selected', true);
    $('.txtEmailAddress').val(emailAddress);

    $('.lblPassword').hide();
    $('.chkPassword').show();
    $('.chkPassword input').prop('checked', false);

    validator1 = document.getElementById($('.rfvPassword').attr('id'));
    validator2 = document.getElementById($('.rfvPasswordConfirm').attr('id'));
    ValidatorEnable(validator1, false);
    ValidatorEnable(validator2, false);

    $('.manageUser').dialog({ width: '500px', modal: true, title: 'Edit User Details' });
    $('.ui-dialog').insertAfter('.manageUserReference');
    $('.hfUserCode').val(userCode);
    $('.passwordContainer').hide();
}


function SetSaveUser() {
    if (ValidatePage())
        $('.manageUser').dialog('close');
}

function ValidatePage() {
    if (typeof (Page_ClientValidate) == 'function') { Page_ClientValidate(); }
    if (Page_IsValid) { return true; }
    else { return false; }
}

function TogglePasswordEnable() {
    if ($(this).prop('checked') == true) {
        $('.passwordContainer').show();
        validator1 = document.getElementById($('.rfvPassword').attr('id'));
        validator2 = document.getElementById($('.rfvPasswordConfirm').attr('id'));
        ValidatorEnable(validator1, true);
        ValidatorEnable(validator2, true);
    }
    else {
        $('.passwordContainer').hide();
        validator1 = document.getElementById($('.rfvPassword').attr('id'));
        validator2 = document.getElementById($('.rfvPasswordConfirm').attr('id'));
        ValidatorEnable(validator1, false);
        ValidatorEnable(validator2, false);
    }
}

function CleanForm() {
    document.forms[0].reset();
    for (i = 0; i < Page_Validators.length; i++) {
        Page_Validators[i].style.visibility = 'hidden';
    }
    return false;
}

function ConfirmDeleteUser() {
    return confirm('Are you sure you want to delete the selected user?');
}


//----------------- ------------------------ //

function SetTriggersRole() {
    $('.addRole').click(ShowAddRoleBox);
    $('.editRole').click(ShowEditRoleBox);
    $('.deleteRole').click(ConfirmDeleteRole);
    $('.saveRole').click(SetSaveRole);
    $('.chkPassword input').change(TogglePasswordEnable);
    SetAccessRights();
}

function ShowAddRoleBox() {
    CleanForm();
    $('.lblPassword').show();
    $('.chkPassword').hide();

    $('.txtRoleName').val('');
    $('.ddlRole option:first').prop('selected', true);
    $('.txtEmailAddress').val('');

    $('.manageRole').dialog({ width: '500px', modal: true, title: 'Add New Role' });
    $('.ui-dialog').insertAfter('.manageRoleReference');
    $('.hfRoleCode').val('');
}

function ShowEditRoleBox() {
    CleanForm();
    var row = $(this).closest('.searchItem');
    var roleName = $.trim($(row).find('.roleName').text());
    var roleCode = $.trim($(row).find('.hfListRoleCode').val());

    $('.txtRoleName').val(roleName);

    $('.manageRole').dialog({ width: '500px', modal: true, title: 'Edit Role Details' });
    $('.ui-dialog').insertAfter('.manageRoleReference');
    $('.hfRoleCode').val(roleCode);
}


function SetSaveRole() {
    if (ValidatePage())
        $('.manageRole').dialog('close');
}

function ConfirmDeleteRole() {
    return confirm('Are you sure you want to delete the selected role?');
}

function SetAccessRights() {
//    if ($('#hfIsAdmin').val() == 'false' && $('#hfCanAddUpdate').val() == 'false') {
//        $('.editFunction').remove();
//    }
}


//----------------- ------------------------ //

function GetAccessDetailsForPage(event) {

    // make li selected

    if ($(this).attr('custom1') == 'true') {

        $('.liSelected').removeClass('liSelected');
        $(this).addClass('liSelected');

        var roleCode = $('.ddlRole').val();
        var menuItemCode = $(this).attr('id');
        var service = new CustomerServices.CustomerSvc();
        service.GetRolePageAccess(roleCode, menuItemCode, function (result) {

            result = JSON.parse(result);
            $('#tblAccess').find('input[type="radio"]').prop('checked', false);
            $('#tblAccess').hide().fadeIn('slow');
            if (result != null) {
                var accessFunctionCode = result.Access_Function_Code;


                if (accessFunctionCode != null && accessFunctionCode != '') {
                    $('#tblAccess').find('.hfAccessFunctionCode[value="' + accessFunctionCode + '"]').closest('tr').find('input[type="radio"]').prop('checked', true)
                }

            }
        }, null, null);
    }
    event.preventDefault();
    return false;
}

function SaveAccessDetailsForPage(event) {
    
    var roleCode = $('.ddlRole').val();
    var menuItemCode = $('.liSelected').attr('id');
    var accessFunctionCode = $('#tblAccess').find('input[type="radio"]:checked').closest('tr').find('.hfAccessFunctionCode').val();

    var service = new CustomerServices.CustomerSvc();
    service.SavePageRoleAccess(roleCode, menuItemCode, accessFunctionCode, function (result) {
        alert('Access saved for selected page');
    }, null, null);
    event.preventDefault();
    return false;
}
