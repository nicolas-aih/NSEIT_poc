$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });


    $("#txtFromDate").datepicker({
        showMonthAfterYear: true,
        dateFormat: 'dd-M-yy',
        changeMonth: true,
        changeYear: true,
        yearRange: "2000:" + (ServerYear + 1),
        onClose: function () {
            $(this).valid();
        }
    });

    $("#txtTillDate").datepicker({
        showMonthAfterYear: true,
        dateFormat: 'dd-M-yy',
        changeMonth: true,
        changeYear: true,
        yearRange: "2000:" + (ServerYear + 1),
        onClose: function () {
            $(this).valid();
        }
    });

    pageload();
    function pageload()
    {
        $("#dtFrom").hide();
        $("#dtTill").hide();
        $("#dtFile").hide();
        $("#lblTemplate").hide();
    }

    $("#txtFile").change(function () {
        $("#ResponseFile").html('');
    })

    $("#frmUploadMain").validate(
        {
            rules: {
                txtFile: {
                    required: function () {
                        SelectedValue = $("#cboAction option:selected").val();
                        if (SelectedValue == "2" || SelectedValue == "3" || SelectedValue == "4")
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    ,
                    extension: "csv",
                    filesize: 5242880
                },
                txtFromDate: {
                    required: function () {
                        SelectedValue = $("#cboAction option:selected").val();
                        if (SelectedValue == "1" ){
                            return true;
                        }
                        else {
                            return false;
                        }
                    },
                },
                txtTillDate: {
                    required: function () {
                        SelectedValue = $("#cboAction option:selected").val();
                        if (SelectedValue == "1") {
                            return true;
                        }
                        else {
                            return false;
                        }
                    },
                }
            },
            messages: {
                txtFile: {
                    required: MandatoryFieldMsg,
                    extension: "Please upload tab seperated file as per the template",
                    filesize: "File size must be less than 5 megabytes"
                },
                txtFromDate: {
                    required: MandatoryFieldMsg,
                },
                txtTillDate: {
                    required: MandatoryFieldMsg,
                }
            },
            submitHandler: function (form) {
                debugger;
                var data = new FormData(form);
                $.ajax({
                    type: "POST",
                    enctype: 'multipart/form-data',
                    url: "../Candidates/UploadExamResults",
                    data: data,
                    processData: false,
                    contentType: false,
                    cache: false,
                    success: function (msg) {
                        debugger;
                        var s = '';
                        var Result = JSON.parse(msg);
                        if (Result._STATUS_ == "SUCCESS") {
                            $("#ResponseFile").html('');
                            $("#ResponseFile").html("<label><a href='" + Result._RESPONSE_FILE_ + "'>Download Response File</a></label>");
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
        }
    );

    $("#cboAction").change(function () {
        $("#ResponseFile").html('');
        SelectedValue = $("#cboAction option:selected").val();
        if (SelectedValue == "")
        {
            $("#dtFrom").hide();
            $("#dtTill").hide();
            $("#dtFile").hide();
            $("#lblTemplate").hide();
        }
        if (SelectedValue == "1")
        {
            $("#dtFrom").show();
            $("#dtTill").show();
            $("#dtFile").hide();
            $("#lblTemplate").hide();
        }
        if (SelectedValue == "2" || SelectedValue == "3" || SelectedValue == "4")
        {
            $("#dtFrom").hide();
            $("#dtTill").hide();
            $("#dtFile").show();
            $("#lblTemplate").show();
            switch (SelectedValue)
            {
                case "2":
                    $("#lblTemplate").html("<label><a href='../UploadTemplates/AllocateSlot.csv'>Download Template</label>");
                    break;
                case "3":
                    $("#lblTemplate").html("<label><a href='../UploadTemplates/UploadResults.csv'>Download Template</label>");
                    break;
                case "4":
                    $("#lblTemplate").html("<label><a href='../UploadTemplates/AIMSResponse.csv'>Download Template</label>");
                    break;
            }
        }
        
    })

});