// This function is used to remove diacritics from Vietnamese characters
function removeDiacritics(str) {
    return str.normalize("NFD").replace(/[\u0300-\u036f]/g, "");
}

document.addEventListener("DOMContentLoaded", function () {
    const searchInput = document.getElementById("searchInput");
    const table = document.querySelector("table");
    const rows = table.querySelectorAll("tbody tr");

    searchInput.addEventListener("input", function () {
        const searchTerm = removeDiacritics(this.value.trim().toLowerCase());

        rows.forEach((row) => {
            const bookTitle = row.querySelector("td:first-child").textContent;
            const normalizedTitle = removeDiacritics(bookTitle.toLowerCase());

            // Show or hide rows based on the search term
            if (normalizedTitle.includes(searchTerm)) {
                row.style.display = "table-row";
            } else {
                row.style.display = "none";
            }
        });
    });
});
