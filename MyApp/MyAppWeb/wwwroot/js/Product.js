var dtable;
$(document).ready(function () {
    dtable = $('#myTable').DataTale({
        "ajax": {
            "url":"/Admin/Product/AllProducts"
        },
            "columns": [
                {"data":"name"},
                {"data":"description"},
                { "data": "price" },
                { "data": "category.name" },
                {
                    "data": "id",
                    "render": function (data) {
                        return 
                            '<a href="/Admin/Product/CreateUpdate?id=${data}">
                                <i class="bi bi-pencil-square"></i></a>
                            <a onClick=RemoveProduct("/Admin/Product/"${data}) >
                                <i class="bi bi-trash"></i></a>
                            '
                    }
                }
            ]
    });
});
function RemoveProduct(url)
{
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be ",
        icon: 'warning',
        showCanceButton: true,
        confirmButton: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes,delete it!'
    }).then(() => {
        if (result.isConfirmred) {
            $.ajax({
                url: url,
                type: 'DELETE',
                Success: function (data) {
                    if (data.success) {
                        dtable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}