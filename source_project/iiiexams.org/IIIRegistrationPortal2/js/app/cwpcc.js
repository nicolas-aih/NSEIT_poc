$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

    generatereport();

    function generatereport() {
        alert('Called');
        $("#ResponseFile").html('');
        $.ajax({
            type: "POST",
            url: "../Reports/CenterWisePendingScheduleCountReport",
            data: null,
            processData: false,
            contentType: false,
            cache: false,
            success: function (msg) {

                var Result = JSON.parse(msg);
                if (Result._STATUS_ == "SUCCESS") {
                    $("#ResponseFile").html('');
                    $("#ResponseFile").html("<label><a href='" + Result._RESPONSE_FILE_ + "'>Download Response File</a></label>");
                    alert(Result._MESSAGE_);
                }
                else {
                    if (Result._RESPONSE_FILE_) {
                        $("#ResponseFile").html('');
                        $("#ResponseFile").html("<label><a href='" + Result._RESPONSE_FILE_ + "'> Download Response File</a></label>");
                    }
                    alert(Result._MESSAGE_);
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        })
    }
})