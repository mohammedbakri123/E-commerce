"use strict";

localStorage.clear();

const btnSignUp = document.querySelector(".btn-sign_up");
const btnSignIn = document.querySelector(".btn-sign_in");
const loginRight = document.querySelector(".login-right");
const imgBack = document.querySelector(".img-back");
const loginForm = document.querySelector(".login-form");

function sign_in() {
  loginForm.innerHTML = `

          <div class="field">
            <img src="../assets/image/business.png" class="icon" alt="" />
            <input type="text" placeholder="Email" name="email"/>
          </div>
          <div class="field">
            <img src="../assets/image/padlock.png" class="icon" alt="" />
            <input type="password" placeholder="Password" name="password"/>
          </div>
          <button class="btn btn-submit">sign in</button>
        `;

  btnSignIn.style.color = "black";
  btnSignUp.style.color = "white";

  imgBack.style.top = "42%";
}

function sign_up() {
  loginForm.innerHTML = `
          <div class="field">
            <img src="../assets/image/user-1.png" class="icon" alt="" />
            <input type="text" placeholder="Username" name="name"/>
          </div>
          <div class="field">
            <img src="../assets/image/business.png" class="icon" alt="" />
            <input type="text" placeholder="Email" name="email"/>
          </div>
          <div class="field">
            <img src="../assets/image/padlock.png" class="icon" alt="" />
            <input type="password" placeholder="Password" name="password"/>
          </div>
          <div class="field">
            <img src="../assets/image/padlock.png" class="icon" alt="" />
            <input type="password" placeholder="Confirm Password" name="confirmPassword"/>
          </div>
          <button class="btn btn-submit">sign up</button>
        `;

  btnSignIn.style.color = "white";
  btnSignUp.style.color = "black";

  imgBack.style.top = "-15%";
}

async function fetchLoginOrRegister(url, data) {
  try {
    const res = await fetch(url, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(data),
    });

    if (!res.ok) {
      const error = await res.text();
      throw new Error(`Server error: ${error}`);
    }

    const dataRes = await res.json();
    console.log("Login response:", dataRes);

    if (dataRes.token) {
      localStorage.clear();
      localStorage.setItem("userData", JSON.stringify(dataRes));
      alert("Login successful!");
      window.location.href = "index.html";
    } else {
      alert("Login failed: " + (dataRes.message || "Unknown error"));
    }
  } catch (error) {
    alert(`Error during login: ${error.message}`);
    console.log(error);
    console.error("Error during login:", error);
  }
}

btnSignIn.addEventListener("click", function () {
  sign_in();
});

btnSignUp.addEventListener("click", function () {
  sign_up();
});

loginForm.addEventListener("submit", async function (e) {
  e.preventDefault();

  const formData = new FormData(loginForm);
  const data = Object.fromEntries(formData.entries());

  if (Object.keys(data).length === 2) {
    await fetchLoginOrRegister("http://localhost:5106/Auth/login", data);
  }

  if (Object.keys(data).length === 4) {
    await fetchLoginOrRegister("http://localhost:5106/Auth/register", data);
  }
});
