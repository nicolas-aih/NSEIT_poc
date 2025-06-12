$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

    $("#MainForm").hide();
    $("#SearchForm").show();

    $("#txtURN").change(function () {
        $("#txtExamCenter").val('');
        $("#txtLanguage").val('');
        $("#hdnExamCenterId").val('');
        $("#hdnExamLanguageId").val('');
        $("#txtExamCenterState").val('');
        $("#txtExamCenterDistrict").val('');
        $("#imgFilePhotoO").attr('src', '');
        $("#imgFileSignO").attr('src', '');
        $("#cboAllowWhatsappMessage").val('');
        $("#txtWhatsAppNo").val('');
        $("#MainForm").hide();
    })

    $("#txtDOB").datepicker({
        showMonthAfterYear: true,
        dateFormat: 'dd-M-yy',
        changeMonth: true,
        changeYear: true,
        yearRange: "1900:" + ServerYear,
    });

    $("#cboStates").change(function (event, DistrictId) {
        $("#cboDistricts").html("<option value=''>-- Select --</option>");
        $("#cboExamCenters").html("<option value=''>-- Select --</option>");
        //debugger;
        var _StateId = $("#cboStates option:selected").val();
        if (_StateId == "" || _StateId == undefined || _StateId == null) {
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
                        $("#cboDistricts").html(s);
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        });
    })

    $("#cboDistricts").change(function () {
        //debugger;
        $("#cboExamCenters").html("<option value=''>-- Select --</option>");
        var _DistrictId = $("#cboDistricts option:selected").val();
        if (_DistrictId == "" || _DistrictId == undefined || _DistrictId == null) {
            return;
        } 
        var JsonObject = {
            DistrictId: _DistrictId,
        };
        $.ajax({
            type: "POST",
            url: "../Home/GetCentersForDistricts",
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
                        alert("No Exam Centers were found for selected district");
                    }
                    else {
                        s += "<option value=''>-- Select --</option>"
                        for (i = 0; i < data.length; i++) {
                            s += "<option value=" + data[i].center_id + ">" + data[i].center_name + "</option>"
                        }
                        $("#cboExamCenters").html(s);
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        });
    })

    function readURL(input, target) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                target.attr('src', e.target.result);
            }
            reader.readAsDataURL(input.files[0]);
        }
    }
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

    jQuery.validator.addMethod(
        "ValidateWhatsAppNumber",
        function (value, element) {
            //debugger;
            if (value == undefined || value == "" || value == null) {
                return true;
            }

            var _URN = $("#hdnURN").val();
            var isSuccess = false;
            var JsonObject = {
                URN: _URN,
                MobileNo: value
            };
            $.ajax({
                type: "POST",
                async: false,
                url: "../Services/ValidateWhatsAppCorporatesForMod",
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
        "ValidateExamcenter",
        function (value, element) {
            //debugger;            
            var DistrictValue = $('#cboDistricts').val().trim();
            if ((value == "" || value == undefined || value == null) && (DistrictValue != "")) {
                return false;
            }
            else {
                return true;
            }

        }, ''
    );
    jQuery.validator.addMethod(
        "ValidateDistrict",
        function (value, element) {
            //debugger;            
            var StateValue = $('#cboStates').val().trim();
            if ((value == "" || value == undefined || value == null) && (StateValue != "")) {
                return false;
            }
            else {
                return true;
            }

        }, ''
    );
    var validator = $("#frmMain").validate({
        ignore: [],
        rules: {
            txtFilePhoto: {
                extension: "jpg|jpeg|png",
                filesize: 51200
            },
            txtFileSign: {
                extension: "jpg|jpeg|png",
                filesize: 51200
            },

            txtWhatsAppNo: {
                digits: true,
                minlength: 10,
                maxlength: 20,
                ValidateWhatsAppNumber:true
            },
            cboDistricts: {
                ValidateDistrict: true
            },
            cboExamCenters: {
                ValidateExamcenter: true
            }
        },
        messages: {
            txtFilePhoto: {
                extension: "Please upload image file (*.jpg / *.jpeg / *.png)",
                filesize: "File size must be less than 50 kilobytes"
            },
            txtFileSign: {
                extension: "Please upload image file (*.jpg / *.jpeg / *.png)",
                filesize: "File size must be less than 50 kilobytes"
            },
            txtWhatsAppNo: {
                ValidateWhatsAppNumber: function () {
                    return $("#hdnWhatsAppMessage").val();
                }
            },
            cboDistricts: {
                ValidateDistrict: "Please select District"
            },
            cboExamCenters: {
                ValidateExamcenter: "Please select Exam Center"
            }
        },
        submitHandler: function (form) {
            //debugger;
            //var datachanged; //= false;
            datachanged = false;
            var OldExamCenterId = $("#hdnExamCenterId").val();
            var OldLanguageId = $("#hdnExamLanguageId").val();
            var NewExamCenterId = $("#cboExamCenters").val();
            var NewLanguageId = $("#cboLanguage").val();
            var PhotoFile = $("#txtFilePhoto").val();
            var SignFile = $("#txtFileSign").val();
            var oldAllowWhatsappMessage = $("#lblAllowWhatsappMessage").val();
            var NewAllowWhatsappMessage = $("#cboAllowWhatsappMessage").val();
            var OldWhatsappNumber = $("#txtCurrentWhatsAppNo").val();
            var NewWhatsappNumber = $("#txtWhatsAppNo").val();

            if (PhotoFile) {
                datachanged = true;
            }
            if (SignFile) {
                datachanged = true;
            }
            if (OldExamCenterId === NewExamCenterId || NewExamCenterId == "" || NewExamCenterId == undefined || NewExamCenterId == null) {
                //No change
            }
            else {
                datachanged = true;
            }
            if (OldLanguageId === NewLanguageId || NewLanguageId == "" || NewLanguageId == undefined || NewLanguageId == null) {
                //No change
            }
            else {
                datachanged = true;
            }

            if (oldAllowWhatsappMessage === NewAllowWhatsappMessage || NewAllowWhatsappMessage == "" || NewAllowWhatsappMessage == undefined || NewAllowWhatsappMessage == null) {
                //No change
            }
            else {
                datachanged = true;
            }
            if (OldWhatsappNumber === NewWhatsappNumber || NewWhatsappNumber == "" || NewWhatsappNumber == undefined || NewWhatsappNumber == null) {
                //No change
            }
            else {
                datachanged = true;
            }

            if (!datachanged) {
                alert("No data changed");
            }
            else {
                var data = new FormData(form);
                $.ajax({
                    type: "POST",
                    enctype: 'multipart/form-data',
                    url: "../Candidates/SaveModifications",
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
                            $('#frmMain').trigger("reset");
                            $("#imgFilePhotoO").attr('src', '');
                            $("#imgFileSignO").attr('src', '');
                            $("#imgFilePhoto").attr('src', '../Images/Photo.jpg');
                            $("#imgFileSign").attr('src', '../Images/Signature.jpg');
                            $("#MainForm").hide();
                            alert(result._MESSAGE_);
                        }
                        $("#cboDistricts").html("<option value=''>-- Select --</option>");
                        $("#cboExamCenters").html("<option value=''>-- Select --</option>");
                    },
                    error: function (msg) {
                        HandleAjaxError(msg);
                    }
                })
            }
        }
    });

    $("#frmSearch").validate({
        rules: {
            txtURN: {
                required: true,
                minlength: 13,
                check_exp: regexAlphaNumeric
            },
            txtDOB: {
                required: true,
            },
        },
        messages: {
            txtURN: {
                required: MandatoryFieldMsg,
                minlength: "URN should be minimum 13 characters in length",
                check_exp: "Please enter valid URN"
            },
            txtDOB: {
                required: MandatoryFieldMsg,
            },
        },
        submitHandler: function (form) {
            //debugger;
            var _URN = $("#txtURN").val();
            var _DOB = $("#txtDOB").val();

            var JsonObject = {
                URN: _URN,
                DOB: _DOB,
            }
            $.ajax({
                type: "POST",
                url: "../Candidates/Modifications",
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
                            $('#frmMain').trigger("reset");
                            $("#imgFilePhotoO").attr('src', '');
                            $("#imgFileSignO").attr('src', '');
                            $("#imgFilePhoto").attr('src', '../Images/Photo.jpg');
                            $("#imgFileSign").attr('src', '../Images/Signature.jpg');
                            for (i = 0; i < data.length; i++) {
                                $("#txtExamCenter").val(data[i].ExamCenterName);
                                $("#txtLanguage").val(data[i].ExamLanguage);
                                $("#hdnExamCenterId").val(data[i].ExamCenterId);
                                $("#hdnExamLanguageId").val(data[i].ExamLanguageId);
                                $("#txtExamCenterState").val(data[i].state_name);
                                $("#txtExamCenterDistrict").val(data[i].district_name);
                                $("#txtCurrentMobileNumber").val(data[i].varMobileNo);
                                $("#lblAllowWhatsappMessage").val(data[i].allowwhatsapp_message);
                                $("#txtCurrentWhatsAppNo").val(data[i].whatsapp_number);
                                $("#hdnURN").val(_URN);

                                $("#imgFilePhotoO").attr('src', 'data:image/jpg;base64,' + data[i].Photo);
                                $("#imgFileSignO").attr('src', 'data:image/jpg;base64,' + data[i].Sign);
                                break;
                            }
                        }
                        $("#MainForm").show();
                    }
                },
                error: function (msg) {
                    HandleAjaxError(msg);
                }
            });
        }
    })
});