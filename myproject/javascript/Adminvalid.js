document.addEventListener("DOMContentLoaded", function () {
    var adminForm = document.getElementById("adminForm");
    var usernameField = document.getElementById("Username");
    var passwordField = document.getElementById("Password");
    var confirmPasswordField = document.getElementById("ConfirmPassword");
    var emailField = document.getElementById("Email");

    adminForm.onsubmit = function (event) {
        if (!validateField(usernameField) || !validateField(passwordField) || !validateField(confirmPasswordField) || !validateField(emailField)) {
            event.preventDefault();
        }
    };

    function validateField(field) {
        var value = field.value;
        var errors = [];
        var errorContainer = document.getElementById(field.id + "Error"); // Get the error container element

        if (field.id === "Username") {
            if (value.trim() === "") {
                errors.push("Admin Name is required.");
            }
        } else if (field.id === "Password") {
            if (value.trim() === "") {
                errors.push("Admin Password is required.");
            } else if (value.length < 8) {
                errors.push("Password must be at least 8 characters.");
            } else if (!/^(?=.*[A-Za-z])(?=.*\d)/.test(value)) {
                errors.push("Password must include both a letter and a digit.");
            }
        } else if (field.id === "Email") {
            if (value.trim() === "") {
                errors.push("Admin Email is required.");
            } else if (!validateEmail(value)) {
                errors.push("Invalid Email Address.");
            }
        } else if (field.id === "ConfirmPassword") {
            var passwordValue = passwordField.value;
            if (value !== passwordValue) {
                errors.push("Confirm Password must match Password.");
            }
        }

        if (errors.length > 0) {
            // Display errors
            errorContainer.textContent = errors.join(", ");
            return false; // Return false to indicate validation failure
        } else {
            // Clear errors
            errorContainer.textContent = "";
            return true; // Return true to indicate validation success
        }
    }

    function validateEmail(email) {
        var emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
        return emailRegex.test(email);
    }
});
