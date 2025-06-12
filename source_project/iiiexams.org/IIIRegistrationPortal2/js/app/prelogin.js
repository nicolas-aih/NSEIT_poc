$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

    $("#pnlForgotPassword").hide();
    $("#pnlChangePassword").hide();
    $("#fp").click(function () {
        $("#frmForgotPassword").trigger("reset");
        clearValidation($("#frmForgotPassword"));
        $("#pnlLoginBox").hide();
        $("#pnlForgotPassword").show();
        $("#pnlChangePassword").hide();
    });
    $("#cp").click(function () {
        $("#frmChangePassword").trigger("reset");
        clearValidation($("#frmChangePassword"));
        $("#pnlLoginBox").hide();
        $("#pnlForgotPassword").hide();
        $("#pnlChangePassword").show();
    });

    $("#backToLogin").click(function () {
        $("#frmLogin").trigger("reset");
        clearValidation($("#frmLogin"));
        $("#pnlLoginBox").show();
        $("#pnlChangePassword").hide();
        $("#pnlForgotPassword").hide();
    });
    $("#backToLogin2").click(function () {
        $("#frmLogin").trigger("reset");
        clearValidation($("#frmLogin"));
        $("#pnlLoginBox").show();
        $("#pnlChangePassword").hide();
        $("#pnlForgotPassword").hide();
    });

    $("#frmSearchCenters").validate({ // initialize the plugin
        rules: {
            txtPincode: {
                required: true,
                digits: true,
                minlength: 6,
            }
        },
        messages: {
            txtPincode: {
                required: MandatoryFieldMsg,
                digits: ValidPincodeMsg,
                minlength: ValidPincodeMsg,
            }
        },
        submitHandler: function (form) {
            var _Pincode = $("#txtPincode").val();
            var JsonObject = { Pincode: _Pincode };
            $.ajax({
                type: "POST",
                url: "../Home/FindNearest3ExamCenter",
                data: JSON.stringify(JsonObject),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var s = '';
                    var result = JSON.parse(msg);
                    if (result._STATUS_ == 'FAIL') {
                        alert(result._ERROR_);
                    }
                    else {
                        $("#Nearest3Centers").html('');
                        var data = JSON.parse(result._DATA_);
                        if (data.length == undefined) {
                            s += "<span>No data found</span>"
                        }
                        else {
                            for (i = 0; i < data.length; i++) {
                                s += "<li>" + data[i].varExamCenterName + "<br /><span>" + data[i].numDistance + " (kms)</span></li>";
                            }
                        }
                        $("#Nearest3Centers").html(s);
                    }
                    //alert(msg);
                },
                error: function (msg) {
                    HandleAjaxError(msg);
                }
            });
        }
    });

    $("#frmLogin").validate({
        rules: {
            txtUserId: {
                required: true,
            },
            txtPassword: {
                required: true,
            }
        },
        messages: {
            txtUserId: {
                required: MandatoryFieldMsg,
            },
            txtPassword: {
                required: MandatoryFieldMsg,
            }
        },
        submitHandler: function (form) {
            //if (sessionStorage.getItem("_isloggedon") != null)
            //{
            //    alert(sessionStorage.getItem("_isloggedon"));
            //}
            //if (sessionStorage.getItem("_isloggedon")== "Y") {
            //    alert("Another User Is Already Logged On In This Browser. Unable To Proceed. Either Logoff and then login OR Open New Browser To Login.");
            //    return;
            //}
            var _UserId = $("#txtUserId").val();
            var _Password = $("#txtPassword").val();
            var JsonObject = { UserId: _UserId, Password: _Password };
            //debugger;
            $.ajax({
                type: "POST",
                url: "../Users/Login",
                data: JSON.stringify(JsonObject),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    //debugger;
                    var Result = JSON.parse(msg);
                    if (Result._STATUS_ == "SUCCESS") {
                        //if (sessionStorage) {
                        //    sessionStorage.setItem("_isloggedon", "Y");
                        //}
                        location.href = "../Home/Index";
                        sessionStorage.clear();
                    }
                    else {
                        //sessionStorage.removeItem("_isloggedon")
                        alert(Result._MESSAGE_);
                    }
                    form.reset();
                    $("#txtUserId").focus();
                },
                error: function (msg) {
                    HandleAjaxError(msg);
                }
            });
        }
    });

    $("#frmChangePassword").validate({
        rules: {
            txtcpUserID: {
                required: true
            },
            txtcpOldPwd: {
                required: true
            },
            txtcpNewPwd: {
                required: true
            },
            txtcpConfirmPwd: {
                required: true,
                equalTo: "#txtcpNewPwd"
            }
        },
        messages: {
            txtcpUserID: {
                required: MandatoryFieldMsg
            },
            txtcpOldPwd: {
                required: MandatoryFieldMsg
            },
            txtcpNewPwd: {
                required: MandatoryFieldMsg
            },
            txtcpConfirmPwd: {
                required: MandatoryFieldMsg,
                equalTo: "The value entered does not match with the new password entered above"
            }
        },
        submitHandler: function (form) {
            //debugger;
            var _UserId = $("#txtcpUserID").val();
            var _Password = $("#txtcpOldPwd").val();
            var _NewPassword = $("#txtcpNewPwd").val();
            var _ConfirmPassword = $("#txtcpConfirmPwd").val();
            var JsonObject = { UserId: _UserId, Password: _Password, NewPassword: _NewPassword, ConfirmPassword: _ConfirmPassword };
            //debugger;
            $.ajax({
                type: "POST",
                url: "../Users/ChangePassword",
                data: JSON.stringify(JsonObject),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    //debugger;
                    var Result = JSON.parse(msg);
                    if (Result._STATUS_ == "SUCCESS") {
                        alert(Result._MESSAGE_);
                    }
                    else {
                        alert(Result._MESSAGE_);
                    }
                    form.reset();
                    $("#txtcpUserID").focus();
                },
                error: function (msg) {
                    HandleAjaxError(msg);
                }
            });
        }
    });

    $("#frmForgotPassword").validate({
        rules: {
            txtfpUserId: {
                required: true
            },
            txtfpEmailId: {
                required: true,
                email: true
            }
        },
        messages: {
            txtfpUserId: {
                required: MandatoryFieldMsg
            },
            txtfpEmailId: {
                required: MandatoryFieldMsg,
                email: "Please enter valid email id"
            }
        },
        submitHandler: function (form) {
            //debugger;
            var _UserId = $("#txtfpUserId").val();
            var _EmailId = $("#txtfpEmailId").val();
            var JsonObject = { UserId: _UserId, EmailId: _EmailId };
            //debugger;
            $.ajax({
                type: "POST",
                url: "../Users/ResetPassword",
                data: JSON.stringify(JsonObject),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var Result = JSON.parse(msg);
                    if (Result._STATUS_ == "SUCCESS") {
                        alert(Result._MESSAGE_);
                    }
                    else {
                        alert(Result._MESSAGE_);
                    }
                    form.reset();
                    $("#txtfpUserId").focus();
                },
                error: function (msg) {
                    HandleAjaxError(msg);
                }
            });
        }
    });
});