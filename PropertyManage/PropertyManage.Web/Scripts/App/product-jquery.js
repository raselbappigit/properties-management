$(function () {
    //for display more collapse data from info
    var prodObjData;

    $('#prodDataTable tbody td img').live('click', function () {
        var nTr = this.parentNode.parentNode;
        if (this.src.match('details_close')) {
            this.src = "/Images/App/details_open.png";
            prodObjData.fnClose(nTr);
        }
        else {
            this.src = "/Images/App/details_close.png";
            var prodid = $(this).attr("rel");
            $.get("/Product/ProductInfos?prodId=" + prodid, function (infos) {
                prodObjData.fnOpen(nTr, infos, 'details');
            });
        }
    });

    prodObjData = $('#prodDataTable').dataTable({
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
        "sAjaxSource": "/Product/GetProducts",
        "aoColumns": [{ "sName": "ID",
            "bSearchable": false,
            "bSortable": false,
            "fnRender": function (oObj) {
                return '<img class="prodInfos img-expand-collapse" src="/Images/App/details_open.png" title="Product Info" alt="expand/collapse" rel="' +
                                oObj.aData[0] + '"/>' +
                                '<a class="prodDetailsLink" href=\"/Product/Details/' +
                                oObj.aData[0] + '\" ><img src="/Images/App/detail.png" title="Details" class="tb-space" alt="Detail"></a>' +
                                '<a class="prodEditLink" href=\"/Product/Edit/' +
                                oObj.aData[0] + '\" ><img src="/Images/App/edit.png" title="Edit" class="tb-space" alt="Edit"></a>' +
                                '<a class="prodDeleteLink" href=\"/Product/Delete/' +
                                oObj.aData[0] + '\" ><img src="/Images/App/delete.png" title="Delete" class="tb-space" alt="Delete"></a>' +
                                '<a class="prodAssignInfoLink" href=\"/Product/Privilege/' +
                                oObj.aData[0] + '\" ><img src="/Images/App/setting.png" title="Set Privilege" class="tb-space" alt="Set Privilege"></a>';

            }

        },
                          { "sName": "NAME" },
                          { "sName": "MAINCOST" },
                          { "sName": "OTHERCOST" },
                          { "sName": "CATEGORYNAME" },
                          { "sName": "PROJECTNAME" },
                          { "sName": "UNITNAME" }
            ]
    });
});