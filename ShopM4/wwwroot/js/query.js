$(document).ready(function () {
    $('#myTable').DataTable({
        "ajax": {
                "url" : "/query/" + "GetQueryList"
        },
        "columns": [
            { "data": "id" }
            { "data": "fullName" },
            { "data": "phoneNumber" },
            { "data": "email" },
            {
                "data": null,
                "render": function (data) { return 'Button'; }
                }
        ]
    });
});