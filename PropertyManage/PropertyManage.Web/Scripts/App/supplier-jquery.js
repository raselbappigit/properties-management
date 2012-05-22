$(function () {
    //for display more collapse data from role
    var suppObjData;

    $('#suppDataTable tbody td img').live('click', function () {
        var nTr = this.parentNode.parentNode;
        if (this.src.match('details_close')) {
            this.src = "/Images/App/details_open.png";
            suppObjData.fnClose(nTr);
        }
        else {
            this.src = "/Images/App/details_close.png";
            var suppid = $(this).attr("rel");
            $.get("/Supplier/SupplierInfos?suppId=" + suppid, function (infos) {
                suppObjData.fnOpen(nTr, infos, 'details');
            });
        }
    });

    suppObjData = $('#suppDataTable').dataTable({
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
        "sAjaxSource": "/Supplier/GetSuppliers",
        "aoColumns": [{ "sName": "ID",
            "bSearchable": false,
            "bSortable": false,
            "fnRender": function (oObj) {
                return '<img class="suppBlocks img-expand-collapse" src="/Images/App/details_open.png" title="Supplier Info" alt="expand/collapse" rel="' +
                                oObj.aData[0] + '"/>' +
                                '<a class="suppDetailsLink" href=\"/Supplier/Details/' +
                                oObj.aData[0] + '\" ><img src="/Images/App/detail.png" title="Details" class="tb-space" alt="Detail"></a>' +
                                '<a class="suppEditLink" href=\"/Supplier/Edit/' +
                                oObj.aData[0] + '\" ><img src="/Images/App/edit.png" title="Edit" class="tb-space" alt="Edit"></a>' +
                                '<a class="suppDeleteLink" href=\"/Supplier/Delete/' +
                                oObj.aData[0] + '\" ><img src="/Images/App/delete.png" title="Delete" class="tb-space" alt="Delete"></a>';

            }

        },
                          { "sName": "NAME" },
                          { "sName": "ADDRESS" },
                          { "sName": "MOBILE" },
                          { "sName": "EMAIL" },
                          { "sName": "CONTACTPERSON" }
            ]
    });
});