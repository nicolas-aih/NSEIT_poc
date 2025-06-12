$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

    $("#frmMain").validate({
        rules:
        {
            cboCompanyType: {
                required: true,
            },
            cboCompany: {
                required: true,
            },
        },
        messages:
        {
            cboCompanyType: {
                required: MandatoryFieldMsg,
            },
            cboCompany: {
                required: MandatoryFieldMsg,
            },
        },
        submitHandler: function (form) {
            $("#data").html('');

            var data = new FormData(form);

            $.ajax({
                type: "POST",
                url: "../Accounts/CreditBalance2",
                enctype: 'multipart/form-data',
                data: data,
                processData: false,
                contentType: false,
                cache: false,
                success: function (msg) {
                    debugger;
                    var s = '';
                    var result = JSON.parse(msg);
                    if (result._STATUS_ == 'FAIL') {
                        alert(result._MESSAGE_);
                        
                    }
                    else
                    {
                        $("#data").html(result._MESSAGE_);
                        //window.location = window.location;
                    }
                },
                error: function (msg) {
                    alert(msg);
                }
            });
        }
    });

    $("#cboCompany").change(function () {
        $("#data").html('');
    });

    $("#cboCompanyType").change(function () {
        $("#data").html('');
        $("#cboCompany").html('');
        var CompanyType = $("#cboCompanyType option:selected").val();
        if (CompanyType === "") {
            alert("Please select company type");
            return;
        }

        var data = new FormData();
        data.append("cboCompanyType", CompanyType);

        $.ajax({
            type: "POST",
            url: "../Accounts/GetCompanyList",
            enctype: 'multipart/form-data',
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
                        alert("No companies found for selected company type");
                    }
                    else {
                        s = ""
                        for (i = 0; i < data.length; i++) {
                            s += "<option value='" + data[i].company_code + "'>" + data[i].company_name + "</option>"
                        }
                        $("#cboCompany").html(s);
                    }
                }
            },
            error: function (msg) {
                alert(msg);
            }
        });
    });
});