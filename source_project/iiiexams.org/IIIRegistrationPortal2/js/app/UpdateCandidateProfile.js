$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

    $("#cboUpdateAction").change(function () {
        var ActionName = $('#cboUpdateAction').val();
        if (ActionName == "") {
            $("#UpdateDiv").hide();
            return false;
        }
        //removing rules
        $("#txtValue").rules("remove", "check_exp number email digits");
        $("#txtValue").val("");
        $("#txtURN").val("")
        $("#Result").hide();
        $("#txtValue").prop('readonly', false);
        $("#txtValue").removeAttr('style');


        if (ActionName == "Update_Name") {
            $("#UpdateDiv").show();
            $('#txtValue').show();
            $('#labUpdate').html("Candidate's Name : <span class='mandatory'>*</span>");
            $("#txtValue").datepicker("destroy");            
            $("#txtValue").rules("add", { check_exp: regexAlphaOnlyWithSpace, });
            $('#cboLanguage').hide();

        }
        else if (ActionName == "Update_Fathersname") {
            $("#UpdateDiv").show();
            $('#txtValue').show();
            $('#labUpdate').html("Father's / Husband's Name : <span class='mandatory'>*</span>");
            $("#txtValue").datepicker("destroy");
            $("#txtValue").rules("add", { check_exp: regexAlphaOnlyWithSpace, });
            $('#cboLanguage').hide();

        }
        else if (ActionName == "Update_DOB") {
            $("#UpdateDiv").show();
            $('#txtValue').show();
            $('#labUpdate').html("Date of Birth : <span class='mandatory'>*</span>");
            $("#txtValue").prop('readonly', true);
            $("#txtValue").datepicker({
                showMonthAfterYear: true,
                dateFormat: 'dd-M-yy',
                changeMonth: true,
                changeYear: true,
                yearRange: "1900:" + ServerYear,
                onClose: function () {           
                    $(this).valid();                    
                }
            });
            $("#txtValue").rules("add", { ValidateDOB: true });
            $("#txtValue").attr('style', 'background-color:white;');
            $('#cboLanguage').hide();

        }
        else if (ActionName == "Update_Email") {
            $("#UpdateDiv").show();
            $('#txtValue').show();
            $('#labUpdate').html("Email Id : <span class='mandatory'>*</span>");
            $("#txtValue").datepicker("destroy");
            $("#txtValue").rules("add", { email: true, });
            $('#cboLanguage').hide();

        }
        else if (ActionName == "Update_Mobile") {
            $("#UpdateDiv").show();
            $('#txtValue').show();
            $('#labUpdate').html("Mobile No. : <span class='mandatory'>*</span>");
            $("#txtValue").datepicker("destroy");
            $("#txtValue").rules("add", { digits: true});
            $('#cboLanguage').hide();

        }
        else if (ActionName == "Update_PAN") {
            $("#UpdateDiv").show();
            $('#txtValue').show();
            $('#labUpdate').html("PAN : <span class='mandatory'>*</span>");
            $("#txtValue").datepicker("destroy");
            $("#txtValue").rules("add", { check_exp: regexPAN });
            $('#cboLanguage').hide();
            $("#txtValue").attr('style', 'text-transform: uppercase;');

        }
        else if (ActionName == "Update_Lang") {
            $("#UpdateDiv").show();
            $('#txtValue').hide();
            $('#labUpdate').html("Examination Language :  <span class='mandatory'>*</span>");
            $("#txtValue").datepicker("destroy");
            $('#cboLanguage').show();
        }

        $("#txtURN").rules("add", {
            required: true,
            minlength: 13,
            check_exp: regexAlphaNumeric });
            
    });

    $("#cboUpdateAction").trigger("change");

    jQuery.validator.addMethod(
        "ValidateDOB",
        function (value, element) {            
            if (value == undefined || value == "" || value == null) {
                return true;
            }
            if ($("#cboUpdateAction option:selected").val() != "Update_DOB") {
                return true;
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
        "ValidateEMail",
        function (value, element) {
            //debugger;
            if (value == undefined || value == "" || value == null) {
                return true;
            }
            var _PAN = $("#txtPAN").val();
            var isSuccess = false;
            var JsonObject = {
                EmailId: value,
                Applicantid: 0,
                PAN: _PAN
            };
            $.ajax({
                type: "POST",
                async: false,
                url: "../Services/ValidateEmailCorporates",
                data: JSON.stringify(JsonObject),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    //debugger;
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
    var validator = $("#frmMain").validate({
        rules: {
            txtURN: {
                required: true,
                minlength: 13,
                check_exp: regexAlphaNumeric
            },
            txtValue: {         
                required: true                
                
            },
            cboLanguage:{
                required: function () {
                    return $("#cboUpdateAction option:selected").val() == "Update_Lang";
                }
            }
        },
        messages: {
            txtURN: {
                required: MandatoryFieldMsg,
                minlength: "URN should be minimum 13 characters in length",
                check_exp: "Please enter valid URN"                
            },
            txtValue: {
                required: MandatoryFieldMsg,                
                txtPAN: "Please enter valid PAN", 
                digits: ValidMobileMsg,
                email: ValidEmailIdMsg,
                check_exp: "Please enter valid value"
            },
            cboLanguage: {
                required: MandatoryFieldMsg
            }

        },
        submitHandler: function (form) {                                         
            var data = new FormData(form);
            var x = '';
            $.ajax({
                type: "POST",
                enctype: 'multipart/form-data',
                url: "../Utility/SaveCandidateProfile",
                data: data,
                processData: false,
                contentType: false,
                cache: false,
                success: function (msg) {
                    //debugger;
                    var s = '';
                    var Result = JSON.parse(msg);
                    if (Result._STATUS_ === "SUCCESS") {

                        if (Result.URN === undefined) {
                            x = '<H4>' + Result._MESSAGE_;
                        }
                        else {
                            x = '<H4>' + Result._MESSAGE_ + '<br></H4>';
                        }

                        $("#Result").html(x);
                        $("#Result").show();
                        form.reset();                        
                    }
                    else {
                        x = Result._MESSAGE_;
                        $("#Result").html(x);
                        $("#Result").show();
                    }
                },
                error: function (msg) {
                    HandleAjaxError(msg);
                }
            });
        }
    });
});
