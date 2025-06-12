$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

    $("#cboStatus").change(function () {
        $("#data").html("");

        var Status = $("#cboStatus option:selected").val();
        if (Status === "") {
            alert("Please select status");
            return;
        }

        var data = new FormData();
        data.append("cboStatus", Status);
        $.ajax({
            type: "POST",
            url: "../Candidates/GetPendingURNApprovals",
            enctype: 'multipart/form-data',
            data: data,
            processData: false,
            contentType: false,
            cache: false,
            success: function (msg) {
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
                        s = "<table class='table table-striped table-bordered table-hover'>";
                        s += "<thead><tr>";
                            s += "<td>Company Name</td>";
                            s += "<td>Company Type</td>";
                            s += "<td>License Type</td>";
                            s += "<td>Request Date</td>";
                            s += "<td>Last Modified On</td>";
                            s += "<td>Candidate Name</td>";
                            s += "<td>Basic Qualification</td>";
                            s += "<td>Board Name</td>";
                            s += "<td>Roll number</td>";
                            s += "<td>Year of passing</td>";
                            s += "<td>Professional Qualification</td>";
                            s += "<td>Contact Details</td>";
                            //if (Status === "P") {
                            //    s += "<td>Approve / Reject</td>";
                            //}
                            if (Status === "R") {
                                s += "<td>Action</td>";
                            }
                            s += "<td>Approver's remarks</td>";
                            s += "<td>Status</td>";
                            s += "<td>Approved / Rejected By</td>";
                            s += "<td>Approval / Rejection timestamp</td>";
                            if (Status === "A") {
                                s += "<td>URN</td>";
                            }
                        s += "</tr></thead>";
                        for (i = 0; i < data.length; i++) {
                            s += "<tr>";
                            s += "<td>" + data[i].companyname + "</td>";
                            s += "<td>" + data[i].licensetype + "</td>";
                            s += "<td>" + data[i].corporatetype + "</td>";
                            s += "<td>" + data[i].dtcreatedon + "</td>";
                            s += "<td>" + data[i].dtlastmodifiedon + "</td>";
                            s += "<td>" + data[i].chrnameinitial + " " + data[i].varapplicantname + "</td>";
                            s += "<td><a href='javascript:r(" + data[i].bntapplicantdataid + ")'>" + data[i].chrbasicqualification + "</a></td>";
                            //s += "<td>" + data[i].chrbasicqualification + "</td>";
                            s += "<td>" + data[i].varbasicqualboardname + "</td>";
                            s += "<td>" + data[i].varbasicqualrollnumber + "</td>";
                            s += "<td>" + data[i].dtbasicqualyearofpassing + "</td>";
                            s += "<td><a href='javascript:q(" + data[i].bntapplicantdataid + ")'>" + data[i].chrqualification + "</a></td>";
                            s += "<td>" + data[i].varMobileNo + "<br>" + data[i].varEmailID + "</td>";
                            //if (Status === "P") {// Swap these based on the User id...
                            //    s += "<td><a href='javascript:a(" + data[i].bntapplicantdataid + ")'>Approve / Reject</a></td>";
                            //}
                            if (Status === "R") {
                                s += "<td><a href='javascript:m(" + data[i].bntapplicantdataid + ")'>Edit</a></td>";
                            }

                            s += "<td>" + data[i].approvers_remarks + "</td>";
                            s += "<td>" + data[i].status + "</td>";
                            s += "<td>" + data[i].approved_by + "</td>";
                            s += "<td>" + data[i].approval_timestamp + "</td>";
                            if (Status === "A") {
                                s += "<td>" + data[i].chrRollNumber + "</td>";
                            }
                            s += "</tr>";
                        }
                        s += "</table>";
                    }
                    $("#data").html(s);
                    $("#data").show();
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        });
    });

    window.a = function (Id) {
        $("#hddId").val(Id);
        $("#divForm").dialog({
            height: 300,
            width: 300,
            modal: true,
            resizable: false
        });
    };

    window.m = function (k) {
        window.location = "../Candidates/URNRequestModification?Id=" + k;
    };

    window.q = function (Id) {
        var data = new FormData();
        data.append("Id", Id);
        $.ajax({
            type: "POST",
            url: "../Candidates/DownloadQD",
            enctype: 'multipart/form-data',
            data: data,
            processData: false,
            contentType: false,
            cache: false,
            success: function (msg) {
                var Result = JSON.parse(msg);
                if (Result._STATUS_ === "SUCCESS") {
                    if (Result._RESPONSE_FILE_) {
                        window.open(Result._RESPONSE_FILE_);
                    }
                    //alert(Result._MESSAGE_);
                }
                else {
                    alert(Result._MESSAGE_);
                }
            }
        });
    }

    window.r = function (Id) {
        var data = new FormData();
        data.append("Id", Id);
        $.ajax({
            type: "POST",
            url: "../Candidates/DownloadBQD",
            enctype: 'multipart/form-data',
            data: data,
            processData: false,
            contentType: false,
            cache: false,
            success: function (msg) {
                var Result = JSON.parse(msg);
                if (Result._STATUS_ === "SUCCESS") {
                    if (Result._RESPONSE_FILE_) {
                        window.open(Result._RESPONSE_FILE_);
                    }
                    //alert(Result._MESSAGE_);
                }
                else {
                    alert(Result._MESSAGE_);
                }
            }
        });
    };


    $("#butCancel").click(function () {
        $("#frmApp").trigger("reset");
        $("#divForm").dialog("close");
    });

    $("#frmApp").validate({
        igonre: [],
        rules: {
            cboStatus2: {
                required: true
            },
            txtRemarks: {
                required: true
            }
        },
        messages: {
            cboStatus2: {
                required: MandatoryFieldMsg
            },
            txtRemarks: {
                required: MandatoryFieldMsg
            }
        },
        submitHandler: function (form) {
            var data = new FormData(form);
            $.ajax({
                type: "POST",
                enctype: 'multipart/form-data',
                url: "../Candidates/ApproveRejectURN",
                data: data,
                processData: false,
                contentType: false,
                cache: false,
                success: function (msg) {
                    var Result = JSON.parse(msg);
                    if (Result._STATUS_ === "SUCCESS") {
                        alert(Result._MESSAGE_);
                        $("#divForm").dialog("close");
                        $("#frmApp").trigger("reset");
                        $("#cboStatus").trigger("change");
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