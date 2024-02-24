document.getElementById("category-select").addEventListener("change", function () {
    var selectedCategory = this.value;
    var bookItems = document.getElementsByClassName("book-item");

    for (var i = 0; i < bookItems.length; i++) {
        var bookItem = bookItems[i];
        if (selectedCategory === "all" || bookItem.classList.contains(selectedCategory)) {
            bookItem.style.display = "block";
        } else {
            bookItem.style.display = "none";
        }
    }
});
