$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

    $('#txtAppDateFrom').val(ServerDate);
    $('#txtAppDateTo').val(ServerDate);
    $("#txtAppDateFrom").datepicker({
        showMonthAfterYear: true,
        dateFormat: 'dd-M-yy',
        changeMonth: true,
        changeYear: true,
        yearRange: '-10:+10'
    });
    $("#txtAppDateTo").datepicker({
        showMonthAfterYear: true,
        dateFormat: 'dd-M-yy',
        changeMonth: true,
        changeYear: true,
        yearRange: '-10:+10'
    });
    $("#txtExamDateFrom").datepicker({
        showMonthAfterYear: true,
        dateFormat: 'dd-M-yy',
        changeMonth: true,
        changeYear: true,
        yearRange: '-10:+10'
    });
    $("#txtExamDateTo").datepicker({
        showMonthAfterYear: true,
        dateFormat: 'dd-M-yy',
        changeMonth: true,
        changeYear: true,
        yearRange: '-10:+10'
    });

    //txtAppDateFrom
    //txtAppDateTo
    jQuery.validator.addMethod(
        "ValidateApplicantDate",
        function (value, element) {
            var _FromDate = $("#txtAppDateFrom").val();
            if (_FromDate == undefined || _FromDate == "" || _FromDate == null) {
                return true;
            }
            if (value == undefined || value == "" || value == null) {
                return true;
            }
            var isSuccess = false;
            var JsonObject = {
                FromDate: _FromDate,
                TillDate: value
            };
            $.ajax({
                type: "POST",
                async: false,
                url: "../Services/ValidateFromTillDate",
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
        }, "The Application date till must be greater than or equal to Application from date."
    );

    //txtExamDateFrom
    //txtExamDateTo
    jQuery.validator.addMethod(
        "ValidateExamDate",
        function (value, element) {
            var _FromDate = $("#txtExamDateFrom").val();
            if (_FromDate == undefined || _FromDate == "" || _FromDate == null) {
                return true;
            }
            if (value == undefined || value == "" || value == null) {
                return true;
            }
            var isSuccess = false;
            var JsonObject = {
                FromDate: _FromDate,
                TillDate: value
            };
            $.ajax({
                type: "POST",
                async: false,
                url: "../Services/ValidateFromTillDate",
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
        }, "The Exam date till must be greater than or equal to Exam from date."
    );

    $("#frmMain").validate({
        rules:
        {
            txtAppDateFrom: {
                required: true,
            },
            txtAppDateTo: {
                required: true,
                ValidateApplicantDate: true
            },
            txtExamDateTo: {
                ValidateExamDate: true
            },
            cboApplicationStatus: {
                required: true,
            },
        },
        messages:
        {
            txtAppDateFrom: {
                required: MandatoryFieldMsg,
            },
            txtAppDateTo: {
                required: MandatoryFieldMsg,
            },
            cboApplicationStatus: {
                required: MandatoryFieldMsg,
            },
        },
        submitHandler: function (form) {
            //debugger;
            var data = new FormData(form);

            $.ajax({
                type: "POST",
                enctype: 'multipart/form-data',
                url: "../Reports/SponsorshipStatusP",
                data: data,
                processData: false,
                contentType: false,
                cache: false,
                success: function (msg) {
                    debugger;
                    var s = '';
                    var Result = JSON.parse(msg);
                    if (Result._STATUS_ == "SUCCESS") {
                        if (Result._RESPONSE_FILE_) {
                            $("#ResponseFile").html('');
                            $("#ResponseFile").html("<label><a href='" + Result._RESPONSE_FILE_ + "'>Download Report</a></label>");
                        }
                        alert(Result._MESSAGE_);
                    }
                    else {
                        alert(Result._MESSAGE_);
                    }
                    //form.reset();
                },
                error: function (msg) {
                    debugger;
                    HandleAjaxError(msg);
                }
            });
        }
    });
});