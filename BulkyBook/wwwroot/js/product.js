﻿var dataTable;


$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Product/GetAll"
        },
        "columns": [
            {"data": "title", "width": "15%"},
            {"data": "isbn", "width": "15%"},
            {"data": "price", "width": "15%"},
            {"data": "author", "width": "15%"},
            {"data": "category.name", "width": "15%"},
            {
                "data": "id",
                "render": function (data) {

                    return`
                     <div class="group w-75 btn-group">
                    <a href="/Admin/Product/Upsert?id=${data}"
                     class="btn btn-primary mx-2"> Edit <i class="bi bi-pencil-square"></i></a>
                    <a href="/Admin/Product/Upsert?id=${data}" 
                      class="btn btn-danger mx-2"> Delete <i class="bi bi-x-square"></i></a>
                </div>
                     `
                },
                "width": "15%"
            },
        ]
    });
}