$(function () {
    //for display more collapse data from role
    var unitValObjData;

    $('#unitValDataTable tbody td img').live('click', function () {
        var nTr = this.parentNode.parentNode;
        if (this.src.match('details_close')) {
            this.src = "/Images/App/details_open.png";
            unitValObjData.fnClose(nTr);
        }
        else {
            this.src = "/Images/App/details_close.png";
            var unitValid = $(this).attr("rel");
            $.get("/UnitValue/UnitValueProjects?unitValId=" + unitValid, function (projects) {
                unitValObjData.fnOpen(nTr, projects, 'details');
            });
        }
    });

    unitValObjData = $('#unitValDataTable').dataTable({
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
        "sAjaxSource": "/UnitValue/GetUnitValues",
        "aoColumns": [{ "sName": "ID",
            "bSearchable": false,
            "bSortable": false,
            "fnRender": function (oObj) {
                return '<img class="unitValProjects img-expand-collapse" src="/Images/App/details_open.png" title="Unit Type" alt="expand/collapse" rel="' +
                                oObj.aData[0] + '"/>' +
                                '<a class="unitValDetailsLink" href=\"/UnitValue/Details/' +
                                oObj.aData[0] + '\" ><img src="/Images/App/detail.png" title="Details" class="tb-space" alt="Detail"></a>' +
                                '<a class="unitValEditLink" href=\"/UnitValue/Edit/' +
                                oObj.aData[0] + '\" ><img src="/Images/App/edit.png" title="Edit" class="tb-space" alt="Edit"></a>' +
                                '<a class="unitValDeleteLink" href=\"/UnitValue/Delete/' +
                                oObj.aData[0] + '\" ><img src="/Images/App/delete.png" title="Delete" class="tb-space" alt="Delete"></a>';

            }

        },
                          { "sName": "NAME" },
                          { "sName": "NOTE" },
                          { "sName": "UNITTYPENAME" }
            ]
    });
});