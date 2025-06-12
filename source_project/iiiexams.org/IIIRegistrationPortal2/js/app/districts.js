$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

    $("#cboStates").change(function (event, DistrictId, Pincode) {
        $("#data").html('');
        $("#data").hide();
        var _StateId = $("#cboStates option:selected").val();
        if (_StateId === "") {
            _StateId = 0;
        }
        var JsonObject = {
            StateId: _StateId,
        };
        $.ajax({
            type: "POST",
            url: "../Home/GetDistrictsForStates",
            data: JSON.stringify(JsonObject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                //debugger;
                var s = '';
                var result = JSON.parse(msg);
                if (result._STATUS_ === 'FAIL') {
                    alert(result._MESSAGE_);
                }
                else {
                    var data = JSON.parse(result._DATA_);
                    DistrictDetails = data;
                    if (data.length == undefined || data.length == 0) {
                        alert("No districts were found for selected state");
                    }
                    else {
                        s = "<table class='table table-striped table-bordered table-hover'>"
                        if (_StateId == 0) {
                            s += "<thead><tr><td>State</td><td>District Id</td><td>District Name</td><td>Pincode - From</td><td>Pincode - Till</td></tr></thead>";
                            for (i = 0; i < data.length; i++) {
                                s += "<tr>";
                                s += "<td>" + data[i].state_name + "</td><td>" + data[i].district_id + "</td><td>" + data[i].district_name + "</td><td>" + ((data[i].from_pincode == 0) ? "-" : data[i].from_pincode) + "</td><td>" + ((data[i].to_pincode == 0) ? "-" : data[i].from_pincode) + "</td>";
                                s += "</tr>";
                            }
                        }
                        else {
                            s += "<thead><tr><td>District Id</td><td>District Name</td><td>Pincode - From</td><td>Pincode - Till</td></tr></thead>";
                            for (i = 0; i < data.length; i++) {
                                s += "<tr>";
                                s += "<td>" + data[i].district_id + "</td><td>" + data[i].district_name + "</td><td>" + ((data[i].from_pincode == 0) ? "-" : data[i].from_pincode) + "</td><td>" + ((data[i].to_pincode == 0) ? "-" : data[i].from_pincode) + "</td>";
                                s += "</tr>";
                            }
                        }
                        s += "</table>";
                        $("#data").html(s);
                        $("#data").show();
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        });
    });

    window.downloadalldistricts = function  () {
        var JsonObject = {
            StateId: 0
        };
        $.ajax({
            type: "POST",
            url: "../Home/DownloadDistricts",
            data: JSON.stringify(JsonObject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                //debugger;
                var s = '';
                var result = JSON.parse(msg);
                if (result._STATUS_ === 'FAIL') {
                    alert(result._MESSAGE_);
                }
                else {
                    if (result._RESPONSE_FILE_) {
                        var link = document.createElement('a');
                        link.href = result._RESPONSE_FILE_;
                        link.setAttribute("download", "DistrictList.xlsx");
                        link.click();
                        //alert(Result._MESSAGE_);
                    }
                    else {
                        alert(Result._MESSAGE_);
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        });
    };
});
