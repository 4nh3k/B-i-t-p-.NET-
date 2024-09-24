document
  .querySelector("header .search-bar button")
  .addEventListener("click", function () {
    const searchQuery = document.querySelector(
      "header .search-bar input"
    ).value;
    if (searchQuery) {
      alert(`Searching for: ${searchQuery}`);
    } else {
      alert("Please enter a search query");
    }
  });

document.querySelectorAll(".sidebar .side-article").forEach((article) => {
  article.addEventListener("mouseenter", function () {
    article.style.boxShadow = "0 0 20px rgba(0, 0, 0, 0.2)";
  });

  article.addEventListener("mouseleave", function () {
    article.style.boxShadow = "0 0 15px rgba(0, 0, 0, 0.1)";
  });
});
