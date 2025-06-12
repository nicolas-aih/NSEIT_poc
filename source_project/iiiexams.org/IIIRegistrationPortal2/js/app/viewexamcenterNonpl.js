$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

    $("#data").hide();
    $("#GridFilter").show();

    window.cl = function() {
        $.ajax({
            type: "POST",
            url: "../Home/cl",
            enctype: 'multipart/form-data',
            data: null,
            processData: false,
            contentType: false,
            cache: false,
            success: function (msg) {
                var Result = JSON.parse(msg);
                if (Result._STATUS_ === "SUCCESS") {
                    if (Result._RESPONSE_FILE_) {
                        $("#ResponseFile").html('');
                        $("#ResponseFile").html("<label><a href='" + Result._RESPONSE_FILE_ + "'>Download File</a></label>");
                    }
                    alert(Result._MESSAGE_);
                }
                else {
                    alert(Result._MESSAGE_);
                }
            }
        })
    }


    $("#cboStatesF").change(function () {
        var _StateId = $("#cboStatesF option:selected").val();
        if (_StateId == '' || _StateId == undefined || _StateId == null) {
            $("#data").html('');
            return;
        }
        var JsonObject = {
            StateId: _StateId,
            //CenterId : -1
        };
        $.ajax({
            type: "POST",
            url: "../Services/GetCentersForStatePreL",
            data: JSON.stringify(JsonObject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                //debugger;
                var s = '';
                var result = JSON.parse(msg);
                if (result._STATUS_ == 'FAIL') {
                    $("#data").html('');
                    alert(NO_DATA_FOUND);
                }
                else {
                    var data = JSON.parse(result._DATA_);
                    if (data.length == undefined || data.length == 0) {
                        $("#data").html('');
                        alert(NO_DATA_FOUND);
                    }
                    else {
                        s = "<table class='table table-striped table-bordered table-hover'>"
                        s += "<thead><tr><td>Center Name</td><td>Address</td><td>Pincode</td><td>District</td></tr></thead>";
                        for (i = 0; i < data.length; i++) {
                            s += "<tr>";
                            s += "<td>" + data[i].varExamCenterName + "</td>";
                            s += "<td>" + data[i].varHouseNo + "<br>" + data[i].varStreet + "<br>" + data[i].varTown + "</td>";
                            s += "<td>" + data[i].intPincode + "</td>"
                            s += "<td>" + data[i].varDistrictName + "</td>"
                            s += "</tr>";
                        }
                        s += "</table>";
                        $("#data").html(s);
                        $("#data").show();
                    }
                }
            },
            error: function (msg) {
                alert(msg);
            }
        })
    })
});