$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

    $.ajax({
        type: "POST",
        url: "../Users/GetProfile",
        //data: JSON.stringify(JsonObject),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var result = JSON.parse(msg);
            if (result._STATUS_ == 'FAIL') {
                alert(result._MESSAGE_);
            }
            else {
                var data = JSON.parse(result._DATA_);
                if (data.length == undefined || data.length == 0) {
                    alert("Details Not Found");
                }
                else {
                    $("#cboCompanyType").val(data[0].company_type)
                    $("#txtCompanyName").val(data[0].varUserName)
                    $("#txtAddress").val(data[0].Address);
                    $("#txtSTDCode").val(data[0].varSTDCode);
                    $("#txtLandlineNo").val(data[0].PhoneNumber);
                    $("#txtMobilePO").val(data[0].varMobileNo);
                    $("#txtEmailPO").val(data[0].varEmailID);
                    $("#txtNamePO").val(data[0].ContactPersonName);

                }
            }
        },
        error: function (msg) {
            HandleAjaxError(msg);
        }
    });

    $("#frmMain").validate({
        rules:
        {
            txtAddress: {
                required: true,
                check_exp: regexLowAscii
            },
            txtSTDCode: {
                required: true,
                digits: true,
                minlength : 2
            },
            txtLandlineNo: {
                required: true,
                digits: true,
            },
            txtMobilePO: {
                required: true,
                digits: true,
                minlength: 10
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
            txtAddress: {
                required: MandatoryFieldMsg,
                check_exp: JunkCharMessage
            },
            txtSTDCode: {
                required: MandatoryFieldMsg,
                digits: "Please enter valid STD Code. STD Code should be a numeric",
                minlength: "STD Code must be mimimum 2 digits in length"
            },
            txtLandlineNo: {
                required: MandatoryFieldMsg,
                digits: "Please enter valid landline number. landline number should be numeric",
            },
            txtMobilePO: {
                required: MandatoryFieldMsg,
                digits: "Please enter valid mobile number. Mobile number should be numeric",
                minlength : "Mobile number should be minimum 10 digits long"
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
            var _Address1 = $("#txtAddress").val();
            var _Pincode = $("#txtPincode").val();
            var _STDCode = $("#txtSTDCode").val();
            var _PhoneNo = $("#txtLandlineNo").val();
            var _MobileNo = $("#txtMobilePO").val();
            var _EMailId = $("#txtEmailPO").val();
            var _POName = $("#txtNamePO").val();

            var JsonObject = {
                Address1: _Address1,
                POName: _POName,
                EMailId: _EMailId,
                MobileNo: _MobileNo,
                STDCode: _STDCode,
                PhoneNo: _PhoneNo,
            };
            //debugger;
            $.ajax({
                type: "POST",
                url: "../Users/SaveProfile",
                data: JSON.stringify(JsonObject),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var Result = JSON.parse(msg);
                    if (Result._STATUS_ == "SUCCESS") {
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