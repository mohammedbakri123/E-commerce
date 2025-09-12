"use strict";

const favorites = document.querySelector(".Favorites");

const storedUser = localStorage.getItem("userData");
const currentUser = JSON.parse(storedUser);

function favoriteHTML(favorite) {
  return `  
    <tr class="row">
      <td class="product-name">${favorite.variantName}</td>
      <td class="date">${favorite.createdAt}</td>
      <td>
        <button class="delete-btn" data-favoriteId="${favorite.id}">Delete</button>
      </td>
    </tr>`;
}

async function fetchFav() {
  try {
    const res = await fetch(
      `http://localhost:5106/Favorite/getAll?userId=${currentUser.userId}`
    );

    const data = await res.json();
    console.log(data);

    data.map((favorite) => {
      favorites.insertAdjacentHTML("beforeend", favoriteHTML(favorite));
    });

    favorites.addEventListener("click", async function (e) {
      if (e.target.classList.contains("delete-btn")) {
        const row = e.target.closest("tr");
        const variantId = e.target.dataset.favoriteid;

        console.log("Deleting row with ID:", variantId);

        try {
          const res = await fetch(
            `http://localhost:5106/Favorite/delete/${variantId}`,
            { method: "DELETE" }
          );

          if (res.ok) {
            row.remove();
          } else {
            console.error("Failed to delete:", await res.text());
          }
        } catch (error) {
          console.error(error);
        }
      }
    });
  } catch (error) {
    console.error(error.message);
  }
}

window.addEventListener("DOMContentLoaded", async function () {
  await fetchFav();
});
