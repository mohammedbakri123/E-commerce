"use strict";

const pageState = document.querySelector(".page-state");
const formProduct = document.querySelector("form");
const addBtn = formProduct.querySelector('button[type="submit"]');
const cancelBtn = formProduct.querySelector('button[type="reset"]');

let editBtn;
let currentEditProductId = null;
const all_Products_table = document.querySelector(".all-Products");

// --- Init ---
document.addEventListener("DOMContentLoaded", () => {
  FetchAllCategories();
  FetchAllBrands();
  getAllProducts();
  initEditButton();
});

// --- Fetch Categories ---
async function FetchAllCategories() {
  try {
    const response = await fetch("http://localhost:5106/SubCategory/all");
    if (!response.ok) throw new Error("Failed to fetch categories");

    const data = await response.json();
    const select = document.getElementById("subCategorySelect");
    if (!select) return console.error("Category Select element not found");

    select.innerHTML = '<option value="">-- Select Category --</option>';

    data.forEach((item) => {
      const option = document.createElement("option");
      option.value = item.subCategoryId ?? item.id; // adjust to API
      option.textContent = item.subCategoryName ?? item.name;
      select.appendChild(option);
    });
  } catch (error) {
    console.error("Error fetching categories:", error);
  }
}

// --- Fetch Brands ---
async function FetchAllBrands() {
  try {
    const response = await fetch("http://localhost:5106/Brand/all");
    if (!response.ok) throw new Error("Failed to fetch Brands");

    const data = await response.json();
    const select = document.getElementById("BrandSelect");
    if (!select) return console.error("BrandSelect element not found");

    select.innerHTML = '<option value="">-- Select Brand --</option>';

    data.forEach((item) => {
      const option = document.createElement("option");
      option.value = item.brandId ?? item.id;
      option.textContent = item.brandName ?? item.name;
      select.appendChild(option);
    });
  } catch (error) {
    console.error("Error fetching brands:", error);
  }
}

// --- Init Edit Button ---
function initEditButton() {
  if (!document.querySelector("#edit-btn")) {
    editBtn = document.createElement("button");
    editBtn.type = "button";
    editBtn.id = "edit-btn";
    editBtn.textContent = "Update Product";
    editBtn.classList.add("btn", "btn-warning");
    editBtn.style.float = "right";
    editBtn.style.display = "none";
    formProduct.appendChild(editBtn);
  } else {
    editBtn = document.querySelector("#edit-btn");
  }

  editBtn.addEventListener("click", handleEditSubmit);
}

// --- Fetch All Products ---
async function getAllProducts() {
  try {
    const response = await fetch("http://localhost:5106/Product/getAll");
    if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
    const data = await response.json();
    renderProductTable(data);
  } catch (err) {
    console.error(err);
    all_Products_table.innerHTML = `<tr><td colspan="7">Failed to load products</td></tr>`;
  }
}

// --- Render Products ---
function renderProductTable(products) {
  all_Products_table.innerHTML = "";
  products.forEach((product) => {
    const row = document.createElement("tr");
    row.dataset.id = product.productId ?? product.id;
    row.innerHTML = `
        <td>${product.productId ?? product.id}</td>
        <td>${product.name}</td>
        <td>${product.brandName}</td>
        <td>${product.subCategoryName}</td>
        <td>${product.createdDate}</td>
        <td>
          <a href="#" class="btn btn-success edit-btn">Edit</a>
          <a href="#" class="btn btn-danger delete-btn">Delete</a>
        </td>
    `;
    all_Products_table.appendChild(row);
  });
}

// --- Handle Edit/Delete Clicks ---
all_Products_table.addEventListener("click", async (e) => {
  e.preventDefault();
  const row = e.target.closest("tr");
  if (!row) return;

  const productId = row.dataset.id;
  console.log(productId);

  if (e.target.classList.contains("edit-btn")) {
    await loadProductToForm(productId);
  } else if (e.target.classList.contains("delete-btn")) {
    await deleteProduct(productId, row);
  }
});

// --- Load Product Into Form for Editing ---
async function loadProductToForm(productId) {
  try {
    const res = await fetch(`http://localhost:5106/Product/get/${productId}`);
    if (!res.ok) throw new Error("Failed to fetch product");

    const product = await res.json();

    console.log(product);
    formProduct.name.value = product.name;
    formProduct.subCategoryId.value = product.subCategoryID;
    formProduct.brandId.value = product.brandID;

    currentEditProductId = product.productId ?? product.id;

    addBtn.style.display = "none";
    editBtn.style.display = "inline-block";
    pageState.textContent = `Edit Product ${currentEditProductId}`;
  } catch (err) {
    console.error(err);
    alert("Error loading product: " + err.message);
  }
}

// --- Delete Product ---
async function deleteProduct(productId, row) {
  if (!confirm("Are you sure you want to delete this Product?")) return;

  try {
    const res = await fetch(
      `http://localhost:5106/Product/delete/${productId}`,
      {
        method: "DELETE",
      }
    );
    if (!res.ok) throw new Error("Failed to delete Product");
    row.remove();
    alert("Product deleted successfully!");
  } catch (err) {
    console.error(err);
    alert("Error: " + err.message);
  }
}

// --- Helpers ---
function getFormData() {
  const formData = new FormData(formProduct);
  return Object.fromEntries(formData.entries());
}

// --- Add New Product ---
formProduct.addEventListener("submit", handleAddSubmit);

async function handleAddSubmit(e) {
  e.preventDefault();
  const data = getFormData();

  await sendProductData("http://localhost:5106/Product/add", data);
  formProduct.reset();
  getAllProducts();
}

// --- Edit Existing Product ---
async function handleEditSubmit() {
  if (!currentEditProductId) return alert("No product selected for edit");

  const data = getFormData();
  data.id = currentEditProductId;
  console.log(currentEditProductId);
  console.log(data);
  await sendProductData("http://localhost:5106/Product/update", data, "PUT");

  formProduct.reset();
  currentEditProductId = null;
  addBtn.style.display = "inline-block";
  editBtn.style.display = "none";
  pageState.textContent = "Add New Product";
  getAllProducts();
}

// --- Send Data Helper ---
async function sendProductData(url, body, method = "POST") {
  try {
    const res = await fetch(url, {
      method,
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(body),
    });

    if (!res.ok) {
      const errorData = await res.json().catch(() => null);
      throw new Error(errorData?.message || res.statusText);
    }

    const result = await res.json();
    alert("Operation successful!");
    return result;
  } catch (err) {
    console.error(err);
    alert("Error: " + err.message);
  }
}
