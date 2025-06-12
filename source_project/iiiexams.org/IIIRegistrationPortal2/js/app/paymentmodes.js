$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

    $("#cboCompanyType").change(function () {
        ResetDetailForm();
    });

    $("#txtCompanyLoginId").change(function () {
        ResetDetailForm();
    });

    $("#frmMain").validate({
        rules:
        {
            cboCompanyType: {
                required: true,
            },
            txtCompanyLoginId: {
                required: true
            }
        },
        messages:
        {
            cboCompanyType: {
                required: MandatoryFieldMsg
            },
            txtCompanyLoginId: {
                required: MandatoryFieldMsg
            }
        },
        submitHandler: function (form) {
            var data = new FormData(form);
            ResetDetailForm();
            $.ajax({
                type: "POST",
                url: "../Services/GetCompanyPaymentModes",
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
                        //var data = JSON.parse(result._DATA_);
                        $("#hddCompanyType").val($("#cboCompanyType option:selected").val());
                        $("#hddCompanyLoginId").val($("#txtCompanyLoginId").val());

                        //Set the Values...
                        $("#txtCompanyName").val(result.CNAME);
                        if (result.CMODE === 'Y') {
                            $("#chkCreditBalance").attr("checked", "checked");
                        }
                        else {
                            $("#chkCreditBalance").removeAttr("checked");
                        }
                        if (result.OMODE === 'Y') {
                            $("#chkOnlinePayment").attr("checked", "checked");
                        }
                        else {
                            $("#chkOnlinePayment").removeAttr("checked");
                        }

                        $("#frmDetails").show();
                    }
                },
                error: function (msg) {
                    HandleAjaxError(msg);
                }
            });
        }
    });

    jQuery.validator.addMethod(
        "ValidatePaymentMode",
        function (value, element) {
            if ($('#chkCreditBalance').is(":checked") || $("#chkOnlinePayment").is(':checked')) {
                return true;
            }
            else {
                return false;
            }
        }, ''
    );

    $("#frmDetails").validate({
        rules:
        {
            chkCreditBalance: {
                ValidatePaymentMode: true,
            },
            chkOnlinePayment: {
                ValidatePaymentMode: true
            }
        },
        messages:
        {
            chkCreditBalance: {
                ValidatePaymentMode: 'Please select atleast one mode of payment'
            },
            chkOnlinePayment: {
                ValidatePaymentMode: 'Please select atleast one mode of payment'
            }
        },
        submitHandler: function (form) {
            var data = new FormData(form);
            $.ajax({
                type: "POST",
                url: "../Services/SaveCompanyPaymentModes",
                data: data,
                processData: false,
                contentType: false,
                cache: false,
                success: function (msg) {
                    var s = '';
                    var result = JSON.parse(msg);
                    if (result._STATUS_ === 'FAIL') {
                        alert(result._MESSAGE_);
                    }
                    else {
                        //Set the Values...
                        alert(result._MESSAGE_);
                        window.location = window.location;
                    }
                },
                error: function (msg) {
                    HandleAjaxError(msg);
                }
            });
        }
    });


    function ResetDetailForm() {
        $("#txtCompanyName").val("");
        $("#hddCompanyLoginId").val("");
        $("#chkCreditBalance").removeAttr("checked");
        $("#chkOnlinePayment").removeAttr("checked");
        $("#frmDetails").hide();
    }
    
});