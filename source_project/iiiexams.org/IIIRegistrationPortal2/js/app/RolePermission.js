

$(document).ready(function () {

    var tbl = '';
    var AllMenuIds = new Array;
    var AlloldValue = new Array;
    var AllnewValue = new Array;


    var MenuId = [];
    var oldValue = [];
    var newValue = [];


    FillRoles();

    $("#ddlRoleType").select2();


    var SearchId = 27;//$("#ddlRoleType option:selected").val();

    var isrole = $("#ddltype option:selected").val();



    $("#chkAll").click(function ()
    {
        if ($(this).is(":checked"))
        {
            //tbl.rows().select();

            $("#tblMenuData").find('tr').each(function (i, el)
            {
                //debugger;
                var $tds = $(this).find('td');
                var menuId = $tds.eq(0).text();
                $(this).find($("#chktdAll" + menuId).prop("checked", true));

            });

        }
        else
        {
            $("#tblMenuData").find('tr').each(function (i, el) {
                //debugger;
                var $tds = $(this).find('td');
                var menuId = $tds.eq(0).text();
                $(this).find($("#chktdAll" + menuId).prop("checked", false));

            });
            //tbl.rows().deselect();
        }

    });



    var JsonObject =
    {

        intSearchId: SearchId,
        isRole: isrole,
    }
    $.ajax({
        type: "POST",
        url: "../Services/GetMenuData",
        data: JSON.stringify(JsonObject),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $(".modal").show();
        },
        complete: function () {
            $(".modal").hide();
        },
        success: function (msg) {

            var retval = JSON.parse(msg);
            var Griddata = JSON.parse(retval._DATA_);
            var s = '';


            if (Griddata.length == undefined || Griddata.length == 0) {


            }
            else {

                for (i = 1; i < Griddata.length; i++) {



                    s += "<tr>";
                    s += "</td><td width='30px' style=display:none>" + Griddata[i].sntMenuID +
                        "</td></td > <td width='300px'>" + Griddata[i].varMenu + "</td>"
                    s += "</td><td width='30px' style=display:none>" + Griddata[i].MenuAccess
                    s += "<td width='10px'>" + "<input type=checkbox class=chktdAll id=chktdAll" + Griddata[i].sntMenuID + " style=margin-left:8px;height:15px;width:15px;vertical-align:sub /></td>"
                    //"</td></td><td width='20px' style='display:none'>" + Griddata[i].ADD + "</td>" +
                    //"<td width='20px' style='text-align:center'>" + "<input type=checkbox name=chkSelectDLMember class=chkAdd id=chkAdd" + [i] + " style=height:15px;width:15px;vertical-align:sub /></td></tr>"
                    //"</td></td><td width='20px' style='display:none'>" + Griddata[i].EDIT + "</td>" +
                    //"</td > <td width='20px'  style='text-align:center'>" + "<input type=checkbox name=chkSelectDLMember class=chkEdit id=chkEdit" + [i] + " style=height:15px;width:15px;vertical-align:sub /></td>" +
                    //"</td></td><td width='20px' style='display:none'>" + Griddata[i].DELETE + "</td>" +
                    //"<td width='20px'  style='text-align:center'>" + "<input type=checkbox name=chkSelectDLMember class=chkDelete id=chkDelete" + [i] + " style=height:15px;width:15px;vertical-align:sub /></td>" +
                    //"</td></td><td width='20px' style='display:none'>" + Griddata[i].EXPORT + "</td>" +
                    //"<td width='20px'  style='text-align:center'>" + "<input type=checkbox id=chkExport" + i + " style=height:15px;width:15px;vertical-align:sub /></td></tr>";



                }
                s += "</table>";
                $("#tbdata").html(s);

                AssignRights();


                tbl = $('#tblMenuData').DataTable({
                    bSortable: true,
                    paging: false,


                    columnDefs: [{
                        orderable: false,

                        //className: 'select-checkbox',
                        targets: 0,
                    }],

                    select: {
                        style: 'multi',
                        selector: 'td:first-child'
                    },
                    order: [[0, 'asc']]
                });

            }


        },
        error: function (msg) {
            alert(msg.d);
        }


    });

    $("#ddltype").change(function () {
        var type = $("#ddltype option:selected").val();

        if (type == "1") {
            FillRoles();

            var searchVal = 27;
            var isrole = $("#ddltype option:selected").val();


            var JsonObject =
            {

                intSearchId: searchVal,
                isRole: isrole,
            }
            $.ajax({
                type: "POST",
                url: "../Services/GetMenuData",
                data: JSON.stringify(JsonObject),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () {
                    $(".modal").show();
                },
                complete: function () {
                    $(".modal").hide();
                },
                success: function (msg) {

                    var retval = JSON.parse(msg);
                    var Griddata = JSON.parse(retval._DATA_);
                    var s = '';


                    if (Griddata.length == undefined || Griddata.length == 0) {


                    }
                    else {


                        for (i = 1; i < Griddata.length; i++) {



                            s += "<tr>";
                            s += "</td><td width='30px'style=display:none>" + Griddata[i].sntMenuID +
                                "</td></td><td width='300px'>" + Griddata[i].varMenu + "</td>"
                            s += "</td><td width='30px'style=display:none>" + Griddata[i].MenuAccess
                            s += "<td width='40px'>" + "<input type=checkbox class=chktdAll id=chktdAll" + Griddata[i].sntMenuID + " style=margin-left:8px;height:15px;width:15px;vertical-align:sub /></td>"
                            //"</td></td><td width='20px' style='display:none'>" + Griddata[i].ADD + "</td>" +
                            //"<td width='20px' style='text-align:center'>" + "<input type=checkbox name=chkSelectDLMember class=chkAdd id=chkAdd" + [i] + " style=height:15px;width:15px;vertical-align:sub /></td></tr>"
                            //"</td></td><td width='20px' style='display:none'>" + Griddata[i].EDIT + "</td>" +
                            //"</td > <td width='20px'  style='text-align:center'>" + "<input type=checkbox name=chkSelectDLMember class=chkEdit id=chkEdit" + [i] + " style=height:15px;width:15px;vertical-align:sub /></td>" +
                            //"</td></td><td width='20px' style='display:none'>" + Griddata[i].DELETE + "</td>" +
                            //"<td width='20px'  style='text-align:center'>" + "<input type=checkbox name=chkSelectDLMember class=chkDelete id=chkDelete" + [i] + " style=height:15px;width:15px;vertical-align:sub /></td>" +
                            //"</td></td><td width='20px' style='display:none'>" + Griddata[i].EXPORT + "</td>" +
                            //"<td width='20px'  style='text-align:center'>" + "<input type=checkbox id=chkExport" + i + " style=height:15px;width:15px;vertical-align:sub /></td></tr>";




                        }
                        s += "</table>";

                        $("#tbdata").html(s);

                        AssignRights();


                    }

                },
                error: function (msg) {
                    alert(msg.d);
                }
            });
        }
        else {
            FillUsers();
            var searchVal = 18886;
            var isrole = $("#ddltype option:selected").val();


            var JsonObject =
            {

                intSearchId: searchVal,
                isRole: isrole,
            }
            $.ajax({
                type: "POST",
                url: "../Services/GetMenuData",
                data: JSON.stringify(JsonObject),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () {
                    $(".modal").show();
                },
                complete: function () {
                    $(".modal").hide();
                },
                success: function (msg) {

                    var retval = JSON.parse(msg);
                    var Griddata = JSON.parse(retval._DATA_);
                    var s = '';


                    if (Griddata.length == undefined || Griddata.length == 0) {


                    }
                    else {


                        for (i = 1; i < Griddata.length; i++) {

                            s += "<tr>";
                            s += "</td><td width='30px'style=display:none>" + Griddata[i].sntMenuID +
                                "</td></td><td width='300px'>" + Griddata[i].varMenu + "</td>"
                            s += "</td><td width='30px'style=display:none>" + Griddata[i].MenuAccess
                            s += "<td width='40px'>" + "<input type=checkbox class=chktdAll id=chktdAll" + Griddata[i].sntMenuID + " style=margin-left:8px;height:15px;width:15px;vertical-align:sub /></td>"
                            //"</td></td><td width='20px' style='display:none'>" + Griddata[i].ADD + "</td>" +
                            //"<td width='20px' style='text-align:center'>" + "<input type=checkbox name=chkSelectDLMember class=chkAdd id=chkAdd" + [i] + " style=height:15px;width:15px;vertical-align:sub /></td></tr>"
                            //"</td></td><td width='20px' style='display:none'>" + Griddata[i].EDIT + "</td>" +
                            //"</td > <td width='20px'  style='text-align:center'>" + "<input type=checkbox name=chkSelectDLMember class=chkEdit id=chkEdit" + [i] + " style=height:15px;width:15px;vertical-align:sub /></td>" +
                            //"</td></td><td width='20px' style='display:none'>" + Griddata[i].DELETE + "</td>" +
                            //"<td width='20px'  style='text-align:center'>" + "<input type=checkbox name=chkSelectDLMember class=chkDelete id=chkDelete" + [i] + " style=height:15px;width:15px;vertical-align:sub /></td>" +
                            //"</td></td><td width='20px' style='display:none'>" + Griddata[i].EXPORT + "</td>" +
                            //"<td width='20px'  style='text-align:center'>" + "<input type=checkbox id=chkExport" + i + " style=height:15px;width:15px;vertical-align:sub /></td></tr>";



                        }
                        s += "</table>";

                        $("#tbdata").html(s);

                        AssignRights();
                    }

                },
                error: function (msg) {
                    alert(msg.d);
                }
            });


        }

    });

    $("#ddlRoleType").change(function () {

        LoadGrid();
    });

    $("#btnSave").click(function () {


        var isrole = $("#ddltype option:selected").val();
        var searchVal = $("#ddlRoleType option:selected").val();

        $("#tblMenuData").find('tr').each(function (i, el) {

            var $tds = $(this).find('td');

            MenuId = $tds.eq(0).text();
            AllMenuIds.push(MenuId);

            oldValue = $tds.eq(2).text();
            AlloldValue.push(oldValue);


            newValue = $('#chktdAll' + MenuId).is(":checked") ? "true" : "false";
            AllnewValue.push(newValue);

        });

        var JsonObject =
        {
            intSearchId: searchVal,
            isRole: isrole,
            MenuId: AllMenuIds,
            oldvalue: AlloldValue,
            newValue: AllnewValue
        }

        $.ajax({
            type: "POST",
            url: "../Services/SaveMenuPermission",
            data: JSON.stringify(JsonObject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {

                var s = '';
                var Result = JSON.parse(msg);

                if (Result._STATUS_ == "SUCCESS") {

                    alert(Result._MESSAGE_);
                    location.reload();

                }
                else {

                    alert(Result._MESSAGE_);
                }
                //form.reset();

            },
            error: function (msg) {
                alert("Save Failed...");
            }
        });


    });


});



