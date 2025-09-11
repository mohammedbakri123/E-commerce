"use strict";

const btnSignUp = document.querySelector(".btn-sign_up");
const btnSignIn = document.querySelector(".btn-sign_in");
const loginRight = document.querySelector(".login-right");
const imgBack = document.querySelector(".img-back");
const loginForm = document.querySelector(".login-form");

btnSignIn.addEventListener("click", function () {
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
});

btnSignUp.addEventListener("click", function () {
  loginForm.innerHTML = `
          <div class="field">
            <img src="../assets/image/user-1.png" class="icon" alt="" />
            <input type="text" placeholder="Username" name="username"/>
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
});

loginForm.addEventListener("submit", function (e) {
  e.preventDefault();

  const formData = new FormData(loginForm);
  const data = Object.fromEntries(formData.entries());

  console.log(data);
});
