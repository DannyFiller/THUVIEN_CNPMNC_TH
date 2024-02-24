//// filter.js

//$(document).ready(function () {
//    // Sự kiện khi thay đổi giá trị của dropdown thể loại
//    $('#maTheLoai').change(function () {
//        $('#filterForm').submit(); // Gửi yêu cầu khi có thay đổi
//    });

//    // Sự kiện submit form
//    $('#filterForm').submit(function (event) {
//        event.preventDefault(); // Ngăn chặn submit form mặc định

//        // Gửi yêu cầu Ajax để lấy danh sách sách
//        $.ajax({
//            url: $(this).attr('action'),
//            type: $(this).attr('method'),
//            data: $(this).serialize(),
//            success: function (data) {
//                $('#bookList').html(data); // Cập nhật nội dung danh sách sách
//            },
//            error: function () {
//                console.log("Lỗi khi gửi yêu cầu Ajax");
//            }
//        });
//    });
//});
