"use strict";

const productsCards = document.querySelector(".products-cards");
const btnAddToCart = document.querySelector(".add-to-card-button");

function productCart(variant) {
  return `<article class="product-card grid gird--2--row">
            <div class="product-img-container">
              <img
                src="../assets/image/iphone16_PNG13 1.png"
                class="product-img"
                alt="iphone 16"
              />
            </div>

            <div class="description">
              <h3 class="product-name">📱 <a href="product_details.html?variantId=${variant.variantId}">${variant.productName}</a></h3>
              <p class="product-rating">💲 ${variant.price}$</p>
              <div class="product-buttons">
                <button class="btn add-to-card-button" data-variantId="${variant.variantId}">
                  <img src="../assets/Icons/cart2.svg" alt="cart" />
                </button>
                <button class="btn buy-button">Buy now</button>
              </div>
            </div>
          </article>`;
}

async function fetchProduct() {
  try {
    const res = await fetch(
      "http://localhost:5106/Variant/GetAllWithPriceAndQuantity"
    );

    const data = await res.json();
    console.log(data);

    data.map((variant) => {
      productsCards.insertAdjacentHTML("afterbegin", productCart(variant));
    });
  } catch (error) {
    console.error(error.message);
  }
}

window.addEventListener("DOMContentLoaded", async function () {
  await fetchProduct();
});
