$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

    //$("#txtFromDate").datepicker({
    //    showMonthAfterYear: true,
    //    changeMonth: true,
    //    changeYear: true,
    //    dateFormat: 'dd-M-yy',
    //    onClose: function () {
    //        /* Validate a specific element: */
    //        $(this).valid();
    //    }
    //});

    //$("#txtToDate").datepicker({
    //    showMonthAfterYear: true,
    //    changeMonth: true,
    //    changeYear: true,
    //    dateFormat: 'dd-M-yy',
    //    onClose: function () {
    //        /* Validate a specific element: */
    //        $(this).valid();
    //    }
    //});
    
    //var Hint = 0;
    //jQuery.validator.addMethod(
    //    "ValidateEndDate",
    //    function (value, element) {
    //        Hint = 0;
    //        var _FromDate = $("#txtFromDate").val();
    //        if (_FromDate == undefined || _FromDate == "" || _FromDate == null) {
    //            return true;
    //        }
    //        if (value == undefined || value == "" || value == null) {
    //            return true;
    //        }
    //        var isSuccess = false;
    //        var _dtFromDate = new Date(_FromDate);
    //        var _dtToDate = new Date(value);

    //        if (_dtFromDate === 'Invalid Date' || _dtToDate === 'Invalid Date')
    //        {
    //            Hint = 1;
    //            isSuccess = false;
    //        }
    //        else {
    //            _dtFromDate.setHours(0, 0, 0, 0);
    //            _dtToDate.setHours(0, 0, 0, 0);
    //            if (_dtFromDate.getTime() <= _dtToDate.getTime()) {
    //                isSuccess = true;
    //            }
    //        }

    //        return isSuccess;
    //    },
    //    function () {
    //        if (Hint == 0)
    //        {
    //            return "Display Till Date must be greater or equal to than Display Start Date.";
    //        }
    //        else if (Hint == 1) 
    //        {
    //            return "Invalid Date(s)";
    //        }
    //    }
    //);

    //jQuery.validator.addMethod(
    //    "ValidateStartDate",
    //    function (value, element) {
    //        if (value == undefined || value == "" || value == null) {
    //            return true;
    //        }
    //        var isSuccess = false;
    //        var JsonObject = {
    //            date: value,
    //        };
    //        $.ajax({
    //            type: "POST",
    //            async: false,
    //            url: "../Services/ValidateNotificationFromDate",
    //            data: JSON.stringify(JsonObject),
    //            contentType: "application/json; charset=utf-8",
    //            dataType: "json",
    //            success: function (msg) {
    //                //debugger;
    //                var retval = JSON.parse(msg);
    //                if (retval._STATUS_ == "FAIL") {
    //                    isSuccess = false;
    //                }
    //                else {
    //                    isSuccess = true;
    //                }
    //            },
    //            error: function (msg) {
    //                isSuccess = false;
    //                HandleAjaxError(msg);
    //            }
    //        });
    //        return isSuccess;
    //    }, "Display Start Date cannot be a past date"
    //);

    loadTbxSchedule();

    function loadTbxSchedule() {
        $.ajax({
            type: "POST",
            url: "../Services/GetTbxSchedule",
            //data: JSON.stringify(JsonObject),
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
                        alert(NO_DATA_FOUND);
                    }
                    else {
                        s += "";

                        for (i = 0; i < data.length; i++) {
                            //header
                            s += "<div style='border:1px solid gray'>"; //1
                            s += "<div class='row mt12' style='background-color:lightgray;margin:5px;padding:5px' >";//2
                                s += "<div class='col-sm-12'>" + data[i].varExamCenterName + "</div>"//3
                                s += "</div>"//2

                                s += "<div class='row mt12' style='margin:5px;padding:5px'>";//4
                                s += "<div class='col-sm-4'>Registration Start date :</div ><div class='col-sm-8'>" + data[i].registration_start_date + "</div>";
                                s += "</div>";

                                s += "<div class='row mt12' style='margin:5px;padding:5px'>";//4
                                s += "<div class='col-sm-4'>Registration End date :</div ><div class='col-sm-8'>" + data[i].registration_end_date + "</div>";
                                s += "</div>";

                                s += "<div class='row mt12' style='margin:5px;padding:5px'>";//4
                                s += "<div class='col-sm-4'>Exam date :</div ><div class='col-sm-8'>" + data[i].exam_date + "</div>";
                                s += "</div>";

                                s += "<div class='row mt12' style='margin:5px;padding:5px'>";//4
                                s += "<div class='col-sm-4'>Address :</div ><div class='col-sm-8'>" + data[i].address + "</div>";
                                s += "</div>";//5

                            s += "</div><br>"; //1

                           }
                    }

                        $("#data").html(s);
                        $("#data").show();
                        //$("#frmMain").hide();
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        })
    }

    //$("#butNew").click(function () {
    //    $("#frmMain").trigger("reset");
    //    $("#frmMain").show();
    //    $("#hdnNotificationId").val('0');
    //    $("#GridFilter").hide();
    //    $("#data").html();
    //    $("#data").hide();
    //});

    //$('#butCancel').click(function () {
    //    window.location = window.location;
    //})

    //$("#txtfileName").change(function () {
    //    var file = this.files[0];
    //    name = file.name;
    //    size = file.size;
    //    type = file.type;
    //    validator.element($('#txtFilePhoto'));
    //    if ($(this).valid()) {
    //        readURL(this, $('#imgFilePhoto'));
    //    }
    //});

    //var validator = $("#frmMain").validate({
    //    rules:
    //    {
    //        txtFromDate: {
    //            required: true,
    //            ValidateStartDate: true
    //        },
    //        txtToDate: {
    //            required: true,
    //            ValidateEndDate: true
    //        },
    //        txtcaption: {
    //            required: true,
    //            check_exp: regexLowAscii,
    //        },
    //        txtfileName: {
    //            extension: "xls|xlsx|xlsb|txt|jpeg|png|doc|docx|zip|pdf",
    //            filesize: 1048576
    //        }
    //    },
    //    messages:
    //    {
    //        txtFromDate: {
    //            required: MandatoryFieldMsg
    //        },
    //        txtToDate: {
    //            required: MandatoryFieldMsg
    //        },
    //        txtcaption: {
    //            required: MandatoryFieldMsg,
    //            check_exp: JunkCharMessage
    //        },
    //        txtfileName:
    //        {
    //            extension: "Please upload file with extension (*.xls / *.xlsx / *.xlsb /*.txt / *.jpeg / *.png /*.doc /*.docx /*.zip /*.jpg)",
    //            filesize: "File size must be less than 1 Megabyte"
    //        }
    //    },
    //    submitHandler: function (form) {
    //        if ($('input:checkbox').filter(':checked').length < 1) {
    //            alert('Please select atleast 1 role from Target Audience');
    //            return false;
    //        }
    //        else {
    //            var data = new FormData(form);
    //            $.ajax({
    //                type: "POST",
    //                url: "/Home/SaveNotification",
    //                data: data,
    //                contentType: false, // Not to set any content header
    //                processData: false, // Not to process data
    //                success: function (msg) {
    //                    var Result = JSON.parse(msg);
    //                    if (Result._STATUS_ == "SUCCESS") {
    //                        alert(Result._MESSAGE_);
    //                        window.location = window.location;
    //                    }
    //                    else {
    //                        alert(Result._MESSAGE_);
    //                    }
    //                },
    //                error: function (msg) {
    //                    HandleAjaxError(msg);
    //                }
    //            });
    //        }
    //    }
    //});

    //window.edit = function (NotId) {
    //    debugger;
    //    var JsonObject = {
    //        NotificationId: NotId,
    //    };
    //    $.ajax({
    //        type: "POST",
    //        url: "/Services/GetAllNotifications",
    //        data: JSON.stringify(JsonObject),
    //        contentType: "application/json; charset=utf-8",
    //        dataType: "json",
    //        success: function (msg) {
    //            //debugger;
    //            var s = '';
    //            var result = JSON.parse(msg);
    //            if (result._STATUS_ == 'FAIL') {
    //                alert(result._MESSAGE_);
    //            }
    //            else {
    //                var data = JSON.parse(result._DATA_);
    //                if (data.length == undefined || data.length == 0) {
    //                    Alert("No data found");
    //                    $("#data").show();
    //                    $("#frmMain").hide();
    //                }
    //                else {
    //                    $("#frmMain").trigger("reset");
    //                    //----- OK
    //                    $("#hdnNotificationId").val(data[0].notification_id);
    //                    $("#txtFromDate").val(data[0].start_date);
    //                    $("#txtToDate").val(data[0].end_date);
    //                    $("#txtcaption").val(data[0].notification_caption);
    //                    $("#txtnotificationtxt").val(data[0].notification_text);
    //                    if (data[0].attachment == null || data[0].attachment == undefined || data[0].attachment == "") {
    //                        $("#downloadfile").html('This notification has no files attached')
    //                    }
    //                    else {
    //                        $("#downloadfile").html("<a href='../NotificationDocs/" + data[0].attachment + "' target='_blank'>View attachment</a>")
    //                    }
    //                    if (data[0].display_ca == 'Yes')
    //                    {
    //                        $("#chkRoleCA").attr('checked', true);
    //                    }
    //                    else {
    //                        $("#chkRoleCA").attr('checked', false);
    //                    }  
    //                    if (data[0].display_wa == 'Yes') {
    //                        $("#chkRoleWA").attr('checked', true);
    //                    }
    //                    else {
    //                        $("#chkRoleWA").attr('checked', false);
    //                    }  
    //                    if (data[0].display_imf == 'Yes') {
    //                        $("#chkRoleIMF").attr('checked', true);
    //                    }
    //                    else {
    //                        $("#chkRoleIMF").attr('checked', false);
    //                    }  
    //                    if (data[0].display_br == 'Yes') {
    //                        $("#chkRoleBR").attr('checked', true);
    //                    }
    //                    else {
    //                        $("#chkRoleBR").attr('checked', false);
    //                    }  
    //                    if (data[0].display_i == 'Yes') {
    //                        $("#chkRoleInsurer").attr('checked', true);
    //                    }
    //                    else {
    //                        $("#chkRoleInsurer").attr('checked', false);
    //                    }                        
    //                    if (data[0].display_others == 'Yes') {
    //                        $("#chkRoleOther").attr('checked', true);
    //                    }
    //                    else {
    //                        $("#chkRoleOther").attr('checked', false);
    //                    }  
    //                    if (data[0].halt_display == 'Yes') {
    //                        $("#chkHaltDisplay").attr('checked', true);
    //                    }
    //                    else {
    //                        $("#chkHaltDisplay").attr('checked', false);
    //                    }  
    //                    //----- End OK
    //                    $("#data").hide();
    //                    $("#GridFilter").hide();
    //                    $("#frmMain").valid();
    //                    $("#frmMain").show();
    //                }
    //            }
    //        },
    //        error: function (msg) {
    //            HandleAjaxError(msg);
    //        }
    //    })
    //}
});