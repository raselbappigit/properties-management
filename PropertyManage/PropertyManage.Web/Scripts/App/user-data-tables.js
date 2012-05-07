$(function () {
    //for display more collapse data from role
    var usrObjData;

    $('#userDataTable tbody td img').live('click', function () {
        var nTr = this.parentNode.parentNode;
        if (this.src.match('details_close')) {
            this.src = "/Images/App/details_open.png";
            usrObjData.fnClose(nTr);
        }
        else {
            this.src = "/Images/App/details_close.png";
            var usrid = $(this).attr("rel");
            $.get("/User/UsrRoles?usrId=" + usrid, function (roles) {
                usrObjData.fnOpen(nTr, roles, 'details');
            });
        }
    });

    usrObjData = $('#userDataTable').dataTable({
        "bJQueryUI": true,
        "bAutoWidth": false,
        "bSort": false,
        "oLanguage": {
            "sLengthMenu": "Display _MENU_ records per page",
            "sZeroRecords": "Nothing found - Sorry",
            "sInfo": "Showing _START_ to _END_ of _TOTAL_ records",
            "sInfoEmpty": "Showing 0 to 0 of 0 records",
            "sInfoFiltered": "(filtered from _MAX_ total records)"
        },
        "bProcessing": true,
        "bServerSide": true,
        "sAjaxSource": "/User/GetUsers",
        "aoColumns": [{ "sName": "ID",
            "bSearchable": false,
            "bSortable": false,
            "fnRender": function (oObj) {
                return '<img class="usrRoles img-expand-collapse" src="/Images/App/details_open.png" alt="expand/collapse" rel="' +
                                oObj.aData[0] + '"/>' +
                                '<a class="usrDetailsLink" href=\"/User/Details/' +
                                oObj.aData[0] + '\" data-dialog-title="Details User">Details</a>' +
                                '<a class="usrEditLink" href=\"/User/Edit/' +
                                oObj.aData[0] + '\" data-dialog-title="Edit User">Edit</a>' +
                                '<a class="usrDeleteLink" href=\"/User/Delete/' +
                                oObj.aData[0] + '\" data-dialog-title="Delete User">Delete</a>' +
                                '<a class="usrAssignRoleLink" href=\"/User/AssignRole/' +
                                oObj.aData[0] + '\" data-dialog-title="Assign Role">Role</a>';

            }

        },
                          { "sName": "USERNAME" },
                          { "sName": "EMAIL" },
                          { "sName": "IS_APPROVED" },
                          { "sName": "CREATION_DATE" },
                          { "sName": "LAST_LOGIN" },
                          { "sName": "LAST_ACTIVITY" },
                          { "sName": "LAST_PASS_CHANGE" }
            ]
    });
});