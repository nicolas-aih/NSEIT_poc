$(document).ready(function () {
    $(document)
        .ajaxStart(function () {
            $(".modal").show();
        })
        .ajaxStop(function () {
            $(".modal").hide();
        });

    var DistrictDetails = ""; 
    var ImageMust = false;
    loadInsurer();

    function loadInsurer()
    {
        $.ajax({
            type: "POST",
            url: "../Services/GetInsurer",
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
                        $("#cboInsurer").html('');
                        s += "<option value=''>-- Select --</option>";
                        for (i = 0; i < data.length; i++) {
                            val = data[i].intTblMstInsurerUserID + "|" + data[i].InsuranceType;
                            text = data[i].varInsurerID + ' - ' + data[i].varName;
                            if (data.length == 1) {
                                s += "<option value='" + val + "' selected>" + text + "</option>";
                            }
                            else {
                                s += "<option value='" + val + "'>" + text + "</option>";
                            }
                        }
                        $("#cboInsurer").html(s);
                        if (data.length == 1) {
                            $('#cboInsurer').trigger("change");
                        }
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        })
    }

    $("#frmMain").hide();
    //$("#data").html('');
    //$("#data").hide('');

    $("#cboStates").change(function (event, DistrictId, Pincode) {
        debugger;
        DistrictDetails = "";
        $("#cboDistricts").html('');
        $("#txtPincode").val('');
        $("#pincodeRange").html("");
        var _StateId = $("#cboStates option:selected").val();
        if (_StateId == '' || _StateId == undefined || _StateId == null) {
            return;
        }
        var JsonObject = {
            StateId: _StateId,
        };
        $.ajax({
            type: "POST",
            url: "../Home/GetDistrictsForStates",
            data: JSON.stringify(JsonObject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                debugger;
                var s = '';
                var result = JSON.parse(msg);
                if (result._STATUS_ == 'FAIL') {
                    alert(result._MESSAGE_);
                }
                else {
                    var data = JSON.parse(result._DATA_);
                    DistrictDetails = data;
                    if (data.length == undefined || data.length == 0) {
                        alert("No districts were found for selected state");
                    }
                    else {
                        s += "<option value=''>-- Select --</option>"
                        for (i = 0; i < data.length; i++) {
                            s += "<option value=" + data[i].district_id + ">" + data[i].district_name + "</option>"
                        }
                        $("#cboDistricts").html(s);
                        if (DistrictId != undefined) {
                            $("#cboDistricts").val(DistrictId);
                            $("#cboDistricts").trigger("change", Pincode);
                        }
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        });
    })
    $("#cboDistricts").change(function (event, Pincode) {
        $("#txtPincode").val('');
        $("#pincodeRange").html("");
        var _DistrictId = $("#cboDistricts option:selected").val();
        if (_DistrictId == "") {

        }
        else {
            for (i = 0; i < DistrictDetails.length; i++) {
                if (DistrictDetails[i].district_id == _DistrictId) {
                    var s = "";
                    if (DistrictDetails[i].from_pincode == 0 && DistrictDetails[i].to_pincode == 0) {
                        s = "Pincode range not available.";
                    }
                    else {
                        s = "Pincode Range : " + DistrictDetails[i].from_pincode + " - " + DistrictDetails[i].to_pincode;
                    }
                    $("#pincodeRange").html(s);
                    break;
                }
            }
            if (Pincode != undefined) {
                $("#txtPincode").val(Pincode);
            }
        }
    })

    $('#cboInsurer').change(function (event, intTblMstAgntCounselorUserID) {
        $("#frmMain").hide();
        $("#data").html('');
        $("#data").hide();
        //debugger;
        var _InsurerUserID = $("#cboInsurer option:selected").val();
        if (_InsurerUserID == '' || _InsurerUserID == null || _InsurerUserID == undefined) {
            alert('Please Select Insurer');
            return;
        }
        else {
            _InsurerUserID = _InsurerUserID.split('|')[0];
        }
        var JsonObject = {
            InsurerId: _InsurerUserID,
            DPId: -1,
            //fields: { "intTblMstDPUserID", "varDPID", "varName"}
        };
        $.ajax({
            type: "POST",
            url: "../Services/GetDPforInsurer",
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
                    var data = JSON.parse(result._DATA_);
                    if (data.length == undefined || data.length == 0) {
                        alert(NO_DATA_FOUND);
                    }
                    else {
                        s = "<table class='table table-striped table-bordered table-hover'>"
                        s += "<thead><tr><td>&nbsp</td><td>DP Id</td><td>DP Name</td></tr></thead>";
                        for (i = 0; i < data.length; i++) {
                            s += "<tr>";
                            s += "<td><a href='javascript:edit(" + data[i].intTblMstDPUserID + ")'>Edit</a></td><td>" + data[i].varDPID + "</td><td>" + data[i].varName + "</td>";
                            s += "</tr>";
                        }
                        s += "</table>";
                        $("#data").html(s);
                        $("#data").show();
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        })
    });
    $("#butNewDP").click(function () {
        debugger;
        ImageMust = true;
        var _InsurerUserID = $("#cboInsurer option:selected").val();
        if (_InsurerUserID == '' || _InsurerUserID == null || _InsurerUserID == undefined) {
            alert('Please Select Insurer');
            return;
        }
        _InsurerUserID = _InsurerUserID.split('|');
        var _InsuranceType = _InsurerUserID[1];
        _InsurerUserID = _InsurerUserID[0];
        var _InsurerName = $("#cboInsurer option:selected").text();
        //Make ajax call to get the new DPID... if -1 then dont proceed else proceed
        var JsonObject = {
            InsurerUserId: _InsurerUserID,
        };
        $.ajax({
            type: "POST",
            url: "../Users/GenerateNewDPId",
            data: JSON.stringify(JsonObject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                debugger;
                var s = '';
                var result = JSON.parse(msg);
                if (result._STATUS_ == 'FAIL') {
                    alert(result._MESSAGE_);
                }
                else {
                    DPId = result.DPUserID;

                    $("#data").hide();
                    $("#GridFilter").hide();
                    $("#frmMain").show();
                    $("#frmMain").trigger("reset");

                    $("#hdnDPUserId").val('0');
                    $("#txtInsurerName").val(_InsurerName);
                    $("#hdnInsurerUserId").val(_InsurerUserID);
                    $("#txtDPId").val(DPId);
                    $("#txtInsurerType").val(_InsuranceType);
                    $("#ImgSign").attr('src', "../Images/Signature.jpg");
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        })
    });

    window.edit = function (DPId) {
        debugger;
        var _InsurerUserID = $("#cboInsurer option:selected").val();
        _InsurerUserID = _InsurerUserID.split('|')[0];

        var _InsurerName = $("#cboInsurer option:selected").text();

        if (_InsurerUserID == '' || _InsurerUserID == null || _InsurerUserID == undefined) {
            alert('Please Select Insurer');
            return;
        }
        var JsonObject = {
            InsurerId: _InsurerUserID,
            DPId: DPId,
            //fields: { "intTblMstDPUserID", "varDPID", "varName"}
        };
        $.ajax({
            type: "POST",
            url: "../Services/GetDPforInsurerEx",
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
                    var data = JSON.parse(result._DATA_);
                    if (data.length == undefined || data.length == 0) {
                        Alert("No data found");
                        $("#data").show();
                        $("#frmMain").hide();
                    }
                    else {
                        $("#hdnDPUserId").val(data[0].intUserID);
                        $("#txtInsurerName").val(_InsurerName);
                        $("#hdnInsurerUserId").val(_InsurerUserID);
                        $("#txtDPId").val(data[0].varDPID);
                        $("#txtInsurerType").val(data[0].InsuranceType);
                        $("#txtName").val(data[0].varName);
                        $("#txtAddress").val(data[0].varHouseNo);
                        $("#txtStreet").val(data[0].varStreet);
                        $("#txtTown").val(data[0].varTown);
                        //$("#txtPincode").val(data[0].intPINCode);
                        $("#txtTelephoneO").val(data[0].varTelOffice);
                        $("#txtTelephoneR").val(data[0].varTelResidence);
                        $("#txtMobileNo").val(data[0].varMobileNo);
                        $("#txtFax").val(data[0].varFax);
                        $("#txtEmailID").val(data[0].varEmailID);
                        //$("#txtDate").val(data[0].);
                        if (data[0].imgDPSignatureB64 != null) {
                            $("#ImgSign").attr('src', 'data:image/jpg;base64,' + data[0].imgDPSignatureB64);
                            ImageMust = false;
                        }
                        else
                        {
                            ImageMust = true;
                        }
                        $("#cboStates").val(data[0].tntStateID);
                        $("#cboStates").trigger("change", [data[0].sntDistrictID, data[0].intPINCode]);
                        var isactive = data[0].bitIsActive;
                        if (isactive == false) {
                            $("#chkActive").attr('checked', false)
                        }
                        else {
                            $("#chkActive").attr('checked', true)
                        }
                        //$("#chkActive").val(data[0].bitIsActive);
                        $("#data").hide();
                        $("#GridFilter").hide();
                        $("#frmMain").show();
                    }
                }
            },
            error: function (msg) {
                HandleAjaxError(msg);
            }
        })
    }

    $('#butCancel').click(function () {
        window.location = window.location;
        //var myForm = $("#frmMain");
        //clearValidation(myForm);
        //myForm = $("#frmSearch");
        //clearValidation(myForm);

        //$("#frmMain").trigger("reset");
        //$("#frmSearch").trigger("reset");

        //$("#frmMain").hide();
        //$("#data").html('');
        //$("#data").hide();
        //$("#GridFilter").show();
        //$("#cboInsurer").val("");
    })
    $("#txtFileSign").change(function () {
        var file = this.files[0];
        validator.element($('#txtFileSign'));
        if ($(this).valid()) {
            readURL(this, $('#ImgSign'));
        }
    });
    $('#ImgSign').click(function () {
        $("#txtFileSign").trigger('click');
    })

    var validator = $("#frmMain").validate({
        ignore:[],
        rules: {
            //txtDPId: {
            //    required:true
            //},
            //txtInsurerType:{
            //    required: true
            //},
            txtName: {
                required: true,
                check_exp: regexAlphaOnlyWithSpace,
                minlength: 5
            },
            txtAddress: {
                check_exp: regexLowAscii
            },
            txtStreet: {
                check_exp: regexLowAscii
            },
            txtTown: {
                check_exp: regexLowAscii
            },
            txtPincode: {
                required: true,
                digits: true,
                minlength: 6
            },
            txtTelephoneO: {
                digits: true,
            },
            txtTelephoneR: {
                digits: true,
            },
            txtMobileNo: {
                required: true,
                digits: true,
                minlength: 10
            },
            txtFax: {
                digits: true,
            },
            txtEmailID: {
                required: true,
                email: true,
            },
            txtFileSign: {
                required: function () {
                    return ImageMust;
                },
                extension: "jpg|jpeg|png",
                filesize: 51200
            },
            cboStates: {
                required: true
            },
            cboDistricts: {
                required: true
            }
        },
        messages: {
            //txtDPId: {
            //    required: MandatoryFieldMsg
            //},
            //txtInsurerType: {
            //    required: MandatoryFieldMsg
            //},
            txtName: {
                required: MandatoryFieldMsg,
                check_exp: "Please enter valid name. Only alphabets and space without leading / trailing space are allowed",
                minlength: "Name should be minimum 5 characters long"
            },
            txtAddress: {
                check_exp: JunkCharMessage
            },
            txtStreet: {
                check_exp: JunkCharMessage
            },
            txtTown: {
                check_exp: JunkCharMessage
            },
            txtPincode: {
                required: MandatoryFieldMsg,
                digits: ValidPincodeMsg,
                minlength: ValidPincodeMsg
            },
            txtTelephoneO: {
                digits: ValidPhoneMsg
            },
            txtTelephoneR: {
                digits: ValidPhoneMsg,
            },
            txtMobileNo: {
                required: MandatoryFieldMsg,
                digits: ValidMobileMsg,
                minlength: ValidMobileMsg
            },
            txtFax: {
                digits: ValidFaxMsg
            },
            txtEmailID: {
                required: MandatoryFieldMsg,
                email: ValidEmailIdMsg
            },
            txtFileSign: {
                required: "Please select signature",
                extension: "Please upload image file (*.jpg / *.jpeg / *.png)",
                filesize: "File size must be less than 50 kilobytes"
            },
            cboStates: {
                required: MandatoryFieldMsg
            },
            cboDistricts: {
                required: MandatoryFieldMsg
            }
        },
        submitHandler: function (form) {
            var data = new FormData(form);
            $.ajax({
                type: "POST",
                enctype: 'multipart/form-data',
                url: "../Users/SaveDP",
                data: data,
                processData: false,
                contentType: false,
                cache: false,
                success: function (msg) {
                    debugger;
                    var s = '';
                    var Result = JSON.parse(msg);
                    if (Result._STATUS_ == "SUCCESS") {
                        alert(Result._MESSAGE_);
                        window.location = window.location;
                        //form.reset();
                        //$("#frmMain").hide();
                        //$("#GridFilter").show();
                        //$("#cboInsurer").val("");
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
    })
});