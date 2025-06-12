$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

    $("#frmMain").hide();
    $("#GridFilter").show();
    $("#data").hide();
    loadInsurerData();

    function loadInsurer() {
        $.ajax({
            type: "POST",
            url: "../Services/GetInsurer",
            //data: JSON.stringify(JsonObject),
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
                        alert(result._MESSAGE_);
                    }
                    else {
                        $("#cboInsurer").html('');
                        s += "<option value=''>-- Select --</option>";
                        for (i = 0; i < data.length; i++) {
                            val = data[i].intTblMstInsurerUserID ;
                            text = data[i].varInsurerID + ' - ' + data[i].varName;
                            if (data.length == 1) {
                                s += "<option value='" + val + "' selected>" + text + "</option>";
                            }
                            else {
                                s += "<option value='" + val + "'>" + text + "</option>";
                            }
                        }
                        $("#cboInsurer").html(s);
                        if (data.length == 1) {
                            $('#cboInsurer').trigger("change");
                        }
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        })
    }

    function loadInsurerData() {
        $("#frmMain").hide();
        $.ajax({
            type: "POST",
            url: "../Services/GetDPRangeData",
            //data: JSON.stringify(JsonObject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                debugger;
                var s = '';
                var result = JSON.parse(msg);
                if (result._STATUS_ == 'FAIL') {
                    alert(result._MESSAGE_);
                }
                else {
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
                            s = "<table class='table table-striped table-bordered table-hover'>"
                            s += "<thead><tr><td>Insurer Code</td><td>Insurer Name</td><td>Range From</td><td>Range Till</td><td>Current Value</td></tr></thead>";
                            for (i = 0; i < data.length; i++) {
                                s += "<tr>";
                                s += "<td>" + data[i].varInsurerID + "</td><td>" + data[i].varName + "</td><td>" + data[i].sntDPRangeFrom + "</td><td>" + data[i].sntDPRangeTo + "</td><td>" + data[i].sntCurrentValue + "</td>";
                                s += "</tr>";
                            }
                            s += "</table>";
                            $("#data").html(s);
                            $("#data").show();
                        }
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        })
    };

    $("#butNew").click(function () {
        $("#data").html('');
        $("#data").hide();
        $("#frmMain").trigger("reset");
        $("#frmMain").show();
        $("#GridFilter").hide();
        $("#data").hide();
        loadInsurer();
    });
    $("#frmMain").validate({
        rules:
        {
            txtRangeCount: {
                required: true,
                digits: true,
            },
            cboInsurer: {
                required: true,
                digits: "Please enter valid number"
            },
        },
        messages:
        {
            txtRangeCount: {
                required: MandatoryFieldMsg,
            },
            cboInsurer: {
                required: MandatoryFieldMsg,
            },
        },
        submitHandler: function (form) {
            $("#data").html('');
            var _txtRangeCount = $("#txtRangeCount").val();
            var _txtInsurerCode = $("#cboInsurer option:selected").val();

            var JsonObject = {
                Insurercode: _txtInsurerCode,
                dpCount: _txtRangeCount,
            };
            $.ajax({
                type: "POST",
                url: "../Services/SaveDPRange",
                data: JSON.stringify(JsonObject),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var s = '';
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
                    HandleAjaxError(msg);
                }
            });
        }
    });
    $("#btnCancel").click(function () {
        window.location = window.location;
    });

});