$(document).ready(function () {
    //$("#result").hide();
    $("#butCancel").click(function () {
        window.location = '/Candidates/UpdateUrnStatus';
    });
    $("#frmMain").validate({
        rules:
        {
            urn: {
                required: true,
            },
        },
        messages:
        {
            urn: {
                required: MandatoryFieldMsg,
            },
        },

        submitHandler: function (form) {
            var data = new FormData(form);
            $.ajax({
                type: "POST",
                enctype: 'multipart/form-data',
                url: "../Candidates/UpdateUrnStatus",
                data: data,
                processData: false,
                contentType: false,
                cache: false,
                success: function (msg) {
                    var s = '';
                    var Result = JSON.parse(msg);
                    if (Result._STATUS_ == "SUCCESS") {
                       
                        //$("#ResponseFile").html("<label> " + Result._MESSAGE_ + " </label>");
                        alert(Result._MESSAGE_);
                    }
                    else {
                        alert("Error");
                    }
                    form.reset();
                },
                error: function (msg) {
                    HandleAjaxError(msg);
                }
            });
        }
    });
})