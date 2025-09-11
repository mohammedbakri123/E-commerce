document.addEventListener("DOMContentLoaded", () => {
  fetch("../doc/header.html")
    .then((response) => response.text())
    .then((data) => {
      console.log(data);
      document.querySelector(".header").innerHTML = data;
    })
    .catch((error) => console.error("Error while loading hydra", error));
});
