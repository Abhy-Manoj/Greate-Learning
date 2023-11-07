function validateUsername() {
    var username = document.getElementById('Username').value;
    var usernameError = document.getElementById('username-error');

    // Check username
    if (username.trim() === "") {
        usernameError.innerHTML = "Username is required.";
    } else {
        usernameError.innerHTML = "";
    }
}

function validatePassword() {
    var password = document.getElementById('Password').value;
    var passwordError = document.getElementById('password-error');

    // Check password
    if (password.trim() === "") {
        passwordError.innerHTML = "Password is required.";
    } else {
        passwordError.innerHTML = "";
    }
}
