$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

    $("#txtURN").on("keyup keydown blur change", function () {
        if ($("#txtURN").val() == undefined || $("#txtURN").val() == "" || $("#txtURN").val() == null)
        {

        }
        else{
            $("#data").html('');
            $("#data").hide();
        }
    });

    $("#frmMain").validate({
        rules:
        {
            txtURN: {
                required: true,
                minlength: 13
            }
        },
        messages:
        {
            txtURN: {
                required: MandatoryFieldMsg,
                minlength: "URN should be minimum 13 characters in length"
            }
        },
        submitHandler: function (form) {
            form.submit();
        }
    });
});

function DownloadP(urn) {
    var data = new FormData();
    data.append("txtURN", urn);
    data.append("Type", "B");
    $.ajax({
        type: "POST",
        enctype: 'multipart/form-data',
        url: "../Candidates/DownloadQualData",
        data: data,
        processData: false,
        contentType: false,
        cache: false,
        success: function (msg) {
            //debugger;
            var s = '';
            var Result = JSON.parse(msg);
            if (Result._STATUS_ == "SUCCESS") {
                //window.open(Result._RESPONSE_FILE_, "_blank");
                var link = document.createElement('a');
                link.href = Result._RESPONSE_FILE_;
                link.setAttribute("download", "BasicQual_" + urn + ".jpg");
                link.click();
                //$("#Results").html('');
                //$("#Results").html('<label><a href=' + Result._RESPONSE_FILE_ + ' target = "_blank">Download File</a></label>');
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

function DownloadQ(urn) {
    var data = new FormData();
    data.append("txtURN", urn);
    data.append("Type", "P");
    $.ajax({
        type: "POST",
        enctype: 'multipart/form-data',
        url: "../Candidates/DownloadQualData",
        data: data,
        processData: false,
        contentType: false,
        cache: false,
        success: function (msg) {
            //debugger;
            var s = '';
            var Result = JSON.parse(msg);
            if (Result._STATUS_ == "SUCCESS") {
                //window.open(Result._RESPONSE_FILE_, "_blank");
                var link = document.createElement('a');
                link.href = Result._RESPONSE_FILE_;
                link.setAttribute("download", "ProfessionalQual_" + urn + ".jpg");
                
                link.click();
                //$("#Results").html('');
                //$("#Results").html('<label><a href=' + Result._RESPONSE_FILE_ + ' target = "_blank">Download File</a></label>');
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


    function DownloadHT(urn)
    {
        var data = new FormData();
        data.append("txtURN", urn);
        $.ajax({
            type: "POST",
            enctype: 'multipart/form-data',
            url: "../Candidates/HallTicket2",
            data: data,
            processData: false,
            contentType: false,
            cache: false,
            success: function (msg) {
                //debugger;
                var s = '';
                var Result = JSON.parse(msg);
                if (Result._STATUS_ == "SUCCESS") {
                    window.open(Result._RESPONSE_FILE_, "_blank");
                    //$("#Results").html('');
                    //$("#Results").html('<label><a href=' + Result._RESPONSE_FILE_ + ' target = "_blank">Download File</a></label>');
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

    function DownloadSc(urn)
    {
        var data = new FormData();
        data.append("txtURN", urn);
        $.ajax({
            type: "POST",
            enctype: 'multipart/form-data',
            url: "../Candidates/Scorecard2",
            data: data,
            processData: false,
            contentType: false,
            cache: false,
            success: function (msg) {
                //debugger;
                var s = '';
                var Result = JSON.parse(msg);
                if (Result._STATUS_ == "SUCCESS") {
                    window.open(Result._RESPONSE_FILE_, "_blank");
                    //$("#Results").html('');
                    //$("#Results").html('<label><a href=' + Result._RESPONSE_FILE_ + ' target = "_blank">Download File</a></label>');
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
