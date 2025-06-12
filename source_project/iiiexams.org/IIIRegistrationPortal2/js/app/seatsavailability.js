$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

    $("#data").hide();
    $("#GridFilter").show();
    $("#frmMain").hide();

    var DistrictDetails = ""; 

    $("#txtFromDate").datepicker({
        showMonthAfterYear: true,
        dateFormat: 'dd-M-yy',
        changeMonth: true,
        changeYear: true,
        yearRange: ServerYear + ":" + (ServerYear + 1),
        minDate: ServerDate,
        maxDate: ServerDatePlusYear,
        onClose: function () {
            /* Validate a specific element: */
            //$("form").validate().element("#txtDOB");
            $(this).valid();
            //$("form #txtDOB").trigger("blur"); 
        }
    });

    //$("#cboStates").change(function () {
    //    var _StateId = $("#cboStates option:selected").val();
    //    if (_StateId === '' || _StateId === undefined || _StateId === null) {
    //        return;
    //    }
    //    var JsonObject = {
    //        StateId: _StateId,
    //        CenterId: -1
    //    };
    //    $.ajax({
    //        type: "POST",
    //        url: "../Services/GetCentersForStateEx",
    //        data: JSON.stringify(JsonObject),
    //        contentType: "application/json; charset=utf-8",
    //        dataType: "json",
    //        success: function (msg) {
    //            //debugger;
    //            var s = '';
    //            var result = JSON.parse(msg);
    //            if (result._STATUS_ === 'FAIL') {
    //                alert(result._MESSAGE_);
    //            }
    //            else {
    //                var data = JSON.parse(result._DATA_);
    //                if (data.length == undefined || data.length == 0) {
    //                    alert(NO_DATA_FOUND);
    //                }
    //                else {
    //                    s = "<option value=''>-- Select --</option>";
    //                    for (i = 0; i < data.length; i++) {
    //                        s += "<option value='" + data[i].varExamCenterCode + "'>" + data[i].varExamCenterName + "</option>";
    //                    }
    //                    $("#cboCenter").html(s);
    //                }
    //            }
    //        },
    //        error: function (msg) {
    //            HandleAjaxError(msg);
    //        }
    //    });
    //});


    $('#butCancel').click(function () {
        window.location = window.location;
    });


    var validator = $("#frmSearch").validate({
        ignore: [],
        rules: {
            cboStates: {
                required: true
            },
            //cboCenter: {
            //    required: true
            //},
            txtFromData: {
                required: true
            }
        },
        messages: {
            cboStates: {
                required: MandatoryFieldMsg
            },
            //cboCenter: {
            //    required: MandatoryFieldMsg
            //},
            txtFromDate: {
                required: MandatoryFieldMsg
            }
        },
        submitHandler: function (form) {
            var data = new FormData(form);
            data.append("cboCenter", -1);
            $.ajax({
                type: "POST",
                enctype: 'multipart/form-data',
                url: "../Services/GetAvailableSeats",
                data: data,
                processData: false,
                contentType: false,
                cache: false,
                success: function (msg) {
                    var s = '';
                    var result = JSON.parse(msg);
                    if (result._STATUS_ === 'FAIL') {
                        alert(result._MESSAGE_);
                    }
                    else {
                        var data = result._DATA_;
                        $("#data").html(data);
                        $("#data").show();
                        //if (data.length == undefined || data.length == 0) {
                        //    alert(NO_DATA_FOUND);
                        //}
                        //else {

                        //}

                        var dialogTitle = "Seats Availability Summary For - " + $("#cboStates option:selected").text() + " from " + $("#txtFromDate").val() + " onwards";

                        $("#divSeatSummary").dialog({
                            title: dialogTitle,
                            height: 500,
                            width: 1200,
                            modal: true,
                            resizable: false,
                            
                        });
                        $("#divSeatSummary").scrollTop(0);
                    }
                },
                error: function (msg) {
                    HandleAjaxError(msg);
                }
            });
        }
    });
});

window.d = function (i, j) {
    var data = new FormData();
    data.append("cboStates", -1);
    data.append("cboCenter", i);
    data.append("txtFromDate", j);

    $.ajax({
        type: "POST",
        enctype: 'multipart/form-data',
        url: "../Services/GetAvailableSeats",
        data: data,
        processData: false,
        contentType: false,
        cache: false,
        success: function (msg) {
            var s = '';
            var result = JSON.parse(msg);
            if (result._STATUS_ === 'FAIL') {
                alert(result._MESSAGE_);
            }
            else {
                var data = result._DATA_;
                $("#data2").html(data);
                $("#data2").show();
                //if (data.length == undefined || data.length == 0) {
                //    alert(NO_DATA_FOUND);
                //}
                //else {

                //}

                $("#divSeatDetails").attr("title", "Seats Availability Details");
                $("#divSeatDetails").dialog({
                    height: 400,
                    width: 600,
                    modal: true,
                    resizable: false
                });
            }
        },
        error: function (msg) {
            HandleAjaxError(msg);
        }
    });
};