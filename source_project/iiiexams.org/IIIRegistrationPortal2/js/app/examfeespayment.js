$(document).ready(function () {
    $(document)
    .ajaxStart(function () {
        $(".modal").show();
    })
    .ajaxStop(function () {
        $(".modal").hide();
        })

    $("#txtBatchNo").on('keyup keypress blur change', function () {
        $('#hdBatchNo').val('');
        $('#hdAmount').val('');
        $("#tbldata").html('');
        $("#frmMain").hide();
    })

    $("#btnCancel").click(function () {
        window.location = window.location;
    })

    $("#frmFilter").validate({
        rules: {
            txtBatchNo: {
                required: true,
                check_exp: regexAlphaNumeric
            }
        },
        messages: {
            required: MandatoryFieldMsg,
            check_exp: "Please enter valid Transaction Id / URN",
        },
        submitHandler: function (form) {
            var data = new FormData(form);

            $.ajax({
                type: "POST",
                enctype: 'multipart/form-data',
                url: "../Services/GetBatchDetailsForPayment",
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
                            $('#exambatchno').html(result.transaction_id);
                            $('#hdBatchNo').val(result.transaction_id);
                            $('#paymentmode').html(result.payment_mode);
                            $('#noofurns').html(result.total_urns);
                            $('#hdAmount').val(result.grand_total);

                            var URNCount = 0;

                            s = "<thead><tr>";
                            s += "<th>URN</th>";
                            s += "<th>Candidate's Name</th>";
                            s += "<th>Examination Center</th>";
                            s += "<th>TCC Expiry Date</th>";
                            s += "<th>Fees</th>";
                            s += "</tr></thead>";
                            s += "<tbody>";
                            for (i = 0; i < data.length; i++) {
                                s += "<tr>";
                                s += "<td>" + data[i].urn + "</td>";
                                s += "<td>" + data[i].candidate_name + "</td>";
                                s += "<td>" + data[i].exam_center + "</td>";
                                s += "<td>" + data[i].dtTCCExpiryDate + "</td>";
                                s += "<td style='text-align:right'>" + data[i].total_amount.toFixed(2) + "</td>";
                                s += "</tr>";

                                URNCount++;
                            }
                            s += "<tr>";
                            s += "<td colspan=4><b>Total Fees</b></td>";
                            s += "<td style='text-align:right'><b>" + result.grand_total + "</b></td>";
                            s += "</tr>";
                            s += "</tbody>";

                            $('#noofurns').html(URNCount);
                            $("#tbldata").html(s);
                            $("#frmMain").show();
                            
                        }
                    }
                },
                error: function (msg) {
                    HandleAjaxError(msg);
                }
            })
        }
    })
});