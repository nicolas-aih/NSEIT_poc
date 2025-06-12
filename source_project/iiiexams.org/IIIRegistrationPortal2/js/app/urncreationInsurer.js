$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

    $("#cboCORType").trigger("change");   

    function readURL(input, target) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                target.attr('src', e.target.result);
            }
            reader.readAsDataURL(input.files[0]);
        }
    }

    var DistrictDetails_perm = "";
    var DistrictDetails_current = "";
    var PANMessage = "";

    jQuery.validator.addMethod(
        "ValidateDOB",
        function (value, element) {
            if (value == undefined || value == "" || value == null) {
                return;
            }
            var isSuccess = false;
            var JsonObject = {
                date: value,
            };
            $.ajax({
                type: "POST",
                async: false,
                url: "../Services/ValidateDOB",
                data: JSON.stringify(JsonObject),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    //debugger;
                    var retval = JSON.parse(msg);
                    if (retval._STATUS_ == "FAIL") {
                        isSuccess = false;
                    }
                    else {
                        isSuccess = true;
                    }
                },
                error: function (msg) {
                    isSuccess = false;
                    HandleAjaxError(msg);
                }
            });
            return isSuccess;
        }, "The candidate must be minimum 18 years old"
    );

    jQuery.validator.addMethod(
        "ValidateYearOfPassing",
        function (value, element) {
            if (value == undefined || value == "" || value == null) {
                return true;
            }
            var isSuccess = false;
            var JsonObject = {
                date: value,
            };
            $.ajax({
                type: "POST",
                async: false,
                url: "../Services/ValidateYOP",
                data: JSON.stringify(JsonObject),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    //debugger;
                    var retval = JSON.parse(msg);
                    if (retval._STATUS_ == "FAIL") {
                        isSuccess = false;
                    }
                    else {
                        isSuccess = true;
                    }
                },
                error: function (msg) {
                    isSuccess = false;
                    HandleAjaxError(msg);
                }
            });
            return isSuccess;
        }, "The Year of passing cannot be a future date"
    );

    jQuery.validator.addMethod(
        "ValidateYearOfPassing2",
        function (value, element) {
            var _DOB = $("#txtDOB").val();
            if (_DOB == undefined || _DOB == "" || _DOB == null) {
                return true;
            }
            if (value == undefined || value == "" || value == null) {
                return true;
            }
            var isSuccess = false;
            var JsonObject = {
                dateDOB: _DOB,
                dateYOP: value
            };
            $.ajax({
                type: "POST",
                async: false,
                url: "../Services/ValidateYOP2",
                data: JSON.stringify(JsonObject),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    //debugger;
                    var retval = JSON.parse(msg);
                    if (retval._STATUS_ == "FAIL") {
                        isSuccess = false;
                    }
                    else {
                        isSuccess = true;
                    }
                },
                error: function (msg) {
                    isSuccess = false;
                    HandleAjaxError(msg);
                }
            });
            return isSuccess;
        }, "The Year of passing must be greater than Date of Birth."
    );
    /*
    jQuery.validator.addMethod(
        "ValidateAadhaar",
        function (value, element) {
            //debugger;
            if (value == undefined || value == "" || value == null) {
                return true;
            }
            var _PAN = $("#txtPAN").val();
            var isSuccess = false;
            var JsonObject = {
                AadhaarNo: value,
                PAN: _PAN
            };
            $.ajax({
                type: "POST",
                async: false,
                url: "../Services/ValidateAadhaarCorporates",
                data: JSON.stringify(JsonObject),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    //debugger;
                    var s = '';
                    var retval = JSON.parse(msg);
                    if (retval._STATUS_ == "FAIL") {
                        $("#hdnAadhaarMessage").val(retval._MESSAGE_);
                        isSuccess = false;
                    }
                    else {
                        isSuccess = true;
                        $("#hdnAadhaarMessage").val('');
                    }
                },
                error: function (msg) {
                    isSuccess = false;
                    HandleAjaxError(msg);
                }
            });
            return isSuccess;
        }, ''
    );
    */

    jQuery.validator.addMethod(
        "IsPANValid",
        function (value, element) {
            //debugger;
            PANMessage = "";
            if (value == undefined || value == "" || value == null) {
                return true;
            }
            var isSuccess = false;
            var JsonObject = {
                PAN: value,
                ApplicantId: -1
            };
            $.ajax({
                type: "POST",
                async: false,
                url: "../Services/ValidatePAN2",
                data: JSON.stringify(JsonObject),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var s = '';
                    var retval = JSON.parse(msg);
                    if (retval._STATUS_ == "FAIL") {
                        isSuccess = false;
                        PANMessage = retval._MESSAGE_;
                    }
                    else {
                        isSuccess = true;
                        PANMessage = retval._MESSAGE_;
                    }
                },
                error: function (msg) {
                    isSuccess = false;
                    HandleAjaxError(msg);
                }
            });
            return isSuccess;
        }, ''
    );

    jQuery.validator.addMethod(
        "IsUnique",
        function (value, element) {
            debugger;
            var isSuccess = false;

            var _InternalRefNo = $("#txtEmployeeCode").val();
            var JsonObject = {
                InternalRefNo: _InternalRefNo,
                ApplicantId: -1
            };
            $.ajax({
                type: "POST",
                async: false,
                url: "../Services/ValidateInternalRefNo",
                data: JSON.stringify(JsonObject),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var s = '';
                    var retval = JSON.parse(msg);
                    if (retval._STATUS_ == "FAIL") {
                        isSuccess = false;
                    }
                    else {
                        isSuccess = true;
                    }
                },
                error: function (msg) {
                    isSuccess = false;
                    HandleAjaxError(msg);
                }
            });
            return isSuccess;
        }, 'The entered number is already in use'
    );

    jQuery.validator.addMethod(
        "ValidatePincodeCurr",
        function (value, element) {
            var isSuccess = false;
            var _Pincode = $("#txtPincode_current").val();
            var _PincodeRange = $("#pincodeRange_current").html();
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
        "ValidatePincodePerm",
        function (value, element) {
            var isSuccess = false;
            var _Pincode = $("#txtPincode_perm").val();
            var _PincodeRange = $("#pincodeRange_perm").html();
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

    $("#txtDOB").datepicker({
        showMonthAfterYear: true,
        dateFormat: 'dd-M-yy',
        changeMonth: true,
        changeYear: true,
        yearRange: "1900:" + ServerYear,
        onClose: function () {
            /* Validate a specific element: */
            //$("form").validate().element("#txtDOB");
            $(this).valid();
            //$("form #txtDOB").trigger("blur"); 
        }
    });

    //$("#txtDOB").on("change")(function(e){
    //    $("#txtDOB").valid();
    //});

    $("#txtYearofPassing").datepicker({
        showMonthAfterYear: true,
        dateFormat: 'M-yy',
        changeMonth: true,
        changeYear: true,
        yearRange: "1900:" + ServerYear,
        onClose: function () {
            /* Validate a specific element: */
            $(this).valid();
        }
    });

    $("#Result").hide();
    $("#Form").show();

    $("#txtFilePhoto").change(function () {
        var file = this.files[0];
        name = file.name;
        size = file.size;
        type = file.type;
        validator.element($('#txtFilePhoto'));
        if ($(this).valid()) {
            readURL(this, $('#imgFilePhoto'));
        }
    });
    $('#imgFilePhoto').click(function () {
        $("#txtFilePhoto").trigger('click');
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

    $("#cboSalutation").change(function () {
        // debugger;
        var Salutation = $("#cboSalutation option:selected").val();
        if (Salutation == "Mr.") {
            $("#cboGender").val("M");
            $("#cboGender").attr("disabled", true);
        }
        if (Salutation == "Mx.") {
            $("#cboGender").val("O");
            $("#cboGender").attr("disabled", true);
        }
        if (Salutation == "Mrs." || Salutation == "Ms.") {
            $("#cboGender").val("F");
            $("#cboGender").attr("disabled", true);
        }
        if (Salutation == "Dr.") {
            $("#cboGender").val("");
            $("#cboGender").attr("disabled", false);
        }
    })

    $("#cboCORType").change(function () {
        $("#cboBasicQualification").html('');
        var _CORType = $("#cboCORType option:selected").val();
        if (_CORType == "") {
            $("#cboBasicQualification").html("<option value=''>-- Select --</option>");
            $("#cboProfessionalQualification").html("<option value=''>-- Select --</option>");
            //$("#cboTelemarketer").html("<option value=''>-- Select --</option>");
            return;
        }
        if (_CORType === 'AV') {

            $("#lblEmployeeCode").html('Employee No :<span class="mandatory">*</span>');
            $("#lblCPEMailId span").html('*');
            $("#txtCPEMailId").removeAttr("disabled");
            //$("#divTelemarketer").show();
        }
        else {
            $("#lblEmployeeCode").html('Insurer Ref. No. :<span class="mandatory"></span>');
            $("#lblCPEMailId span").html('');
            $("#txtCPEMailId").val('');
            $("#txtCPEMailId").attr("disabled", "disabled");
            //$("#divTelemarketer").hide();
        }

        //DisplayHideBasicQualificationDoc();
        //DisplayHideQualificationDoc();

        var data = new FormData();
        data.append("cboCORType", _CORType);

        $.ajax({
            type: "POST",
            enctype: 'multipart/form-data',
            url: "../Services/GetDetailsForCOR",
            data: data,
            processData: false,
            contentType: false,
            cache: false,
            success: function (msg) {
                //debugger;
                $("#cboBasicQualification").html('');
                $("#cboProfessionalQualification").html('');
                //$("#cboTelemarketer").html('');
                var s = '';
                var result = JSON.parse(msg);
                if (result._STATUS_ == 'FAIL') {
                    alert(result._MESSAGE_);
                }
                else {
                    var data = JSON.parse(result._DATA0_);
                    if (data.length == undefined || data.length == 0) {
                        $("#cboBasicQualification").html("<option value=''>-- Select --</option>");
                        alert("No Basic Qualifications were found for selected CoR");
                    }
                    else {
                        s += "<option value=''>-- Select --</option>"
                        for (i = 0; i < data.length; i++) {
                            s += "<option value=" + data[i].qualification_code + ">" + data[i].qualification + "</option>"
                        }
                        $("#cboBasicQualification").html(s);
                    }
                    s = '';
                    data = JSON.parse(result._DATA1_);
                    if (data.length == undefined || data.length == 0) {
                        $("#cboProfessionalQualification").html("<option value=''>-- Select --</option>");
                        alert("No Professional Qualifications were found for selected CoR");
                    }
                    else {
                        s += "<option value=''>-- Select --</option>"
                        for (i = 0; i < data.length; i++) {
                            s += "<option value=" + data[i].qualification_code + ">" + data[i].qualification + "</option>"
                        }
                        $("#cboProfessionalQualification").html(s);
                    }
                    s = '';
                    //if (_CORType === 'AV') {
                    //    data = '';
                    //    data = JSON.parse(result._DATA2_);
                    //    if (data.length == undefined || data.length == 0) {
                    //        $("#cboTelemarketer").html("<option value=''>-- Select --</option>");
                    //        alert("No Telemarketers were found for the company");
                    //    }
                    //    else {
                    //        s += "<option value=''>-- Select --</option>"
                    //        for (i = 0; i < data.length; i++) {
                    //            s += "<option value=" + data[i].tm_id + ">" + data[i].tm_name + "</option>"
                    //        }
                    //        $("#cboTelemarketer").html(s);
                    //    }
                    //}
                    //else {
                    //    s += "<option value=''>-- Select --</option>"
                    //    $("#cboTelemarketer").html(s);
                    //}
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        });

    });


    $("#cboStates_current").change(function (event, DistrictId) {
        //debugger;
        DistrictDetails_current = "";
        $("#cboDistricts_current").html('');
        var _StateId = $("#cboStates_current option:selected").val();
        if (_StateId == "") {
            $("#cboDistricts_current").html("<option value=''>-- Select --</option>")
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
                    DistrictDetails_current = data;
                    if (data.length == undefined || data.length == 0) {
                        alert("No districts were found for selected state");
                    }
                    else {
                        s += "<option value=''>-- Select --</option>"
                        for (i = 0; i < data.length; i++) {
                            s += "<option value=" + data[i].district_id + ">" + data[i].district_name + "</option>"
                        }
                        $("#cboDistricts_current").html(s);
                        if (DistrictId != undefined) {
                            $("#cboDistricts_current").val(DistrictId);
                        }
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        });
    })

    $("#cboStates_perm").change(function (event, DistrictId) {
        $("#cboDistricts_perm").html('');
        DistrictDetails_perm = "";
        var _StateId = $("#cboStates_perm option:selected").val();
        if (_StateId == "") {
            $("#cboDistricts_perm").html("<option value=''>-- Select --</option>")
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
                    DistrictDetails_perm = data;
                    if (data.length == undefined || data.length == 0) {
                        alert("No districts were found for selected state");
                    }
                    else {
                        s += "<option value=''>-- Select --</option>"
                        for (i = 0; i < data.length; i++) {
                            s += "<option value=" + data[i].district_id + ">" + data[i].district_name + "</option>"
                        }
                        $("#cboDistricts_perm").html(s);
                        if (DistrictId != undefined) {
                            $("#cboDistricts_perm").val(DistrictId);
                            $("#cboDistricts_perm").trigger("change");
                            $("#cboDistricts_perm").valid();
                        }
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        });
    })

    $("#cboDistricts_current").change(function () {
        debugger;
        $("#pincodeRange_current").html("");
        var _DistrictId = $("#cboDistricts_current option:selected").val();
        if (_DistrictId == "") {

        }
        else {
            for (i = 0; i < DistrictDetails_current.length; i++) {
                if (DistrictDetails_current[i].district_id == _DistrictId) {
                    var s = "";
                    if (DistrictDetails_current[i].from_pincode == 0 && DistrictDetails_current[i].to_pincode == 0) {
                        var s = "Pincode range not available.";
                    }
                    else {
                        var s = "Pincode range : " + DistrictDetails_current[i].from_pincode + " - " + DistrictDetails_current[i].to_pincode;
                    }
                    $("#pincodeRange_current").html(s);
                    break;
                }
            }
        }
    })

    $("#cboDistricts_perm").change(function () {
        $("#pincodeRange_perm").html("");
        var _DistrictId = $("#cboDistricts_perm option:selected").val();
        if (_DistrictId == "") {

        }
        else {
            for (i = 0; i < DistrictDetails_perm.length; i++) {
                if (DistrictDetails_perm[i].district_id == _DistrictId) {
                    var s = "";
                    if (DistrictDetails_perm[i].from_pincode == 0 && DistrictDetails_perm[i].to_pincode == 0) {
                        s = "Pincode range not available.";
                    }
                    else {
                        s = "Pincode range : " + DistrictDetails_perm[i].from_pincode + " - " + DistrictDetails_perm[i].to_pincode;
                    }
                    $("#pincodeRange_perm").html(s);
                    break;
                }
            }
        }
    })

    $("#cboBranchState").change(function (event, DistrictId) {
        $("#cboBranchDistrict").html('');
        var _StateId = $("#cboBranchState option:selected").val();
        if (_StateId == "") {
            $("#cboBranchDistrict").html("<option value=''>-- Select --</option>")
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
                        s += "<option value=''>-- Select --</option>"
                        for (i = 0; i < data.length; i++) {
                            s += "<option value=" + data[i].district_id + ">" + data[i].district_name + "</option>"
                        }
                        $("#cboBranchDistrict").html(s);
                        if (DistrictId != undefined) {
                            $("#cboBranchDistrict").val(DistrictId);
                        }
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        });
    })

    $("#cboBranchDistrict").change(function (event, BranchNo) {
        $("#cboBranch").html('');
        var _StateId = $("#cboBranchState option:selected").val();
        var _DistrictId = $("#cboBranchDistrict option:selected").val();
        if (_StateId == "" || _DistrictId == "") {
            $("#cboBranch").html("<option value=''>-- Select --</option>")
            return;
        }
        var JsonObject = {
            StateId: _StateId,
            DistrictId: _DistrictId,
        };
        $.ajax({
            type: "POST",
            url: "../Branches/GetBranchesForStateDistrict",
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
                        alert("No branches were found for selected state and district combination");
                    }
                    else {
                        s += "<option value=''>-- Select --</option>"
                        for (i = 0; i < data.length; i++) {
                            s += "<option value=" + data[i].BranchNo + ">" + data[i].BranchCode + " - " + data[i].BranchName + "</option>"
                        }
                        $("#cboBranch").html(s);
                        if (BranchNo != undefined) {
                            $("#cboBranch").val(BranchNo);
                        }
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        });
    })

    $("#txtPincode_current").blur(function (event, ExamCenter) {
        //debugger;
        if (!$("#txtPincode_current").valid()) {
            return;
        }
        var _Pincode = $("#txtPincode_current").val().trim();
        if (_Pincode == undefined || _Pincode == "" || _Pincode == null) {
            //alert("Please enter the current pincode, to view the list of centers");
            return;
        }
        else {
            $("#cboExamCenter").html('');
            var JsonObject = { Pincode: _Pincode };
            $.ajax({
                type: "POST",
                url: "../Services/FindNearestExamCenter",
                data: JSON.stringify(JsonObject),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var s = '';
                    var result = JSON.parse(msg);
                    if (result._STATUS_ == 'FAIL') {
                        alert(result._ERROR_);
                    }
                    else {

                        var data = JSON.parse(result._DATA_);
                        if (data.length == undefined) {
                            s += "<span>No data found</span>"
                        }
                        else {
                            s += "<option value=''>-- Select --</option>"
                            s += "<optgroup  label='--NSEIT Centers--'>"
                            for (i = 0; i < data.length; i++) {
                                if (data[i].center_type === 'NORMAL') {
                                    s += "<option value=" + data[i].sntExamCenterID + "|" + data[i].numDistance + ">" + data[i].varExamCenterName + "</option>";
                                }
                            }
                            s += "</optgroup >"
                            s += "<optgroup  label='--NSEIT-Ext-Centers--'>"
                            for (i = 0; i < data.length; i++) {
                                if (data[i].center_type === 'TAB') {
                                    s += "<option value=" + data[i].sntExamCenterID + "|" + data[i].numDistance + ">" + data[i].varExamCenterName + "</option>";
                                }
                            }
                            s += "</optgroup >"
                            s += "<optgroup  label='--NSEIT Remotely Proctored Center--'>"
                            for (i = 0; i < data.length; i++) {
                                if (data[i].center_type === 'RP') {
                                    s += "<option value=" + data[i].sntExamCenterID + "|" + data[i].numDistance + ">" + data[i].varExamCenterName + "</option>";
                                }
                            }
                            s += "</optgroup >"
                        }
                        $("#cboExamCenter").html(s);
                    }
                    //alert(msg);
                },
                error: function (msg) {
                    HandleAjaxError(msg);
                }
            });
        }
    });

    $("#cboExamCenter").change(function () {
        var Value = $("#cboExamCenter option:selected").val();
        var X = Value.split("|");
        if (X[1] == "" || X[1] == undefined || X[1] == null || X[1] == 'null') {
            $("#centerHelp").html("distance data is not available");
        }
        else {
            $("#centerHelp").html(X[1] + " kms");
        }
    })

    var validator = $("#frmMain").validate(
        {
            //onfocusout: false,
            //onkeyup: false,
            ignore: [],
            rules: {
                txtApplicantName: {
                    required: true,
                    check_exp: regexAlphaOnlyWithSpace,
                },
                txtFHName: {
                    required: true,
                    check_exp: regexAlphaOnlyWithSpace,
                },
                txtDOB: {
                    required: true,
                    ValidateDOB: true
                    //validation for completion of 18 years of age
                },
                txtPAN: {
                    required: true,
                    check_exp: regexPAN,
                    IsPANValid: true
                },
                txtDrivingLic: {
                    check_exp: regexAlphaNumeric,
                },
                txtPassport: {
                    check_exp: regexAlphaNumeric,
                },
                txtVoterId: {
                    check_exp: regexAlphaNumeric,
                },
                txtPhotoId: {
                    check_exp: regexAlphaNumeric,
                },
                txtFilePhoto: {
                    required: true,
                    extension: "jpg|jpeg|png",
                    filesize: 51200
                },
                txtFileSign: {
                    required: true,
                    extension: "jpg|jpeg|png",
                    filesize: 51200
                },
                txtBoardName: {
                    required: true,
                    check_exp: regexAlphaOnlyWithSpace,
                },
                txtRollnumber: {
                    required: true,
                    check_exp: regexAlphaNumeric,
                },
                txtYearofPassing: {
                    required: true,
                    ValidateYearOfPassing: true,
                    ValidateYearOfPassing2: true
                },
                txtHouseNo_current: {
                    required: true,
                    check_exp: regexLowAscii,
                },
                txtStreet_current: {
                    required: true,
                    check_exp: regexLowAscii
                },
                txtCity_current: {
                    required: true,
                    check_exp: regexLowAscii,
                },
                cboDistricts_current: {
                    required: true,
                },
                txtPincode_current: {
                    required: true,
                    digits: true,
                    minlength: 6,
                    ValidatePincodeCurr: true
                },
                txtHouseNo_perm: {
                    required: true,
                    check_exp: regexLowAscii,
                },
                txtStreet_perm: {
                    required: true,
                    check_exp: regexLowAscii,
                },
                txtCity_perm: {
                    required: true,
                    check_exp: regexLowAscii,
                },
                cboDistricts_perm: {
                    required: true,
                },
                txtPincode_perm: {
                    required: true,
                    digits: true,
                    minlength: 6,
                    ValidatePincodePerm: true
                },
                txtLandlineNo: {
                    digits: true,
                },
                txtMobileNo: {
                    required: true,
                    digits: true,
                    minlength: function () {
                        if ($("#cboNationality option:selected").text().toUpperCase() == 'INDIAN') {
                            return 10;
                        }
                        else if ($("#cboNationality option:selected").val() == '') {
                            return 10;
                        }
                        else {
                            return 4;
                        }
                    },
                    ValidateMobile: true
                },
                cboAllowWhatsappMessage: {
                    required: true
                },
                txtWhatsAppNo: {
                    digits: true,
                    minlength: function () {
                        if ($("#cboNationality option:selected").text().toUpperCase() === 'INDIAN') {
                            return 10;
                        }
                        else if ($("#cboNationality option:selected").val() === '') {
                            return 10;
                        }
                        else {
                            return 4;
                        }
                    }
                },
                txtEMailId: {
                        required: true,
                        email: true
                    },
                txtCPEMailId: {
                        required: function () {
                            return $("#cboCORType option:selected").val() == "AV";
                        },
                        email: true,
                        NotEqualTo: "#txtEMailId"
                    },
                    txtPrimaryOccupation: {
                        required: true,
                        check_exp: regexAlphaNumericWithSpace,
                    },
                    txtEmployeeCode: {
                        required: function () {
                            return $("#cboCORType option:selected").val() == "AV";
                        },
                        check_exp: regexAlphaNumeric,
                        IsUnique: true
                    },
                    //cboTelemarketer: {
                    //    required: function() {
                    //        return $("#cboCORType option:selected").val() == "AV";
                    //    }
                    //},
                    //cboBranchState: {
                    //    required: true,
                    //},
                    //cboBranchDistrict: {
                    //    required: true,
                    //},
                    //cboBranch: {
                    //    required: true,
                    //},
                    cboInsuranceCategory: {
                        required: true,
                    },
                    cboCORType: {
                        required: true,
                    },
                    cboSalutation: {
                        required: true,
                    },
                    cboGender: {
                        required: true,
                    },
                    cboCategory: {
                        required: true,
                    },
                    cboArea: {
                        required: true,
                    },
                    cboNationality: {
                        required: true,
                    },
                    cboBasicQualification: {
                        required: true,
                    },
                    cboProfessionalQualification: {
                        required: true,
                    },
                    txtOtherQualification: {
                        required: function () {
                            return $("#cboProfessionalQualification option:selected").val() == "OTH";
                        },
                        check_exp: regexAlphaNumericWithSpace,
                        minlength: 2
                    },
                    cboStates_current: {
                        required: true,
                    },
                    cboStates_perm: {
                        required: true,
                    },
                    cboExamBody: {
                        required: true,
                    },
                    cboLanguage: {
                        required: true,
                    },
                    cboExamCenter: {
                        required: true,
                    },
                    chkDeclaration: {
                        required: true,
                    }
                },
                messages: {
                    txtApplicantName: {
                        required: MandatoryFieldMsg,
                        check_exp: "Only alphabets and space without leading / trailing space are allowed",
                    },
                    txtFHName: {
                        required: MandatoryFieldMsg,
                        check_exp: "Only alphabets and space without leading / trailing space are allowed",
                    },
                    txtDOB: {
                        required: MandatoryFieldMsg,
                    },
                    txtPAN: {
                        required: MandatoryFieldMsg,
                        check_exp: "Please enter valid PAN",
                        IsPANValid: function () {
                            return PANMessage;
                        }
                    },
                    txtDrivingLic: {
                        check_exp: "Only alphabets and numbers without leading / trailing space are allowed",
                    },
                    txtPassport: {
                        check_exp: "Only alphabets and numbers without leading / trailing space are allowed",
                    },
                    txtVoterId: {
                        check_exp: "Only alphabets and numbers without leading / trailing space are allowed",
                    },
                    txtPhotoId: {
                        check_exp: "Only alphabets and numbers without leading / trailing space are allowed",
                    },
                    txtFilePhoto: {
                        required: "Please select photo",
                        extension: "Please upload image file (*.jpg / *.jpeg / *.png)",
                        filesize: "File size must be less than 50 kilobytes"
                    },
                    txtFileSign: {
                        required: "Please select signature",
                        extension: "Please upload image file (*.jpg / *.jpeg / *.png)",
                        filesize: "File size must be less than 50 kilobytes"
                    },
                    txtBoardName: {
                        required: MandatoryFieldMsg,
                        check_exp: "Only alphabets and space without leading / trailing space are allowed",
                    },
                    txtRollnumber: {
                        required: MandatoryFieldMsg,
                        check_exp: "Only alphabets and numbers without leading / trailing space are allowed",
                    },
                    txtYearofPassing: {
                        required: MandatoryFieldMsg,
                    },
                    txtHouseNo_current: {
                        required: MandatoryFieldMsg,
                        check_exp: "Please enter valid address without leading / trailing space",
                    },
                    txtStreet_current: {
                        required: MandatoryFieldMsg,
                        check_exp: "Please enter valid address without leading / trailing space",
                    },
                    txtCity_current: {
                        required: MandatoryFieldMsg,
                        check_exp: "Please enter valid address without leading / trailing space"
                    },
                    cboDistricts_current: {
                        required: MandatoryFieldMsg,
                    },
                    txtPincode_current: {
                        required: MandatoryFieldMsg,
                        digits: ValidPincodeMsg,
                        minlength: ValidPincodeMsg
                    },
                    txtHouseNo_perm: {
                        required: MandatoryFieldMsg,
                        check_exp: "Please enter valid address without leading / trailing space",
                    },
                    txtStreet_perm: {
                        required: MandatoryFieldMsg,
                        check_exp: "Please enter valid address without leading / trailing space",
                    },
                    txtCity_perm: {
                        required: MandatoryFieldMsg,
                        check_exp: "Please enter valid address without leading / trailing space",
                    },
                    cboDistricts_perm: {
                        required: MandatoryFieldMsg,
                    },
                    txtPincode_perm: {
                        required: MandatoryFieldMsg,
                        digits: ValidPincodeMsg,
                        minlength: ValidPincodeMsg
                    },
                    txtLandlineNo: {
                        digits: ValidPhoneMsg,
                    },
                    txtMobileNo: {
                        required: MandatoryFieldMsg,
                        digits: ValidMobileMsg,
                        minlength: function () {
                            if ($("#cboNationality option:selected").text().toUpperCase() == 'INDIAN') {
                                return ValidMobileMsg;
                            }
                            else {
                                return "Please enter valid mobile number.Expected : minimum 4 digit number";
                            }
                        },
                        ValidateMobile: function () {
                            return $("#hdnMobileMessage").val();
                        }
                    },
                    cboAllowWhatsappMessage: {
                        required: MandatoryFieldMsg
                    },
                    txtWhatsAppNo: {
                        digits: ValidMobileMsg,
                        minlength: ValidMobileMsg
                    },
                    txtEMailId: {
                        required: MandatoryFieldMsg,
                        email: ValidEmailIdMsg
                    },
                    txtCPEMailId: {
                        required: MandatoryFieldMsg,
                        email: ValidEmailIdMsg,
                        NotEqualTo: "The contact person's email id should be different than candidate's email id"
                    },
                    txtPrimaryOccupation: {
                        required: MandatoryFieldMsg,
                        check_exp: "Only alphabets, numbers and space without leading / trailing space are allowed",
                    },
                    txtEmployeeCode: {
                        required: MandatoryFieldMsg,
                        check_exp: "Only alphabets, numbers without leading / trailing space are allowed",
                    },
                    //cboTelemarketer: {
                    //    required: MandatoryFieldMsg
                    //},
                    //cboBranchState: {
                    //    required: MandatoryFieldMsg,
                    //},
                    //cboBranchDistrict: {
                    //    required: MandatoryFieldMsg,
                    //},
                    //cboBranch: {
                    //    required: MandatoryFieldMsg,
                    //},
                    cboInsuranceCategory: {
                        required: MandatoryFieldMsg,
                    },
                    cboCORType: {
                        required: MandatoryFieldMsg,
                    },
                    cboSalutation: {
                        required: MandatoryFieldMsg,
                    },
                    cboGender: {
                        required: MandatoryFieldMsg,
                    },
                    cboCategory: {
                        required: MandatoryFieldMsg,
                    },
                    cboArea: {
                        required: MandatoryFieldMsg,
                    },
                    cboNationality: {
                        required: MandatoryFieldMsg,
                    },
                    cboBasicQualification: {
                        required: MandatoryFieldMsg,
                    },
                    cboProfessionalQualification: {
                        required: MandatoryFieldMsg,
                    },
                    txtOtherQualification: {
                        required: MandatoryFieldMsg,
                        check_exp: "Only alphabets, numbers and space without leading / trailing space are allowed. Min 2 characters",
                        minlength: "Only alphabets, numbers and space without leading / trailing space are allowed. Min 2 characters"
                    },
                    cboStates_current: {
                        required: MandatoryFieldMsg,
                    },
                    cboStates_perm: {
                        required: MandatoryFieldMsg,
                    },
                    cboExamBody: {
                        required: MandatoryFieldMsg,
                    },
                    cboLanguage: {
                        required: MandatoryFieldMsg,
                    },
                    cboExamCenter: {
                        required: MandatoryFieldMsg,
                    },
                    chkDeclaration: {
                        required: 'You must confirm the declaration by checking the check box',
                    }
                },
                errorPlacement: function (error, element) {
                    if (element.attr("name") === "chkDeclaration") {
                        error.insertAfter($("#lblDeclaration"));
                    } else {
                        error.insertAfter(element)
                    }
                },
                submitHandler: function (form) {
                    var data = new FormData(form);

                    $.ajax({
                        type: "POST",
                        enctype: 'multipart/form-data',
                        url: "../Candidates/URNCreation",
                        data: data,
                        processData: false,
                        contentType: false,
                        cache: false,
                        success: function (msg) {
                            //debugger;
                            var s = '';
                            var Result = JSON.parse(msg);
                            if (Result._STATUS_ == "SUCCESS") {
                                var x = '<H4>' + Result._MESSAGE_ + '<br>' + 'The URN no generated is : ' + Result.URN + '</H4>';
                                $("#Result").html(x);
                                $("#Result").show();
                                form.reset();
                                $("#Form").hide();
                            }
                            else {
                                var x = Result._MESSAGE_;
                                $("#Result").html(x);
                                $("#Result").show();
                            }
                        },
                        error: function (msg) {
                            HandleAjaxError(msg);
                        }
                    });
                }
            })

    $("#txtFile").change(function () {
        $("#ResponseFile").html('');
    })

    $("#frmUploadMain").validate(
        {
            rules: {
                txtFile: {
                    required: true,
                    extension: "zip",
                    filesize: 5242880
                },
                chkDeclarationU: {
                    required: true
                }
            },
            messages: {
                txtFile: {
                    required: MandatoryFieldMsg,
                    extension: "Please upload zip file (*.zip)",
                    filesize: "File size must be less than 5 megabytes"
                },
                chkDeclarationU:
                {
                    required: 'You must confirm the declaration by checking the check box',
                }
            },
            errorPlacement: function (error, element) {
                if (element.attr("name") === "chkDeclarationU") {
                    error.insertAfter($("#lblDeclarationU"));
                } else {
                    error.insertAfter(element)
                }
            },
            submitHandler: function (form) {
                var data = new FormData(form);
                $.ajax({
                    type: "POST",
                    enctype: 'multipart/form-data',
                    url: "../Candidates/UploadURN",
                    data: data,
                    processData: false,
                    contentType: false,
                    cache: false,
                    success: function (msg) {
                        //debugger;
                        var s = '';
                        var Result = JSON.parse(msg);
                        if (Result._STATUS_ == "SUCCESS") {
                            $("#ResponseFile").html('');

                            s = "<label><a href='" + Result._RESPONSE_FILE_ + "'>Download Response File in Excel Format</a></label><br>";
                            s += "<label><a href='" + Result._RESPONSE_FILE2_ + "'>Download Response File in Tab Separated Text Format</a></label>";
                            $("#ResponseFile").html(s);
                            alert(Result._MESSAGE_);
                        }
                        else {
                            if (Result._RESPONSE_FILE_) {
                                $("#ResponseFile").html('');
                                s = "<label><a href='" + Result._RESPONSE_FILE_ + "'>Download Response File in Excel Format</a></label><br>";
                                s += "<label><a href='" + Result._RESPONSE_FILE2_ + "'>Download Response File in Tab Separated Text Format</a></label>";
                                $("#ResponseFile").html(s);
                            }
                            alert(Result._MESSAGE_);
                        }
                        form.reset();
                    },
                    error: function (msg) {
                        HandleAjaxError(msg);
                    }
                });
            }
        }
    );

    $("#txtHouseNo_current").on('keyup keypress blur change', function () {
        if ($("#chkSameAsCurrent").prop("checked")) {
            $("#txtHouseNo_perm").val($("#txtHouseNo_current").val());
            $("#txtHouseNo_perm").valid();
        }
    });
    $("#txtStreet_current").on('keyup keypress blur change', function () {
        if ($("#chkSameAsCurrent").prop("checked")) {
            $("#txtStreet_perm").val($("#txtStreet_current").val());
            $("#txtStreet_perm").valid();
        }
    });
    $("#txtCity_current").on('keyup keypress blur change', function () {
        if ($("#chkSameAsCurrent").prop("checked")) {
            $("#txtCity_perm").val($("#txtCity_current").val());
            $("#txtCity_perm").valid();
        }
    });
    $("#cboStates_current").change(function () {
        if ($("#chkSameAsCurrent").prop("checked")) {
            $("#cboStates_perm").val($("#cboStates_current").val());
            $("#cboStates_perm").trigger("change");
            $("#cboStates_perm").valid();
        }
    });
    $("#cboDistricts_current").change(function () {
        if ($("#chkSameAsCurrent").prop("checked")) {
            $("#cboDistricts_perm").val($("#cboDistricts_current option:selected").val());
            $("#cboDistricts_perm").trigger("change");
            $("#cboDistricts_perm").valid();
        }
    });
    $("#txtPincode_current").on('keyup keypress blur change', function () {
        if ($("#chkSameAsCurrent").prop("checked")) {
            $("#txtPincode_perm").val($("#txtPincode_current").val());
            $("#txtPincode_perm").valid();
        }
    });

    $("#chkSameAsCurrent").change(function () {
        if (this.checked) {
            $("#txtHouseNo_perm").val($("#txtHouseNo_current").val());
            $("#txtStreet_perm").val($("#txtStreet_current").val());
            $("#txtCity_perm").val($("#txtCity_current").val());
            $("#cboStates_perm").val($("#cboStates_current option:selected").val());
            $("#cboStates_perm").trigger("change", $("#cboDistricts_current option:selected").val());
            $("#cboDistricts_perm").val($("#cboDistricts_current").val());
            $("#txtPincode_perm").val($("#txtPincode_current").val());
            $("#txtHouseNo_perm").attr("disabled", "disabled");
            $("#txtStreet_perm").attr("disabled", "disabled");
            $("#txtCity_perm").attr("disabled", "disabled");
            $("#cboStates_perm").attr("disabled", "disabled");
            $("#cboDistricts_perm").attr("disabled", "disabled");
            $("#txtPincode_perm").attr("disabled", "disabled");
        }
        else {
            $("#txtHouseNo_perm").removeAttr("disabled");
            $("#txtStreet_perm").removeAttr("disabled");
            $("#txtCity_perm").removeAttr("disabled");
            $("#cboStates_perm").removeAttr("disabled");
            $("#cboDistricts_perm").removeAttr("disabled");
            $("#txtPincode_perm").removeAttr("disabled");
        }
        $("#txtPincode_perm").valid();
        $("#txtHouseNo_perm").valid();
        $("#txtStreet_perm").valid();
        $("#txtCity_perm").valid();
        $("#cboStates_perm").valid();
        $("#cboDistricts_perm").valid();
        $("#txtPincode_perm").valid();
    });

    $("#cboNationality").change(function () {
        if ($("#cboNationality option:selected").text().toUpperCase() == 'INDIAN') {
            $("#txtMobileNo").attr('maxlength', '10');
            $("#txtWhatsAppNo").attr('maxlength', '10');
        }
        else {
            $("#txtMobileNo").attr('maxlength', '20');
            $("#txtWhatsAppNo").attr('maxlength', '20');
        }
    });

    $("#cboProfessionalQualification").change(function () {
        //debugger;
        if ($("#cboProfessionalQualification option:selected").val() == "OTH") {
            $("#lblOtherQualification").html('Other Qualification : <span class="mandatory">*</span>');
            $("#txtOtherQualification").removeAttr("disabled");
        }
        else {
            $("#lblOtherQualification").html('Other Qualification : <span class="mandatory">&nbsp;</span>');
            $("#txtOtherQualification").val('');
            $("#txtOtherQualification").attr("disabled", "disabled");
        }
    });

    $("#cboAllowWhatsappMessage").trigger("change");
    $("#cboAllowWhatsappMessage").change(function () {
        if ($('#cboAllowWhatsappMessage option:selected').val() === 'Y') {
            $("#lblWhatsAppNo").show();
            $("#txtWhatsAppNo").show();
        }
        else {
            $("#lblWhatsAppNo").hide();
            $("#txtWhatsAppNo").hide();
            $("#txtWhatsAppNo").val('');
        }
    });


    jQuery.validator.addMethod(
        "ValidateMobile",
        function (value, element) {
            if (value == undefined || value == "" || value == null) {
                isSuccess = true;
            }
            var _CORType = $("#cboCORType option:selected").val();
            if (_CORType === "AV") {
                //debugger;
                var _PAN = $("#txtPAN").val();
                var isSuccess = false;
                var JsonObject = {
                    MobileNo: value,
                    Applicantid: 0,
                    PAN: _PAN
                };
                $.ajax({
                    type: "POST",
                    async: false,
                    url: "../Services/ValidateMobileCorporates",
                    data: JSON.stringify(JsonObject),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        //debugger;
                        var s = '';
                        var retval = JSON.parse(msg);
                        if (retval._STATUS_ == "FAIL") {
                            $("#hdnMobileMessage").val(retval._MESSAGE_);
                            isSuccess = false;
                        }
                        else {
                            isSuccess = true;
                            $("#hdnMobileMessage").val('');
                        }
                    },
                    error: function (msg) {
                        isSuccess = false;
                        HandleAjaxError(msg);
                    }
                });
            }
            else {
                isSuccess = true;
            }
            return isSuccess;
        }, ''
    );

})