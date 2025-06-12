function readURL(input, target) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            target.attr('src', e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    }
}

jQuery.validator.addMethod(
    "check_exp",
    function (value, element, regexp) {
        if (regexp.constructor != RegExp)
            regexp = new RegExp(regexp);
        else if (regexp.global)
            regexp.lastIndex = 0;
        return this.optional(element) || regexp.test(value);
    }, ""
);

$.validator.addMethod('filesize', function (value, element, param) {
    return this.optional(element) || (element.files[0].size <= param)
}, '');

jQuery.validator.addMethod(
    "number_less_than",
    function (value, element, compareto) {
        return this.optional(element) || value < $(compareto).val();
    }, ""
);

jQuery.validator.addMethod(
    "number_more_than",
    function (value, element, compareto) {
                return this.optional(element) || parseFloat(value) > parseFloat(compareto);
    }, ""
);


jQuery.validator.addMethod(
    "number_lessOrQeual",
    function (value, element, compareto) {
        return this.optional(element) || value <= $(compareto).val();
    }, ""
);

jQuery.validator.addMethod(
    "number_MoreOrEqual",
    function (value, element, compareto) {
        return this.optional(element) || value >= $(compareto).val();
    }, ""
);

jQuery.validator.addMethod(
    "NotEqualTo",
    function (value, element, compareto) {
        return this.optional(element) || value != $(compareto).val();
    }, ""
);

function clearValidation(formElement) {
    //Internal $.validator is exposed through $(form).validate()
    var validator = $(formElement).validate();
    //Iterate through named elements inside of the form, and mark them as error free
    $('[name]', formElement).each(function () {
        validator.successList.push(this);//mark as error free
        validator.showErrors();//remove error messages if present
    });
    validator.resetForm();//remove error class on name elements and clear history
    validator.reset();//remove all error and success data
}

function HandleAjaxError(msg) {
    if (msg.status == 401 /*&& msg.responseText == "USER_SESSION_EXPIRED"*/) {
        window.location = "../Home/Relogin";
    }
    else {
        alert(msg);
    }
}