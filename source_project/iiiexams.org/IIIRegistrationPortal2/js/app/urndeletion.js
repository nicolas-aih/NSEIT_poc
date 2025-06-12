$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

    $("#frmSearch").show();
    $("#frmMain").hide();

    $("#frmSearch").validate({
        rules: {
            txtURN: {
                required: true,
                minlength: 13,
                check_exp: regexAlphaNumeric
            }
        },
        messages: {
            txtURN: {
                required: MandatoryFieldMsg,
                minlength: "URN should be minimum 13 characters in length",
                check_exp: "Please enter valid URN"
            }
        },
        submitHandler: function (form) {
            debugger;
            var myForm = $("#frmMain");
            clearValidation(myForm);

            var data = new FormData(form);
            
            $.ajax({
                type: "POST",
                url: "../Candidates/GetURNDetailsForDeletion",
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
                    else {
                        var data = JSON.parse(result._DATA_);
                        if (data.length == undefined || data.length == 0) {
                            alert("No data found");
                        }
                        else {
                            $('#frmMain').trigger("reset");
                            for (i = 0; i < data.length; i++) {
                                $("#hddURN").val(data[i].chrRollNumber);
                                //$("#txtURN2").val(data[i].chrRollNumber);
                                $("#txtApplicantName").val(data[i].varApplicantName);
                                $("#txtDOB").val(data[i].dtApplicantDOB);
                                $("#txtPAN").val(data[i].varPAN);
                                $("#txtEmployeeCode").val(data[i].varInsurerExtnRefNo);
                                break;
                            }
                            $("#txtURN").val('');
                        }
                        $("#frmMain").show();
                    }
                },
                error: function (msg) {
                    HandleAjaxError(msg);
                }
            });
        }
    })

    $("#Result").hide();
    $("#Form").show();

    var validator = $("#frmMain").validate(
        {
            submitHandler: function (form) {
                var data = new FormData(form);

                $.ajax({
                    type: "POST",
                    enctype: 'multipart/form-data',
                    url: "../Candidates/DeleteURN",
                    data: data,
                    processData: false,
                    contentType: false,
                    cache: false,
                    success: function (msg) {
                        debugger;
                        var s = '';
                        var Result = JSON.parse(msg);
                        if (Result._STATUS_ == "SUCCESS") {
                            form.reset();
                            $("#frmMain").hide();
                            alert(Result._MESSAGE_);
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

    $("#txtURN").on("keyup keydown blur change", function () {
        if ($("#txtURN").val() == undefined || $("#txtURN").val() == "" || $("#txtURN").val() == null) {

        }
        else {
            $("#frmMain").trigger("reset");
            $("#frmMain").hide();
        }
    });
});