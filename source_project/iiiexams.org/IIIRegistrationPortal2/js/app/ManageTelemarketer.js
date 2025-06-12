$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });
    

    function LoadTelemarketerData()
    {           
        var data = new FormData();
        data.append('tm_id', -1);
        data.append("is_active", 0);

        $.ajax({
            type: "POST",
            url: "../Telemarketer/LoadTelemarketerData",
            data: data,
            contentType: false, // Not to set any content header
            processData: false,
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
                        s = "<table class='table table-striped table-bordered table-hover'>"
                        s += "<thead><tr><td>TRAI Reg. No.</td><td>Full Name</td><td>Is Active ?</td><td>&nbsp</td></tr></thead>";
                        for (i = 0; i < data.length; i++) {
                            s += "<tr>";
                            s += "<td>" + data[i].tm_trai_reg_no + "</td><td>" + data[i].tm_name + "</td><td>" + data[i].is_active_desc + "</td><td><a href='javascript:deleteData(" + data[i].tm_id + ")'>Delete</a> | <a href='javascript:edit(" + data[i].tm_id + ")'> Edit</a></td>";
                            s += "</tr>";
                        }
                        s += "</table>";
                        $("#divData").html(s);
                        $("#divData").show();
                        $("#divDownload").show();

                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        })
    }

    LoadTelemarketerData();

    //loading single telemarketer data for edit form
    window.edit = function (Id) {
        var data = new FormData();
        data.append('tm_id', Id);
        data.append("is_active", 0);
        
        $.ajax({
            type: "POST",
            url: "../Telemarketer/LoadTelemarketerData",
            data: data,
            contentType: false, // Not to set any content header
            processData: false,
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
                        Alert("No data found");
                        $("#divData").show();
                        $("#divDownload").show();
                        $("#frmMain").hide();
                    }
                    else {

                        $("#txt_tmid").val(data[0].tm_id);
                        $("#txtName").val(data[0].tm_name);
                        $("#txtTraiRegNo").val(data[0].tm_trai_reg_no);
                        $("#txtAddress").val(data[0].tm_address);
                        $("#txtCPName").val(data[0].tm_contact_person_name);
                        $("#txtCPEmailId").val(data[0].tm_cp_email_id);
                        $("#txtCPContactNo").val(data[0].tm_cp_contact_no);   
                        $("#txtDPName").val(data[0].tm_designated_person_name);
                        $("#txtDPEmailId").val(data[0].tm_dp_email_id);
                        $("#txtDPContactNo").val(data[0].tm_dp_contact_no);                       
                        $("#cboIsActive").val(data[0].is_active);                       
                        $("#frmMain").show();
                        $("#divData").hide();
                        $("#divDownload").hide();
                        //$("#btn_tm_rp").hide();
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        })
    }

    $("#btnDownload").on("click", function () {
        $.ajax({
            type: "POST",
            url: "../Telemarketer/DownloadData",
            data: null,
            contentType: false, // Not to set any content header
            processData: false,
            success: function (msg) {
                var s = '';
                var Result = JSON.parse(msg);
                if (Result._STATUS_ == "SUCCESS") {
                    if (Result._RESPONSE_FILE_) {
                        var link = document.createElement('a');
                        link.href = Result._RESPONSE_FILE_;
                        link.setAttribute("download", "Telemarketer.xlsx");
                        link.click();
                    }
                    alert(Result._MESSAGE_);
                }
                else {
                    if (Result._RESPONSE_FILE_) {
                        var link = document.createElement('a');
                        link.href = Result._RESPONSE_FILE_;
                        link.setAttribute("download", "Telemarketer.xlsx");
                        link.click();
                    }
                    alert(Result._MESSAGE_);
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        });



    });
    
    window.deleteData = function (Id) {
        if (!confirm("Are you sure you want to delete this record?")) {
            return;
        }
        $("#data").html('');
        var JsonObject = {
            TmId: Id
        };
        $.ajax({
            type: "POST",
            url: "../telemarketer/DeleteTelemarketer",
            data: JSON.stringify(JsonObject),
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
                    
                    if (result._STATUS_ == "SUCCESS") {                        
                        alert(result._MESSAGE_);
                        LoadTelemarketerData();                        
                        $("#data").show();

                    }
                    else {
                        alert(Result._MESSAGE_);
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        })
    }


    $('#btnCancel').click(function () {        
        $('#frmMain').trigger("reset");
        $("#divData").show();
        $("#divDownload").show();
        $('#frmMain').hide();

    })

    //saving telemarketer data
    $("#frmMain").validate({
        rules: {
            txtName: {
                required: true,
                check_exp: regexAlphaNumericWithSpace,
            },
            txtTraiRegNo: {
                required: true
            },
            cboIsActive: {
                required: true
            },
            txtAddress:
            {
                required: true,
                check_exp: regexLowAscii
            },
            txtCPName: {
                required: true,
                check_exp: regexAlphaOnlyWithSpace
            },
            txtCPEmailId: {
                required: true,
                email: true
            },
            txtCPContactNo: {
                required : true,
                digits: true,
                minlength: 10,
                maxlength: 10
            },
            txtDPName: {
                required: true,
                check_exp: regexAlphaOnlyWithSpace
            },
            txtDPEmailId: {
                required: true,
                email: true
            },
            txtDPContactNo: {
                required: true,
                digits: true,
                minlength: 10,
                maxlength: 10
            }
        },
        messages: {
            txtName: {
                required: MandatoryFieldMsg,
                check_exp: "Only alphabets and number with space, without leading / trailing space are allowed",
            },
            txtTraiRegNo: {
                required: MandatoryFieldMsg,
            },
            cboIsActive: {
                required: MandatoryFieldMsg
            },
            txtAddress: {
                required: MandatoryFieldMsg,
                check_exp: "Invalid character found"
            },
            txtCPName: {
                check_exp: "Only alphabets and space, without leading / trailing space are allowed",
            },
            txtCPEmailId: {
                required: MandatoryFieldMsg,
                email: ValidEmailIdMsg
            },
            txtCPContactNo: {
                required: MandatoryFieldMsg,
                digits: ValidMobileMsg,
                minlength: ValidMobileMsg
            },
            txtDPName: {
                check_exp: "Only alphabets and space, without leading / trailing space are allowed",
            },
            txtDPEmailId: {
                required: MandatoryFieldMsg,
                email: ValidEmailIdMsg
            },
            txtDPContactNo: {
                required: MandatoryFieldMsg,
                digits: ValidMobileMsg,
                minlength: ValidMobileMsg
            }
        },
        invalidHandler: function (event, validator) {
            alert("Unable to save data. Some of the fields have errors (The same are highlighted).");
        },
        submitHandler: function (form) {
            $("#data").html('');
            data = new FormData(form);
            $.ajax({
                type: "POST",
                enctype: 'multipart/form-data',
                url: "../telemarketer/EditTelemarketer",
                data: data,
                processData: false,
                contentType: false,
                cache: false,
                success: function (msg) {
                    //debugger;
                    var s = '';
                    var Result = JSON.parse(msg);
                    if (Result._STATUS_ == "SUCCESS") {
                        form.reset();
                        alert(Result._MESSAGE_);
                        LoadTelemarketerData();
                        $("#frmMain").hide();
                        $("#divData").show();
                        $("#divDownload").show();
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