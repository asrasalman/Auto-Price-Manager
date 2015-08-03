$(function () {

    // Initialize validation on the entire ASP.NET form.
    $("form").validate({
        // This prevents validation from running on every
        //  form submission by default.
        onsubmit: false,
        onkeyup: false,
        onblur: false,
        onfocusout: false,
        validClass: "validated",
        success: "validated",
        errorClass: "error",
        ignoreTitle: true,
        showErrors: function (errorMap, errorList) {
            for (var i = 0; i < errorList.length; i++) {
                //display required field errors in seperate div
                if (errorList[i].message == "This field is required.") {
                    var container = $(errorList[i].element).parents(".validationGroup");
                    $(errorList[i].element).addClass("error");
                    var errorBox = container.find(".errorbox");
                    if (errorBox.find(".errorboxinnerbox").length == 0)
                        errorBox.prepend("<div class=\"errorboxinnerbox\"><strong>There was a problem with the details you entered, please see below for more information:</strong><ul></ul></div>");

                    var elementName = errorList[0].element.title || errorList[0].element.name;
                    elementName = elementName.replace("ctl00$ContentPane$", "");
                    if (errorBox.find(".errorboxinnerbox ul").html().indexOf("<li id=\"required" + errorList[0].element.name + "\">" + elementName + " required.</li>") == -1)
                        errorBox.find(".errorboxinnerbox ul").append("<li id=\"required" + errorList[0].element.name + "\">" + elementName + " required.</li>");
                    errorBox.slideDown();
                }
                else
                    this.defaultShowErrors(); //display other messages as normal
            }
        },

        errorPlacement: function (error, element) {
            // Set positioning based on the elements position in the form
            var elem = $(element);
            // Check we have a valid error message
            if (!error.is(':empty')) {
                // Apply the tooltip only if it isn't valid
                elem.filter(':not(.valid)').qtip({
                    overwrite: true,
                    content: error,
                    position: {
                        my: 'bottom center',
                        at: 'top center',
                        viewport: $(window)
                    },
                    show: {
                        event: false,
                        ready: true
                    },
                    hide: false,
                    style: {
                        classes: 'ui-tooltip-red ui-tooltip-rounded' // Make it red... the classic error colour!
                    }
                })

                // If we have a tooltip on this element already, just update its content
               .qtip('option', 'content.text', error);
            }

            // If the error is empty, remove the qTip
            else { elem.qtip('destroy'); }
        }
    });

    $.validator.addMethod("RegexCountryMobileNumber", function (value, element, expression) {
        if (expression == null) return true;
        var regex = new RegExp(expression);
        return regex.test($.trim(value));
    }, "Please enter a valid mobile number");

    $.validator.addMethod("RegexPostcodeCountryMatch", function (value, element, expression) {
        if (expression == null) return true;
        var regex = new RegExp(expression);
        return regex.test($.trim(value));
    }, "This postcode is an invalid format for the destination country.");

    $.validator.addMethod("NotPOBox", function (value, element) {
        return (new RegExp("^((?![Pp]\.?\s*[Oo]\.?\s*[Bb][Oo][Xx]).)*$")).test(value);
    }, "Sorry we do not deliver to/from Post Office Boxes.");

    $.validator.addMethod("NotBFPO", function (value, element) {
        return (new RegExp("^((?![Bb][Ff][Pp][Oo]).)*$")).test(value);
    }, "Sorry we do not deliver to/from BFPO Addresses.");

    $.validator.addMethod("NotPersonalEffect", function (value, element) { // for international parcels, customs need better description
        return (new RegExp("^((?!person[al][el].*?effect).)*$")).test(value);
    }, "The description personal effects is not allowed. Please include more details.");

    // Search for controls marked with the causesValidation flag 
    //  that are contained anywhere within elements marked as 
    //  validationGroups, and wire their click event up.
    $('.validationGroup .causesValidation').live("click", ValidateAndSubmit);

    // Select any input[type=text] elements within a validation group
    //  and attach keydown handlers to all of them.
    $('.validationGroup :text').live("keydown", function (evt) {
        // Only execute validation if the key pressed was enter.
        if (evt.keyCode == 13) {
            ValidateAndSubmit(evt);
        }
    });

    //validate field when focus lost
    $('.validationGroup :text, .validationGroup select, .validationGroup input').live("blur", function () { ValidateThis(this) });

});

function ValidateThis(element) {

    // Ascend from the button that triggered this click event 
    //  until we find a container element flagged with 
    //  .validationGroup and store a reference to that element.
    var $group = $(element).parents('.validationGroup');

    //hide existing bubble

    window.setTimeout(function () {

        if (element.type == "checkbox") return;

        //remove existing bubble
        $(element).qtip('destroy');

        //remove any required field messages
        $group.find(".errorbox ul li").each(function () {
            if (this.id == "required" + element.name)
                $(this).remove();
        });

        //hide error box if no required field messages
        if ($group.find(".errorbox ul li").length > 0)
            $group.find(".errorbox").slideDown();
        else
            $group.find(".errorbox").slideUp();

        //validate field
        if ($(element).val() == "") return;
        if ($(element).valid()) {
            $(element).removeClass("error");
            $(element).addClass("validated");
        }

    }, 400);
}

function ValidateAll() {
    // Clear all errors
    $(".errorBox").slideUp();
    $(".errorboxinnerbox ul").html("");
    $("input,select,textarea").removeClass("error");

    var isValid = true;
    var firstError = null;
    // Validate whole page
    $(".validationGroup:visible").each(function () {
        $(this).find("input:visible,select:visible,textarea:visible").each(function () {
            $(this).qtip('destroy');
            if (this.type == "checkbox" && $(this).is(".required"))
                isValid = this.checked;
            if (this.type == "checkbox" && !$(this).is(".required")) return;
            if ((!$(this).is(".required")) && ($(this).val() == "")) return;
            if (!$(this).valid()) {
                isValid = false;
                if (firstError == null)
                    firstError = $(this);
            }
        });
    });

    if (firstError != null) firstError.focus();
    if ($("body").find(".errorbox:visible").length > 0) {
        $('html,body').animate('slow');
    }
    return isValid;
}

function ValidateAndSubmit(evt) {
    // Ascend from the button that triggered this click event 
    //  until we find a container element flagged with 
    //  .validationGroup and store a reference to that element.
    var $group = $(evt.currentTarget).parents('.validationGroup');
    
    //clear summary
    $group.find(".errorbox").slideUp();
    $group.find(".errorboxinnerbox ul").html("");

    //clear all error highlighting
    $group.find("input:visible,select:visible,textarea:visible").removeClass("error");

    var isValid = true;

    // Descending from that .validationGroup element, find any input
    //  elements within it, iterate over them, and run validation on 
    //  each of them.
    $group.find("input:visible,select:visible,textarea:visible").each(function (i, item) {
        $(item).qtip('destroy');
        if ((!$(item).is(".required")) && ($(item).val() == "")) return;
        if (!$(item).valid()) {
            isValid = false;
            console.log(i + " false" );
        }

    });

    // If any fields failed validation, prevent the button's click 
    //  event from triggering form submission.
    if (!isValid) {
        evt.preventDefault();
        if ($group.find(".errorbox:visible").length > 0) {
            $('html,body').animate('slow');
        }
    }
    else {
        SavePricingDetails();
    }

    
}