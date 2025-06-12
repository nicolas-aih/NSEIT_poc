$(document).ready(function () {
    $("#frmUploadMain").validate(
        {
            rules: {
                txtFile: {
                    required: true,
                    extension: "xls|xlsx"
                   
                },
                chkDeclarationU: {
                    required: true
                }
            },
            messages: {
                txtFile: {
                    required: MandatoryFieldMsg,
                    extension: "Please upload excel file (*.xls / *.xlsx)",
                    filesize: "File size must be less than 5 megabytes"
                }
            },
            errorPlacement: function (error, element) {
                if (element.attr("name") === "chkDeclarationU") {
                    error.insertAfter($("#lblDeclarationU"));
                } else {
                    error.insertAfter(element);
                }
            },
            submitHandler: function (form) {
                var data = new FormData(form);
                $.ajax({
                    type: "POST",
                    enctype: 'multipart/form-data',
                    url: "../Candidates/UploadUrnExamCenterUpdate",
                    data: data,
                    processData: false,
                    contentType: false,
                    cache: false,
                    success: function (msg) {
                        var s = '';
                        var Result = JSON.parse(msg);
                        if (Result._STATUS_ == "SUCCESS") {
                            $("#ResponseFile").html('');
                            s = "<label><a href='" + Result._RESPONSE_FILE_ + "'>Download Response File in Excel Format</a></label> <br>";                           
                            $("#ResponseFile").html(s);

                            alert(Result._MESSAGE_);
                        }
                        else {
                            if (Result._RESPONSE_FILE_) {
                                $("#ResponseFile").html('');
                                s = "<label><a href='" + Result._RESPONSE_FILE_ + "'>Download Response File in Excel Format</a></label> <br>";                                
                                $("#ResponseFile").html(s);
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
});