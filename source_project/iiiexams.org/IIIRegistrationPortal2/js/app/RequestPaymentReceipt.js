$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

   
    
    $('#txtMonth').MonthPicker({        
        MonthFormat: 'M-yy', // Short month name, Full year.
        Button: false,
        MaxMonth : 0
    });

    $("#frmMain").validate({
        rules:
        {
            txtMonth: {
                required: true                
            }            
        },
        messages:
        {
            txtMonth: {
                required: MandatoryFieldMsg                
            }
            
        },
        submitHandler: function (form) {            
            var data = new FormData(form);
            $.ajax({
                type: "POST",
                enctype: 'multipart/form-data',
                url: "../Reports/RequestPaymentReceipts",
                data: data,
                processData: false,
                contentType: false,
                cache: false,
                success: function (msg) {
                    //debugger;
                    var s = '';
                    var Result = JSON.parse(msg);
                    if (Result._STATUS_ === "SUCCESS") {
                        alert(Result._MESSAGE_);
                        $("#Results").html('');
                        $("#Results").html('<label>Request sent successfully. Once processed, the files will be available for download under Reports Dashboard section</label>');
                        $('#txtMonth').val('');
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
    });


});