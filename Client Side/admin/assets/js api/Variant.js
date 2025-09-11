"use strict";

// --- DOM Elements ---
const formVariant = document.getElementById("variantForm");
const addBtn = formVariant.querySelector('button[type="submit"]');
let editBtn;
let currentEditVariantId = null;

const allVariantsTable = document.querySelector(".all-variants");

// --- Init Edit Button ---
function initEditButton() {
  if (!document.querySelector("#edit-variant-btn")) {
    editBtn = document.createElement("button");
    editBtn.type = "button";
    editBtn.id = "edit-variant-btn";
    editBtn.textContent = "Update Variant";
    editBtn.classList.add("btn", "btn-warning");
    editBtn.style.float = "right";
    editBtn.style.display = "none";
    formVariant.appendChild(editBtn);
  } else {
    editBtn = document.querySelector("#edit-variant-btn");
  }

  editBtn.addEventListener("click", handleEditSubmit);
}
initEditButton();

// --- Fetch Products for Dropdown ---
async function fetchAllProductsForVariant() {
  try {
    const res = await fetch("http://localhost:5106/Product/getAll");
    if (!res.ok) throw new Error("Failed to fetch products");

    const products = await res.json();
    const select = document.getElementById("variantProductSelect");
    select.innerHTML = '<option value="">-- Select Product --</option>';
    products.forEach((p) => {
      const option = document.createElement("option");
      option.value = p.id ?? p.productId;
      option.textContent = p.name ?? p.productName;
      select.appendChild(option);
    });
  } catch (err) {
    console.error(err);
  }
}

// --- Fetch and Render Variants ---
async function fetchAllVariants() {
  try {
    const res = await fetch(
      "http://localhost:5106/Variant/GetAllWithPriceAndQuantity"
    );
    if (!res.ok) throw new Error("Failed to fetch variants");
    const variants = await res.json();

    allVariantsTable.innerHTML = "";
    variants.forEach((v) => {
      const row = document.createElement("tr");
      row.dataset.id = v.variantId;
      row.innerHTML = `
        <td>${v.variantId}</td>
        <td>${v.productName}</td>
        <td>${v.variantProperties}</td>
        <td>${v.imagePaths}</td>
        <td>${v.status ? "Active" : "Inactive"}</td>
        <td>${v.price ?? "-"}</td>
        <td>${v.quantity ?? "-"}</td>
        <td>
          <a href="#" class="btn btn-success edit-variant">Edit</a>
          <a href="#" class="btn btn-danger delete-variant">Delete</a>
        </td>
      `;
      allVariantsTable.appendChild(row);
    });
  } catch (err) {
    console.error(err);
  }
}

// --- Load Variant Into Form ---
async function loadVariantToForm(variantId) {
  try {
    const res = await fetch(
      `http://localhost:5106/Variant/getWithPriceAndQuantity/${variantId}`
    );
    if (!res.ok) throw new Error("Failed to fetch variant");
    const v = await res.json();

    document.getElementById("variantProductSelect").value = v.productId;
    document.getElementById("variantProperties").value = v.variantProperties;
    document.getElementById("imagePaths").value = v.imagePaths;
    document.getElementById("variantStatus").value = v.status;
    document.getElementById("price").value = v.price ?? "";
    document.getElementById("quantity").value = v.quantity ?? "";

    currentEditVariantId = v.variantId;
    addBtn.style.display = "none";
    editBtn.style.display = "inline-block";
  } catch (err) {
    console.error(err);
  }
}

// --- Send Variant Data ---
async function sendVariantData(url, body, method = "POST", isUpdate = false) {
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
    const variant = await res.json();

    // After variant is created/updated, sync stock
    await syncStockWithVariant(variant.variantId);

    alert("Operation successful!");
    fetchAllVariants();
  } catch (err) {
    console.error(err);
    alert("Error: " + err.message);
  }
}

// --- Sync Stock (Add/Update) ---
async function syncStockWithVariant(variantId) {
  const price = parseFloat(document.getElementById("price").value) || 0;
  const quantity = parseInt(document.getElementById("quantity").value) || 0;

  const stockData = {
    variantId,
    entranceQuantity: quantity,
    currentQuantity: quantity,
    entranceDate: new Date().toISOString(),
    expireDate: new Date(
      new Date().setFullYear(new Date().getFullYear() + 1)
    ).toISOString(),
    costPrice: price,
    sellPrice: price,
    supplierId: 1, // default supplier for demo
  };

  try {
    const res = await fetch("http://localhost:5106/Stock/add", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(stockData),
    });
    if (!res.ok) throw new Error("Failed to sync stock");
  } catch (err) {
    console.error("Stock sync failed:", err);
  }
}

// --- Handle Form Submit for Add ---
formVariant.addEventListener("submit", async (e) => {
  e.preventDefault();
  const data = {
    productId: parseInt(document.getElementById("variantProductSelect").value),
    variantProperties: document.getElementById("variantProperties").value,
    imagePaths: document.getElementById("imagePaths").value,
    status: document.getElementById("variantStatus").value === "true",
  };

  if (currentEditVariantId) {
    data.variantId = currentEditVariantId;
    await sendVariantData(
      "http://localhost:5106/Variant/update",
      data,
      "PUT",
      true
    );
  } else {
    await sendVariantData("http://localhost:5106/Variant/add", data, "POST");
  }

  formVariant.reset();
  currentEditVariantId = null;
  addBtn.style.display = "inline-block";
  editBtn.style.display = "none";
});

// --- Handle Edit Button Click ---
async function handleEditSubmit() {
  if (!currentEditVariantId) return alert("No variant selected for edit");
  const data = {
    variantId: currentEditVariantId,
    productId: parseInt(document.getElementById("variantProductSelect").value),
    variantProperties: document.getElementById("variantProperties").value,
    imagePaths: document.getElementById("imagePaths").value,
    status: document.getElementById("variantStatus").value === "true",
  };
  await sendVariantData(
    "http://localhost:5106/Variant/update",
    data,
    "PUT",
    true
  );
  formVariant.reset();
  currentEditVariantId = null;
  addBtn.style.display = "inline-block";
  editBtn.style.display = "none";
}

// --- Handle Edit/Delete in Table ---
allVariantsTable.addEventListener("click", async (e) => {
  e.preventDefault();
  const row = e.target.closest("tr");
  if (!row) return;
  const id = row.dataset.id;

  if (e.target.classList.contains("delete-variant")) {
    if (!confirm("Are you sure?")) return;
    try {
      const res = await fetch(`http://localhost:5106/Variant/delete/${id}`, {
        method: "DELETE",
      });
      if (!res.ok) throw new Error("Failed to delete variant");
      row.remove();
      alert("Variant deleted!");
    } catch (err) {
      console.error(err);
      alert("Error: " + err.message);
    }
  } else if (e.target.classList.contains("edit-variant")) {
    await loadVariantToForm(id);
  }
});

// --- Initialize ---
document.addEventListener("DOMContentLoaded", () => {
  fetchAllProductsForVariant();
  fetchAllVariants();
});
