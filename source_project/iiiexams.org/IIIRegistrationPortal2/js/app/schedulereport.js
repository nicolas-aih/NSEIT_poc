$(document).ready(function () {
    $("#txtFromDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
    });

    $("#txtToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
    });

    $("#cboReportType").change(function () {
        $("#ResponseFile").html('');
    })

    $("#txtFromDate").on('blur change', function () {
        $("#ResponseFile").html('');
    });

    $("#txtToDate").on('blur change', function () {
        $("#ResponseFile").html('');
    });

    jQuery.validator.addMethod(
        "ValidateTillDate",
        function (value, element) {
            var _FromDate = $("#txtFromDate").val();
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
        }, "The till date must be greater than or equal to from date."
    );

    $("#frmMain").validate({
        rules:
        {
            cboReportType: {
                required: true
            },
            txtFromDate: {
                required: true
            },
            txtToDate: {
                required: true,
                ValidateTillDate: true
            }
        },
        messages:
        {
            cboReportType: {
                required: MandatoryFieldMsg
            },
            txtFromDate: {
                required: MandatoryFieldMsg
            },
            txtToDate: {
                required: MandatoryFieldMsg,
                //ValidateTillDate: true
            }
        },
        submitHandler: function (form) {
            var data = new FormData(form);
            $.ajax({
                type: "POST",
                url: "../Reports/ScheduleReport",
                enctype: 'multipart/form-data',
                data: data,
                processData: false,
                contentType: false,
                cache: false,
                success: function (msg) {
                    var Result = JSON.parse(msg);
                    if (Result._STATUS_ == "SUCCESS") {
                        if (Result._RESPONSE_FILE_) {
                            $("#ResponseFile").html('');
                            $("#ResponseFile").html("<label><a href='" + Result._RESPONSE_FILE_ + "'>Download Response File</a></label>");
                        }
                        alert(Result._MESSAGE_);
                    }
                    else {
                        alert(Result._MESSAGE_);
                    }
                }
            })
        }
    })
})