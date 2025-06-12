$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

        $("#frm").hide();
        LoadData();

        $("#butCancel").click(function () {
            window.location = window.location;
        })

        function LoadData() {
            $("#data").html("");
            var JsonObject = {
                Hint: 2,
                InstructionId: -1,
                CompanyCode: -1
            }
            $.ajax({
                type: "POST",
                url: "../Accounts/GetInstructions",
                data: JSON.stringify(JsonObject),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    debugger;
                    var s = '';
                    var result = JSON.parse(msg);
                    if (result._STATUS_ == 'FAIL') {
                        alert(result._ERROR_);
                    }
                    else {
                        var data = JSON.parse(result._DATA_);
                        if (data.length == undefined || data.length == 0) {
                            alert("No data found");
                        }
                        else {
                            s = "<table class='table table-striped table-bordered table-hover'>"
                            s += "<thead><tr><td>&nbsp</td><td>Company Name</td><td>Transaction Type</td><td>Reference Number</td><td>Amount</td><td>Mode of Payment</td><td>Date of Transaction</td><td>Remarks</td><td>Created By</td><td>Creation timestamp</td></tr></thead>";
                            for (i = 0; i < data.length; i++) {
                                s += "<tr>";
                                s += "<td><a href='javascript:edit(" + data[i].instruction_id + ")'>Approve / Reject</a></td><td>" + data[i].company_name + "</td><td>" + data[i].instruction_type_desc + "</td><td>" + data[i].instrument_no + "</td><td>" + data[i].amount + "</td><td>" + data[i].mode_of_payment + "</td><td>" + data[i].date_of_payment_s + "</td><td>" + data[i].remark + "</td>"
                                s += "<td>" + data[i].created_by_name + "</td><td>" + data[i].creation_datetime_s + "</td>"
                                s += "</tr>";
                            }
                            s += "</table>";
                        }
                        $("#data").html(s);
                        $("#data").show();
                    }

                    //created_by_name
                    //creation_datetime_s
                    //company_name
                    //approved_by_name
                    //approval_datetime_s

                },
                error: function (msg) {
                    alert(msg);
                }
            });
        }

        window.edit = function (InstructionId) {
            var JsonObject = {
                Hint: 2,
                InstructionId: InstructionId,
                CompanyCode: -1
            }
            $.ajax({
                type: "POST",
                url: "../Accounts/GetInstructions",
                data: JSON.stringify(JsonObject),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var s = '';
                    var result = JSON.parse(msg);
                    if (result._STATUS_ == 'FAIL') {
                        alert(result._ERROR_);
                    }
                    else {
                        var data = JSON.parse(result._DATA_);
                        if (data.length == undefined) {
                            s += "<span>No data found</span>"
                        }
                        else {
                            $("#txtCompanyName").val(data[0].company_name);
                            $("#hddInstructionId").val(data[0].instruction_id);
                            //data[i].CompanyName
                            $("#cboInstructionType").val(data[0].instruction_type_desc);
                            $("#txtReferenceNo").val(data[0].instrument_no);
                            $("#txtAmount").val(data[0].amount);
                            $("#txtModeOfPayment").val(data[0].mode_of_payment);
                            $("#txtDateOfPayment").val(data[0].date_of_payment_s);
                            $("#txtRemarks").val(data[0].remark);
                            $("#txtNarration").val(data[0].narration);

                            $("#txtCreatedBy").val(data[0].created_by_name);
                            $("#txtCreationTimeStamp").val(data[0].creation_datetime_s);

                            $("#data").html('');
                            $("#data").hide();
                            $("#frm").show();
                        }
                    }
                },
                error: function (msg) {
                    alert(msg);
                }
            });
        }

        $("#frmMain").validate({
            rules:
            {
                cboStatus: {
                    required: true,
                },
                txtApproversRemark: {
                    required: true,
                    check_exp: regexLowAscii
                }
            },
            messages:
            {
                cboStatus: {
                    required: MandatoryFieldMsg,
                },
                txtApproversRemark: {
                    required: MandatoryFieldMsg,
                    check_exp: JunkCharMessage
                }
            },
            submitHandler: function (form) {
                $("#data").html('');

                var data = new FormData(form);

                $.ajax({
                    type: "POST",
                    url: "../Accounts/ApproveReject",
                    enctype: 'multipart/form-data',
                    data: data,
                    processData: false,
                    contentType: false,
                    cache: false,
                    success: function (msg) {
                        var Result = JSON.parse(msg);
                        if (Result._STATUS_ == "SUCCESS") {
                            alert(Result._MESSAGE_);
                            window.location = window.location;
                        }
                        else {
                            alert(Result._MESSAGE_);
                        }
                    },
                    error: function (msg) {
                        alert(msg);
                    }
                });
            }
        });
 });