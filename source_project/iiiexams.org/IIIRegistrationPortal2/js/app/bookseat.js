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

    function clearControls()
    {
        $("#txtURN").val('');
        $("#txtURN2").val('');
        $("#hdURN").val('');
        $("#cboCenter").html('');
        $("#cboLanguage").html('');
        $("#cboDates").html('');
        $("#tblbatches").html('');
    }

    $("#txtURN").on('keyup keypress blur change', function () {
        $("#txtURN2").val('');
        $("#hdURN").val('');
        $("#cboCenter").html('');
        $("#cboLanguage").html('');
        $("#cboDates").html('');
        $("#tblbatches").html('');

        $("#dvApplicantDetails").hide();
    })


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
                url: "../Scheduler/GetCandidateDetails",
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

                            s = '';
                            for (i = 0; i < data2.length; i++) {
                                if (data2[i].is_sel == 1) {
                                    s += "<option value='" + data2[i].id + "' selected>" + data2[i].lang + "</option>";
                                }
                                else
                                {
                                    s += "<option value='" + data2[i].id + "'>" + data2[i].lang + "</option>";
                                }
                            }
                            $("#cboLanguage").html(s);

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
        $("#cboDates").html('');
        $("#tblbatches").html('');

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
        var data = new FormData();

        var CenterId = $("#cboCenter option:selected").val();
        var LanguageId = $("#cboLanguage option:selected").val();
        var URN = $("#hdURN").val();
        var TestDate = $("#cboDates option:selected").val();

        data.append("urn", URN);
        data.append("center_id", CenterId);
        data.append("language_id", LanguageId);
        data.append("preferred_date", TestDate);
        data.append("start_time", start_time);

        $.ajax({
            type: "POST",
            enctype: 'multipart/form-data',
            url: "../Scheduler/BookSeat",
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
                    alert(result._MESSAGE_);
                    if (confirm("Do you want to download your hallticket now ?"))
                    {
                        DownloadHT(URN);
                    }
                    window.location = window.location;
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
            alert("Please select exam date");
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