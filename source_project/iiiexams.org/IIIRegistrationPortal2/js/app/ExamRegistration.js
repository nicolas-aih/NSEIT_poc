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
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        minDate: '01-Apr-2019',
        maxDate: ServerDate
    });
    $("#txtToDate").datepicker({
        showMonthAfterYear: true,
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        minDate: '01-Apr-2019',
        maxDate: ServerDate
    });

    function clearForm()
    {
        $('#ddlBatchmode').val(1);
        $('#ddlSchedulingMode').val(1);
        $("#txtRemarks").val('');
        $("#dvBatch").hide();
        $("#tbldata").html('');
        $("#dvApplicantDetails").show();
    }

    var tbl;
    $("#dvBatch").hide();
    $("#dvTrainedApplicants").hide();

    $('#txtFromDate').val(ServerDate);
    $('#txtToDate').val(ServerDate);
    
    $('#ddlExamBody').change(function (event) {
        //debugger;
        var _ExamBodyID = $(this).val();
        var JsonObject = {
            ExamBodyID: _ExamBodyID,
        };
        $.ajax({
            type: "POST",
            url: "../Batches/GetExamCenters",
            data: JSON.stringify(JsonObject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var retval = JSON.parse(msg);
                var s = '';

                if (retval.length == undefined || retval.length == 0) {
                    alert("No Exam Centers were found for selected Exam Body");
                    s = "<option value=0>--All--</option > ";
                    $("#ddlCenter").html(s);
                }
                else {
                    s = "<option value=0>--All--</option > ";
                    for (i = 0; i < retval.length; i++) {
                        s += "<option value=" + retval[i].sntExamCenterID + ">" + retval[i].varExamCenterName + "</option>"
                    }
                    $("#ddlCenter").html(s);
                }
            }
        })
    });

    jQuery.validator.addMethod(
        "ValidateToDate",
        function (value, element) {
            var _FromDate = $("#txtFromDate").val();
            if (_FromDate == undefined || _FromDate == "" || _FromDate == null) {
                return true;
            }
            if (value == undefined || value == "" || value == null) {
                return true;
            }
            var isSuccess = false;
            var JsonObject = {
                FromDate: _FromDate,
                TillDate: value
            };
            $.ajax({
                type: "POST",
                async: false,
                url: "../Services/ValidateFromTillDate",
                data: JSON.stringify(JsonObject),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    //debugger;
                    var retval = JSON.parse(msg);
                    if (retval._STATUS_ == "FAIL") {
                        isSuccess = false;
                    }
                    else {
                        isSuccess = true;
                    }
                },
                error: function (msg) {
                    isSuccess = false;
                    HandleAjaxError(msg);
                }
            });
            return isSuccess;
        }, "The To date must be greater than or equal to From date."
    );

    $("#chkAll").click(function () {
        for (i = 0; i < 50; i++) {
            $('#chk' + i).prop('checked', $('#chkAll').is(":checked"));
        }
    });

    $("#btncancel").click(function () {
        window.location = window.location;
    })

    $("#ddlpaymentmode").change(function () {//Change to display the 
        var ddlValue = $("#ddlpaymentmode option:selected").val();
        if (ddlValue == "Credit") {
            $("#ddlBatchmode").html("<option value=''>--Select--</option><option value='1'>Bulk Batch</option>");
        }
        else {
            $("#ddlBatchmode").html("<option value=''>--Select--</option><option value='1'>Bulk Batch</option><option value='2'>Single Candidate Batch</option>");
        }
    });

    $("#blkfile").change(function () {
        //validator.element($('#blkfile'));
        $(this).valid();
    });


    var validator2 = $("#frmFilter").validate({
        rules:
        {
            txtFromDate: {
                required: true,
            },
            txtToDate:{
                required: true,
                ValidateToDate: true,
            },
            ddlExamBody: {
                required: true,                
            },
            ddlCenter: {
                required: true,
            }
        },
        messages:
        {
            txtFromDate: {
                required: MandatoryFieldMsg,
            },
            txtToDate: {
                required: MandatoryFieldMsg,
            },
            ddlExamBody: {
                required: MandatoryFieldMsg,
            },
            ddlCenter: {
                required: MandatoryFieldMsg,
            }
        },
        submitHandler: function (form)
        {
            var data = new FormData(form);
            $.ajax({
                type: "POST",
                contentType: 'multipart/form-data',
                url: "../Batches/GetTrainedApplicants",
                data: data,
                processData: false,
                contentType: false,
                cache: false,
                success: function (msg) {
                    var s = '';
                    var t = '';
                    var retval = JSON.parse(msg);
                    var Griddata = JSON.parse(retval._DATA_);
                    var paymentMode = JSON.parse(retval.PaymentMode);

                    t += "<option value=''>--Select--</option>";
                    for (i = 0; i < paymentMode.length; i++) {
                        t += "<option value='" + paymentMode[i].varPaymentMode + "'>" + paymentMode[i].varPaymentMode + "</option>"
                    }
                    $("#ddlpaymentmode").html(t);

                    if (Griddata.length == undefined || Griddata.length == 0) {
                        //tblHead;
                        //$("#tbldata").html(tblHead);
                        alert("No Data Found For The Selected Criteria");
                    }
                    else {
                        var tblHead = ''; //"<table class='table-responsive' id='tbldata'>"
                        tblHead += "<thead>";
                        tblHead += "<tr>";
                        tblHead += "<th class='text-center' style='width:40px;'></th>";
                        tblHead += "<th class='text-center' style='display:none'>Applicant Id</th>";
                        tblHead += "<th class='text-center' >Applicant Name</th>";
                        tblHead += "<th class='text-center' >URN</th>";
                        tblHead += "<th class='text-center' style='display:none'>row count</th>";
                        tblHead += "<th class='text-center' >Preferred<br>Exam Date<br>(Mandatory)</th>";
                        tblHead += "<th class='text-center' >Email</th>";
                        tblHead += "<th class='text-center' >TCC<br>Expiry<br>Date</th>";
                        tblHead += "</tr>";
                        tblHead += "</thead>";

                        tblHead += "<tbody>"
                        for (i = 0; i < Griddata.length; i++) {
                            tblHead += "<tr>"
                            tblHead += "<td ><input type='checkbox' id='chk" + i + "' name='chk" + i + "'></td>";
                            tblHead += "<td style='display:none'>" + Griddata[i].bntApplicantID + "</td>";
                            tblHead += "<td><input type='text' style='width: 300px;' class='form-control' readonly=readonly value='" + Griddata[i].varApplicantName + "'/></td>";
                            tblHead += "<td><input type='text' style='width: 150px;' class='form-control' id='txtURN" + i + "' name='txtURN" + i + "' readonly=readonly value='" + Griddata[i].chrRollNumber + "'/></td>";
                            tblHead += "<td style='display:none'>" + i + "</td>";
                            tblHead += "<td><input type='text' style = 'width: 150px;' class='form-control dtpick' id='txtOnDate" + i + "' name='txtOnDate" + i + "' /></td>";
                            tblHead += "<td><input type=text style = 'width: 150px;' class='form-control' id='txtEmail" + i + "' name='txtEmail" + i + "' class='txtEmail'/></td>";
                            tblHead += "<td><input type='text' style='width: 150px;' class='form-control' readonly=readonly value='" + Griddata[i].dtTCCExpiryDate + "'/></td>";
                            tblHead += "</tr>"
                        }
                        tblHead += "</tbody>";
                        //tblHead += "</table>"
                        $("#tbldata").html(tblHead);

                        //alert(minimumDate);
                        $('#tbldata').find($('.dtpick').prop('readonly', 'readonly'));
                        $('#tbldata').find('.dtpick').datepicker({
                            showMonthAfterYear: true,
                            changeMonth: true,
                            changeYear: true,
                            dateFormat: 'dd M yy',
                            minDate: new Date(minimumDate),
                            maxDate: '+30D',
                        });

                        var t = '';
                        var ddlValue = $("#ddlpaymentmode option:selected").val();
                        var Rolename = '@PortalSession.RoleName';

                        if (ddlValue == "Credit") {
                            if (Rolename == "Corporate Designated Person" || Rolename == "Designated Person" || Rolename == "Agent Counselor") {
                                $("#dvBatchMode").hide();
                            }
                            else {
                                $("#dvBatchMode").hide();
                            }
                        }
                        else {
                            $("#dvBatchMode").show();
                        }

                        $("#ResponseFile1").hide();
                        $("#dvApplicantDetails").hide();
                        $("#dvTrainedApplicants").show();
                        $("#dvBatch").show();
                    }
                },
                error: function (msg) {
                    HandleAjaxError(msg);
                }
            });

        }
    })

    $("#tbldata").on('select deselect', function (e, dt, type, indexes) {
        var countSelectedRows = tbl.rows({ selected: true }).count();
        var countItems = tbl.rows().count();

        if (countItems > 0) {
            if (countSelectedRows == countItems) {
                $('#chkAll').prop('checked', true);
            }
            else {
                $('#chkAll').prop('checked', false);
            }
        }

        if (e.type === 'select') {
            $('#chkAll', tbl.rows({ selected: true }).nodes()).prop('checked', true);
        }
        else {
            $('#chkAll', tbl.rows({ selected: false }).nodes()).prop('checked', false);
        }
    });

    var validator = $("#frmUploadMain").validate({
        rules:
        {
            blkfile:
            {
                required: true,
                extension: "xls"
            }
        },
        messages:
        {
            blkfile:
            {
                required: MandatoryFieldMsg,
                extension: "Please upload excel file (*.xls)"
            }
        },
        submitHandler: function (form) {
            //var filePath = $("#blkfile").get(0);
            //var files = filePath.files;
            var data = new FormData(form);
            //data.append(files[0].size, files[0], files[0].type);

            $.ajax({
                type: "POST",
                enctype: 'multipart/form-data',
                url: "../Batches/UploadRegistration",
                data: data,
                dataType: "json",
                contentType: false, // Not to set any content header
                processData: false, // Not to process data
                cache: false,

                success: function (msg) {
                    //debugger;
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
                            $("#ResponseFile").html("<label><a href='" + Result._RESPONSE_FILE_ + "'> Download Response File</a></label>");
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

    var validator3 = $("#frmMain").validate({
        rules: {
            ddlpaymentmode: {
                required:true
            },
            ddlBatchmode:{
                required: true
            },
            ddlSchedulingMode: {
                required: true
            }
        },
        messages: {
            ddlpaymentmode: {
                required: MandatoryFieldMsg
            },
            ddlBatchmode: {
                required: MandatoryFieldMsg
            },
            ddlSchedulingMode: {
                required: MandatoryFieldMsg
            }
        },
        
        submitHandler: function (form) {
            debugger;
            var regex = /^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/;

            //0 --> 49
            CanProceed = true;
            RowSelected = false;
            for (i = 0; i < 50; i++)
            {
                IsDateValid = true;
                IsEmailValid = true;
                if ($('#chk' + i).is(":checked"))
                {
                    RowSelected = true;

                    PreferredDate = $("#txtOnDate" + i).val();
                    Email = $("#txtEmail" + i).val();

                    if (PreferredDate === "" || PreferredDate == undefined) {
                        IsDateValid = false;
                    }
                    if (Email != "" && Email != undefined) {
                        var result = Email.replace(/\s/g, "").split(",");
                        for (var j = 0; j < result.length; j++) {
                            if (!regex.test(result[j])) {
                                IsEmailValid = false;
                            }
                        }
                    }

                    if (IsDateValid == false) {
                        CanProceed = false;
                        $("#txtOnDate" + i).css({ "border-color": "red" });
                    }
                    else {
                        $("#txtOnDate" + i).css({ "border-color": "" });
                        $("#txtOnDate" + i).addClass("form-control");
                    }
                    if (IsEmailValid == false) {
                        CanProceed = false;
                        $("#txtEmail" + i).css({ "border-color": "red" });
                    }
                    else {
                        $("#txtEmail" + i).css({ "border-color": "" });
                        $("#txtEmail" + i).addClass("form-control");
                    }
                }
                else
                {
                    //Dont do any thing...
                }
            }
            if (!RowSelected)
            {
                alert("No Row Selected");
                return;
            }
            else {
                if (!CanProceed)
                {
                    alert("Data is not completely valid. Please check the fields marked in red for invalid / incomplete data");
                    return;
                }
            }

            var data = new FormData(form);

            $.ajax({
                type: "POST",
                url: "../Batches/UpdateApplicantDetails",
                data: data,
                processData: false,
                contentType: false,
                cache: false,

                success: function (msg) {
                    //debugger;
                    var s = '';
                    var Result = JSON.parse(msg);
                    if (Result._STATUS_ == "SUCCESS") {
                        clearForm();
                        $("#ResponseFile1").show();
                        $("#ResponseFile1").html('');
                        $("#ResponseFile1").html("<label><a href='" + Result._RESPONSE_FILE_ + "'>Download Response File</a></label>");
                        alert(Result._MESSAGE_);
                    }
                    else {
                        if (Result._RESPONSE_FILE_) {
                            $("#ResponseFile1").show();
                            $("#ResponseFile1").html('');
                            $("#ResponseFile1").html("<label><a href='" + Result._RESPONSE_FILE_ + "'> Download Response File</a></label>");
                        }
                        alert(Result._MESSAGE_);
                    }
                    //form.reset();

                },
                error: function (msg) {
                    HandleAjaxError(msg);
                }
            });
        }
    })
});


