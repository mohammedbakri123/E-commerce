"use strict";

const products = document.querySelector(".products");
const summary_total = document.querySelector(".summary-total");
const order_btn = document.querySelector(".order-btn");

const storedUser = localStorage.getItem("userData");
const currentUser = JSON.parse(storedUser);
let currentCartId;
let totalAmount;

function productCartHTML(cartItem, productName) {
  return `  
    <tr class="row">
      <td class="product-name">${productName}</td>
      <td class="price">${cartItem.pricePerUnit}</td>
      <td>
        <button class="delete-btn" data-cartItemId="${cartItem.cartItemId}">Delete</button>
      </td>
    </tr>`;
}

async function fetchProductCart() {
  try {
    const res = await fetch(
      `http://localhost:5106/Cart/getLast/${currentUser.userId}`
    );

    const data = await res.json();

    currentCartId = data.cartId;
    console.log(data);

    const res2 = await fetch(
      `http://localhost:5106/CartItem/getAll/${data.cartId}`
    );

    const data2 = await res2.json();
    console.log(data2);

    totalAmount = data2.reduce((total, cartItem) => {
      return total + cartItem.pricePerUnit;
    }, 0);

    summary_total.innerHTML = totalAmount + "$";

    data2.map(async (cartItem) => {
      const res3 = await fetch(
        `http://localhost:5106/Variant/get/${cartItem.variantId}`
      );

      const data3 = await res3.json();

      products.insertAdjacentHTML(
        "beforeend",
        productCartHTML(cartItem, data3.productName)
      );
    });

    products.addEventListener("click", async function (e) {
      if (e.target.classList.contains("delete-btn")) {
        const row = e.target.closest("tr");
        const cartItemId = e.target.dataset.cartitemid;

        console.log("Deleting cart item with ID:", cartItemId);

        try {
          const res = await fetch(
            `http://localhost:5106/CartItem/delete/${cartItemId}`,
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
  await fetchProductCart();
});

order_btn.addEventListener("click", async function () {
  console.log("hello");
  const dataBody = {
    userId: currentUser.userId,
    totalAmount: totalAmount,
    paymentMethod: "by hand",
    deliveryInfoId: 1,
    cartId: currentCartId,
  };

  try {
    const res = await fetch("http://localhost:5106/Order/add", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(dataBody),
    });

    const data = await res.json();
    console.log(data);

    if (data) {
      alert("we will contact with you soon");
      location.reload();
    }
  } catch (error) {
    console.error(error.message);
  }
});
