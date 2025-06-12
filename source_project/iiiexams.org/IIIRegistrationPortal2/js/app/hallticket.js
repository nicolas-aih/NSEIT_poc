$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

    $("#Results").html('');

    $("#txtDOB").on('change keyup keydown blur', function () {
        $("#Results").html('');
    });

    $("#txtExamDate").on('change keyup keydown blur', function () {
        $("#Results").html('');
    });

    $("#txtURN").on('change keyup keydown blur', function () {
        $("#Results").html('');
    });

    $("#txtDOB").datepicker({
        showMonthAfterYear: true,
        dateFormat: 'dd-M-yy',
        changeMonth: true,
        changeYear: true,
        yearRange: "1900:" + ServerYear,
        maxDate: ServerDate,
        onClose: function () {
            /* Validate a specific element: */
            //$("form").validate().element("#txtDOB");
            $(this).valid();
            //$("form #txtDOB").trigger("blur"); 
        }
    });

    $("#txtExamDate").datepicker({
        showMonthAfterYear: true,
        dateFormat: 'dd-M-yy',
        changeMonth: true,
        changeYear: true,
        yearRange: "1900:" + (ServerYear + 1),
        minDate: ServerDate,
        onClose: function () {
            /* Validate a specific element: */
            //$("form").validate().element("#txtDOB");
            $(this).valid();
            //$("form #txtDOB").trigger("blur"); 
        }
    });

    jQuery.validator.addMethod("ValidateDates",
        function (value, element) {
            var DOB = $("#txtDOB").val();
            var EDT = $("#txtExamDate").val();
            if ((DOB === undefined || DOB === "" || DOB === null) && (EDT === undefined || EDT === "" || EDT === null)) {
                return false;
            }
            else {
                return true;
            }
        }, '');

    $("#frmMain").validate({
        rules:
        {
            txtURN: {
                required: true,
                minlength: 13,
                maxlength: 14
            },
            txtDOB: {
                ValidateDates: true
            },
            txtExamDate: {
                ValidateDates: true
            }
        },
        messages:
        {
            txtURN: {
                required: MandatoryFieldMsg,
                minlength: "URN should be 13 OR 14 characters in length",
                maxlength: "URN should be 13 OR 14 characters in length"
            },
            txtDOB: {
                ValidateDates: "You need to enter either Date of Birth OR Exam Date"
            },
            txtExamDate: {
                ValidateDates: "You need to enter either Date of Birth OR Exam Date"
            }
        },
        submitHandler: function (form) {
            var urn = $("#txtURN").val();
            var data = new FormData(form);
            $.ajax({
                type: "POST",
                enctype: 'multipart/form-data',
                url: "../Candidates/HallTicket",
                data: data,
                processData: false,
                contentType: false,
                cache: false,
                success: function (msg) {
                    //debugger;
                    var s = '';
                    var Result = JSON.parse(msg);
                    if (Result._STATUS_ === "SUCCESS") {
                        var link = document.createElement('a');
                        link.href = Result._RESPONSE_FILE_;
                        link.setAttribute("download", "HT_" + urn + ".pdf");
                        link.click();

                        //$("#Results").html('');
                        //$("#Results").html('<label><a href=' + Result._RESPONSE_FILE_ + ' target = "_blank">Download File</a></label>');
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


});