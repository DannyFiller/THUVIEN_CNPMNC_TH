
    $(document).ready(function () {
        $('.delete-btn').on('click', function () {
            var maDocGia = $(this).data('maDocGia');
            var confirmDelete = confirm('Bạn có chắc muốn xóa độc giả này?');
            if (confirmDelete) {
                deleteDocGia(maDocGia);
            }
        });
    function deleteDocGia(maDocGia) {
            var form = $('<form>', {
        'action': '/Admin/DocGia/DeleteDocGia',
        'method': 'post'
            });

        var antiForgeryToken = $('<input>', {
            'type': 'hidden',
            'name': '__RequestVerificationToken',
            'value': '@Html.AntiForgeryToken()'
            });

            var maDocGiaInput = $('<input>', {
                'type': 'hidden',
                'name': 'maDocGia',
                'value': maDocGia
            });

                form.append(antiForgeryToken);
                form.append(maDocGiaInput);
                $('body').append(form);

                form.submit();
        }
    });
