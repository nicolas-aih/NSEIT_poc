$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

    $("#frmMain").validate({
        rules:
        {
            txtCompanyName: {
                required: true,
                check_exp: regexLowAscii
            },
            txtAddress: {
                required: true,
                check_exp: regexLowAscii
            },
            txtSTDCode: {
                required: true,
                digits: true,
            },
            txtLandlineNO: {
                required: true,
                digits: true,
            },
            txtMobilePO: {
                required: true,
                digits: true,
            },
            txtEmailPO: {
                required: true,
                email: true,
            },
            txtNamePO: {
                required: true,
                check_exp: regexAlphaOnlyWithSpace,
            }
        },
        messages:
        {
            txtCompanyName: {
                required: MandatoryFieldMsg,
                check_exp: "Please enter valid company name"
            },
            txtAddress: {
                required: MandatoryFieldMsg,
                check_exp: "Please enter valid address"
            },
            txtSTDCode: {
                required: MandatoryFieldMsg,
                digits: "Please enter valid STD Code. STD Code should be a numeric",
            },
            txtLandlineNO: {
                required: MandatoryFieldMsg,
                digits: "Please enter valid landline number. landline number should be numeric",
                maxlength: ""
            },
            txtMobilePO: {
                required: MandatoryFieldMsg,
                digits: "Please enter valid mobile number. Mobile number should be numeric",
                maxlength: ""
            },
            txtEmailPO: {
                required: MandatoryFieldMsg,
                email: "Please enter valid email id",
            },
            txtNamePO: {
                required: MandatoryFieldMsg,
                check_exp: "Please enter valid name. Name may contain alphabets and space"
            }
        },
        submitHandler: function (Form) {
            var _CompanyType = $("#cboCompanyType").val();
            var _CompanyName = $("#txtCompanyName").val();
            var _CompanyAddress = $("#txtAddress").val();
            var _STDCode = $("#txtSTDCode").val();
            var _Landline = $("#txtLandlineNO").val();
            var _Mobile = $("#txtMobilePO").val();
            var _EMailId = $("#txtEmailPO").val();
            var _POName = $("#txtNamePO").val();

            var JsonObject = {
                CompanyType: _CompanyType, CompanyName: _CompanyName, CompanyAddress: _CompanyAddress,
                STDCode: _STDCode, Landline: _Landline, Mobile: _Mobile, EmailId: _EMailId, POName: _POName
            };
            $.ajax({
                type: "POST",
                url: "../Users/Loginrequest",
                data: JSON.stringify(JsonObject),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    //debugger;
                    var retval = JSON.parse(msg);
                    $("#data").html('');
                    if (retval._STATUS_ == "SUCCESS") {
                        s = "<span>" + retval._MESSAGE_ + "</span>";
                    }
                    else {
                        s = "<span class='error'>" + retval._MESSAGE_ + "</span>";
                    }
                    $("#data").html(s);
                    //alert(msg);
                },
                error: function (msg) {
                    HandleAjaxError(msg);
                }
            });
        }
    });
});
