$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

    $("#txtDateOfPayment").datepicker({
        showMonthAfterYear: true,
        dateFormat: 'dd-M-yy',
        changeMonth: true,
        changeYear: true,
        yearRange: "2019:" + ServerYear,
        maxDate: ServerDate
    });

    jQuery.validator.addMethod(
        "nonzero_amt",
        function (value, element) {
            return this.optional(element) || parseFloat(value) > parseFloat("0");
        }, "The amount can't be zero" 
    );


    $("#frmMain").validate({
        rules:
        {
            cboInstructionType: {
                required: true,
            },
            txtReferenceNo: {
                required: true,
                check_exp: regexLowAscii
            },
            txtAmount: {
                required: true,
                //digits: true,
                check_exp: "^[0-9]{1,10}([\.][0-9]{1,2})?$",
                //number:true
                nonzero_amt:true
            },
            cboModeOfPayment: {
                required: true,
            },
            txtDateOfPayment: {
                required: true,
            },
            txtRemarks: {
                required: true,
                check_exp: regexLowAscii
            },
            txtNarration: {
                required: true,
                check_exp: regexLowAscii
            }
        },
        messages:
        {
            cboInstructionType: {
                required: MandatoryFieldMsg,
            },
            txtReferenceNo: {
                required: MandatoryFieldMsg,
                check_exp: JunkCharMessage
            },
            txtAmount: {
                required: MandatoryFieldMsg,
                //digits: "Please enter valid amount",
                check_exp: "Expected a number greater than zero. The number should be maximum 12 digits (10 digits predecimal & 2 digits post decimal).",
                //number_more_than: "The amount must be greater than zero"
            },
            cboModeOfPayment: {
                required: MandatoryFieldMsg,
            },
            txtDateOfPayment: {
                required: MandatoryFieldMsg,
            },
            txtRemarks: {
                required: MandatoryFieldMsg,
                check_exp: JunkCharMessage
            },
            txtNarration: {
                required: MandatoryFieldMsg,
                check_exp: JunkCharMessage
            }
        },
        submitHandler: function (form) {
            $("#data").html('');

            var data = new FormData(form);

            $.ajax({
                type: "POST",
                url: "../Accounts/AddVoucher",
                enctype: 'multipart/form-data',
                data: data,
                processData: false,
                contentType: false,
                cache: false,
                success: function (msg) {
                    debugger;
                    var s = '';
                    var result = JSON.parse(msg);
                    if (result._STATUS_ == 'FAIL') {
                        alert(result._MESSAGE_);
                    }
                    else
                    {
                        alert(result._MESSAGE_);
                        window.location = window.location;
                    }
                },
                error: function (msg) {
                    alert(msg);
                }
            });
        }
    });

    $("#cboCompanyType").change(function () {
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
});