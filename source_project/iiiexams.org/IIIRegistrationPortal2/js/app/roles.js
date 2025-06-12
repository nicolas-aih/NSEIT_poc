$(document).ready(function () {
    loadRoles();
    function loadRoles() {
        debugger;
        $.ajax({
            type: "POST",
            url: "../Services/GetAllRoles",
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
                        alert(NO_DATA_FOUND);
                    }
                    else {
                        s = "<table class='table table-striped table-bordered table-hover' width='1500px'>"
                        s += "<thead><tr><td>&nbsp</td><td>Role Name</td><td>Description</td><td>Is Active</td></tr></thead>";
                        //sntRoleID, varRoleName, varRemark, bitIsActive, role_code
                        for (i = 0; i < data.length; i++) {
                            s += "<tr>";
                            s += "<td><a href='javascript:edit(" + data[i].sntRoleID + ")'>Edit</a></td><td width='300px'>" + data[i].varRoleName + "</td><td width='300px'>" + data[i].varRemark + "</td><td>" + (data[i].bitIsActive ? 'Active':'Inactive') + "</td>";
                            s += "</tr>";
                        }
                        s += "</table>";

                        $("#data").html(s);
                        $("#data").show();
                        $("#frmMain").hide();
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        })
    }

    $("#butNew").click(function () {
        $("#frmMain").trigger("reset");
        $("#txtRoleCode").removeAttr('disabled');
        $("#txtRoleName").removeAttr('disabled');
        $("#frmMain").show();
        $("#hdnRoleId").val('0');
        $("#GridFilter").hide();
        $("#data").html();
        $("#data").hide();
    });

    $('#butCancel').click(function () {
        window.location = window.location;
    })


    $("#frmMain").validate({
        rules:
        {
            txtRoleCode: {
                required: true,
                check_exp: regexAlphaNumericWithSpace
            },
            txtRoleName: {
                required: true,
                check_exp: regexAlphaNumericWithSpace
            },
            txtRoleDescription: {
                required: true,
                check_exp: regexAlphaNumericWithSpace
            },
        },
        messages:
        {
            txtRoleCode: {
                required: MandatoryFieldMsg,
                check_exp: "Only alphabets, numbers and space without leading / trailing space are allowed"
            },
            txtRoleName: {
                required: MandatoryFieldMsg,
                check_exp: "Only alphabets, numbers and space without leading / trailing space are allowed"
            },
            txtRoleDescription: {
                required: MandatoryFieldMsg,
                check_exp: "Only alphabets, numbers and space without leading / trailing space are allowed"
            },
        },
        submitHandler: function (form) {
            var data = new FormData(form);
            $.ajax({
                type: "POST",
                url: "../Users/SaveRole",
                data: data,
                contentType: false, // Not to set any content header
                processData: false, // Not to process data
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
                    HandleAjaxError(msg);
                }
            });
        }
    });

    window.edit = function (roleId) {
        debugger;
        var JsonObject = {
            RoleId: roleId,
        };
        $.ajax({
            type: "POST",
            url: "../Services/GetAllRoles",
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
                        Alert("No data found");
                        $("#data").show();
                        $("#frmMain").hide();
                    }
                    else {
                        $("#frmMain").trigger("reset");
                        //----- OK
                        
                        $("#hdnRoleId").val(data[0].sntRoleID);
                        $("#txtRoleCode").val(data[0].role_code);
                        $("#txtRoleName").val(data[0].varRoleName);
                        $("#txtRoleDescription").val(data[0].varRemark);

                        var isactive = data[0].bitIsActive;
                        if (isactive == false) {
                            $("#chkIsActive").attr('checked', false)
                        }
                        else {
                            $("#chkIsActive").attr('checked', true)
                        }
                        //----- End OK
                        //Control State
                        $("#txtRoleCode").attr('disabled', 'disabled');
                        $("#txtRoleName").attr('disabled', 'disabled');
                        //End Control State

                        $("#data").hide();
                        $("#GridFilter").hide();
                        $("#frmMain").valid();
                        $("#frmMain").show();
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        })
    }
});