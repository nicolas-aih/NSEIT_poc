$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

    $("#MainForm").hide();

    $("#txtURN").on("keyup keydown change blur", function () {
        if ($("#txtURN").val() == undefined || $("#txtURN").val() == "" || $("#txtURN").val() == null) {

        }
        else {
            $("#MainForm").hide();
        }
    })

    $("#txtExaminationDate").datepicker({
        showMonthAfterYear: true,
        dateFormat: 'dd-M-yy',
        changeMonth: true,
        changeYear: true,
        yearRange: "2000:" + (ServerYear + 1),
        onClose: function () {
            $(this).valid();
        }
    });

    $('#txtExaminationTime').timepicker({
        timeFormat: 'h:mm:00 p',
        interval: 15,
        minTime: '5:45',
        maxTime: '23:30',
        startTime: '05:45',
        dynamic: true,
        dropdown: true,
        scrollbar: true
    });

    $("#frmSearch").validate({
        rules:
        {
            txtURN: {
                required: true,
                minlength: 13
            }
        },
        messages:
        {
            txtPAN: {
                required: MandatoryFieldMsg,
                minlength: "URN should be 13 or 14 characters in length"
            }
        },
        submitHandler: function (form) {
            debugger;
            $("#data").html('');
            var _URN = $("#txtURN").val();
            var JsonObject = { URN: _URN };
            $.ajax({
                type: "POST",
                url: "../Candidates/GetExamDetails",
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
                            alert("No details were found for entered URN");
                        }
                        else {
                            Status = data[0].tntApplicantStatusID;
                            Status2 = data[0].bitApplicantConfirmedForExam;
                            if (Status == 7 && Status2 == 1) {
                                $("#txtCandidateURN").html(data[0].chrRollNumber);
                                $("#txtApplicationDate").html(data[0].Appdate);
                                $("#txtInsuranceCompany").html(data[0].InsName);
                                $("#txtInsuranceType").html(data[0].InsType);
                                $("#txtCandidateName").html(data[0].varApplicantName);
                                $("#imgFilePhoto").attr('src', 'data:image/jpg;base64,' + data[0].Photo);
                                $("#txtCandidateDOB").html(data[0].dtApplicantDOB);
                                $("#txtExaminationCenter").html(data[0].varExamCenterName);
                                $("#hdnURN").val(_URN);
                                $("#txtURN").val('');

                                $("#txtExaminationDate").val(data[0].dtExamDate);
                                $("#txtExaminationTime").val(data[0].dtExamTime);
                                $("#txtExamineeId").val(data[0].varExamRollNo);
                                $("#txtMarks").val(data[0].intExamMarks);
                                $("#cboStatus").val(data[0].Result);
                                $('#cboAction').val('');
                                $("#MainForm").show();
                                $("#frmMain").hide();
                            }
                            else if (Status == 7 && Status2 == 0) {
                                alert("The applicant is not registered for the exam");
                            }
                            else if (Status == 8)
                            {
                                alert("Details are already entered for the URN");
                            }
                            else if (Status < 7)
                            {
                                alert("The applicant is not trained");
                            }
                            else
                            {
                                alert("The URN is not in valid status to allow for these updates.");
                            }
                        }
                    }
                },
                error: function (msg) {
                    HandleAjaxError(msg);
                }
            });
        }
    });

    SelectedAction = "";
    $("#cboAction").change(function () {
        SelectedAction = $("#cboAction option:selected").val();
        $("#hdnAction").val(SelectedAction);
        if (SelectedAction == "")
        {
            $("#frmMain").hide();
        }
        if (SelectedAction == "1")
        {
            $("#frmMain").show();
            $("#Row1").show();
            $("#Row2").hide();
            $("#Row3").show();
        }
        if (SelectedAction == "2")
        {
            $("#frmMain").show();
            $("#Row1").show();
            $("#Row2").show();
            $("#Row3").show();
        }
        if (SelectedAction == "3")
        {
            $("#frmMain").show();
            $("#Row1").hide();
            $("#Row2").hide();
            $("#Row3").show();
        }
    });

    $("#frmMain").validate({
        ignore: [],
        rules: {
            txtExaminationDate: {
                required: function() {
                    if (SelectedAction == "1" || SelectedAction == "2") {
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            },
            txtExaminationTime: {
                required: function () {
                    if (SelectedAction == "1" || SelectedAction == "2") {
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            },
            txtExamineeId: {
                required: function () {
                    if (SelectedAction == "1" || SelectedAction == "2") {
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            },
            txtMarks: {
                required: function () {
                    if (SelectedAction == "2") {
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            },
            cboStatus: {
                required: function () {
                    if (SelectedAction == "2") {
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            }
        },
        messages: {
            txtExaminationDate: {
                required: MandatoryFieldMsg,
            },
            txtExaminationTime: {
                required: MandatoryFieldMsg,
            },
            txtExamineeId: {
                required: MandatoryFieldMsg,
            },
            txtMarks: {
                required: MandatoryFieldMsg,
            },
            cboStatus: {
                required: MandatoryFieldMsg,
            }
        },
        submitHandler: function (form) {
            var data = new FormData(form);

            $.ajax({
                type: "POST",
                enctype: 'multipart/form-data',
                url: "../Candidates/SaveExamDetails",
                data: data,
                processData: false,
                contentType: false,
                cache: false,
                success: function (msg) {
                    debugger;
                    var s = '';
                    var Result = JSON.parse(msg);
                    if (Result._STATUS_ === "SUCCESS") {
                        form.reset();
                        $("#frmMain").hide();
                        $('#cboAction').val('');
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
});