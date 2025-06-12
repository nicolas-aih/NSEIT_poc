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

    $("#cboCompany").change(function () {
        $("#ResponseFile").html('');
    });

    $("#cboCompanyType").change(function () {
        $("#ResponseFile").html('');
        $("#cboCompany").html('');

        var CompanyType = $("#cboCompanyType option:selected").val();
        if (CompanyType === "") {
            alert("Please select company type");
            return;
        }

        var data = new FormData();
        data.append("cboCompanyType", CompanyType);

        $.ajax({
            type: "POST",
            url: "../Accounts/GetCompanyList",
            enctype: 'multipart/form-data',
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
                    var data = JSON.parse(result._DATA_);
                    if (data.length == undefined || data.length == 0) {
                        alert("No companies found for selected company type");
                    }
                    else {
                        s = ""
                        for (i = 0; i < data.length; i++) {
                            s += "<option value='" + data[i].company_code + "'>" + data[i].company_name + "</option>"
                        }
                        $("#cboCompany").html(s);
                    }
                }
            },
            error: function (msg) {
                alert(msg);
            }
        });
    });

    $("#frmMain").validate({
        rules:
        {
            txtFromDate: {
                required: true
            },
            txtToDate: {
                required: true,
                ValidateTillDate: true
            }
        },
        messages: {
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
                url: "/Accounts/LedgerReport",
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