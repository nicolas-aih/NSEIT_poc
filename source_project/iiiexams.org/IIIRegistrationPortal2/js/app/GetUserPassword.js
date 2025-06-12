$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

    var validator = $("#frmMain").validate({
        rules: {
            txtUserLoginId: {
                required: true                
            }
        },
        messages: {
            txtUserLoginId: {
                required: MandatoryFieldMsg
                
            }           
        },
        submitHandler: function (form) {
            var data = new FormData(form);
            var x = '';
            $.ajax({
                type: "POST",
                enctype: 'multipart/form-data',
                url: "../Utility/GetUserPassword",
                data: data,
                processData: false,
                contentType: false,
                cache: false,
                success: function (msg) {
                    //debugger;
                    var s = '';
                    var Result = JSON.parse(msg);
                    if (Result._STATUS_ === "SUCCESS") {

                        x = '<H4>Password : ' + Result.Password + '<br></H4>';

                        $("#Result").html(x);
                        $("#Result").show();
                        form.reset();
                    }
                    else {
                        x = Result._MESSAGE_;
                        $("#Result").html(x);
                        $("#Result").show();
                    }
                },
                error: function (msg) {
                    HandleAjaxError(msg);
                }
            });
        }
    });
});