var regexPincode = "^[1-9][0-9]{5}$"; 		//"^([1-9][0-9]{5}|[1-9]{1}[0-9]{2}\s[0-9]{3})$"
var regexEmail = "";
var regexPhone = "";
var regexMobile = "";
var regexNumOnly = "^[0-9]+$";
var regexDecimal2 = "^[0-9]+\.[0-9]{2}$";
var regexDecimalAny = "^[0-9]+\.[0-9]+$";
var regexAlphaOnly = "^[A-Za-z]+$";
var regexAlphaOnlyWithSpace = "^[A-Za-z ]+$";
var regexAlphaNumeric = "^[A-Za-z0-9]+$";
var regexAlphaNumericWithSpace = "^[A-Za-z0-9 ]+$";
var regexLowAscii = "^[ -~]+$";
var regexLowAsciiExt = "^[ -~\t\n\r]+$";
var regexPAN = "^[A-Za-z]{3}[P|p][A-Za-z][0-9]{4}[A-Za-z]$";

var MandatoryFieldMsg = "This field is required";
var ValidEmailIdMsg = "Please enter valid email id";
var ValidPhoneMsg = "Please enter valid phone number";
var ValidMobileMsg = "Please enter valid mobile number";
var ValidPincodeMsg = "Please enter valid Pincode number";
var ComboNotSelected = "Please select from list";


/*
jQuery.validator.addMethod(
    "check_exp",
    function (value, element, regexp) {
        if (regexp.constructor != RegExp)
            regexp = new RegExp(regexp);
        else if (regexp.global)
            regexp.lastIndex = 0;
        return this.optional(element) || regexp.test(value);
    }, ""
);*/

