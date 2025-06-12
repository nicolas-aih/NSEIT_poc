$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        })

    $("#txtFromDate").datepicker({
        showMonthAfterYear: true,
        dateFormat: 'dd-M-yy',
        changeMonth: true,
        changeYear: true,
        yearRange: "1900:" + ServerYear,
        onClose: function () {
            /* Validate a specific element: */
            //$("form").validate().element("#txtDOB");
            $(this).valid();
            //$("form #txtDOB").trigger("blur"); 
        }
    });

    $("#txtToDate").datepicker({
        showMonthAfterYear: true,
        dateFormat: 'dd-M-yy',
        changeMonth: true,
        changeYear: true,
        yearRange: "1900:" + ServerYear,
        onClose: function () {
            /* Validate a specific element: */
            //$("form").validate().element("#txtDOB");
            $(this).valid();
            //$("form #txtDOB").trigger("blur"); 
        }
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

    $("#txtBatchNo").on('keyup keypress blur change', function () {
        $('#hdBatchNo').val('');
        $('#hdAmount').val('');
        $("#tbldata").html('');
        $("#frmMain").hide();
    })

    $("#btnCancel").click(function () {
        window.location = window.location;
    })

    $("#cboSearchBy").click(function(){
        var _SearchBy = $("#cboSearchBy option:selected").val();
        if (_SearchBy === "BATCHNO")
        {
            $("#divBatchNo").show();
            $("#divFromDate").hide();
            $("#divToDate").hide();
            $("#divStatus").hide();
            $("#divSubmit").show();
        }
        if (_SearchBy === "DATERANGE")
        {
            $("#divBatchNo").hide();
            $("#divFromDate").show();
            $("#divToDate").show();
            $("#divStatus").show();
            $("#divSubmit").show();
            $("#divExcel").show();
        }
        if (_SearchBy === "") {
            $("#divBatchNo").hide();
            $("#divFromDate").hide();
            $("#divToDate").hide();
            $("#divStatus").hide();
            $("#divSubmit").hide();
            $("#divExcel").hide();
        }

        $("#txtBatchNo").val("");
        $("#txtFromDate").val("");
        $("#txtToDate").val("");
        $("#divStatus option:selected").val("");
    })

    $("#frmSearch").validate({
        igonre:[],
        rules: {
            txtBatchNo: {
                required: function () {
                    if ($("#cboSearchBy option:selected").val().toUpperCase() === 'BATCHNO') {
                        return true;
                    } else {
                        return false;
                    }
                },
            check_exp: regexAlphaNumeric
            },
            txtFromDate: {
                required: function () {
                    if ($("#cboSearchBy option:selected").val().toUpperCase() === 'DATERANGE') {
                        return true;
                    } else {
                        return false;
                    }
                },
            },
            txtToDate: {
                required: function () {
                    if ($("#cboSearchBy option:selected").val().toUpperCase() === 'DATERANGE') {
                        return true;
                    } else {
                        return false;
                    }
                },
                ValidateTillDate:true
            },
            cboStatus: {
                required: function () {
                    if ($("#cboSearchBy option:selected").val().toUpperCase() === 'DATERANGE') {
                        return true;
                    } else {
                        return false;
                    }
                }
            }
        },
        messages: {
            txtBatchNo: {
                required: MandatoryFieldMsg,
                check_exp: "Please enter valid Transaction Id"
            },
            txtFromDate: {
                required: MandatoryFieldMsg
            },
            txtToDate: {
                required: MandatoryFieldMsg
            },
            cboStatus: {
                required: MandatoryFieldMsg
            }
        },
        submitHandler: function (form) {
            var data = new FormData(form);

            $.ajax({
                type: "POST",
                enctype: 'multipart/form-data',
                url: "../Services/GetBatchList",
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
                        if (data.length == undefined || data.length == 0) 
                        {
                            alert("No data found");
                        }
                        else {
                            var URNCount = 0;

                            /*
                            exam_batch_no
                            creation_date
                            urn_count
                            status
                            */

                            s = "<thead><tr>";
                            s += "<th>Batch No</th>";
                            s += "<th>Creation date</th>";
                            s += "<th>URN Count</th>";
                            s += "<th>Payment mode</th>" 
                            s += "<th>Status</th>";
                            s += "<th>Scheduled Count</th>";
                            s += "<th>Unscheduled Count</th>";
                            s += "<th>Created By</th>";
                            s += "</tr></thead>";
                            s += "<tbody>";
                            for (i = 0; i < data.length; i++) {
                                s += "<tr>";
                                //if (data[i].status_id == 2 || data[i].status_id == 5) {
                                //    s += "<td>" + data[i].exam_batch_no + "</td>";
                                //}
                                //else {
                                    s += "<td><a href=\"javascript:viewbatchdetails('" + data[i].exam_batch_no + "')\">" + data[i].exam_batch_no + "</a></td>";
                                //}
                                s += "<td>" + data[i].creation_date + "</td>";
                                s += "<td>" + data[i].urn_count + "</td>";
                                s += "<td>" + data[i].payment_mode + "</td>";
                                s += "<td>" + data[i].status + "</td>";
                                s += "<td>" + data[i].scheduled_count + "</td>";
                                s += "<td>" + data[i].unscheduled_count + "</td>";
                                s += "<td>" + data[i].varUserName + "</td>";
                                s += "</tr>";
                            }
                            s += "</tbody>";

                            $("#tblBatches").html(s);
                        }
                    }
                },
                error: function (msg) {
                    HandleAjaxError(msg);
                }
            })
        }
    })

    window.viewbatchdetails = function (BatchNo) {
        var data = new FormData();
        data.append("txtBatchNo", BatchNo);
        $.ajax({
            type: "POST",
            enctype: 'multipart/form-data',
            url: "../Services/GetBatchDetailsForMgmt",
            data: data,
            processData: false,
            contentType: false,
            cache: false,
            success: function (msg) {
                var s = '';
                var s2 = '';
                var result = JSON.parse(msg);
                if (result._STATUS_ == 'FAIL') {
                    alert(result._MESSAGE_);
                }
                else {
                    var data = JSON.parse(result._DATA0_);
                    var dataPayments = JSON.parse(result._DATA1_);
                    if (data.length == undefined || data.length == 0) {
                        alert("No data found");
                    }
                    else {
                        $('#exambatchno').html(result.transaction_id);
                        $('#hdBatchNo').val(result.transaction_id);
                        $('#paymentmode').html(result.payment_mode);
                        $('#noofurns').html(result.total_urns);

                        s = "<thead><tr>";
                        s += "<th>URN</th>";
                        s += "<th>Candidate's Name</th>";
                        s += "<th>Examination Center</th>";
                        s += "<th>TCC Expiry Date</th>";
                        s += "<th>Scheduling mode</th>";        
                        s += "<th>Fees</th>";
                        s += "</tr></thead>";
                        s += "<tbody>";
                        for (i = 0; i < data.length; i++) {
                            if (data[i].IsProblemURN === 'Y') {
                                s += "<tr>";
                                s += "<td style='color:red'>" + data[i].urn + "</td>";
                                s += "<td style='color:red'>" + data[i].candidate_name + "</td>";
                                s += "<td style='color:red'>" + data[i].exam_center + "</td>";
                                s += "<td style='color:red'>" + data[i].dtTCCExpiryDate + "</td>";
                                s += "<td style='color:red'>" + data[i].scheduling_mode + "</td>";
                                s += "<td style='text-align:right;style='color:red'>" + data[i].total_amount.toFixed(2) + "</td>";
                                s += "</tr>";
                            }
                            else {
                                s += "<tr>";
                                s += "<td>" + data[i].urn + "</td>";
                                s += "<td>" + data[i].candidate_name + "</td>";
                                s += "<td>" + data[i].exam_center + "</td>";
                                s += "<td>" + data[i].dtTCCExpiryDate + "</td>";
                                s += "<td>" + data[i].scheduling_mode + "</td>";
                                s += "<td style='text-align:right'>" + data[i].total_amount.toFixed(2) + "</td>";
                                s += "</tr>";
                            }
                        }
                        s += "<tr>";
                        s += "<td colspan=5><b>Total Fees</b></td>";
                        s += "<td style='text-align:right'><b>" + result.grand_total + "</b></td>";
                        s += "</tr>";
                        s += "</tbody>";

                        if (result.payment_mode.toUpperCase() === 'CREDIT')
                        {
                            s2 = "";
                        }
                        else 
                        {
                            s2 = "<thead><tr><th colspan=5>Payment details</th></tr><tr>";
                            s2 += "<th>Payment Date</th>";
                            s2 += "<th>NSEIT  Ref No</th>";
                            s2 += "<th>Payment Gateway Ref No</th>";
                            s2 += "<th>Status</th>";
                            s2 += "<th>Payment timestamp</th>";
                            s2 += "</tr></thead>";
                            s2 += "<tbody>";
                            for (i = 0; i < dataPayments.length; i++) {
                                    s2 += "<tr>";
                                    s2 += "<td>" + dataPayments[i].payment_date + "</td>";
                                    s2 += "<td>" + dataPayments[i].nseit_ref_no + "</td>";
                                    s2 += "<td>" + dataPayments[i].pg_ref_no + "</td>";
                                    s2 += "<td>" + dataPayments[i].pg_status + "</td>";
                                    s2 += "<td>" + dataPayments[i].payment_timestamp + "</td>";
                                    s2 += "</tr>";
                            }
                            s2 += "</tbody>";
                        }

                        $("#divBatchDetails").dialog({
                            height: 500,
                            width: 800,
                            modal: true,
                            resizable: false
                        });
                        $("#tblBatchData").html(s);
                        $("#tblPaymentData").html(s2);
                        $("#frmMain").show();
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        })
    }

    $("#btnCancel").click(function () {
        window.location = window.location;
    })
});