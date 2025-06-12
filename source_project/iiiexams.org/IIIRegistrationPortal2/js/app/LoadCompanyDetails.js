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
            txtCompanyName: {
                required: true
            }
        },
        messages: {
            txtCompanyName: {
                required: MandatoryFieldMsg
            }

        },
        submitHandler: function (form) {
            var data = new FormData(form);
            var x = '';
            $.ajax({
                type: "POST",
                enctype: 'multipart/form-data',
                url: "../Utility/LoadCompanyDetails",
                data: data,
                processData: false,
                contentType: false,
                cache: false,
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
                            alert("No data found for entered data.");
                            s = '<h4> No Data Found</h4>'
                            $("#data").html(s);
                            $("#data").show();
                        }
                        else {
                            s = "<table class='table table-striped table-bordered table-hover'>";
                            s += "<thead><tr><td>Company Name</td><td>PAN</td><td>Address</td><td>Contact Person Name</td><td>Email Id</td><td>Corporate User ID</td><td>Request ID</td><td>User ID</td></tr></thead>";
                            for (i = 0; i < data.length; i++) {
                                s += "<tr>";
                                s += "<td>" + data[i].CompanyName + "</td><td>" + data[i].PAN + "</td><td>" + data[i].Address + "</td><td>" + data[i].ContactPersonName + "</td><td>" + data[i].EmailId + "</td> <td> " + data[i].CorporateUserID + "</td ><td>" + data[i].RequestID + "</td><td>" + data[i].UserID + "</td>";
                                s += "</tr>";
                            }
                            s += "</table>";
                            $("#data").html(s);
                            $("#data").show();
                            
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