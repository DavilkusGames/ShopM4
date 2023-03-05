
$(document).ready(function () {
    $('#myTable').DataTable({
        "ajax": {
            "url": "/query/" + "GetQueryList"
        },
        "columns": [
            { "data": "id" },
            { "data": "fullName" },
            { "data": "phoneNumber" },
            { "data": "email" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <a href="/query/details/${data}" class="btn btn-success"> Go </a>
                    `;
                }
            }
        ]
    });
});