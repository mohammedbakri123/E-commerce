// const pageState = document.querySelector(".page-state");
// const formadmin = document.querySelector("form");
// const addBtn = formadmin.querySelector('button[type="submit"]');
// let editBtn;
// let currentEditAdminId = null;
// const all_Admins_table = document.querySelector(".all-Admins");

// function initEditButton() {
//   if (!document.querySelector("#edit-btn")) {
//     editBtn = document.createElement("button");
//     editBtn.type = "button";
//     editBtn.id = "edit-btn";
//     editBtn.textContent = "Edit Admin";
//     editBtn.classList.add("btn", "btn-warning");
//     editBtn.style.float = "right";
//     editBtn.style.display = "none";
//     formUser.appendChild(editBtn);
//   } else {
//     editBtn = document.querySelector("#edit-btn");
//   }

//   editBtn.addEventListener("click", handleEditSubmit);
// }

// async function getAllAdmins() {
//   try {
//     const response = await fetch("http://localhost:5106/Admin/getAll");
//     if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
//     const data = await response.json();
//     renderAdminTable(data);
//     console.log(data);
//   } catch (err) {
//     console.error(err);
//     all_Admins_table.innerHTML = `<tr><td colspan="6">Failed to load users</td></tr>`;
//   }
// }
// getAllAdmins();
// function renderAdminTable(admins) {
//   all_Admins_table.innerHTML = "";
//   admins.forEach((admin) => {
//     const row = document.createElement("tr");
//     row.dataset.id = admin.adminId;
//     row.innerHTML = `
//                 <td>${admin.userId}</td>
//                 <td>${admin.adminId}</td>
//                 <td>${admin.userName}</td>
//                 <td>${admin.adminLastName}</td>
//                 <td>${admin.phoneNumber}</td>
//                 <td>${admin.address}</td>
//                  <td>
//                     <a href="#" class="btn btn-success edit-btn">Edit</a>
//                     <a href="#" class="btn btn-danger delete-btn">Delete</a>
//                 </td>
//     `;
//     all_Admins_table.appendChild(row);
//   });
// }

// all_Admins_table.addEventListener("click", async (e) => {
//   e.preventDefault();
//   const row = e.target.closest("tr");
//   const AdminID = row.dataset.id;
//   console.log(row.dataset.id);
//   pageState.innerHTML = `edit user ${AdminID}`;
//   console.log(e.target);

//   if (e.target.classList.contains("edit-btn")) {
//     await loadUserToForm(AdminID);
//   } else if (e.target.classList.contains("delete-btn")) {
//     alert("ksdfkjs");
//     await deleteAdmin(AdminID, row);
//   }
//   // else if (e.target.classList.contains("active-btn")) {
//   //   await activateUser(userId, row);
//   // }
// });

// async function deleteAdmin(AdminID, row) {
//   if (!confirm("Are you sure you want to delete this Admin?")) return;

//   try {
//     const res = await fetch(`http://localhost:5106/Admin/delete/${AdminID}`, {
//       method: "DELETE",
//     });
//     if (!res.ok) throw new Error("Failed to delete Admin");
//     row.remove();
//     alert("Admin deleted successfully!");
//   } catch (err) {
//     console.error(err);
//     alert("Error: " + err.message);
//   }
// }

// formadmin.addEventListener("submit", handleAddSubmit);

// function getFormData() {
//   const formData = new FormData(formadmin);
//   return Object.fromEntries(formData.entries());
// }

// async function handleAddSubmit(e) {
//   e.preventDefault();
//   const data = getFormData();
//   console.log(data);

//   await sendAdminData("http://localhost:5106/Admin/add", data);
//   formadmin.reset();
//   getAllAdmins();
// }
// async function sendAdminData(url, body) {
//   try {
//     const res = await fetch(url, {
//       method: "POST",
//       headers: { "Content-Type": "application/json" },
//       body: JSON.stringify(body),
//     });

//     if (!res.ok) {
//       const errorData = await res.json().catch(() => null);
//       throw new Error(errorData?.message || res.statusText);
//     }

//     const result = await res.json();
//     alert("Operation successful!");
//     return result;
//   } catch (err) {
//     console.error(err);
//     alert("Error: " + err.message);
//   }
// }

