$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

    $("#txtPAN").on("keyup keydown change blur", function () {
        if ($("#txtPAN").val() == undefined || $("#txtPAN").val() == "" || $("#txtPAN").val() == null) {

        }
        else
        {
            $("#data").html('');
        }
    })

    $("#frmMain").validate({
        rules:
        {
            txtPAN: {
                required: true,
                minlength: 10,
                check_exp: /^[A-Z]{3}P[A-Z][0-9]{4}[A-Z]$/
            }
        },
        messages:
        {
            txtPAN: {
                required: MandatoryFieldMsg,
                minlength: "PAN should be 10 characters in length",
                check_exp: "Please enter valid PAN"
            }
        },
        submitHandler: function (form) {
            $("#data").html('');
            var _PAN = $("#txtPAN").val();
            var JsonObject = { PAN: _PAN };
            $.ajax({
                type: "POST",
                url: "../Candidates/PANLookup",
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
                            alert("No URN were found for entered PAN");
                        }
                        else {
                            s = "<table class='table table-striped table-bordered table-hover'>";
                            s += "<thead><tr><td>PAN</td><td>URN</td><td>Insurance Category</td><td>Company Name</td></tr></thead>";
                            for (i = 0; i < data.length; i++) {
                                s += "<tr>";
                                s += "<td>" + data[i].PAN + "</td><td>" + data[i].URN + "</td><td>" + data[i].InsuranceCategory + "</td><td>" + data[i].CompanyName + "</td>";
                                s += "</tr>";
                            }
                            s += "</table>";
                            $("#data").html(s);
                            $("#data").show();
                            $("#txtPAN").val('');
                        }
                    }
                },
                error: function (msg) {
                    HandleAjaxError(msg);
                }
            });
        }
    });
});