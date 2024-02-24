
    $(document).ready(function () {
        $('.delete-button').click(function () {
            var id = $(this).data('id');
            $('#confirmationPopup').fadeIn();
            $('#confirmDeleteButton').data('id', id);
        });

        $('#confirmDeleteButton').click(function () {
            var id = $(this).data('id');
            $.ajax({
                type: 'POST',
                url: '@Url.Action("DeleteItem", "SanPham")',
                data: { id: id },
                success: function (data) {
                    if (data.success) {
                        showAlert("Xóa thành công!", "success");
                        $('#confirmationPopup').fadeOut();
                        // Reload the page after successful deletion
                        location.reload();
                    } else {
                        showAlert("Không thể xóa. Có sách tồn tại liên quan.", "error");
                        $('#confirmationPopup').fadeOut();
                    }
                },
                error: function () {
                    showAlert("Đã xảy ra lỗi khi xóa.", "error");
                    $('#confirmationPopup').fadeOut();
                }
            });
        });

        $('#cancelDeleteButton').click(function () {
            $('#confirmationPopup').fadeOut();
        });

        function showAlert(message, type) {
            var alertClass = "custom-alert";
            if (type === "success") {
                alertClass += " success";
            } else if (type === "error") {
                alertClass += " error";
            }

            var alertDiv = $("<div>").addClass(alertClass).text(message);
            var alertContainer = $("<div>").addClass("custom-alert-message").append(alertDiv);
            $("body").append(alertContainer);

            // Auto-close the alert after 3 seconds
            setTimeout(function () {
                alertContainer.fadeOut(function () {
                    $(this).remove();
                });
            }, 3000);
        }
    });
