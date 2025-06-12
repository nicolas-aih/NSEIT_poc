$(document).ready(function () {
    loadUserRoles();
    loadUsers();

    jQuery.validator.addMethod("ValidateLoginIDPattern", function (value, element) {
        if (value == undefined || value == "" || value == null) {
            return true;
        }
        var numRegex = ".*[0-9].*";
        var alphaRegex = ".*[A-Za-z].*";

        if (value.match(regexAlphaNumeric) && value.match(numRegex) && value.match(alphaRegex)) {
            return true;
        }
        else {
            return false;
        }
    }, "The login id must be alpha numeric and must contain combination of alphabets and number");

    function loadUsers() {
        debugger;
        var JsonObject = {
            UserId: -1,
        };
        $.ajax({
            type: "POST",
            url: "../Services/GetUsers",
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
                        alert(NO_DATA_FOUND);
                    }
                    else {
                        s = "<table class='table table-striped table-bordered table-hover' width='1500px'>"
                        s += "<thead><tr><td>&nbsp</td><td>User Login Id</td><td>User Name</td><td>Is Active</td></tr></thead>";
                        //sntRoleID, varRoleName, varRemark, bitIsActive, role_code
                        for (i = 0; i < data.length; i++) {
                            s += "<tr>";
                            s += "<td><a href='javascript:edit(" + data[i].intUserId + ")'>Edit</a></td><td width='300px'>" + data[i].varUserLoginId + "</td><td width='300px'>" + data[i].varUserName + "</td><td>" + (data[i].bitIsActive ? 'Active':'Inactive') + "</td>";
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
    function loadUserRoles() {
        debugger;
        $.ajax({
            type: "POST",
            url: "../Services/GetRolesForUserCreation",
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
                        //sntRoleID, varRoleName, varRemark, bitIsActive, role_code
                        s = "<option value=''>-- Select --</option>";
                        for (i = 0; i < data.length; i++) {
                            s += "<option value=" + data[i].sntRoleID + ">" + data[i].varRoleName + "</option>"
                        }
                        $("#cboRoles").html(s);
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
        $("#txtLoginId").removeAttr('disabled');
        $("#frmMain").show();
        $("#hdnUserId").val('0');
        $("#GridFilter").hide();
        $("#data").html();
        $("#data").hide();
    });

    $('#butCancel').click(function () {
        window.location = window.location;
    })

    $("#frmMain").validate({
        ignore:[],
        rules:
        {
            txtLoginId: {
                required: true,
                minlength: 3,
                check_exp: regexAlphaNumeric,
                ValidateLoginIDPattern: true
            },
            txtUserName: {
                required: true,
                minlength: 5,
                check_exp: regexAlphaOnlyWithSpace,
            },
            cboRoles: {
                required: true,
            },
            txtMobileNo: {
                required: true,
                digits: true,
                minlength: 10
            },
            txtEmailID: {
                required: true,
                email: true
            },

        },
        messages:
        {
            txtLoginId: {
                required: MandatoryFieldMsg,
                minlength: "Login Id must be minimum 3 characters long",
                check_exp: "Only alphabets and numbers without leading / trailing space are allowed",
                //ValidateLoginIDPattern: true
            },
            txtUserName: {
                required: MandatoryFieldMsg,
                minlength: "Name should be minimum 5 characters long",
                check_exp: "Please enter valid name. Only alphabets and space without leading / trailing space are allowed"
            },
            cboRoles: {
                required: MandatoryFieldMsg,
            },
            txtMobileNo: {
                required: MandatoryFieldMsg,
                digits: ValidMobileMsg,
                minlength: ValidMobileMsg
            },
            txtEmailID: {
                required: MandatoryFieldMsg,
                email: ValidEmailIdMsg
            },
        },
        submitHandler: function (form) {
            var data = new FormData(form);
            $.ajax({
                type: "POST",
                url: "../Users/SaveUser",
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
                    alert(msg);
                }
            });
        }
    });

    window.edit = function (userId) {
        debugger;
        var JsonObject = {
            UserId: userId,
        };
        $.ajax({
            type: "POST",
            url: "../Services/GetUsers",
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
                        $("#txtLoginId").val(data[0].varUserLoginId);
                        $("#txtLoginId").attr("readonly", "readonly");
                        $("#hdnUserId").val(data[0].intUserId);
                        $("#txtUserName").val(data[0].varUserName);
                        $("#cboRoles").val(data[0].sntRoleID);

                        $("#txtMobileNo").val(data[0].varMobileNo);
                        $("#txtEmailID").val(data[0].varEMailId);
                        var isactive = data[0].bitIsActive;
                        if (isactive == false) {
                            $("#chkIsActive").attr('checked', false)
                        }
                        else {
                            $("#chkIsActive").attr('checked', true)
                        }

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