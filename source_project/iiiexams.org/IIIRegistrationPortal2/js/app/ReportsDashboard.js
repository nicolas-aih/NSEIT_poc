$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });


    function LoadRequestData() {
        var data = new FormData();  
        $.ajax({
            type: "POST",
            url: "../Reports/LoadReportRequests",
            data: data,
            contentType: false, // Not to set any content header
            processData: false,
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
                        s = "<table class='table table-striped table-bordered table-hover'>"
                        s += "<thead><tr><td>Report Type</td><td>Report Status</td><td>Request Date</td><td>Expiry Date</td><td>&nbsp</td></tr></thead>";
                        for (i = 0; i < data.length; i++) {
                            s += "<tr>";
                            s += "<td>" + data[i].ReportType + "</td><td>" + (data[i].RequestStatus == 'P' ? 'Pending' : 'Completed') + "</td><td>" + data[i].RequestDateTime_T + "</td><td>" + (data[i].KeepFileTill_T == null ? '' : data[i].KeepFileTill_T) + "</td><td>" + (data[i].RequestStatus == 'P' ? 'Report is under process.' : (data[i].ReportFileName == null ? data[i].Message:"<a target='_blank' href='/Reports/DownloadReport?FileName=" + data[i].ReportFileName +"'> Download</a>"))+"</td>";
                            s += "</tr>";
                        }
                        s += "</table>";
                        $("#divData").html(s);
                        $("#divData").show();
                        $("#divDownload").show();

                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        })
    }
    LoadRequestData();    
});