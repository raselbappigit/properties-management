$(function () {
    //for display more collapse data from role
    var projObjData;

    $('#projDataTable tbody td img').live('click', function () {
        var nTr = this.parentNode.parentNode;
        if (this.src.match('details_close')) {
            this.src = "/Images/App/details_open.png";
            projObjData.fnClose(nTr);
        }
        else {
            this.src = "/Images/App/details_close.png";
            var projid = $(this).attr("rel");
            $.get("/Project/ProjectBlocks?projId=" + projid, function (blocks) {
                projObjData.fnOpen(nTr, blocks, 'details');
            });
        }
    });

    projObjData = $('#projDataTable').dataTable({
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
        "sAjaxSource": "/Project/GetProjects",
        "aoColumns": [{ "sName": "ID",
            "bSearchable": false,
            "bSortable": false,
            "fnRender": function (oObj) {
                return '<img class="projBlocks img-expand-collapse" src="/Images/App/details_open.png" title="Project Info" alt="expand/collapse" rel="' +
                                oObj.aData[0] + '"/>' +
                                '<a class="projDetailsLink" href=\"/Project/Details/' +
                                oObj.aData[0] + '\" ><img src="/Images/App/detail.png" title="Details" class="tb-space" alt="Detail"></a>' +
                                '<a class="projEditLink" href=\"/Project/Edit/' +
                                oObj.aData[0] + '\" ><img src="/Images/App/edit.png" title="Edit" class="tb-space" alt="Edit"></a>' +
                                '<a class="projDeleteLink" href=\"/Project/Delete/' +
                                oObj.aData[0] + '\" ><img src="/Images/App/delete.png" title="Delete" class="tb-space" alt="Delete"></a>';

            }

        },
                          { "sName": "NAME" },
                          { "sName": "LOCATION" },
                          { "sName": "DESCRIPTION" },
                          { "sName": "ESTEMATE" },
                          { "sName": "UNITVALUE" },
                          { "sName": "CREATEDATE" }
            ]
    });
});