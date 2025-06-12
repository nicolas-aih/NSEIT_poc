$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

    $("#divOtherQualification").hide(); //?

    var DistrictDetails_perm = "";
    var DistrictDetails_current = ""; 
    var PANMessage = "";

    function readURL(input, target) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                target.attr('src', e.target.result);
            }
            reader.readAsDataURL(input.files[0]);
        }
    }

    //$("#frmSearch").show();
    $("#frmMain").hide();

    LoadData();

    function LoadData() {
        var Id = $("#hdnId").val();
        if (Id === "") {
            alert("Error Occured : Unable to proceed");
        }

        var data = new FormData();
        data.append("Id", Id);
        $.ajax({
            type: "POST",
            enctype: 'multipart/form-data',
            url: "../Candidates/URNRequestModification",
            data: data,
            processData: false,
            contentType: false,
            cache: false,
            success: function (msg) {
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
                        $('#frmMain').trigger("reset");
                        for (i = 0; i < data.length; i++) {
                            $("#txtHouseNo_perm").removeAttr("disabled");
                            $("#txtStreet_perm").removeAttr("disabled");
                            $("#txtCity_perm").removeAttr("disabled");
                            $("#cboStates_perm").removeAttr("disabled");
                            $("#cboDistricts_perm").removeAttr("disabled");
                            $("#txtPincode_perm").removeAttr("disabled");

                            //$("#hdnApplicantId").val(data[i].bntApplicantID);
                            //$("#hdnURN").val(data[i].chrRollNumber);
                            $("#txtApplicationDate").val(data[i].dtApplicationDate_T);
                            $("#cboInsuranceCategory").val(data[i].chrInsuranceCategory);
                            $("#hdnInsuranceCategory").val(data[i].chrInsuranceCategory);
                            $("#cboCORType").val(data[i].CorporateType);
                            $("#cboCORType").trigger("change", [data[i].chrBasicQualification.trim(), data[i].chrQualification.trim(), data[i].tm_id]);
                            $("#hdnCORType").val(data[i].CorporateType);
                            $("#cboSalutation").val(data[i].chrNameInitial.trim());
                            $("#cboSalutation").trigger("change");
                            $("#txtApplicantName").val(data[i].varApplicantName);
                            $("#txtFHName").val(data[i].varApplicantFatherName);
                            $("#txtDOB").val(data[i].dtApplicantDOB_T);
                            $("#cboGender").val(data[i].chrSex.trim());
                            $("#cboCategory").val(data[i].tntCategory);
                            $("#cboArea").val(data[i].chrUrbanRural.trim());
                            $("#cboNationality").val(data[i].sntNationalityCountryID);
                            $("#txtPAN").val(data[i].varPAN);
                            $("#txtDrivingLic").val(data[i].varDrivingLicenseNo);
                            $("#txtPassport").val(data[i].varPassportNo);
                            $("#txtVoterId").val(data[i].varVoterID);
                            $("#txtPhotoId").val(data[i].varGovtIDCard);
                            //$("#imgFilePhoto").val(data[i].);
                            //$("#imgFileSign").val(data[i].);
                            //$("#cboBasicQualification").val(data[i].chrBasicQualification.trim());
                            $("#txtBoardName").val(data[i].varBasicQualBoardName);
                            $("#txtRollnumber").val(data[i].varBasicQualRollNumber);
                            //$("#txtYearofPassing").val(data[i].dtBasicQualYearOfPassing_T);

                            var d = new Date(data[i].dtBasicQualYearOfPassing_T);
                            $("#txtYearofPassing").datepicker();
                            $("#txtYearofPassing").datepicker("setDate", d);

                            $("#cboProfessionalQualification").val(data[i].chrQualification.trim());
                            $("#cboProfessionalQualification").trigger("change");

                            $("#txtHouseNo_current").val(data[i].varCurrHouseNo);
                            $("#txtStreet_current").val(data[i].varCurrStreet);
                            $("#txtCity_current").val(data[i].varCurrTown);
                            $("#cboStates_current").val(data[i].CurrStateId); // trigger change
                            $("#cboStates_current").trigger("change", [data[i].sntCurrDistrictID, data[i].intCurrPINCode, data[i].sntExamCenterID]);
                            //$("#cboDistricts_current").val(data[i].sntCurrDistrictID);
                            //$("#txtPincode_current").val(data[i].intCurrPINCode);
                            $("#txtHouseNo_perm").val(data[i].varPermHouseNo);
                            $("#txtStreet_perm").val(data[i].varPermStreet);
                            $("#txtCity_perm").val(data[i].varPermTown);
                            $("#cboStates_perm").val(data[i].PermStateId); // trigger change
                            $("#cboStates_perm").trigger("change", [data[i].sntPermDistrictID, data[i].intPermPINCode]);
                            //$("#cboDistricts_perm").val(data[i].sntPermDistrictID);
                            //$("#txtPincode_perm").val(data[i].intPermPINCode);
                            $("#txtLandlineNo").val(data[i].varPhoneNo);
                            $("#txtMobileNo").val(data[i].varMobileNo);
                            $("#cboAllowWhatsappMessage").val(data[i].allowwhatsapp_message);
                            $("#cboAllowWhatsappMessage").trigger("change");

                            $("#txtWhatsAppNo").val(data[i].whatsapp_number);
                            $("#txtEMailId").val(data[i].varEmailID);
                            $("#txtCPEMailId").val(data[i].ContactPersonEmailId);
                            $("#txtPrimaryOccupation").val(data[i].varProfessionBusiness);
                            $("#txtEmployeeCode").val(data[i].varInsurerExtnRefNo);
                            $("#cboBranchState").val(data[i].BranchStateId);
                            $("#cboBranchState").trigger("change", [data[i].BranchDistrictId, data[i].intBranchNo]);
                            //$("#cboBranchDistrict").val(data[i].BranchDistrictId);
                            //$("#cboBranch").val(data[i].intBranchNo);
                            $("#cboExamBody").val(5);
                            //$("#txtPincode_current").trigger("blur", data[i].sntExamCenterID);
                            //$("#cboExamCenter").val(data[i].sntExamCenterID);
                            $("#cboLanguage").val(data[i].tntExamLanguageID);



                            $("#imgFilePhoto").attr('src', 'data:image/jpg;base64,' + data[i].Photo);
                            $("#imgFileSign").attr('src', 'data:image/jpg;base64,' + data[i].Sign);
                            ChangeMobileMaxLength();
                            break;
                        }
                        $("#txtURN").val('');
                    }
                    $("#frmMain").show();
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        });
    }

    jQuery.validator.addMethod(
        "ValidateQualificationDoc",
        function (value, element) {
            var COR = $("#cboCORType option:selected").val();
            var Qal = $("#cboProfessionalQualification option:selected").val();
            //$("#hdnQualDoc").val('');
            if (COR === "PO") {
                if (Qal === "NA" || Qal === '') {
                    return true;
                }
                else {
                    if (value === null || value === undefined || value === "") {
                        //$("#hdnQualDoc").val(MandatoryFieldMsg);
                        return false;
                    }
                    else {
                        return true;
                    }
                }
            }
            else {
                return true;
            }
        }, ''
    );

    jQuery.validator.addMethod(
        "ValidateBasicQualificationDoc",
        function (value, element) {
            var COR = $("#cboCORType option:selected").val();
            //$("#hdnQualDoc").val('');
            if (COR === "PO") {
                if (value === null || value === undefined || value === "") {
                    //$("#hdnQualDoc").val(MandatoryFieldMsg);
                    return false;
                }
                else {
                    return true;
                }
            }
            else {
                return true;
            }
        }, '');

    function DisplayHideBasicQualificationDoc() {
        var COR = $("#cboCORType option:selected").val();
        //Basic Qualification 
        if (COR === "PO") {
            $("#divBasicQualificationDoc").show();
        }
        else {
            $("#divBasicQualificationDoc").hide();
        }
    }

    function DisplayHideQualificationDoc() {
        var COR = $("#cboCORType option:selected").val();
        var Qal = $("#cboProfessionalQualification option:selected").val();
        //Professional Qualification
        if (COR === "PO" && !(Qal === "NA" || Qal === "")) {
            $("#divQualificationDoc").show();
        }
        else {
            $("#divQualificationDoc").hide();
        }
    }

    /*Urn specific*/
    jQuery.validator.addMethod(
        "ValidateDOB",
        function (value, element) {
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
        }, 'The candidate must be minimum 18 years old'
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

    jQuery.validator.addMethod(
        "IsPANValid",
        function (value, element) {
            //debugger;
            PANMessage = "";
            if (value == undefined || value == "" || value == null) {
                return true;
            }
            //var _ApplicantId = $("#hdnApplicantId").val();
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
        "IsDebarred",
        function (value, element) {
            if (value == undefined || value == "" || value == null) {
                return true;
            }
            var isSuccess = false;
            var JsonObject = {
                PAN: value,
            };
            $.ajax({
                type: "POST",
                async: false,
                url: "../Services/ValidatePAN",
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
        }, 'The Individual with this PAN is debarred'
    );

    jQuery.validator.addMethod(
        "ValidateEMail",
        function (value, element) {
            //debugger;
            if (value == undefined || value == "" || value == null) {
                return true;
            }
            var _PAN = $("#txtPAN").val();
            var Id = $("#hdnId").val();
            var isSuccess = false;
            var JsonObject = {
                EmailId: value,
                ApplicantDataId: Id,
                PAN: _PAN
            };
            $.ajax({
                type: "POST",
                async: false,
                url: "../Services/ValidateEmailCorporatesApp",
                data: JSON.stringify(JsonObject),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var s = '';
                    var retval = JSON.parse(msg);
                    if (retval._STATUS_ == "FAIL") {
                        $("#hdnEmailMessage").val(retval._MESSAGE_);
                        isSuccess = false;
                    }
                    else {
                        isSuccess = true;
                        $("#hdnEmailMessage").val('');
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
        "ValidateMobile",
        function (value, element) {
            //debugger;
            if (value == undefined || value == "" || value == null) {
                return true;
            }
            var _PAN = $("#txtPAN").val();
            var Id = $("#hdnId").val();
            var isSuccess = false;
            var JsonObject = {
                MobileNo: value,
                ApplicantDataId: Id,
                PAN: _PAN
            };
            $.ajax({
                type: "POST",
                async: false,
                url: "../Services/ValidateMobileCorporatesApp",
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
            return isSuccess;
        }, ''
    );

    jQuery.validator.addMethod(
        "ValidateWhatsAppNumber",
        function (value, element) {
            //debugger;
            if (value == undefined || value == "" || value == null) {
                return true;
            }
            var _PAN = $("#txtPAN").val();
            var Id = $("#hdnId").val();
            var isSuccess = false;
            var JsonObject = {
                MobileNo: value,
                ApplicantDataId: Id,
                PAN: _PAN
            };
            $.ajax({
                type: "POST",
                async: false,
                url: "../Services/ValidateWhatsAppCorporatesApp",
                data: JSON.stringify(JsonObject),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    //debugger;
                    var s = '';
                    var retval = JSON.parse(msg);
                    if (retval._STATUS_ == "FAIL") {
                        $("#hdnWhatsAppMessage").val(retval._MESSAGE_);
                        isSuccess = false;
                    }
                    else {
                        isSuccess = true;
                        $("#hdnWhatsAppMessage").val('');
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
            //debugger;
            var isSuccess = false;
            var _InternalRefNo = $("#txtEmployeeCode").val();
            var Id = $("#hdnId").val();
            var JsonObject = {
                InternalRefNo: _InternalRefNo,
                ApplicantDataId: Id
            };
            $.ajax({
                type: "POST",
                async: false,
                url: "../Services/ValidateInternalRefNoApp",
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

    $("#txtBasicQualificationDoc").change(function () {
        $(this).valid();
    });

    $("#txtQualificationDoc").change(function () {
        $(this).valid();
    });

    $("#cboProfessionalQualification").change(function () {
        DisplayHideQualificationDoc();
    });


    $("#txtFilePhoto").change(function () {
        var file = this.files[0];
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

    $("#cboCORType").change(function (event, BasicQualification, ProfessionalQualification, Telemarketer) {
        $("#cboBasicQualification").html('');
        var _CORType = $("#cboCORType option:selected").val();
        if (_CORType == "") {
            $("#cboBasicQualification").html("<option value=''>-- Select --</option>");
            $("#cboProfessionalQualification").html("<option value=''>-- Select --</option>");
            //$("#cboTelemarketer").html("<option value=''>-- Select --</option>");
            return;
        }
        //if (_CORType === 'AV') {
        //    $("#divTelemarketer").show();
        //}
        //else {
        //    $("#divTelemarketer").hide();
        //}


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
                        if (BasicQualification === "" || BasicQualification === null) {

                        }
                        else {
                            $("#cboBasicQualification").val(BasicQualification);
                            $("#cboBasicQualification").trigger("change");
                        }
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
                        if (ProfessionalQualification === "" || ProfessionalQualification === null) {

                        }
                        else {
                            $("#cboProfessionalQualification").val(ProfessionalQualification);
                            $("#cboProfessionalQualification").trigger("change");
                        }
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
                    //        if (Telemarketer === "" || Telemarketer === null) {

                    //        }
                    //        else {
                    //            $("#cboTelemarketer").val(Telemarketer);
                    //            //$("#cboTelemarketer").trigger("change");
                    //        }
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
    $("#cboStates_current").change(function (event, DistrictId, Pincode, ExamCenter) {
        //debugger;
        var _ExamCenter = ExamCenter;
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
                            $("#cboDistricts_current").trigger("change", [Pincode, _ExamCenter]);
                            $("#cboDistricts_current").valid();
                        }
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        });
    });

    $("#cboStates_perm").change(function (event, DistrictId, Pincode) {
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
                            $("#cboDistricts_perm").trigger("change", Pincode);
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

    $("#cboDistricts_current").change(function (event, Pincode, ExamCenter) {
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
            if (Pincode != undefined) {
                $("#txtPincode_current").val(Pincode);
                $("#txtPincode_current").valid();
                $("#txtPincode_current").trigger("blur", ExamCenter);
            }
        }
    })

    $("#cboDistricts_perm").change(function (event, Pincode) {
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
            if (Pincode != undefined) {
                $("#txtPincode_perm").val(Pincode);
                $("#txtPincode_perm").valid();
            }
        }
    })

    $("#cboBranchState").change(function (event, DistrictId, CenterId) {
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
                            $("#cboBranchDistrict").trigger("change", CenterId);
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
                                    if (ExamCenter != undefined && data[i].sntExamCenterID == ExamCenter) {
                                        s += "<option value=" + data[i].sntExamCenterID + "|" + data[i].numDistance + " selected>" + data[i].varExamCenterName + "</option>";
                                    }
                                    else {
                                        s += "<option value=" + data[i].sntExamCenterID + "|" + data[i].numDistance + ">" + data[i].varExamCenterName + "</option>";
                                    }
                                }
                            }
                            s += "</optgroup >"
                            s += "<optgroup  label='--NSEIT-Ext-Centers--'>"
                            for (i = 0; i < data.length; i++) {
                                if (data[i].center_type === 'TAB') {
                                    if (ExamCenter != undefined && data[i].sntExamCenterID == ExamCenter) {
                                        s += "<option value=" + data[i].sntExamCenterID + "|" + data[i].numDistance + " selected>" + data[i].varExamCenterName + "</option>";
                                    }
                                    else {
                                        s += "<option value=" + data[i].sntExamCenterID + "|" + data[i].numDistance + ">" + data[i].varExamCenterName + "</option>";
                                    }
                                }
                            }
                            s += "</optgroup >"
                            s += "<optgroup  label='--NSEIT Remotely Proctored Center--'>"
                            for (i = 0; i < data.length; i++) {
                                if (data[i].center_type === 'RP') {
                                    if (ExamCenter != undefined && data[i].sntExamCenterID == ExamCenter) {
                                        s += "<option value=" + data[i].sntExamCenterID + "|" + data[i].numDistance + " selected>" + data[i].varExamCenterName + "</option>";
                                    }
                                    else {
                                        s += "<option value=" + data[i].sntExamCenterID + "|" + data[i].numDistance + ">" + data[i].varExamCenterName + "</option>";
                                    }
                                }
                            }
                            s += "</optgroup >"
                        }
                        $("#cboExamCenter").html(s);
                        $("#cboExamCenter").trigger("change");
                        $("#cboExamCenter").valid();
                        //if (ExamCenter != undefined) {
                        //    $("#cboExamCenter").val(ExamCenter);
                        //}
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
                    //required: true,
                    extension: "jpg|jpeg|png",
                    filesize: 51200
                },
                txtFileSign: {
                    //required: true,
                    extension: "jpg|jpeg|png",
                    filesize: 51200
                },
                txtQualificationDoc: {
                    //required: true, 
                    extension: "jpg|jpeg",
                    filesize: 51200
                    //ValidateQualificationDoc: true
                },
                txtBasicQualificationDoc: {
                    //required: true,
                    extension: "jpg|jpeg",
                    filesize: 51200
                    //ValidateBasicQualificationDoc: true
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
                    check_exp: regexLowAscii,
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
                    ValidatePincodePerm : true
                },
                txtLandlineNo: {
                    digits: true,
                },
                txtMobileNo: {
                    required: true,
                    digits: true,
                    minlength: function() {
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
                    },
                    ValidateWhatsAppNumber: true
                },
                txtEMailId: {
                    required: true,
                    email: true,
                    ValidateEMail: true
                },
                txtCPEMailId: {
                    required: true,
                    email: true,
                    NotEqualTo: "#txtEMailId"
                },
                txtPrimaryOccupation: {
                    required: true,
                    check_exp: regexAlphaNumericWithSpace,
                },
                txtEmployeeCode: {
                    required: true,
                    check_exp: regexAlphaNumeric,
                    IsUnique: true
                },
                //cboTelemarketer: {
                //    required: function () {
                //        return $("#cboCORType option:selected").val() == "AV";
                //    }
                //},
                cboBranchState: {
                    required: true,
                },
                cboBranchDistrict: {
                    required: true,
                },
                cboBranch: {
                    required: true,
                },
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
                    //required: "Please select photo",
                    extension: "Please upload image file (*.jpg / *.jpeg / *.png)",
                    filesize: "File size must be less than 50 kilobytes"
                },
                txtFileSign: {
                    //required: "Please select signature",
                    extension: "Please upload image file (*.jpg / *.jpeg / *.png)",
                    filesize: "File size must be less than 50 kilobytes"
                },
                txtQualificationDoc: {
                    //required: "Please select the scanned copy of the qualification certificate", 
                    extension: "Please upload image file (*.jpg / *.jpeg)",
                    filesize: "File size must be less than 50 kilobytes",
                    ValidateQualificationDoc: "Please select the supporting document for the selected professional qualification"
                },
                txtBasicQualificationDoc: {
                    //required: "Please select the scanned copy of the qualification certificate",
                    extension: "Please upload image file (*.jpg / *.jpeg)",
                    filesize: "File size must be less than 50 kilobytes",
                    ValidateBasicQualificationDoc: "Please select the supporting document for the selected basic qualification"
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
                    check_exp: "Please enter valid address without leading / trailing space",
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
                    minlength: ValidMobileMsg,
                    ValidateMobile: function () {
                        return $("#hdnMobileMessage").val();
                    }
                },
                cboAllowWhatsappMessage: {
                    required: MandatoryFieldMsg
                },
                txtWhatsAppNo: {
                    minlength: ValidMobileMsg,
                    ValidateWhatsAppNumber: function () {
                        return $("#hdnWhatsAppMessage").val();
                    }
                },
                txtEMailId: {
                    required: MandatoryFieldMsg,
                    email: ValidEmailIdMsg,
                    ValidateEMail: function () {
                        return $("#hdnEmailMessage").val();
                    }
                },
                txtCPEMailId: {
                    required: MandatoryFieldMsg,
                    email: ValidEmailIdMsg,
                    NotEqualTo: "Candidate Email ID and Contact Person Email ID should not be same",
                    //NotEqualTo: "The contact person's email id should be different than candidate's email id"
                },
                txtPrimaryOccupation: {
                    required: MandatoryFieldMsg,
                    check_exp: "Only alphabets, numbers and space without leading / trailing space are allowed",
                },
                txtEmployeeCode: {
                    required: MandatoryFieldMsg,
                    check_exp: "Only alphabets and numbers without leading / trailing space are allowed",
                },
                //cboTelemarketer: {
                //    required: MandatoryFieldMsg
                //},
                cboBranchState: {
                    required: MandatoryFieldMsg,
                },
                cboBranchDistrict: {
                    required: MandatoryFieldMsg,
                },
                cboBranch: {
                    required: MandatoryFieldMsg,
                },
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
                    error.insertAfter(element);
                }
            },
            invalidHandler: function (event, validator) {
                alert("Unable to save data. Some of the fields have errors (The same are highlighted).");
            },
            submitHandler: function (form) {
                var cor = $("#cboCORType option:selected").val();
                var qual = $("#cboProfessionalQualification option:selected").val();
                if (cor === "PO") {
                    if (qual === "NA") {
                        //true
                    }
                    else {
                        if (confirm("You have selected Educational Qualification as " + $("#cboProfessionalQualification option:selected").text() + " . Your request will be sent to Insurance Institute of India (III). III will verify the certificate and then approve or reject the request. This process will take minimum 2-3 working days.") == false) {
                            return;
                        }
                        //else... true
                    }
                }//else... true

                var data = new FormData(form);
                $.ajax({
                    type: "POST",
                    enctype: 'multipart/form-data',
                    url: "../Candidates/SaveURNRequestModification",
                    data: data,
                    processData: false,
                    contentType: false,
                    cache: false,
                    success: function (msg) {
                        var s = '';
                        var Result = JSON.parse(msg);
                        if (Result._STATUS_ === "SUCCESS") {

                            if (Result.URN === undefined || Result.URN === "") {
                                alert(Result._MESSAGE_);
                                //redirection...
                                window.location = "../Candidates/URNRequestApproval";
                            }
                            else {
                                x = '<H4>' + Result._MESSAGE_ + '<br>' + 'The URN no generated is : ' + Result.URN + '</H4>';
                                $("#Result").html(x);
                                $("#Result").show();
                                form.reset();
                                $("#frmMain").hide();
                            }
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

    $("#txtHouseNo_current").on('keyup keypress blur change',function () {
        if ($("#chkSameAsCurrent").prop("checked")) {
            $("#txtHouseNo_perm").val($("#txtHouseNo_current").val());
            $("#txtHouseNo_perm").valid();
        }
    });
    $("#txtStreet_current").on('keyup keypress blur change',function () {
        if ($("#chkSameAsCurrent").prop("checked")) {
            $("#txtStreet_perm").val($("#txtStreet_current").val());
            $("#txtStreet_perm").valid();
        }
    });
    $("#txtCity_current").on('keyup keypress blur change',function () {
        if ($("#chkSameAsCurrent").prop("checked")) {
            $("#txtCity_perm").val($("#txtCity_current").val());
            $("#txtCity_perm").valid();
        }
    });
    $("#cboStates_current").change(function () {
        if ($("#chkSameAsCurrent").prop("checked")) {
            $("#cboStates_perm").val($("#cboStates_current option:selected").val());
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
    $("#txtPincode_current").on('keyup keypress blur change',function () {
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
    });
    function ChangeMobileMaxLength() {
        if ($("#cboNationality option:selected").text().toUpperCase() == 'INDIAN') {
            $("#txtMobileNo").attr('maxlength', '10');
            $("#txtWhatsAppNo").attr('maxlength', '10');
        }
        else {
            $("#txtMobileNo").attr('maxlength', '20');
            $("#txtWhatsAppNo").attr('maxlength', '20');
        }
    }
    $("#cboNationality").change(function () {
        ChangeMobileMaxLength();
    });

    $("#txtURN").on("keyup keydown blur change", function () {
        if ($("#txtURN").val() == undefined || $("#txtURN").val() == "" || $("#txtURN").val() == null) {

        }
        else {
            $("#frmMain").trigger("reset");
            $("#frmMain").hide();
        }
    });

    window.q = function (Id) {
        var data = new FormData();
        data.append("Id", Id);
        $.ajax({
            type: "POST",
            url: "../Candidates/DownloadQD",
            enctype: 'multipart/form-data',
            data: data,
            processData: false,
            contentType: false,
            cache: false,
            success: function (msg) {
                var Result = JSON.parse(msg);
                if (Result._STATUS_ === "SUCCESS") {
                    if (Result._RESPONSE_FILE_) {
                        window.open(Result._RESPONSE_FILE_);
                    }
                    //alert(Result._MESSAGE_);
                }
                else {
                    alert(Result._MESSAGE_);
                }
            }
        });
    }

    window.r = function (Id) {
        var data = new FormData();
        data.append("Id", Id);
        $.ajax({
            type: "POST",
            url: "../Candidates/DownloadBQD",
            enctype: 'multipart/form-data',
            data: data,
            processData: false,
            contentType: false,
            cache: false,
            success: function (msg) {
                var Result = JSON.parse(msg);
                if (Result._STATUS_ === "SUCCESS") {
                    if (Result._RESPONSE_FILE_) {
                        window.open(Result._RESPONSE_FILE_);
                    }
                    //alert(Result._MESSAGE_);
                }
                else {
                    alert(Result._MESSAGE_);
                }
            }
        });
    };

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
});