$(document).ready(function () {
    // When the delete button is clicked
    $(".delete-btn").on("click", function () {
        var bookId = $(this).data("book-id");

        // Show a confirmation dialog
        if (confirm("Are you sure you want to delete this book?")) {
            // Send an AJAX request to the server to delete the book
            $.ajax({
                url: '/Admin/SanPham/DeleteBook',
                type: 'POST',
                data: { MaSach: bookId },
                success: function (data) {
                    if (data.success) {
                        // Reload the page to see the updated book list
                        location.reload();
                    } else {
                        alert("Failed to delete the book.");
                    }
                },
                error: function () {
                    alert("An error occurred while processing your request.");
                }
            });
        }
    });
});