const pageState = document.querySelector(".page-state");
const formadmin = document.querySelector("form");
const addBtn = formadmin.querySelector('button[type="submit"]');
let editBtn;
let currentEditAdminId = null;
const all_Admins_table = document.querySelector(".all-Admins");

// --- Init Edit Button ---
function initEditButton() {
  if (!document.querySelector("#edit-btn")) {
    editBtn = document.createElement("button");
    editBtn.type = "button";
    editBtn.id = "edit-btn";
    editBtn.textContent = "Edit Admin";
    editBtn.classList.add("btn", "btn-warning");
    editBtn.style.float = "right";
    editBtn.style.display = "none"; // hidden until edit
    formadmin.appendChild(editBtn);
  } else {
    editBtn = document.querySelector("#edit-btn");
  }

  editBtn.addEventListener("click", handleEditSubmit);
}
initEditButton();

// --- Fetch All Admins ---
async function getAllAdmins() {
  try {
    const response = await fetch("http://localhost:5106/Admin/getAll");
    if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
    const data = await response.json();
    renderAdminTable(data);
  } catch (err) {
    console.error(err);
    all_Admins_table.innerHTML = `<tr><td colspan="7">Failed to load admins</td></tr>`;
  }
}
getAllAdmins();

// --- Render Admins ---
function renderAdminTable(admins) {
  all_Admins_table.innerHTML = "";
  admins.forEach((admin) => {
    const row = document.createElement("tr");
    row.dataset.id = admin.adminId;
    row.innerHTML = `
      <td>${admin.userId}</td>
      <td>${admin.adminId}</td>
      <td>${admin.userName || ""}</td>
      <td>${admin.adminLastName}</td>
      <td>${admin.phoneNumber}</td>
      <td>${admin.address}</td>
      <td>
        <a href="#" class="btn btn-success edit-btn">Edit</a>
        <a href="#" class="btn btn-danger delete-btn">Delete</a>
      </td>
    `;
    all_Admins_table.appendChild(row);
  });
}

// --- Handle Clicks (Edit/Delete) ---
all_Admins_table.addEventListener("click", async (e) => {
  e.preventDefault();
  const row = e.target.closest("tr");
  if (!row) return;

  const adminId = row.dataset.id;

  if (e.target.classList.contains("edit-btn")) {
    await loadAdminToForm(adminId);
  } else if (e.target.classList.contains("delete-btn")) {
    await deleteAdmin(adminId, row);
  }
});

// --- Load Admin Into Form for Editing ---
async function loadAdminToForm(adminId) {
  try {
    const res = await fetch(`http://localhost:5106/Admin/get/${adminId}`);
    if (!res.ok) throw new Error("Failed to fetch admin");
    const admin = await res.json();

    // populate form
    formadmin.userid.value = admin.userId;
    formadmin.adminLastName.value = admin.adminLastName;
    formadmin.phoneNumber.value = admin.phoneNumber;
    formadmin.address.value = admin.address;

    currentEditAdminId = admin.adminId;

    // switch UI state
    addBtn.style.display = "none";
    editBtn.style.display = "inline-block";
    pageState.textContent = `Edit Admin ${adminId}`;
  } catch (err) {
    console.error(err);
    alert("Error loading admin: " + err.message);
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
  const formData = new FormData(formadmin);
  return Object.fromEntries(formData.entries());
}

// --- Add New Admin ---
formadmin.addEventListener("submit", handleAddSubmit);

async function handleAddSubmit(e) {
  e.preventDefault();
  const data = getFormData();

  await sendAdminData("http://localhost:5106/Admin/add", data);
  formadmin.reset();
  getAllAdmins();
}

// --- Edit Existing Admin ---
async function handleEditSubmit() {
  if (!currentEditAdminId) return alert("No admin selected for edit");

  const data = getFormData();
  data.adminId = currentEditAdminId; // include id for update
  delete data.userid;
  console.log(data);
  await sendAdminData("http://localhost:5106/Admin/update", data, "PUT");

  // reset state
  formadmin.reset();
  currentEditAdminId = null;
  addBtn.style.display = "inline-block";
  editBtn.style.display = "none";
  pageState.textContent = "Add New Admin";
  getAllAdmins();
}

// --- Send Data Helper ---
async function sendAdminData(url, body, Method = "POST") {
  try {
    const res = await fetch(url, {
      method: Method,
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
