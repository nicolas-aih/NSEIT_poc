$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });
    //debugger;
    $("#frm").hide();

    $("#cboStates").change(function (event, DistrictId) {
        $("#cboDistricts").html('');
        var _StateId = $("#cboStates option:selected").val();
        if (_StateId == "" || _StateId == null || _StateId == undefined) {
            return;
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
                if (result._STATUS_ == 'FAIL') {
                    alert(result._MESSAGE_);
                }
                else {
                    var data = JSON.parse(result._DATA_);
                    if (data.length == undefined || data.length == 0) {
                        alert("No districts were found for selected state");
                    }
                    else {
                        s += "<option value=''>-- Select --</option>"
                        for (i = 0; i < data.length; i++) {
                            s += "<option value=" + data[i].district_id + ">" + data[i].district_name + "</option>"
                        }
                        $("#cboDistricts").html(s);
                        if (DistrictId != undefined) {
                            $("#cboDistricts").val(DistrictId);
                        }
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        });
    })

    $("#cboDistrictsF").change(function () {
        $("#data").html('');
        $("#data").hide();
    })

    $("#cboStatesF").change(function () {
        $("#cboDistrictsF").html('');
        $("#data").html('');
        $("#data").hide();
        var _StateId = $("#cboStatesF option:selected").val();
        if (_StateId == "" || _StateId == null || _StateId == undefined)
        {
            return;
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
                debugger;
                var s = '';
                var result = JSON.parse(msg);
                if (result._STATUS_ == 'FAIL') {
                    alert(result._MESSAGE_);
                }
                else {
                    var data = JSON.parse(result._DATA_);
                    if (data.length == undefined || data.length == 0) {
                        alert("No districts were found for selected state");
                    }
                    else {
                        s += "<option value=''>-- Select --</option>"
                        for (i = 0; i < data.length; i++) {
                            s += "<option value=" + data[i].district_id + ">" + data[i].district_name + "</option>"
                        }
                        $("#cboDistrictsF").html(s);
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        });
    })

    $("#frmFilter").validate({
        rules:
        {
            cboStatesF: {
                required: true
            },
            cboDistrictsF: {
                required: true
            }
        },
        messages: {
            cboStatesF: {
                required: MandatoryFieldMsg
            },
            cboDistrictsF: {
                required: MandatoryFieldMsg
            }
        },
        submitHandler: function (form) {
            $("#data").html("");
            var _StateId = $("#cboStatesF option:selected").val();
            var _DistrictId = $("#cboDistrictsF option:selected").val();
            var JsonObject = {
                StateId: _StateId,
                DistrictId: _DistrictId,
            };
            $.ajax({
                type: "POST",
                url: "../Branches/GetBranchesForStateDistrict",
                data: JSON.stringify(JsonObject),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    //debugger;
                    var s = '';
                    var retval = JSON.parse(msg);
                    if (retval._STATUS_ == "SUCCESS") {
                        var data = JSON.parse(retval._DATA_);
                        if (data.length == undefined || data.length == 0) {
                            alert(NO_DATA_FOUND);
                        }
                        else {
                            s = "<table class='table table-striped table-bordered table-hover'>"
                            s += "<thead><tr><td>&nbsp</td><td>Branch Code</td><td>Branch Name</td><td>State Name</td><td>District Name</td><td>Branch Address</td><td>Is Active?</td></tr></thead>";
                            for (i = 0; i < data.length; i++) {
                                s += "<tr>";
                                s += "<td><a href='javascript:edit(" + data[i].BranchNo + ")'>Edit</a></td><td>" + data[i].BranchCode + "</td><td>" + data[i].BranchName + "</td><td>" + data[i].State + "</td><td>" + data[i].DistrictName + "</td><td>" + data[i].BranchAddress + "</td><td>" + data[i].IsActive + "</td>";
                                s += "</tr>";
                            }
                            s += "</table>";
                            $("#data").html(s);
                            $("#data").show();
                        }
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

    window.edit = function (BranchNo) {
        //debugger;
        $("#GridFilter").hide();
        $("#data").hide();
        $("#frm").show();
        var JsonObject = {
            BranchId: BranchNo,
        };
        $.ajax({
            type: "POST",
            url: "../Branches/GetBranchDetailsForModification",
            data: JSON.stringify(JsonObject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                //debugger;
                var s = '';
                var retval = JSON.parse(msg);
                if (retval._STATUS_ == "SUCCESS") {
                    var data = JSON.parse(retval._DATA_);
                    if (data.length == undefined || data.length == 0) {

                    }
                    else {
                        $("#txtBranchNo").val(data[0].BranchNo);
                        $("#txtBranchAddress").val(data[0].BranchAddress);
                        $("#txtBranchCode").val(data[0].BranchCode);
                        $("#txtBranchName").val(data[0].BranchName);
                        $("#txtPlace").val(data[0].BranchPlace);
                        $("#cboStates").val(data[0].StateId);
                        $("#cboStates").trigger("change", [data[0].DistrictId]);
                        $("#cboIsActive").val(data[0].IsActive);
                        //$("#cboDistricts").val(retval[0].DistrictId);
                    }
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

    $("#frmMain").validate({
        rules:
        {
            txtBranchCode: {
                required: true,
                check_exp: regexLowAscii
            },
            txtBranchName: {
                required: true,
                check_exp: regexLowAscii
            },
            txtBranchAddress: {
                required: true,
                check_exp: regexLowAscii
            },
            txtPlace: {
                required: true,
                check_exp: regexAlphaOnlyWithSpace
            },
            cboStates: {
                required: true,
            },
            cboDistricts: {
                required: true,
            },
            cboIsActive: {
                required: true
            }
        },
        messages:
        {
            txtBranchCode: {
                required: MandatoryFieldMsg,
                check_exp: JunkCharMessage
            },
            txtBranchName: {
                required: MandatoryFieldMsg,
                check_exp: JunkCharMessage
            },
            txtBranchAddress: {
                required: MandatoryFieldMsg,
                check_exp: JunkCharMessage
            },
            txtPlace: {
                required: MandatoryFieldMsg,
                check_exp: "Only alphabets with space are allowed"
            },
            cboStates: {
                required: MandatoryFieldMsg,
            },
            cboDistricts: {
                required: MandatoryFieldMsg,
            },
            cboIsActive: {
                required: MandatoryFieldMsg,
            }
        },
        submitHandler: function (form) {
            $("#data").html('');

            var data = new FormData(form);
            $.ajax({
                type: "POST",
                enctype: 'multipart/form-data',
                url: "../Branches/UpdateBranch",
                data: data,
                processData: false,
                contentType: false,
                cache: false,
                success: function (msg) {
                    //debugger;
                    var s = '';
                    var Result = JSON.parse(msg);
                    if (Result._STATUS_ == "SUCCESS") {
                        alert(Result._MESSAGE_);
                        form.reset();
                        $("#frm").hide();
                        $("#GridFilter").show();
                        $("#btnView").trigger("click");
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