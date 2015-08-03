// Login Form

$(function () {
    var button = $('.btnLogin');
    var box = $('#loginBox');
    var form = $('#loginForm');
    button.removeAttr('href');
    button.mouseup(function (login) {

        box.toggle();
        button.toggleClass('active');
    });
    form.mouseup(function () {
        return false;
    });
    //    $(this).mouseup(function (login) {
    //        alert($(login.target).parent('#loginButton').att('id')      );
    //        if (!($(login.target).parent('#loginButton').length > 0)) {
    //            button.removeClass('active');
    //            box.hide();
    //        }
    //    });
});
