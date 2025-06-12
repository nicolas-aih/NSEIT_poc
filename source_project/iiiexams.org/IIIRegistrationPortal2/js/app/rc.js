$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

    clearControls();
    $("#dvURN").show();
    $("#dvApplicantDetails").hide();

    $("#txtURN").on('keyup change keypress', function () {
        $("#txtURN2").val('');
        $("#hdURN").val('');
        $("#cboCenter").html('');
        $("#txtCandidateName").val('');
        $("#txtCurrCenter").val('');
        $("#txtCurrLanguage").val('');
        $("#txtCurrExamDateAndTime").val('');
        //$("#cboLanguage").html('');
        $("#cboCenter").html('');
        $("#txtRemarks").val('');
        $("#cboDates").html('');
        $("#tblbatches").html('');
        $("#tbldata").html('');
        $("#dvApplicantDetails").hide();
    });   

    function clearControls()
    {
        $("#txtURN").val('');
        $("#txtURN2").val('');
        $("#hdURN").val('');
        $("#cboCenter").html('');
        //$("#cboLanguage").html('');
        $("#cboDates").html('');
        $("#tblbatches").html('');
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

                            //Set TestCenters
                            s = '';
                            for (i = 0; i < data1.length; i++) {
                                if (data1[i].is_sel == 1) {
                                    s += "<option value='" + data1[i].css_id + "' selected>" + data1[i].center_name + "</option>";
                                }
                                else
                                {
                                    s += "<option value='" + data1[i].css_id + "'>" + data1[i].center_name + "</option>";
                                }
                            }
                            $("#cboCenter").html(s);

                            //s = '';
                            //for (i = 0; i < data2.length; i++) {
                            //    if (data2[i].is_sel == 1) {
                            //        s += "<option value='" + data2[i].id + "' selected>" + data2[i].lang + "</option>";
                            //    }
                            //    else
                            //    {
                            //        s += "<option value='" + data2[i].id + "'>" + data2[i].lang + "</option>";
                            //    }
                            //}
                            //$("#cboLanguage").html(s);

                            $("#cboCenter").trigger("change");
                            //s = '';
                            //s += "<option value=''>--Select--</option>";
                            //for (i = 0; i < data3.length; i++) {
                            //    s += "<option value='" + data3[i].exam_date + "'>" + data3[i].exam_date + "</option>";
                            //}
                            //$("#cboDates").html(s);

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

    $("#cboCenter").change(function () {
        // Clear the table
        // Repopulate the dates combo.
        var CenterId = $("#cboCenter option:selected").val();
        if ( CenterId === "" )
        {
            alert("Please select exam center");
            return;
        }

        var data = new FormData();
        data.append("center_id", CenterId);

        $.ajax({
            type: "POST",
            enctype: 'multipart/form-data',
            url: "../Scheduler/GetDatesForCenter",
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
                    var data = JSON.parse(result._DATA_);
                    if (data.length == undefined || data.length == 0) {
                        alert("No data found");
                    }
                    else {
                        //Set TestDates
                        s = '';
                        s += "<option value=''>--Select--</option>";
                        for (i = 0; i < data.length; i++) {
                            s += "<option value='" + data[i].EXAM_DATE + "'>" + data[i].EXAM_DATE + "</option>";
                        }
                        $("#cboDates").html(s);
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        })
    });

    window.BookSeat = function (start_time) {
        $("#tbldata").html('');
        if ($("#txtRemarks").val() === "")
        {
            alert("Please enter remarks");
            return;
        }

        var data = new FormData();
        
        var CenterId = $("#cboCenter option:selected").val();
        //var LanguageId = $("#cboLanguage option:selected").val();
        var URN = $("#hdURN").val();
        var TestDate = $("#cboDates option:selected").val();
        var Remarks = $("#txtRemarks").val();

        data.append("urn", URN);
        data.append("center_id", CenterId);
        //data.append("language_id", LanguageId);
        data.append("preferred_date", TestDate);
        data.append("start_time", start_time);
        data.append("remarks", Remarks);

        $.ajax({
            type: "POST",
            enctype: 'multipart/form-data',
            url: "../Scheduler/Reschedule",
            data: data,
            processData: false,
            contentType: false,
            cache: false,
            success: function (msg) {
                $("#tblbatches").html('');
                var s = '';
                var result = JSON.parse(msg);
                if (result._STATUS_ == 'FAIL')
                {
                    alert(result._MESSAGE_);
                }
                else
                {
                    var data = JSON.parse(result._DATA_);
                    if (data.length == undefined || data.length == 0)
                    {
                        $("#tbldata").html('');
                        alert(NO_DATA_FOUND);
                    }
                    else
                    {   /*
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
                        s += "<thead><tr><td>URN</td><td>New exam center</td><td>New exam date</td><td>New exam time</td><td>CSS Status</td><td>CSS Reason</td><td>Reg Portal Status</td><td>Reg Portal Reason</td></tr></thead>";
                        for (i = 0; i < data.length; i++) {
                            s += "<tr>";
                            s += "<td>" + data[i].urn + "</td>";
                            s += "<td>" + data[i].center_name + "</td>";
                            s += "<td>" + data[i].test_date + "</td>";
                            s += "<td>" + data[i].test_time + "</td>";
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
    }

    $("#cboDates").change(function () {
        // Clear the table
        // Repopulate the table.
        $("#tblbatches").html('');

        var CenterId = $("#cboCenter option:selected").val();
        if (CenterId === "") {
            alert("Please select exam center");
            return;
        }

        var PreferredDate = $("#cboDates option:selected").val();
        if (PreferredDate === "") {
            alert("Please select preferred date");
            return;
        }

        var data = new FormData();
        data.append("center_id", CenterId);
        data.append("preferred_date", PreferredDate);

        $.ajax({
            type: "POST",
            enctype: 'multipart/form-data',
            url: "../Scheduler/GetBatchesForCenterDate",
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
                    var data = JSON.parse(result._DATA_);
                    if (data.length == undefined || data.length == 0) {
                        alert("No data found");
                    }
                    else {
                        //Set Batches
                        s = '';
                        s += "<thead><tr><th>Batch</th><th>Seats available</th><th></th></tr></thead>";
                        for (i = 0; i < data.length; i++) {
                            s += "<tr>";
                            s += "<td>" + data[i].START_TIME + " - " + data[i].END_TIME + "</td>";
                            s += "<td>" + data[i].AVAILABLE_SEATS + "</td>";
                            if (data[i].available_seats == 0){
                                s += "<td>&nbsp;</td>";
                            }
                            else {
                                s += "<td><a href='javascript:BookSeat(" + data[i].START_TIME_N + ");'>Book Seat</a></td>";
                            }
                            s += "</tr>";
                        }
                        $("#tblbatches").html(s);
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        })
    });

    function DownloadHT(urn) {
        var data = new FormData();
        data.append("txtURN", urn);
        $.ajax({
            type: "POST",
            enctype: 'multipart/form-data',
            url: "../Candidates/HallTicket2",
            data: data,
            processData: false,
            contentType: false,
            cache: false,
            success: function (msg) {
                //debugger;
                var s = '';
                var Result = JSON.parse(msg);
                if (Result._STATUS_ == "SUCCESS") {
                    window.open(Result._RESPONSE_FILE_, "_blank");
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
})