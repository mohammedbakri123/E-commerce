// const all_user_table = document.querySelector(".all-users");
// const formUser = document.querySelector(".form-user");

// function CreateRow(user) {
//   return `
//     <tr class="odd gradeX" data-id="${user.id}">
//       <td>${user.id}</td>
//       <td>${user.name}</td>
//       <td>${user.email}</td>
//       <td>${user.status}</td>
//       <td>${user.role}</td>
//       <td>
//         <a href="#" class="btn btn-success edit-btn">Edit</a>
//         <a href="#" class="btn btn-danger delete-btn">Delete</a>
//       </td>
//     </tr>`;
// }

// formUser.addEventListener("submit", async (e) => {
//   e.preventDefault();

//   // جمع البيانات من الفورم
//   const formData = new FormData(formUser);
//   const data = Object.fromEntries(formData.entries());

//   // التأكد من تطابق كلمة المرور والتأكيد
//   if (data.password !== data.comfirmPassword) {
//     alert("Password and Confirm Password do not match!");
//     return;
//   }

//   // إنشاء JSON body للإرسال
//   const body = {
//     firstName: data.firstName,
//     email: data.email,
//     password: data.password,
//     confirmPassword: data.comfirmPassword,
//   };

//   try {
//     const response = await fetch("http://localhost:5106/User/add", {
//       method: "POST",
//       headers: {
//         "Content-Type": "application/json",
//       },
//       body: JSON.stringify(body),
//     });

//     if (response.ok) {
//       const result = await response.json();
//       console.log("User added:", result);
//       alert("User added successfully!");
//       formUser.reset(); // إعادة تعيين الفورم
//     } else {
//       const errorMessage = await response.text();
//       console.error("Error adding user:", errorMessage);
//       alert("Failed to add user: " + (errorMessage || response.statusText));
//     }
//   } catch (err) {
//     console.error(err);
//     alert("Error: " + err.message);
//   }
// });

// async function getAllUser() {
//   try {
//     const response = await fetch("http://localhost:5106/User/all");
//     if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);

//     const data = await response.json();
//     all_user_table.innerHTML = "";

//     data.forEach((user) => {
//       all_user_table.insertAdjacentHTML("beforeend", CreateRow(user));
//     });
//   } catch (error) {
//     console.error("Error fetching users:", error.message);
//     all_user_table.innerHTML = `<tr><td colspan="6">Failed to load users</td></tr>`;
//   }
// }

// getAllUser();

// all_user_table.addEventListener("click", async (e) => {
//   e.preventDefault();
//   const row = e.target.closest("tr");
//   const userId = row.dataset.id;
//   console.log(userId);

//   if (e.target.classList.contains("edit-btn")) {
//     // Example: redirect to edit page
//     // window.location.href = `/edit-user.html?id=${userId}`;
//   } else if (e.target.classList.contains("delete-btn")) {
//     if (confirm("Are you sure you want to delete this user?")) {
//       try {
//         const res = await fetch(`http://localhost:5106/User/${userId}`, {
//           method: "DELETE",
//         });
//         if (res.ok) row.remove();
//         else alert("Failed to delete user");
//       } catch (err) {
//         console.error(err);
//       }
//     }
//   }
// });

const pageState = document.querySelector(".page-state");
const formUser = document.querySelector("form");
const addBtn = formUser.querySelector('button[type="submit"]');
let editBtn;
let currentEditUserId = null;
const all_user_table = document.querySelector(".all-users");

// Initialize Edit Button
function initEditButton() {
  if (!document.querySelector("#edit-btn")) {
    editBtn = document.createElement("button");
    editBtn.type = "button";
    editBtn.id = "edit-btn";
    editBtn.textContent = "Edit User";
    editBtn.classList.add("btn", "btn-warning");
    editBtn.style.display = "none";
    formUser.appendChild(editBtn);
  } else {
    editBtn = document.querySelector("#edit-btn");
  }

  editBtn.addEventListener("click", handleEditSubmit);
}

// Fetch all users and populate table
async function getAllUser() {
  try {
    const response = await fetch("http://localhost:5106/User/all");
    if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
    const data = await response.json();
    renderUserTable(data);
  } catch (err) {
    console.error(err);
    all_user_table.innerHTML = `<tr><td colspan="6">Failed to load users</td></tr>`;
  }
}

