$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

    $("#frmMain").hide();
    $("#data").html('');
    $("#data").hide('');

    jQuery.validator.addMethod("ValidateLoginIDPattern", function (value, element) {
        if (value == undefined || value == "" || value == null) {
            return true;
        }
        var numRegex   = ".*[0-9].*";
        var alphaRegex = ".*[A-Za-z].*";

        if (value.match(regexAlphaNumeric) && value.match(numRegex) && value.match(alphaRegex))
        {
            return true;
        }
        else
        {
            return false;
        }
    }, "The login id must be alpha numeric and must contain combination of alphabets and number");

    var DistrictDetails = ""; 
    loadInsurer();
    function loadInsurer() {
        $.ajax({
            type: "POST",
            url: "../Services/GetInsurer",
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
                    var data = JSON.parse(result._DATA_);
                    if (data.length == undefined || data.length == 0) {
                        alert(result._MESSAGE_);
                    }
                    else {
                        $("#cboInsurer").html('');
                        s += "<option value=''>-- Select --</option>";
                        for (i = 0; i < data.length; i++) {
                            val = data[i].intTblMstInsurerUserID + "|" + data[i].InsuranceType;
                            text = data[i].varInsurerID + ' - ' + data[i].varName;
                            if (data.length == 1)
                            {
                                s += "<option value='" + val + "' selected>" + text + "</option>";
                            }
                            else
                            {
                                s += "<option value='" + val + "'>" + text + "</option>";
                            }
                        }
                        $("#cboInsurer").html(s);
                        if (data.length == 1) {
                            $('#cboInsurer').trigger("change");
                        }
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        })
    }

    $("#cboStates").change(function (event, DistrictId, Pincode) {
        DistrictDetails = "";
        $("#cboDistricts").html('');
        $("#txtPincode").val('');
        $("#pincodeRange").html("");
        var _StateId = $("#cboStates option:selected").val();
        if (_StateId == '' || _StateId == undefined || _StateId == null)
        {
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
                        s = "Pincode Range : " + DistrictDetails[i].from_pincode + " - " + DistrictDetails[i].to_pincode;
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
    $('#cboInsurer').change(function (event, intTblMstAgntCounselorUserID) {
        $("#frmMain").hide();
        $("#data").html('');
        $("#data").hide();
        $("#cboDPS").html('');
        //debugger;
        var _InsurerUserID = $("#cboInsurer option:selected").val();
        if (_InsurerUserID == '' || _InsurerUserID == null || _InsurerUserID == undefined) {
            alert('Please Select Insurer');
            return;
        }
        else {
            _InsurerUserID = _InsurerUserID.split('|')[0];
        }
        var JsonObject = {
            InsurerId: _InsurerUserID,
            DPId: -1
        };
        $.ajax({
            type: "POST",
            url: "../Services/GetDPforInsurer",
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
                        s += "<option value=''>-- Select --</option>";
                        s += "<option value='-1'>-- All DPs --</option>";
                        for (i = 0; i < data.length; i++) {
                            s += "<option value='" + data[i].intTblMstDPUserID + "'>" + data[i].varDPID + " - " + data[i].varName + "</option>";
                        }
                        $("#cboDPS").html(s);
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        })
    });
    $('#cboDPS').change(function (event, intTblMstAgntCounselorUserID) {
        $("#frmMain").hide();
        $("#data").html('');
        $("#data").hide();
        //debugger;
        var _InsurerUserID = $("#cboInsurer option:selected").val();
        if (_InsurerUserID == '' || _InsurerUserID == null || _InsurerUserID == undefined) {
            alert('Please Select Insurer');
            return;
        }
        else {
            _InsurerUserID = _InsurerUserID.split('|')[0];
        }

        var _DPUserID = $("#cboDPS option:selected").val();
        if (_DPUserID == '' || _DPUserID == null || _DPUserID == undefined) {
            alert('Please Select Designated Person');
            return;
        }
        
        var JsonObject = {
            InsurerID: _InsurerUserID,
            DPUserID: _DPUserID,
            ACUserId: -1,
        };
        $.ajax({
            type: "POST",
            url: "../Services/GetACforDP",
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
                        alert("No data found");
                    }
                    else {
                            s = "<table class='table table-striped table-bordered table-hover'>"
                            s += "<thead><tr><td>&nbsp</td><td>Agent Counselor login Id</td><td>Agent Counselor Name</td></tr></thead>";
                            for (i = 0; i < data.length; i++) {
                                s += "<tr>";
                                s += "<td><a href='javascript:edit(" + data[i].intTblMstAgntCounselorUserID + ")'>Edit</a></td><td>" + data[i].varCounselorLoginID + "</td><td>" + data[i].varName + "</td>";
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
    });
    $("#butNewAC").click(function () {
        debugger;
        var _InsurerUserID = $("#cboInsurer option:selected").val();
        var _InsurerName = $("#cboInsurer option:selected").text();
        if (_InsurerUserID == '' || _InsurerUserID == null || _InsurerUserID == undefined) {
            alert('Please Select Insurer');
            return;
        }
        var _DPUserID = $("#cboDPS option:selected").val();
        var _DPUserName = $("#cboDPS option:selected").text();
        if (_DPUserID == '' || _DPUserID == null || _DPUserID == undefined) {
            alert('Please Select Designated Person');
            return;
        }

        _InsurerUserID = _InsurerUserID.split('|');
        var _InsuranceType = _InsurerUserID[1];
        _InsurerUserID = _InsurerUserID[0];

        $("#data").hide();
        $("#GridFilter").hide();
        $("#frmMain").trigger("reset");
        $("#frmMain").show();

        $("#hdnAgentCounsellorUserId").val(0);
        $("#hdnInsurerUserId").val(_InsurerUserID);
        $("#txtInsurerName").val(_InsurerName);
        $("#hdnDPUserId").val(_DPUserID);
        $("#txtDP").val(_DPUserName);
        $("#chkChangePwd").attr("checked", true);
        $("#txtLoginId").removeAttr("disabled");
        $("#hdnACUserId").val(0); //??

    });

    window.edit = function (ACUserId) {
        debugger;
        var _InsurerUserID = $("#cboInsurer option:selected").val();
        if (_InsurerUserID == '' || _InsurerUserID == null || _InsurerUserID == undefined) {
            alert('Please Select Insurer');
            return;
        }
        else {
            _InsurerUserID = _InsurerUserID.split('|')[0];
        }

        var JsonObject = {
            InsurerID: _InsurerUserID,
            DPUserId: -1,
            ACUserId: ACUserId
        };
        $.ajax({
            type: "POST",
            url: "../Services/GetACforDP",
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
                        $("#frmMain").trigger("reset");
                        //----- OK
                        $("#hdnAgentCounsellorUserId").val(data[0].intTblMstAgntCounselorUserID);
                        $("#txtInsurerName").val(data[0].CDPName);
                        $("#hdnInsurerUserId").val(data[0].intTblMstInsurerUserID);
                        $("#txtDP").val(data[0].DPName);
                        $("#hdnDPUserId").val(data[0].intTblMstDPUserID);
                        $("#txtLoginId").val(data[0].varCounselorLoginID);
                        $("#txtLoginId").attr("disabled", "disabled");
                        $("#hdnLoginId").val(data[0].varCounselorLoginID);
                        $("#hdnACUserId").val(data[0].intUserID);
                        $("#txtName").val(data[0].varName);
                        $("#txtAddress").val(data[0].varHouseNo);
                        $("#txtStreet").val(data[0].varStreet);
                        $("#txtTown").val(data[0].varTown);
                        $("#txtTelephoneO").val(data[0].varTelOffice);
                        $("#txtTelephoneR").val(data[0].varTelResidence);
                        $("#txtMobileNo").val(data[0].varMobileNo);
                        $("#txtFax").val(data[0].varFax);
                        $("#txtEmailID").val(data[0].varEmailID);
                        $("#cboStates").val(data[0].tntStateID);
                        $("#cboStates").trigger("change", [data[0].sntDistrictID, data[0].intPINCode] );
                        //$("#txtPincode").val(data[0].intPINCode);
                        //$("#chkActive").val(data[0].bitIsActive);
                        var isactive = data[0].bitIsActive;
                        if (isactive) {
                            $("#chkActive").attr('checked', true);
                        }
                        else
                        {
                            $("#chkActive").attr('checked', false);
                        }

                        isactive = data[0].bitChgPwdOnNxtLogin;
                        if (isactive) {
                            $("#chkChangePwd").attr('checked', true);
                        }
                        else {
                            $("#chkChangePwd").attr('checked', false);
                        }
                        
                        //----- End OK
                        $("#data").hide();
                        $("#GridFilter").hide();
                        $("#frmMain").valid();
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

    $("#frmMain").validate({
        rules: {
            //txtDPId: {
            //    required:true
            //},
            //txtInsurerType:{
            //    required: true
            //},
            txtName: {
                required: true,
                check_exp: regexAlphaOnlyWithSpace,
                minlength: 5
            },
            txtLoginId: {
                required: true,
                check_exp: regexAlphaNumeric,
                minlength: 3,
                ValidateLoginIDPattern: true
            },
            txtAddress: {
                check_exp: regexLowAscii
            },
            txtStreet: {
                check_exp: regexLowAscii
            },
            txtTown: {
                check_exp: regexLowAscii
            },
            txtPincode: {
                required: true,
                digits: true,
                minlength: 6
            },
            txtTelephoneO: {
                digits: true,
            },
            txtTelephoneR: {
                digits: true,
            },
            txtMobileNo: {
                digits: true,
                minlength: 10
            },
            txtFax: {
                digits: true,
            },
            txtEmailID: {
                required: true,
                email: true,
            },
            cboStates: {
                required: true
            },
            cboDistricts: {
                required: true
            }
        },
        messages: {
            //txtDPId: {
            //    required: MandatoryFieldMsg
            //},
            //txtInsurerType: {
            //    required: MandatoryFieldMsg
            //},
            txtName: {
                required: MandatoryFieldMsg,
                check_exp: "Please enter valid name. Only alphabets and space without leading / trailing space are allowed",
                minlength: "Name should be minimum 5 characters long"
            },
            txtLoginId: {
                required: MandatoryFieldMsg,
                check_exp: "Only alphabets and numbers without leading / trailing space are allowed",
                minlength: "Login Id must be minimum 3 characters long",
            },
            txtAddress: {
                check_exp: JunkCharMessage
            },
            txtStreet: {
                check_exp: JunkCharMessage
            },
            txtTown: {
                check_exp: JunkCharMessage
            },
            txtPincode: {
                required: MandatoryFieldMsg,
                digits: ValidPincodeMsg,
                minlength: ValidPincodeMsg
            },
            txtTelephoneO: {
                digits: ValidPhoneMsg
            },
            txtTelephoneR: {
                digits: ValidPhoneMsg
            },
            txtMobileNo: {
                digits: ValidMobileMsg,
                minlength: ValidMobileMsg
            },
            txtFax: {
                digits: ValidFaxMsg
            },
            txtEmailID: {
                required: MandatoryFieldMsg,
                email: ValidEmailIdMsg
            },
            cboStates: {
                required: MandatoryFieldMsg
            },
            cboDistricts: {
                required: MandatoryFieldMsg
            }
        },
        submitHandler: function (form) {
            var data = new FormData(form);
            $.ajax({
                type: "POST",
                enctype: 'multipart/form-data',
                url: "../Users/SaveAC",
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