document.addEventListener("DOMContentLoaded", () => {
  fetch("../doc/footer.html")
    .then((response) => response.text())
    .then((data) => {
      console.log(data);
      document.querySelector(".footer").innerHTML = data;
    })
    .catch((error) => console.error("Error while loading hydra", error));
});