// Render table rows
function renderUserTable(users) {
  all_user_table.innerHTML = "";
  users.forEach((user) => {
    const row = document.createElement("tr");
    row.dataset.id = user.id;
    row.innerHTML = `
      <td>${user.id}</td>
      <td>${user.name}</td>
      <td>${user.email}</td>
      <td>${user.status ? "Active" : "Inactive"}</td>
      <td>${user.role || ""}</td>
      <td>
        <a href="#" class="btn btn-success edit-btn">Edit</a>
        <a href="#" class="btn btn-danger delete-btn">Delete</a>
        <a href="#" class="btn btn-warning active-btn">Active</a>
      </td>
    `;
    all_user_table.appendChild(row);
  });
}

// Handle Add User
formUser.addEventListener("submit", handleAddSubmit);

async function handleAddSubmit(e) {
  e.preventDefault();
  const data = getFormData();

  if (!validatePasswords(data.password, data.comfirmPassword)) return;

  const body = {
    firstName: data.firstName,
    email: data.email,
    password: data.password,
    confirmPassword: data.comfirmPassword,
  };

  await sendUserData("http://localhost:5106/User/add", body);
  formUser.reset();
  getAllUser();
}

// Handle Edit button click in table
all_user_table.addEventListener("click", async (e) => {
  e.preventDefault();
  const row = e.target.closest("tr");
  const userId = row.dataset.id;
  pageState.innerHTML = `edit user ${userId}`;
  console.log(pageState);

  if (e.target.classList.contains("edit-btn")) {
    await loadUserToForm(userId);
  } else if (e.target.classList.contains("delete-btn")) {
    await deleteUser(userId, row);
  } else if (e.target.classList.contains("active-btn")) {
    await activateUser(userId, row);
  }
});

// Get form data as object
function getFormData() {
  const formData = new FormData(formUser);
  return Object.fromEntries(formData.entries());
}

// Validate passwords match
function validatePasswords(password, confirmPassword) {
  if (password !== confirmPassword) {
    alert("Password and Confirm Password do not match!");
    return false;
  }
  return true;
}

// Send data to API
async function sendUserData(url, body) {
  try {
    const res = await fetch(url, {
      method: "POST",
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

// Load user data to form for editing
async function loadUserToForm(userId) {
  try {
    const res = await fetch(`http://localhost:5106/User/${userId}`);
    if (!res.ok) throw new Error("Failed to fetch user data");
    const user = await res.json();

    formUser.firstName.value = user.name || "";
    formUser.email.value = user.email || "";
    currentEditUserId = userId;

    addBtn.style.display = "none";
    editBtn.style.display = "inline-block";
  } catch (err) {
    console.error(err);
    alert("Failed to load user: " + err.message);
  }
}

// Handle Edit Form submission
async function handleEditSubmit() {
  if (!currentEditUserId) return;

  const body = {
    userId: parseInt(currentEditUserId),
    firstName: formUser.firstName.value,
    email: formUser.email.value,
    status: true, // or get from input
  };

  try {
    const res = await fetch("http://localhost:5106/User/update", {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(body),
    });

    if (!res.ok) {
      const errorData = await res.text().catch(() => null);
      throw new Error(errorData?.message || res.statusText);
    }

    alert("User updated successfully!");
    formUser.reset();
    currentEditUserId = null;
    addBtn.style.display = "inline-block";
    editBtn.style.display = "none";
    pageState.innerHTML = `add new user`;

    getAllUser();
  } catch (err) {
    console.error(err);
    alert("Error: " + err.message);
  }
}

// Delete user
async function deleteUser(userId, row) {
  if (!confirm("Are you sure you want to delete this user?")) return;

  try {
    const res = await fetch(`http://localhost:5106/User/${userId}`, {
      method: "DELETE",
    });
    if (!res.ok) throw new Error("Failed to delete user");
    row.remove();
    alert("User deleted successfully!");
  } catch (err) {
    console.error(err);
    alert("Error: " + err.message);
  }
}

// Activate user
async function activateUser(userId, row) {
  try {
    const res = await fetch(`http://localhost:5106/User/activate/${userId}`, {
      method: "POST",
    });
    if (!res.ok) throw new Error("Failed to activate user");
    row.querySelector("td:nth-child(4)").textContent = "Active";
    alert("User activated!");
  } catch (err) {
    console.error(err);
    alert("Error: " + err.message);
  }
}

// Initialize
initEditButton();
getAllUser();
