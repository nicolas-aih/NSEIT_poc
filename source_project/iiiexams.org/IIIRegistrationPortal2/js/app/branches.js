$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        })

    $("#txtPAN").on("onchange", function () {
        $("#data").html('');
    })

    $("#cboStates").change(function () {
        $("#cboDistricts").html('');
        var _StateId = $("#cboStates option:selected").val();
        if (_StateId == "" || _StateId == undefined || _StateId == null)
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
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        });
    })

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

            data = new FormData(form);

            $.ajax({
                type: "POST",
                enctype: 'multipart/form-data',
                url: "../Branches/AddBranch",
                data: data,
                processData: false,
                contentType: false,
                cache: false,
                success: function (msg) {
                    //debugger;
                    var s = '';
                    var Result = JSON.parse(msg);
                    if (Result._STATUS_ == "SUCCESS") {
                        form.reset();
                        alert(Result._MESSAGE_);
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

    $("#txtFile").change(function () {
        $("#ResponseFile").html('');
    });

    $("#frmUploadMain").validate({
        rules: {
            txtFile:
            {
                required: true,
                extension: "xls|xlsx"
            }
        },
        messages: {
            txtFile:
            {
                required: MandatoryFieldMsg,
                extension: "Please upload excel file (*.xls / *.xlsx)"
            }
        },
        submitHandler: function (form) {
            //debugger;
            var data = new FormData(form);

            $.ajax({
                type: "POST",
                enctype: 'multipart/form-data',
                url: "../Branches/UploadBranches",
                data: data,
                processData: false,
                contentType: false,
                cache: false,
                success: function (msg) {
                    //debugger;
                    var s = '';
                    var Result = JSON.parse(msg);
                    if (Result._STATUS_ == "SUCCESS") {
                        $("#ResponseFile").html('');
                        $("#ResponseFile").html("<label><a href='" + Result._RESPONSE_FILE_ + "'>Download Response File</a></label>");
                        alert(Result._MESSAGE_);
                    }
                    else {
                        if (Result._RESPONSE_FILE_) {
                            $("#ResponseFile").html('');
                            $("#ResponseFile").html("<label><a href='" + Result._RESPONSE_FILE_ + "'>Download Response File</a></label>");
                        }
                        alert(Result._MESSAGE_);
                    }
                    form.reset();
                },
                error: function (msg) {
                    HandleAjaxError(msg);
                    //alert(msg);
                }
            });
        }
    });
});
