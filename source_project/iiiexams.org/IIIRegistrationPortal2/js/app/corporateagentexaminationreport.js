$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });
    //debugger;
    var currentYear = new Date().getFullYear();
    var currentMonth = new Date().getMonth();
    for (var i = currentYear; i > currentYear - 10; i--) {
        $("#dropdownYear").append('<option value="' + i.toString() + '">' + i.toString() + '</option>');
    }
    $("#dropdownYear").val(currentYear);

    var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
    for (var i = 0; i <= 11; i++) {
        if (i == currentMonth) {
            $("#dropdownMonth").append('<option value="' + monthNames[i] + ' selected">' + monthNames[i] + '</option>');
            break;
        }
        else {
            $("#dropdownMonth").append('<option value="' + monthNames[i] + '">' + monthNames[i] + '</option>');
        }
    }

    $("#dropdownYear").change(function (event) {
        var SelectedYear = parseInt($(this).val());
        $("#dropdownMonth").empty();
        for (var i = 0; i <= 11; i++) {
            if (SelectedYear == currentYear && i == currentMonth) {
                $("#dropdownMonth").append('<option value="' + monthNames[i] + ' selected">' + monthNames[i] + '</option>');
                break;
            }
            else {
                $("#dropdownMonth").append('<option value="' + monthNames[i] + '">' + monthNames[i] + '</option>');
            }
        }
        $("#ResponseFile").html('');
    });

    $("#dropdownMonth").change(function () {
        $("#ResponseFile").html('');
    });


    $("#frmMain").validate({
        rules:
        {
            dropdownYear: {
                required: true

            },
            dropdownMonth: {
                required: true
            }
        },
        submitHandler: function (form) {
            debugger;
            var data = new FormData(form);
            $.ajax({
                type: "POST",
                url: "../Reports/CorporateAgentExaminationReport",
                data: data,
                contentType: false, // Not to set any content header
                processData: false,
                success: function (msg) {
                    var s = '';
                    var Result = JSON.parse(msg);
                    if (Result._STATUS_ == "SUCCESS") {
                        if (Result._RESPONSE_FILE_) {
                            $("#ResponseFile").html('');
                            $("#ResponseFile").html("<label><a href='" + Result._RESPONSE_FILE_ + "'>Download Response File</a></label>");
                        }
                        alert(Result._MESSAGE_);
                    }
                    else {
                        if (Result._RESPONSE_FILE_) {
                            $("#ResponseFile").html('');
                            $("#ResponseFile").html("<label><a href='" + Result._RESPONSE_FILE_ + "'>Download Response File</a></label>");
                        }
                        alert(Result._MESSAGE_);
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