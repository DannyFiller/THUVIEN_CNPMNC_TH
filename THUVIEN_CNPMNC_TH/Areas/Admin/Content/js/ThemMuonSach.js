$(document).ready(function () {
    // Xử lý sự kiện khi người dùng chọn tên sách từ dropdownlist
    $("#TenSachDropdown").change(function () {
        var selectedBookId = $(this).val(); // Lấy giá trị của tên sách đã chọn
        $("#MaSachHidden").val(selectedBookId); // Gán giá trị của MaSach bằng giá trị đã chọn
    });


    $("#TenDocGiaDropdown").change(function () {
        var selectedDocGiaId = $(this).val(); 
        $("#MaDocGiaHidden").val(selectedDocGiaId); 
    });

    // Xử lý sự kiện khi người dùng chọn tên thủ thư từ dropdownlist
    $("#TenThuThuDropdown").change(function () {
        var selectedThuThuId = $(this).val(); 
        $("#MaDocGiaHidden").val(selectedThuThuId); 
    });
});


