const allOrdersTable = document.querySelector(".all-Orders");

// Fetch all orders and render table
async function getAllOrders() {
  try {
    const res = await fetch("http://localhost:5106/Order/getAll");
    if (!res.ok) throw new Error(`HTTP error! status: ${res.status}`);
    const orders = await res.json();
    renderOrdersTable(orders);
  } catch (err) {
    console.error(err);
    allOrdersTable.innerHTML = `<tr><td colspan="9">Failed to load orders</td></tr>`;
  }
}

// Render orders into table
function renderOrdersTable(orders) {
  allOrdersTable.innerHTML = "";
  orders.forEach((order) => {
    const row = document.createElement("tr");
    row.dataset.id = order.orderId;
    row.innerHTML = `
      <td>${order.orderId}</td>
      <td>${order.userId}</td>
      <td>${order.userName}</td>
      <td>${new Date(order.orderDate).toLocaleString()}</td>
      <td>${order.totalAmount.toFixed(2)}</td>
      <td>${order.paymentMethod}</td>
      <td>${order.cartId}</td>
      <td class="order-status">${order.status}</td>
      <td>
        <button class="btn btn-warning change-status-btn">Change Status</button>
      </td>
    `;
    allOrdersTable.appendChild(row);
  });
}

// Handle click events for changing status
allOrdersTable.addEventListener("click", async (e) => {
  if (!e.target.classList.contains("change-status-btn")) return;

  const row = e.target.closest("tr");
  const orderId = row.dataset.id;
  const currentStatusCell = row.querySelector(".order-status");
  const newStatus =
    "Delivered" === currentStatusCell.textContent ? "Pending" : "Delivered";

  if (!newStatus) return;

  try {
    await changeOrderStatus(orderId, newStatus);
    currentStatusCell.textContent = newStatus;
    alert("Order status updated!");
  } catch (err) {
    console.error(err);
    alert("Failed to update order status: " + err.message);
  }
});

// Send status update to API
async function changeOrderStatus(orderId, status) {
  const body = { orderId: parseInt(orderId), status };
  const res = await fetch("http://localhost:5106/Order/ChangeOrderStatus", {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(body),
  });

  if (!res.ok) {
    const errorData = await res.json().catch(() => null);
    throw new Error(errorData?.message || res.statusText);
  }

  return await res.json();
}

// Initialize
getAllOrders();
