
$(document).ready(function () {
    //$("#txtFromDate").datepicker({
    //    showMonthAfterYear: true,
    //    dateFormat: 'dd-M-yy',
    //    changeMonth: true,
    //    changeYear: true,
    //    yearRange: "1900:" + ServerYear,
    //    onClose: function () {
    //        $(this).valid();
    //    }
    //});

    //$("#txtToDate").datepicker({
    //    showMonthAfterYear: true,
    //    dateFormat: 'dd-M-yy',
    //    changeMonth: true,
    //    changeYear: true,
    //    yearRange: "1900:" + ServerYear,
    //    onClose: function () {
    //        $(this).valid();
    //    }
    //});

    //$("#txtFromDate").on('blur change', function () {
    //    $("#ResponseFile").html('');
    //});

    //$("#txtToDate").on('blur change', function () {
    //    $("#ResponseFile").html('');
    //});

    //jQuery.validator.addMethod(
    //    "ValidateTillDate",
    //    function (value, element) {
    //        var _FromDate = $("#txtFromDate").val();
    //        if (_FromDate == undefined || _FromDate == "" || _FromDate == null) {
    //            return true;
    //        }
    //        if (value == undefined || value == "" || value == null) {
    //            return true;
    //        }
    //        var isSuccess = false;
    //        var JsonObject = {
    //            FromDate: _FromDate,
    //            TillDate: value
    //        };
    //        $.ajax({
    //            type: "POST",
    //            async: false,
    //            url: "../Services/ValidateFromTillDate",
    //            data: JSON.stringify(JsonObject),
    //            contentType: "application/json; charset=utf-8",
    //            dataType: "json",
    //            success: function (msg) {
    //                //debugger;
    //                var retval = JSON.parse(msg);
    //                if (retval._STATUS_ == "FAIL") {
    //                    isSuccess = false;
    //                }
    //                else {
    //                    isSuccess = true;
    //                }
    //            },
    //            error: function (msg) {
    //                isSuccess = false;
    //                HandleAjaxError(msg);
    //            }
    //        });
    //        return isSuccess;
    //    }, "The till date must be greater than or equal to from date."
    //);


    //$("#frmMain").validate({
    //    rules:
    //    {
    //        txtFromDate: {
    //            required: true
    //        },
    //        txtToDate: {
    //            required: true,
    //            ValidateTillDate: true
    //        }
    //    },
    //    submitHandler: function (form) {
    //        var _FromDate = $('#txtFromDate').val();
    //        var _ToDate = $('#txtToDate').val();

    //        var JsonObject =
    //        {
    //            FromDate: _FromDate,
    //            ToDate: _ToDate,
    //        };
    //        $.ajax({
    //            type: "POST",
    //            url: "/Reports/ExaminationReport",
    //            data: JSON.stringify(JsonObject),
    //            contentType: "application/json; charset=utf-8",
    //            dataType: "json",
    //            success: function (msg) {

    //                var Result = JSON.parse(msg);
    //                if (Result._STATUS_ == "SUCCESS") {
    //                    $("#ResponseFile").html('');
    //                    $("#ResponseFile").html("<label><a href='" + Result._RESPONSE_FILE_ + "'>Download Response File</a></label>");
    //                    alert(Result._MESSAGE_);

    //                }
    //                else {
    //                    if (Result._RESPONSE_FILE_) {
    //                        $("#ResponseFile").html('');
    //                        $("#ResponseFile").html("<label><a href='" + Result._RESPONSE_FILE_ + "'> Download Response File</a></label>");
    //                    }
    //                    alert(Result._MESSAGE_);
    //                }
    //            }

    //        })
    //    }
    //})
})

function fr(z) {
    $("#ResponseFile").html('');
    var data = new FormData();
    data.append("option", z);
    $.ajax({
        type: "POST",
        url: "../Reports/ExaminationReport",
        data: data,
        processData: false,
        contentType: false,
        cache: false,
        success: function (msg) {
            var Result = JSON.parse(msg);
            if (Result._STATUS_ == "SUCCESS") {
                $("#ResponseFile").html('');
                s = "<label><a href='" + Result._RESPONSE_FILE_ + "'>Download Report in Excel Format</a></label><br>";
                s += "<label><a href='" + Result._RESPONSE_FILE2_ + "'>Download Report in Tab Separated Text Format</a></label>";
                $("#ResponseFile").html(s);

                //var link = document.createElement('a');
                //link.href = Result._RESPONSE_FILE_;
                //link.setAttribute("download","");
                //link.click();
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

