$(document).ready(function () {
    $("#txtReconDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
    });

    $("#cboReportType").change(function () {
        $("#ResponseFile").html('');
    })

    $("#txtReconDate").on('blur change', function () {
        $("#ResponseFile").html('');
    });


    jQuery.validator.addMethod(
        "ValidateTillDate",
        function (value, element) {
            var _FromDate = $("#txtReconDate").val();
            if (_FromDate == undefined || _FromDate == "" || _FromDate == null) {
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
            txtReconDate: {
                required: true
            }
        },
        messages:
        {
            txtReconDate: {
                required: MandatoryFieldMsg
            }
        },
        submitHandler: function (form) {
            var data = new FormData(form);
            $.ajax({
                type: "POST",
                url: "../Scheduler/ReconcileBooking",
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
                            $("#ResponseFile").html("<label><a href='" + Result._RESPONSE_FILE_ + "'>Download Reconciliation File</a></label>");
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