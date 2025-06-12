var regexPincode = "^[1-9][0-9]{5}$"; 		//"^([1-9][0-9]{5}|[1-9]{1}[0-9]{2}\s[0-9]{3})$"
var regexEmail = "";
var regexPhone = "";
var regexMobile = "";
var regexNumOnly = "^[0-9]+$";
var regexDecimal2 = "^[0-9]+\.[0-9]{2}$";
var regexDecimalAny = "^[0-9]+\.[0-9]+$";
var regexAlphaOnly = "^[A-Za-z]+$"; //OK
var regexAlphaNumeric = "^[A-Za-z0-9]+$"; // "^\w+$" will also work

var regexAlphaOnlyWithSpace = "^[a-zA-Z]+( +[a-zA-Z]+)*$"; //Start and end with alpha, can contain only space in between.
var regexAlphaNumericWithSpace = "^[a-zA-Z0-9]+( +[a-zA-Z0-9]+)*$"; //Start and end with alpha / num, can contain only space in between. "^\w+([ \w]+\w+)*$" should work too.
var regexLowAscii = "^[!-~]+( +[!-~]+)*$"; //Start and end with alpha / num, can contain any character in between.

var regexLowAsciiExt = "^[!-~]+(\s+[!-~]+)*$";

var regexPAN = "^[A-Za-z]{3}[P|p][A-Za-z][0-9]{4}[A-Za-z]$";
var regexLowAsciiLtd = "^[A-Za-z0-9 \(\)\[\]\-:;,.\"'\/\\]+"

var MandatoryFieldMsg = "This field is required";
var ValidEmailIdMsg = "Please enter valid email id";
var ValidPhoneMsg = "Please enter valid phone number. Expected : number";
var ValidFaxMsg = "Please enter valid phone number. Expected : number";
var ValidMobileMsg = "Please enter valid mobile number. Expected : 10 digit number";
var ValidAadhaarMsg = "Please enter valid Aadhaar number. Expected : 12 digit number";
var ValidPincodeMsg = "Please enter valid Pincode number. Expected : 6 digit number";
var ComboNotSelected = "Please select from list"
var JunkCharMessage = "Invalid input. Entered text contains special characters OR leading / trailling spaces.";
var NO_DATA_FOUND = "No data found";