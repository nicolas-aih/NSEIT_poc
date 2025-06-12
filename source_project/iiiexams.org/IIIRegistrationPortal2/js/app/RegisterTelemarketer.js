$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

    var validator = $("#frmMain").validate(
        {
            //onfocusout: false,
            //onkeyup: false,
            ignore: [],
            rules: {
                
                txtName: {
                    required: true,
                    check_exp: regexAlphaNumericWithSpace,
                },
                txtTraiRegNo: {
                    required: true
                },
                cboIsActive: {
                    required :true
                },
                txtAddress:
                {
                    required: true,
                    check_exp: regexLowAscii
                },
                txtCPName: {
                    required: true,
                    check_exp: regexAlphaOnlyWithSpace
                },
                txtCPEmailId: {                   
                    required: true,
                    email: true,
                    required: true
                },
                txtCPContactNo: {
                    required: true,
                    digits: true,
                    minlength: 10,
                    maxlength: 10
                },
                txtDPName: {
                    required: true,
                    check_exp: regexAlphaOnlyWithSpace
                },
                txtDPEmailId: {
                    required: true,
                    email: true
                },
                txtDPContactNo: {
                    required: true,
                    digits: true,
                    minlength: 10,
                    maxlength: 10
                }
            },
            messages: {
                txtName: {
                    required: MandatoryFieldMsg,
                    check_exp: "Only alphabets and number with space, without leading / trailing space are allowed",
                },
                txtTraiRegNo: {
                    required: MandatoryFieldMsg,
                },
                cboIsActive: {
                    required: MandatoryFieldMsg
                },
                txtAddress: {
                    required: MandatoryFieldMsg,
                    check_exp: "Invalid character found"
                },                
                txtCPName: {           
                    required: MandatoryFieldMsg,
                    check_exp: "Only alphabets and space, without leading / trailing space are allowed",
                },
                txtCPEmailId: {
                    required: MandatoryFieldMsg,
                    email: ValidEmailIdMsg
                },
                txtCPContactNo: {
                    required: MandatoryFieldMsg,
                    digits: ValidMobileMsg,
                    minlength: ValidMobileMsg
                },
                txtDPName: {
                    required: MandatoryFieldMsg,
                    check_exp: "Only alphabets and space, without leading / trailing space are allowed",
                },
                txtDPEmailId: {
                    required: MandatoryFieldMsg,
                    email: ValidEmailIdMsg
                },
                txtDPContactNo: {
                    required: MandatoryFieldMsg,
                    digits: ValidMobileMsg,
                    minlength: ValidMobileMsg
                },
            },
            invalidHandler: function (event, validator) {
                alert("Unable to save data. Some of the fields have errors (The same are highlighted).");
            },
            submitHandler: function (form) {                

                var data = new FormData(form);
                var x = '';
                $.ajax({
                    type: "POST",
                    enctype: 'multipart/form-data',
                    url: "../telemarketer/RegisterTelemarketer",
                    data: data,
                    processData: false,
                    contentType: false,
                    cache: false,
                    success: function (msg) {
                        //debugger;
                        var s = '';
                        var Result = JSON.parse(msg);                        
                        if (Result._STATUS_ == "SUCCESS") {
                            form.reset();
                            alert(Result._MESSAGE_);
                        }
                        else {
                            alert(Result._MESSAGE_);
                        }
                    },
                    error: function (msg) {
                        HandleAjaxError(msg);
                    }
                });
            }
        });
});