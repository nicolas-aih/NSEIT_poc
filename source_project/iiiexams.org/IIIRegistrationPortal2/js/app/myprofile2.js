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
            debugger;
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
                    $("#txtLoginId").val(data[0].loginId);
                    $("#txtName").val(data[0].Name);
                    $("#txtInsurer").val(data[0].Insurer);
                    $("#txtDP").val(data[0].DP);
                    $("#txtAddress1").val(data[0].House);
                    $("#txtAddress2").val(data[0].Street);
                    $("#txtAddress3").val(data[0].Town);
                    $("#txtPincode").val(data[0].Pincode);
                    $("#txtEmailId").val(data[0].EmailId);
                    $("#txtPhoneOffice").val(data[0].TelOffice);
                    $("#txtPhoneRes").val(data[0].TelResidence);
                    $("#txtMobile").val(data[0].mobile);
                    $("#txtFAX").val(data[0].Fax);
                    $("#cboStates").val(data[0].StateId);
                    $("#cboStates").trigger("change", [data[0].DistrictId]);

                }
            }
        },
        error: function (msg) {
            debugger;
            HandleAjaxError(msg);
        }
    });

    $("#cboStates").change(function (event, DistrictId) {
        debugger;
        $("#cboDistricts").html('');
        var _StateId = $("#cboStates option:selected").val();
        var JsonObject = {
            StateId: _StateId,
        };
        $.ajax({
            type: "POST",
            url: "../Home/GetDistrictsForStates",
            data: JSON.stringify(JsonObject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                debugger;
                var s = '';
                var result = JSON.parse(msg);
                if (result._STATUS_ == 'FAIL') {
                    alert(result._MESSAGE_);
                }
                else {
                    var data = JSON.parse(result._DATA_);
                    if (data.length == undefined || data.length == 0) {
                        alert("No districts were found for selected state");
                    }
                    else {
                        s += "<option value=''>-- Select --</option>"
                        for (i = 0; i < data.length; i++) {
                            s += "<option value=" + data[i].district_id + ">" + data[i].district_name + "</option>"
                        }
                        $("#cboDistricts").html(s);
                        if (DistrictId != undefined) {
                            $("#cboDistricts").val(DistrictId);
                        }
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        });
    })

    $("#frmMain").validate({
        rules:
        {
            txtAddress1: {
                required: true,
                check_exp: regexLowAscii
            },
            txtAddress2: {
                required: true,
                check_exp: regexLowAscii
            },
            txtAddress3: {
                required: true,
                check_exp: regexLowAscii
            },
            cboStates: {
                required: true,
            },
            cboDistricts: {
                required: true,
            },
            txtPincode: {
                required: true,
                digits: true,
                minlength:6
            },
            txtEmailId: {
                required: true,
                email: true
            },
            txtPhoneOffice: {
                digits: true
            },
            txtPhoneRes: {
                digits: true
            },
            txtMobile: {
                required: true,
                digits: true
            },
            txtFAX: {
                digits: true
            }
        },
        messages:
        {
            txtAddress1: {
                required: MandatoryFieldMsg,
                check_exp: JunkCharMessage
            },
            txtAddress2: {
                required: MandatoryFieldMsg,
                check_exp: JunkCharMessage
            },
            txtAddress3: {
                required: MandatoryFieldMsg,
                check_exp: JunkCharMessage
            },
            cboStates: {
                required: MandatoryFieldMsg,
            },
            cboDistricts: {
                required: MandatoryFieldMsg,
            },
            txtPincode: {
                required: MandatoryFieldMsg,
                digits: ValidPincodeMsg,
                minlength: ValidPincodeMsg
            },
            txtEmailId: {
                required: MandatoryFieldMsg,
                email: "Please enter valid email id"
            },
            txtPhoneOffice: {
                digits: "Please enter valid phone number. Phone number should be a numeric"
            },
            txtPhoneRes: {
                digits: "Please enter valid phone number. Phone number should be a numeric"
            },
            txtMobile: {
                required: MandatoryFieldMsg,
                digits: "Please enter valid mobile phone number. Phone number should be a numeric"
            },
            txtFAX: {
                digits: "Please enter valid fax number. Fax number should be a numeric"
            }
        },
        submitHandler: function (Form) {
            //String Address1, String Address2, String Address3, Int32 Pincode,
            //String TelephoneOffice, String TelephoneResidence, String Fax,
            //String POName, String EMailId, String MobileNo, String STDCode, String PhoneNo,
            //Int32 DistrictId
            var _Address1 = $("#txtAddress1").val();
            var _Address2 = $("#txtAddress2").val();
            var _Address3 = $("#txtAddress3").val();
            var _Pincode = $("#txtPincode").val();
            var _TelephoneOffice = $("#txtPhoneOffice").val();
            var _TelephoneResidence = $("#txtPhoneRes").val();
            var _Fax = $("#txtFAX").val();
            var _EMailId = $("#txtEmailId").val();
            var _DistrictId = $("#cboDistricts option:selected").val();
            var _MobileNo = $("#txtMobile").val();

            var JsonObject = {
                Address1: _Address1,
                Address2: _Address2,
                Address3: _Address3,
                Pincode: _Pincode,
                TelephoneOffice: _TelephoneOffice,
                TelephoneResidence: _TelephoneResidence,
                Fax: _Fax,
                EMailId: _EMailId,
                DistrictId: _DistrictId,
                Mobile: _MobileNo
            };
            $.ajax({
                type: "POST",
                url: "../Users/UpdateProfile2",
                data: JSON.stringify(JsonObject),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    //debugger;
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