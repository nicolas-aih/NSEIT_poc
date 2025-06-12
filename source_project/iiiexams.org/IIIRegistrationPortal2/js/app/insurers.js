$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

    function readURL(input, target) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                target.attr('src', e.target.result);
            }
            reader.readAsDataURL(input.files[0]);
        }
    }

    var ImageMust = false;
    var DistrictDetails = ""; 
    $("#frmMain").hide();
    $("#GridFilter").show();
    $("#data").hide();
    loadInsurer();

    jQuery.validator.addMethod(
        "ValidatePincode",
        function (value, element) {
            var isSuccess = false;
            var _Pincode = $("#txtPincode").val();
            var _PincodeRange = $("#pincodeRange").html();
            _PincodeRange = _PincodeRange.replace("Pincode range : ", "");
            var PinCodes = _PincodeRange.split('-');
            if (PinCodes.length == 2) {
                var PinFrom = PinCodes[0].trim();
                var PinTo = PinCodes[1].trim();
                if (parseInt(PinFrom) != 0 || parseInt(PinTo) != 0) {
                    if (parseInt(_Pincode) < parseInt(PinFrom) || parseInt(_Pincode) > parseInt(PinTo)) {
                        isSuccess = false;
                    }
                    else {
                        isSuccess = true;
                    }
                }
            }
            else {
                isSuccess = true;
            }
            return isSuccess;
        }, 'Invalid PIN code for the given district.'
    );

    jQuery.validator.addMethod(
        "ValidatePincode2",
        function (value, element) {
            var isSuccess = false;
            var _Pincode = $("#txtPincode").val();
            if (parseInt(_Pincode) == 0) {
                isSuccess = false;
            }
            else {
                isSuccess = true;
            }
            return isSuccess;
        }, 'PIN code cannot be zero.'
    );

    function loadInsurer() {
        $("#frmMain").hide();
        $.ajax({
            type: "POST",
            url: "../Services/GetInsurer2",
            //data: JSON.stringify(JsonObject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                //debugger;
                var s = '';
                var result = JSON.parse(msg);
                if (result._STATUS_ == 'FAIL') {
                    alert(result._MESSAGE_);
                }
                else {
                    var s = '';
                    var result = JSON.parse(msg);
                    if (result._STATUS_ == 'FAIL') {
                        alert(result._MESSAGE_);
                    }
                    else {
                        var data = JSON.parse(result._DATA_);
                        if (data.length == undefined || data.length == 0) {
                            alert("No data found");
                        }
                        else {
                            s = "<table class='table table-striped table-bordered table-hover'>"
                            s += "<thead><tr><td>&nbsp</td><td>Insurer</td></tr></thead>";
                            for (i = 0; i < data.length; i++) {
                                s += "<tr>";
                                s += "<td><a href='javascript:edit(" + data[i].intTblMstInsurerUserID + ")'>Edit</a></td><td>" + data[i].varName + "</td>";
                                s += "</tr>";
                            }
                            s += "</table>";
                            $("#data").html(s);
                            $("#data").show();
                        }
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        })
    }

    window.edit = function (_InsurerId) {
        debugger;
        var JsonObject = {
            InsurerId: _InsurerId,
        };
        $.ajax({
            type: "POST",
            url: "../Services/GetInsurer2",
            data: JSON.stringify(JsonObject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                //debugger;
                var s = '';
                var result = JSON.parse(msg);
                if (result._STATUS_ == 'FAIL') {
                    alert(result._MESSAGE_);
                }
                else {
                    var data = JSON.parse(result._DATA_);
                    if (data.length == undefined || data.length == 0) {
                        Alert("No data found");
                        $("#data").show();
                        $("#frmMain").hide();
                    }
                    else {
                        //Code goes here

                        $("#hdnIntUserID").val(data[0].intUserID);
                        $("#hdnInttblmstinsureruserid").val(data[0].intTblMstInsurerUserID);

                        $("#txtInsurerCode").val(data[0].varInsurerID);
                        $("#txtInsurerRegistrationNumber").val(data[0].varInsurerRegNo);
                        $("#txtName").val(data[0].varName);
                        $("#txtCDPName").val(data[0].varCDPName);
                        $("#cboInsurerType").val(data[0].tntInsurerType);
                        $("#txtAddress1").val(data[0].varHouseNo);
                        $("#txtAddress2").val(data[0].varStreet);
                        $("#txtAddress3").val(data[0].varTown);
                        $("#cboStates").val(data[0].tntStateID);
                        $("#cboStates").trigger("change", data[0].sntDistrictID);
                        $("#txtPincode").val(data[0].intPINCode);
                        $("#txtTelephoneO").val(data[0].varTelOffice);
                        $("#txtTelephoneR").val(data[0].varTelResidence);
                        $("#txtFax").val(data[0].varFax);
                        $("#txtEmailId").val(data[0].varEmailID);

                        if (data[0].imgCDPSignatureB64 != null) {
                            $("#imgFileSign").attr('src', 'data:image/jpg;base64,' + data[0].imgCDPSignatureB64);
                            ImageMust = false;
                        }
                        else {
                            ImageMust = true;
                        }

                        $("#GridFilter").hide();
                        $("#data").hide();
                        $("#frmMain").show();
                        
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        })
    }
    $('#butCancel').click(function () {
        window.location = window.location;
    })
    $("#butNew").click(function () {
        debugger;
        $("#data").html('');
        $("#data").hide();
        $("#frmMain").trigger("reset");
        $("#frmMain").show();
        $("#GridFilter").hide();
        $("#data").hide();
        ImageMust = true;
    });
    $("#cboStates").change(function (event, DistrictId) {
        //debugger;
        $("#cboDistricts").html('');
        $("#pincodeRange").html('');

        DistrictDetails_current = "";
        var _StateId = $("#cboStates option:selected").val();
        if (_StateId == "") {
            $("#cboDistricts").html("<option value=''>-- Select --</option>")
            return;
        }
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
                //debugger;
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
                        DistrictDetails = data;
                        s += "<option value=''>-- Select --</option>"
                        for (i = 0; i < data.length; i++) {
                            //s += "<option value=" + data[i].district_id + "|" + data[i].from_pincode + " - " + data[i].to_pincode + ">" + data[i].district_name + "</option>"
                            s += "<option value=" + data[i].district_id + ">" + data[i].district_name + "</option>"
                        }
                        $("#cboDistricts").html(s);
                        if (DistrictId != undefined) {
                            $("#cboDistricts").val(DistrictId);
                            $("#cboDistricts").trigger("change");
                        }
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        });
    })
    $("#cboDistricts").change(function () {
        debugger;
        $("#pincodeRange").html("");
        var _DistrictId = $("#cboDistricts option:selected").val();
        if (_DistrictId == "") {

        }
        else {
            for (i = 0; i < DistrictDetails.length; i++) {
                if (DistrictDetails[i].district_id == _DistrictId) {
                    var s = "";
                    if (DistrictDetails[i].from_pincode == 0 && DistrictDetails[i].to_pincode == 0) {
                        s = "Pincode range not available.";
                    }
                    else {
                        s = "Pincode range : " + DistrictDetails[i].from_pincode + " - " + DistrictDetails[i].to_pincode;
                    }
                    $("#pincodeRange").html(s);
                    break;
                }
            }
        }
    })
    $("#txtFileSign").change(function () {
        var file = this.files[0];
        name = file.name;
        size = file.size;
        type = file.type;
        validator.element($('#txtFileSign'));
        if ($(this).valid()) {
            readURL(this, $('#imgFileSign'));
        }
    });
    $('#imgFileSign').click(function () {
        $("#txtFileSign").trigger('click');
    })

    var validator = $("#frmMain").validate(
        {
            ignore: [],
            rules: {
                txtInsurerCode: {
                    required: true,
                    check_exp: regexAlphaOnly,
                    minlength: 3
                },
                txtInsurerRegistrationNumber: {
                    required: true,
                    check_exp: regexAlphaOnly,
                    minlength: 3
                    //required: true,
                    //minlength: 3,
                    //digits: true
                },
                cboInsurerType: {
                    required: true,
                },
                txtName: {
                    required: true,
                    check_exp: regexAlphaOnlyWithSpace,
                    minlength: 5
                },
                txtCDPName: {
                    required: true,
                    check_exp: regexAlphaOnlyWithSpace,
                    minlength: 5
                },
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
                    minlength: 6,
                    digits: true,
                    ValidatePincode: true,
                    ValidatePincode2: true
                },
                txtTelephoneO: {
                    digits: true
                },
                txtTelephoneR: {
                    digits: true
                },
                txtFax: {
                    digits: true
                },
                txtEmailId: {
                    required: true,
                    email: true
                },
                txtFileSign: {
                    required: function () {
                        return ImageMust;
                    },
                    extension: "jpg|jpeg|png",
                    filesize: 51200
                }
            },
            messages: {
                txtInsurerCode: {
                    required: MandatoryFieldMsg,
                    check_exp: "Insurer code must contain only alphabets without any space",
                    minlength: "Insurer code should be minimum 3 characters long"
                },
                txtInsurerRegistrationNumber: {
                    required: MandatoryFieldMsg,
                    check_exp: "Insurer registration no must contain only alphabets without any space",
                    minlength: "Insurer registration no should be minimum 3 characters long"
                    //digits: "Insurer registration no should be numeric"
                },
                cboInsurerType: {
                    required: MandatoryFieldMsg,
                },
                txtName: {
                    required: MandatoryFieldMsg,
                    check_exp: "Only alphabets and space without leading / trailing space are allowed",
                    minlength: "Company name should be minimum 5 characters long"
                },
                txtCDPName: {
                    required: MandatoryFieldMsg,
                    check_exp: "Only alphabets and space without leading / trailing space are allowed",
                    minlength: "Corporate Designated Person's Name should be minimum 5 characters long"
                },
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
                    minlength: ValidPincodeMsg,
                    digits: ValidPincodeMsg
                },
                txtTelephoneO: {
                    digits: ValidPhoneMsg
                },
                txtTelephoneR: {
                    digits: ValidPhoneMsg
                },
                txtFax: {
                    digits: ValidFaxMsg
                },
                txtEmailId: {
                    required: MandatoryFieldMsg,
                    email: ValidEmailIdMsg
                },
                txtFileSign: {
                    required: "Please select signature",
                    extension: "Please upload image file (*.jpg / *.jpeg / *.png)",
                    filesize: "File size must be less than 50 kilobytes"
                }
            },
            submitHandler: function (form) {
                var data = new FormData(form);
                $.ajax({
                    type: "POST",
                    enctype: 'multipart/form-data',
                    url: "../Users/SaveInsurer",
                    data: data,
                    processData: false,
                    contentType: false,
                    cache: false,
                    success: function (msg) {
                        debugger;
                        var s = '';
                        var Result = JSON.parse(msg);
                        if (Result._STATUS_ == "SUCCESS") {
                            alert(Result._MESSAGE_);
                            window.location = window.location;
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
        }
    );
});