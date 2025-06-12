$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

    $("#data").hide();
    $("#GridFilter").show();
    $("#frmMain").hide();

    var DistrictDetails = ""; 

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

    $("#cboStatesF").change(function () {
        var _StateId = $("#cboStatesF option:selected").val();
        if (_StateId == '' || _StateId == undefined || _StateId == null) {
            return;
        }
        var JsonObject = {
            StateId: _StateId,
            CenterId : -1
        };
        $.ajax({
            type: "POST",
            url: "../Services/GetCentersForState",
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
                        alert(NO_DATA_FOUND);
                    }
                    else {
                        s = "<table class='table table-striped table-bordered table-hover'>"
                        s += "<thead><tr><td>&nbsp</td><td>Center Id</td><td>Exam Center Code (Exams Engine Code)</td><td>CSS Center Code</td><td>Exam Center Name</td></tr></thead>";
                        for (i = 0; i < data.length; i++) {
                            s += "<tr>";
                            s += "<td><a href='javascript:edit(" + data[i].sntExamCenterID + ")'>Edit</a></td><td>" + data[i].sntExamCenterID + "</td><td>" + data[i].varExamCenterCode + "</td><td>" + data[i].css_id +  "</td><td>" + data[i].varExamCenterName + "</td>";
                            s += "</tr>";
                        }
                        s += "</table>";
                        $("#data").html(s);
                        $("#data").show();
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        })
    })


    $("#frmMain").hide();


    $("#cboStates").change(function (event, DistrictId, Pincode) {
        DistrictDetails = "";
        $("#cboDistricts").html('');
        $("#txtPincode").val('');
        $("#pincodeRange").html("");
        var _StateId = $("#cboStates option:selected").val();
        if (_StateId == '' || _StateId == undefined || _StateId == null) {
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
                debugger;
                var s = '';
                var result = JSON.parse(msg);
                if (result._STATUS_ == 'FAIL') {
                    alert(result._MESSAGE_);
                }
                else {
                    var data = JSON.parse(result._DATA_);
                    DistrictDetails = data;
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
                            $("#cboDistricts").trigger("change", Pincode);
                        }
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        });
    })
    $("#cboDistricts").change(function (event, Pincode) {
        $("#txtPincode").val('');
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
            if (Pincode != undefined) {
                $("#txtPincode").val(Pincode);
            }
        }
    })
    
    $("#butNewCenter").click(function () {
        $("#data").hide();
        $("#GridFilter").hide();
        $("#frmMain").show();
        $("#frmMain").trigger("reset");
    });

    window.edit = function (Id) {
        var JsonObject = {
            StateId: -1,
            CenterId: Id
        };
        $.ajax({
            type: "POST",
            url: "../Services/GetCentersForState",
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
                        $("#txtExamCenterCode").val(data[0].varExamCenterCode);
                        $("#txtCSSCode").val(data[0].css_id);
                        $("#hdnExamCenterId").val(data[0].sntExamCenterID);
                        $("#txtExamCenterName").val(data[0].varExamCenterName);
                        $("#txtAddress1").val(data[0].varHouseNo);
                        $("#txtAddress2").val(data[0].varStreet);
                        $("#txtAddress3").val(data[0].varTown);

                        $("#cboStates").val(data[0].tntStateID);
                        $("#cboStates").trigger("change", [data[0].sntDistrictID, data[0].intPincode]);

                        var isactive = data[0].btIsActive;
                        if (isactive == false) {
                            $("#chkIsActive").attr('checked', false)
                        }
                        else {
                            $("#chkIsActive").attr('checked', true)
                        }

                        $("#cboCenterType").val(data[0].center_type);

                        $("#cboCenterType").attr('disabled','disabled');

                        $("#data").hide();
                        $("#GridFilter").hide();
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


    var validator = $("#frmMain").validate({
        ignore:[],
        rules: {
            txtExamCenterCode: {
                required: true,
                digits: true
            },
            txtCSSCode: {
                required: true,
                digits: true
            },
            txtExamCenterName: {
                required: true,
                check_exp: regexLowAscii,
                minlength: 5
            },
            txtAddress1: {
                check_exp: regexLowAscii
            },
            txtAddress2: {
                check_exp: regexLowAscii
            },
            txtAddress3: {
                check_exp: regexLowAscii
            },
            txtPincode: {
                required: true,
                digits: true,
                minlength: 6,
                ValidatePincode: true,
                ValidatePincode2: true
            },
            cboStates: {
                required: true
            },
            cboDistricts: {
                required: true
            },
            cboCenterType: {
                required: true
            }
        },
        messages: {
            txtExamCenterCode: {
                required: MandatoryFieldMsg,
                digits: "The center code should be numeric without any leading / trailing spaces or special character"
            },
            txtCSSCode: {
                required: MandatoryFieldMsg,
                digits: "The center code should be numeric without any leading / trailing spaces or special character"
            },
            txtExamCenterName: {
                required: MandatoryFieldMsg,
                check_exp: JunkCharMessage,
                minlength: "Name should be minimum 5 characters long"
            },
            txtAddress1: {
                check_exp: JunkCharMessage
            },
            txtAddress2: {
                check_exp: JunkCharMessage
            },
            txtAddress3: {
                check_exp: JunkCharMessage
            },
            txtPincode: {
                required: MandatoryFieldMsg,
                digits: ValidPincodeMsg,
                minlength: ValidPincodeMsg
            },
            cboStates: {
                required: MandatoryFieldMsg
            },
            cboDistricts: {
                required: MandatoryFieldMsg
            },
            cboCenterType: {
                required: MandatoryFieldMsg
            }
        },
        submitHandler: function (form) {
            var data = new FormData(form);
            $.ajax({
                type: "POST",
                enctype: 'multipart/form-data',
                url: "../Services/SaveExamCenter",
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
    })
});