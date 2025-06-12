$(document).ready(function () {    
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

    var MinDate = '01-Apr-2023';

    $("#btnClear").on("click", function () {
        document.location = document.location;
    });

    $("#txtFromDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        minDate: MinDate,
        maxDate: ServerDate,
        showMonthAfterYear: true,
        onClose: function () {                         
            $(this).valid();             
        }
    }).on("change", function () {
        $("#txtToDate").datepicker("option", "minDate", $(this).val());

        var d1 = new Date(ServerDate);
        var d2 = new Date($(this).val());
        d2.setDate(d2.getDate() + 30);
        var d3 = new Date((d1 < d2) ? d1 : d2);
        
        $("#txtToDate").datepicker("option", "maxDate", d3);
    });

    $("#txtToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        minDate: MinDate,
        maxDate: ServerDate,
        showMonthAfterYear: true,
        onClose: function () {
            $(this).valid();
        }
    }).on("change", function () {
        $("#txtFromDate").datepicker("option", "maxDate", $(this).val());
    });;
   
    $("#frmMain").validate({
        rules:
        {            
            txtFromDate: {
                required: true
            },
            txtToDate: {
                required: true
            }
        },
        messages:
        {
            txtFromDate: {
                required: MandatoryFieldMsg
            },
            txtToDate: {
                required: MandatoryFieldMsg
            }
        },
        submitHandler: function (form) {
            var data = new FormData(form);
            $.ajax({
                type: "POST",
                url: "../Reports/PaymentReport",
                enctype: 'multipart/form-data',
                data: data,
                processData: false,
                contentType: false,
                cache: false,
                success: function (msg) {
                    var Result = JSON.parse(msg);
                    if (Result._STATUS_ == "SUCCESS") {
                        if (Result._RESPONSE_FILE_) {
                            var link = document.createElement('a');
                            link.href = Result._RESPONSE_FILE_;
                            link.setAttribute("download", "PaymentReport.xlsx");
                            link.click();
                        }
                        alert(Result._MESSAGE_);
                    }
                    else {
                        if (Result._RESPONSE_FILE_) {
                            var link = document.createElement('a');
                            link.href = Result._RESPONSE_FILE_;
                            link.setAttribute("download", "PaymentReport.xlsx");
                            link.click();
                        }
                        alert(Result._MESSAGE_);
                    }
                }
            })
        }
    })
});