document.addEventListener("DOMContentLoaded", () => {
  fetch("../doc/header.html")
    .then((response) => response.text())
    .then((data) => {
      const header = document.querySelector(".header");
      header.innerHTML = data;

      const admin = header.querySelector(".admin");

      const storedUser = localStorage.getItem("userData");
      const currentUser = JSON.parse(storedUser);

      if (currentUser.role === "Customer") admin.remove();
    })
    .catch((error) => console.error("Error while loading hydra", error));
});
