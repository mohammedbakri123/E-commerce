const pageState = document.querySelector(".page-state");
const formProduct = document.querySelector("form");
const addBtn = formProduct.querySelector('button[type="submit"]');
const CancelBtn = formProduct.querySelector('button[type="reset"]');

let editBtn;
let currentEditProductId = null;
const all_Products_table = document.querySelector(".all-Products");

// --- Init Edit Button ---

document.addEventListener("DOMContentLoaded", () => {
  FetchAllCategories();
  FetchAllBrands();
});

async function FetchAllCategories() {
  try {
    const response = await fetch("http://localhost:5106/SubCategory/all");
    if (!response.ok) throw new Error("Failed to fetch categories");

    const data = await response.json();

    const select = document.getElementById("subCategorySelect");
    if (!select) {
      console.error("Category Select element not found");
      return;
    }
    // Clear old options (except the default one)
    select.innerHTML = '<option value="">-- Select Category --</option>';

    // Populate new options
    data.forEach((item) => {
      const option = document.createElement("option");
      option.value = item.id; // adjust based on your DTO property (maybe SubCategoryId)
      option.textContent = item.name; // adjust based on your DTO property (maybe SubCategoryName)
      select.appendChild(option);
    });
  } catch (error) {
    console.error("Error fetching categories:", error);
  }
}
async function FetchAllBrands() {
  try {
    const response = await fetch("http://localhost:5106/Brand/all");
    if (!response.ok) throw new Error("Failed to fetch Brands");

    const data = await response.json();

    const select = document.getElementById("BrandSelect");
    if (!select) {
      console.error("BrandSelect element not found");
      return;
    }

    // Clear old options (except the default one)
    select.innerHTML = '<option value="">-- Select Brand --</option>';

    // Populate new options
    data.forEach((item) => {
      const option = document.createElement("option");
      option.value = item.id; // adjust based on your DTO property (maybe SubCategoryId)
      option.textContent = item.name; // adjust based on your DTO property (maybe SubCategoryName)
      select.appendChild(option);
    });
  } catch (error) {
    console.error("Error fetching categories:", error);
  }
}
// Call function when page loads
function initEditButton() {
  if (!document.querySelector("#edit-btn")) {
    editBtn = document.createElement("button");
    editBtn.type = "button";
    editBtn.id = "edit-btn";
    editBtn.textContent = "Edit Admin";
    editBtn.classList.add("btn", "btn-warning");
    editBtn.style.float = "right";
    editBtn.style.display = "none"; // hidden until edit
    formProduct.appendChild(editBtn);
  } else {
    editBtn = document.querySelector("#edit-btn");
  }

  editBtn.addEventListener("click", handleEditSubmit);
}
initEditButton();

// --- Fetch All Admins ---
async function getAllProducts() {
  try {
    const response = await fetch("http://localhost:5106/Product/getAll");
    if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
    const data = await response.json();
    console.log(data);
    renderProductTable(data);
  } catch (err) {
    console.error(err);
    all_Products_table.innerHTML = `<tr><td colspan="7">Failed to load products</td></tr>`;
  }
}
getAllProducts();

// --- Render Products ---
function renderProductTable(Products) {
  all_Products_table.innerHTML = "";
  Products.forEach((product) => {
    const row = document.createElement("tr");
    row.dataset.id = product.id;
    row.innerHTML = `
        <td>${product.id}</td>
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

// --- Handle Clicks (Edit/Delete) ---
all_Products_table.addEventListener("click", async (e) => {
  e.preventDefault();
  const row = e.target.closest("tr");
  if (!row) return;

  const ProductID = row.dataset.id;

  if (e.target.classList.contains("edit-btn")) {
    await loadProductToForm(ProductID);
  } else if (e.target.classList.contains("delete-btn")) {
    await deleteAdmin(ProductID, row);
  }
});

// --- Load Admin Into Form for Editing ---
async function loadProductToForm(productId) {
  try {
    const res = await fetch(`http://localhost:5106/Product/get/${productId}`);
    if (!res.ok) throw new Error("Failed to fetch product");

    const product = await res.json();

    // Populate form fields
    formProduct.name.value = product.name; // maps to <input name="name">
    formProduct.subCategoryId.value = product.subCategoryId; // maps to <select name="subCategoryId">
    formProduct.brandId.value = product.brandId; // maps to <select name="brandId">

    // Save the id for editing later
    currentEditProductId = product.productId;

    // Switch UI state (assuming you have these elements defined)
    addBtn.style.display = "none";
    editBtn.style.display = "inline-block";
    pageState.textContent = `Edit Product ${product.productId}`;
  } catch (err) {
    console.error(err);
    alert("Error loading product: " + err.message);
  }
}

// --- Delete Admin ---
async function deleteAdmin(adminId, row) {
  if (!confirm("Are you sure you want to delete this Admin?")) return;

  try {
    const res = await fetch(`http://localhost:5106/Admin/delete/${adminId}`, {
      method: "DELETE",
    });
    if (!res.ok) throw new Error("Failed to delete Admin");
    row.remove();
    alert("Admin deleted successfully!");
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

// --- Edit Existing Admin ---
async function handleEditSubmit() {
  if (!currentEditAdminId) return alert("No product selected for edit");

  const data = getFormData();
  data.productId = currentEditProductId; // include id for update
  console.log(data);
  await sendProductData("http://localhost:5106/Product/update", data);

  // reset state
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
