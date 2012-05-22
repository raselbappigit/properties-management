$(function () {
    //for display more collapse data from roleunitTypTyp
    var unitTypObjData;

    $('#unitTypDataTable tbody td img').live('click', function () {
        var nTr = this.parentNode.parentNode;
        if (this.src.match('details_close')) {
            this.src = "/Images/App/details_open.png";
            unitTypObjData.fnClose(nTr);
        }
        else {
            this.src = "/Images/App/details_close.png";
            var unitTypid = $(this).attr("rel");
            $.get("/Unittype/UnittypeUnitValues?unitTypId=" + unitTypid, function (projects) {
                unitTypObjData.fnOpen(nTr, projects, 'details');
            });
        }
    });

    unitTypObjData = $('#unitTypDataTable').dataTable({
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
        "sAjaxSource": "/Unittype/GetUnitTypes",
        "aoColumns": [{ "sName": "ID",
            "bSearchable": false,
            "bSortable": false,
            "fnRender": function (oObj) {
                return '<img class="unitTypUnitValues img-expand-collapse" src="/Images/App/details_open.png" title="Unit Values" alt="expand/collapse" rel="' +
                                oObj.aData[0] + '"/>' +
                                '<a class="unitTypDetailsLink" href=\"/Unittype/Details/' +
                                oObj.aData[0] + '\" ><img src="/Images/App/detail.png" title="Details" class="tb-space" alt="Detail"></a>' +
                                '<a class="unitTypEditLink" href=\"/Unittype/Edit/' +
                                oObj.aData[0] + '\" ><img src="/Images/App/edit.png" title="Edit" class="tb-space" alt="Edit"></a>' +
                                '<a class="unitTypDeleteLink" href=\"/Unittype/Delete/' +
                                oObj.aData[0] + '\" ><img src="/Images/App/delete.png" title="Delete" class="tb-space" alt="Delete"></a>';

            }

        },
                          { "sName": "NAME" }
            ]
    });
});