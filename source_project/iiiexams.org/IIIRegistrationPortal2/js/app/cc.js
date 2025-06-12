$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        })

    clearControls();
    $("#dvURN").show();
    $("#dvApplicantDetails").hide();

    $("#txtURN").on('keyup change keypress', function () {
        $("#txtURN2").val('');
        $("#hdURN").val('');
        $("#txtCandidateName").val('');
        $("#txtCurrCenter").val('');
        $("#txtCurrLanguage").val('');
        $("#txtCurrExamDateAndTime").val('');
        //$("#cboLanguage").html('');
        $("#txtRemarks").val('');
        $("#tbldata").html('');
        $("#dvApplicantDetails").hide();
    });   

    function clearControls()
    {
        $("#txtURN").val('');
        $("#txtURN2").val('');
        $("#hdURN").val('');
        $("#tblData").html('');
        // Also additional hd... 
    }

    $("#frmMain").validate({
        rules: {
            txtURN: {
                required: true
            }
        },
        messages: {
            txtURN: {
                required: MandatoryFieldMsg
            }
        },
        submitHandler: function (form) {
            var data = new FormData(form);
            $.ajax({
                type: "POST",
                enctype: 'multipart/form-data',
                url: "../Scheduler/GetCandidateDetailsRC",
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
                        var data0 = JSON.parse(result._DATA0_); //Candidate
                        var data1 = JSON.parse(result._DATA1_); //Centers
                        var data2 = JSON.parse(result._DATA2_); //Languages
                        //var data3 = JSON.parse(result._DATA3_); //Dates for default center

                        if (data0.length == undefined || data0.length == 0 || data1.length == undefined || data1.length == 0  || data2.length == undefined || data2.length == 0)
                        {
                            alert("Error Occured");
                        }
                        else {
                            //Set URN

                            $("#txtURN2").val(data0[0].urn);
                            $("#hdURN").val(data0[0].urn);
                            $("#txtCandidateName").val(data0[0].applicant_name);

                            $("#txtCurrCenter").val(data0[0].exam_center_name);
                            $("#txtCurrLanguage").val(data0[0].exam_language);
                            $("#txtCurrExamDateAndTime").val(data0[0].exam_date + ' ' + data0[0].exam_time);

                            $("#dvURN").show();
                            $("#dvApplicantDetails").show();
                        }
                    }
                },
                error: function (msg) {
                    HandleAjaxError(msg);
                }
            })
        }
    })

    $("#butCancel").click(function () {
        $("#tbldata").html('');
        if ($("#txtRemarks").val() === "") {
            alert("Please enter remarks");
            return;
        }

        var data = new FormData();

        var URN = $("#hdURN").val();
        var Remarks = $("#txtRemarks").val();

        data.append("urn", URN);
        data.append("remarks", Remarks);

        $.ajax({
            type: "POST",
            enctype: 'multipart/form-data',
            url: "../Scheduler/CC",
            data: data,
            processData: false,
            contentType: false,
            cache: false,
            success: function (msg) {
                $("#tblbatches").html('');
                var s = '';
                var result = JSON.parse(msg);
                if (result._STATUS_ == 'FAIL') {
                    alert(result._MESSAGE_);
                }
                else {
                    var data = JSON.parse(result._DATA_);
                    if (data.length == undefined || data.length == 0) {
                        $("#tbldata").html('');
                        alert(NO_DATA_FOUND);
                    }
                    else {   /*
                        urn,
                        client_id,
                        client_reference_number_old,
                        css_reference_number_old,
                        client_reference_number_new,
                        css_reference_number_new,
                        center_id,
                        test_date,
                        test_duration,
                        test_time,
                        status,
                        reason,
                        iii_status,
                        iii_reason,
                        sntExamCenterId			
                        */
                        s += "<thead><tr><td>URN</td><td>CSS Cancellation Status</td><td>CSS Reason</td><td>Reg Portal Status</td><td>Reg Portal Reason</td></tr></thead>";
                        for (i = 0; i < data.length; i++) {
                            s += "<tr>";
                            s += "<td>" + data[i].urn + "</td>";
                            s += "<td>" + data[i].status + "</td>";
                            s += "<td>" + data[i].reason + "</td>";
                            s += "<td>" + data[i].iii_status + "</td>";
                            s += "<td>" + data[i].iii_reason + "</td>";
                            s += "</tr>";
                        }
                        $("#tbldata").html(s);
                        $("#tbldata").show();
                    }
                    alert(result._MESSAGE_);
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        })
    });
})