const product_name = document.querySelector(".product-name");
const product_price = document.querySelector(".product-price");
const product_describtions = document.querySelector(".product-describtions");
const quantity = document.querySelector(".quantity");
const favBtn = document.querySelector(".add-to-fav-btn");
const favBtnImg = document.querySelector(".add-to-fav-btn img");

const params = new URLSearchParams(window.location.search);
const variantId = params.get("variantId");

const storedUser = localStorage.getItem("userData");
const currentUser = JSON.parse(storedUser);

window.addEventListener("DOMContentLoaded", async function () {
  console.log(variantId);

  try {
    const res = await fetch(
      `http://localhost:5106/Variant/getWithPriceAndQuantity/${variantId}`
    );

    const data = await res.json();
    console.log(data);

    product_name.innerHTML = data.productName;
    product_price.innerHTML = data.price;
    product_describtions.innerHTML = data.variantProperties;
    quantity.innerHTML = `Quantity: ${data.quantity}`;

    const res1 = await fetch(
      `http://localhost:5106/Favorite/getAll?userId=${currentUser.userId}&variantId=${variantId}`
    );

    const data1 = await res1.json();
    //

    if (data1.length !== 0) {
      console.log(data1);
      favBtnImg.src = "../assets/Icons/favorite-fill.png";
    }
  } catch (error) {
    console.error(error.message);
  }
});

favBtn.addEventListener("click", async function () {
  console.log(currentUser.userId);
  try {
    const res1 = await fetch(
      `http://localhost:5106/Favorite/getAll?userId=${currentUser.userId}&variantId=${variantId}`
    );

    const data = await res1.json();
    //

    if (data.length === 0) {
      const dataBody = {
        userId: currentUser.userId,
        variantId: variantId,
      };

      const res2 = await fetch("http://localhost:5106/Favorite/add", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(dataBody),
      });

      if (!res2.ok) {
        const error = await res2.text();
        throw new Error(`Server error: ${error}`);
      }

      const dataRes = await res2.json();

      favBtnImg.src = "../assets/Icons/favorite-fill.png";
      console.log(dataRes);
    }
  } catch (error) {}
});