function FillRoles() {
    // debugger;
    $.ajax({
        type: "POST",
        url: "../Services/GetAllRoles",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $(".modal").show();
        },
        complete: function () {
            $(".modal").hide();
        },
        success: function (msg) {

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
                    //sntRoleID, varRoleName, varRemark, bitIsActive, role_code

                    for (i = 0; i < data.length; i++) {
                        s += "<option value=" + data[i].sntRoleID + ">" + data[i].varRoleName + "</option>"
                    }
                    $("#ddlRoleType").html(s);
                }
            }
        },
        error: function (msg) {
            HandleAjaxError(msg);
        }
    })
}

function FillUsers() {
    // debugger;
    $.ajax({
        type: "POST",
        url: "../Services/GetAllUsers",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $(".modal").show();
        },
        complete: function () {
            $(".modal").hide();
        },
        success: function (msg) {

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
                    //sntRoleID, varRoleName, varRemark, bitIsActive, role_code

                    for (i = 0; i < data.length; i++) {
                        s += "<option value=" + data[i].intUserID + ">" + data[i].varUserLoginID + "</option>"
                    }
                    $("#ddlRoleType").html(s);
                }
            }
        },
        error: function (msg) {
            HandleAjaxError(msg);
        }
    })
}

function AssignRights() {

    $("#tblMenuData").find('tr').each(function (i, el) {
        //debugger;
        var $tds = $(this).find('td');
        var menuId = $tds.eq(0).text();
        var menuAccess = $tds.eq(2).text();



        if (menuAccess == "true") {
            $(this).find($("#chktdAll" + menuId).prop("checked", true));

        }


    });
}


