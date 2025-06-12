$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        })

    LoadCenters();

    function clearControls()
    {
        $("#cboExamCenterNew").html('');
        $("#cboExamDateNew").html('');
        $("#tblbatches").html('');
    };

    $("#txtExamDateOld").datepicker({
        showMonthAfterYear: true,
        dateFormat: 'dd-M-yy',
        changeMonth: true,
        changeYear: true,
        //yearRange: "1900:" + (ServerYear + 1),
        yearRange: "2020:2050",
        minDate: ServerDate,
        //onClose: function () {
        //    $(this).valid();
        //}
    });

    function LoadCenters()
    {
        var JsonObject = {
            StateId: -1,
            CenterId: -1
        };
        $.ajax({
            type: "POST",
            url: "../Services/GetCentersForState",
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
                        alert(NO_DATA_FOUND);
                    }
                    else {
                        s = "<option value=''>--Select--</option>"
                        for (i = 0; i < data.length; i++) {
                            s += "<option value='" + data[i].sntExamCenterID + "'>" + data[i].varExamCenterName + "</option>";
                        }
                        $("#cboExamCenterOld").html(s);
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        })
    }

    //GetSimilarCenters

    //Old
    $("#cboExamCenterOld").change(function () {
        $("#txtExamDateOld").val('');
        $("#cboSlotsOld").html('');
        $("#txtCandidateCount").val('');
        $("#cboExamCenterNew").html('');
        $("#cboExamDateNew").html('');
        $("#tblbatches").html('');
        $("#txtRemarks").val('');

        //ClearNew();

        var _CenterId = $("#cboExamCenterOld option:selected").val();
        if (_CenterId === "") {
            alert("Please select exam center");
            return;
        }

        //Populate cboExamCenterNew...
        var JsonObject = {
            CenterId: _CenterId
        };
        $.ajax({
            type: "POST",
            url: "../Services/GetSimilarCenters",
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
                        alert(NO_DATA_FOUND);
                    }
                    else {
                        s = "<option value=''>--Select--</option>"
                        for (i = 0; i < data.length; i++) {
                            s += "<option value='" + data[i].css_id + "'>" + data[i].varExamCenterName + "</option>";
                        }
                        $("#cboExamCenterNew").html(s);
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        })

    })
    $("#txtExamDateOld").change(function () {
        // Clear the table
        // Repopulate the table.
        //$("#txtExamDateOld").val('');
        $("#cboSlotsOld").html('');
        $("#txtCandidateCount").val('');
        $("#cboExamCenterNew").val('');
        $("#cboExamDateNew").html('');
        $("#tblbatches").html('');
        $("#txtRemarks").val('');

        var CenterId = $("#cboExamCenterOld option:selected").val();
        if (CenterId === "") {
            alert("Please select exam center");
            return;
        }

        var PreferredDate = $("#txtExamDateOld").val();
        if (PreferredDate === "") {
            alert("Please select examination date");
            return;
        }

        var data = new FormData();
        data.append("center_id", CenterId);
        data.append("preferred_date", PreferredDate);

        $.ajax({
            type: "POST",
            enctype: 'multipart/form-data',
            url: "../Scheduler/GetScheduledBatchesForCenterDate",
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
                        s = "<option value=''>--Select--</option>";
                        for (i = 0; i < data.length; i++) {
                            s += "<option value='" + data[i].slot + "'>" + data[i].slot + "</option>";
                        }
                        $("#cboSlotsOld").html(s);
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        })
    });

    $("#cboSlotsOld").change(function () {
        // Clear the table
        // Repopulate the table.
        //$("#txtExamDateOld").val('');
        //$("#cboSlotsOld").html('');
        $("#txtCandidateCount").val('');
        $("#cboExamCenterNew").val('');
        $("#cboExamDateNew").html('');
        $("#tblbatches").html('');
        $("#txtRemarks").val('');

        //ClearNew();

        var CenterId = $("#cboExamCenterOld option:selected").val();
        if (CenterId === "") {
            alert("Please select the exam center");
            return;
        }

        var PreferredDate = $("#txtExamDateOld").val();
        if (PreferredDate === "") {
            alert("Please select examination date");
            return;
        }

        var slot = $("#cboSlotsOld option:selected").val();
        if (slot === "") {
            alert("Please select the slot");
            return;
        }

        var data = new FormData();
        data.append("center_id", CenterId);
        data.append("preferred_date", PreferredDate);
        data.append("slot", slot);

        $.ajax({
            type: "POST",
            enctype: 'multipart/form-data',
            url: "../Scheduler/GetScheduledBatchCountForCenterDate",
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
                        $("#txtCandidateCount").val(data[0].candidate_count);
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        })
    });

    //New
    $("#cboExamCenterNew").change(function () {
        // Clear the table
        // Repopulate the dates combo.
        $("#cboExamDateNew").html('');
        $("#tblbatches").html('');

        var CenterId = $("#cboExamCenterNew option:selected").val();
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
                        $("#cboExamDateNew").html(s);
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        })
    });
    $("#cboExamDateNew").change(function () {
        // Clear the table
        // Repopulate the table.
        $("#tblbatches").html('');

        var CenterId = $("#cboExamCenterNew option:selected").val();
        if (CenterId === "") {
            alert("Please select new exam center");
            return;
        }

        var PreferredDate = $("#cboExamDateNew option:selected").val();
        if (PreferredDate === "") {
            alert("Please select preferred date to reschedule the candidates");
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
                            if (data[i].available_seats == 0) {
                                s += "<td>&nbsp;</td>";
                            }
                            else {
                                s += "<td><a href='javascript:BookSeat(" + data[i].START_TIME_N + ");'>Book Seats</a></td>";
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

    window.BookSeat = function (start_time) {
        $("#tbldata").html('');
        if ($("#txtRemarks").val() === "") {
            alert("Please enter remarks");
            return;
        }

        var cboExamCenterOld = $("#cboExamCenterOld option:selected").val();
        var txtExamDateOld = $("#txtExamDateOld").val();
        var cboSlotsOld = $("#cboSlotsOld option:selected").val(); 
        var cboExamCenterNew = $("#cboExamCenterNew option:selected").val();  //CSSID
        var cboExamDateNew = $("#cboExamDateNew option:selected").val(); 
        var remarks = $("#txtRemarks").val();

        var data = new FormData();
        data.append("cboExamCenterOld", cboExamCenterOld);
        data.append("txtExamDateOld", txtExamDateOld);
        data.append("cboSlotsOld", cboSlotsOld);
        data.append("cboExamCenterNew", cboExamCenterNew);
        data.append("cboExamDateNew", cboExamDateNew);
        data.append("start_time", start_time);
        data.append("remarks", remarks);

        $.ajax({
            type: "POST",
            enctype: 'multipart/form-data',
            url: "../Scheduler/RescheduleBatch",
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
    
})