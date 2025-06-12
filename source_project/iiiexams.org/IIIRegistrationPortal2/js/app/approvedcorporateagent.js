$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

    $("#r1").click(download);
 
        function download() {
            var data = new FormData();
            $.ajax({
                type: "POST",
                url: "../Reports/ApprovedCorporateAgent",
                data: data,
                contentType: false, // Not to set any content header
                processData: false,
                success: function (msg) {
                    var s = '';
                    var Result = JSON.parse(msg);
                    if (Result._STATUS_ == "SUCCESS") {
                        if (Result._RESPONSE_FILE_) {
                            var link = document.createElement('a');
                            link.href = Result._RESPONSE_FILE_;
                            link.setAttribute("download", "ListOfCompanies.xlsx");
                            link.click();
                        }
                        alert(Result._MESSAGE_);
                    }
                    else {
                        if (Result._RESPONSE_FILE_) {
                            var link = document.createElement('a');
                            link.href = Result._RESPONSE_FILE_;
                            link.setAttribute("download", "ListOfCompanies.xlsx");
                            link.click();
                        }
                        alert(Result._MESSAGE_);
                    }
                },
                error: function (msg) {
                    HandleAjaxError(msg);
                }
            });
        }
});