function LoadGrid() {
    var searchVal = $("#ddlRoleType option:selected").val();
    var isrole = $("#ddltype option:selected").val();


    var JsonObject =
    {

        intSearchId: searchVal,
        isRole: isrole,
    }
    $.ajax({
        type: "POST",
        url: "../Services/GetMenuData",
        data: JSON.stringify(JsonObject),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $(".modal").show();
        },
        complete: function () {
            $(".modal").hide();
        },
        success: function (msg) {

            var retval = JSON.parse(msg);
            var Griddata = JSON.parse(retval._DATA_);
            var s = '';


            if (Griddata.length == undefined || Griddata.length == 0) {


            }
            else {


                for (i = 1; i < Griddata.length; i++) {

                    s += "<tr>";
                    s += "</td><td width='30px'style=display:none>" + Griddata[i].sntMenuID +
                        "</td></td><td width='300px'>" + Griddata[i].varMenu + "</td>"
                    s += "</td><td width='30px' style=display:none>" + Griddata[i].MenuAccess
                    s += "<td width='40px'>" + "<input type=checkbox class=chktdAll id=chktdAll" + Griddata[i].sntMenuID + " style=margin-left:8px;height:15px;width:15px;vertical-align:sub /></td>"
                    //"</td></td><td width='20px' style='display:none'>" + Griddata[i].ADD + "</td>" +
                    //"<td width='20px' style='text-align:center'>" + "<input type=checkbox name=chkSelectDLMember class=chkAdd id=chkAdd" + [i] + " style=height:15px;width:15px;vertical-align:sub /></td></tr>"
                    //"</td></td><td width='20px' style='display:none'>" + Griddata[i].EDIT + "</td>" +
                    //"</td > <td width='20px'  style='text-align:center'>" + "<input type=checkbox name=chkSelectDLMember class=chkEdit id=chkEdit" + [i] + " style=height:15px;width:15px;vertical-align:sub /></td>" +
                    //"</td></td><td width='20px' style='display:none'>" + Griddata[i].DELETE + "</td>" +
                    //"<td width='20px'  style='text-align:center'>" + "<input type=checkbox name=chkSelectDLMember class=chkDelete id=chkDelete" + [i] + " style=height:15px;width:15px;vertical-align:sub /></td>" +
                    //"</td></td><td width='20px' style='display:none'>" + Griddata[i].EXPORT + "</td>" +
                    //"<td width='20px'  style='text-align:center'>" + "<input type=checkbox id=chkExport" + i + " style=height:15px;width:15px;vertical-align:sub /></td></tr>";


                }
                s += "</table>";

                $("#tbdata").html(s);

                AssignRights();


            }

        },
        error: function (msg) {
            alert(msg.d);
        }
    });

}




