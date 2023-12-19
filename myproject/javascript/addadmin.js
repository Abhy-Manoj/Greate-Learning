
document.addEventListener("DOMContentLoaded", function () {
    var UsernameField = document.getElementById("Username");
    var EmailField = document.getElementById("Email");
    var PasswordField = document.getElementById("Password");
    var ConfirmPasswordField = document.getElementById("ConfirmPassword");

    // Username field on blur
    UsernameField.onblur = function () {
        validateUsernameField(UsernameField, "UsernameError");
    };

    // Email field on blur
    EmailField.onblur = function () {
        validateEmailField(EmailField, "EmailError");
    };

    // Password field on blur
    PasswordField.onblur = function () {
        validatePasswordField(PasswordField, "PasswordError");
    };

    // ConfirmPassword field on blur
    ConfirmPasswordField.onblur = function () {
        validateConfirmPasswordField(ConfirmPasswordField, "ConfirmPasswordError", PasswordField);
    };

    // Form submit listener
    var adminForm = document.getElementById("adminForm");
    adminForm.addEventListener("submit", function (e) {
        // Prevent form submission if there are errors
        if (hasErrors()) {
            e.preventDefault();
        }
    });

    // Validate username field
    function validateUsernameField(field, errorContainerId) {
        var value = field.value;
        var errors = [];

        // Username validation
        if (value.trim() === "") {
            errors.push("Username is required.");
        } else if (/[^a-zA-Z]/.test(value)) {
            errors.push("Username should contain only letters.");
        }

        displayErrors(errors, errorContainerId);
    }

    // Validate email field
    function validateEmailField(field, errorContainerId) {
        var value = field.value;
        var errors = [];

        // Email validation
        if (value.trim() === "") {
            errors.push("Email is required.");
        } else if (!/^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/.test(value)) {
            errors.push("Invalid Email Address.");
        }

        displayErrors(errors, errorContainerId);
    }

    // Validate password field
    function validatePasswordField(field, errorContainerId) {
        var value = field.value;
        var errors = [];

        // Password validation
        if (value.trim() === "") {
            errors.push("Password is required.");
        } else if (!/^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d!@@#$%^&*()-_+=?]{8,}$/.test(value)) {
            errors.push("Password should be at least 8 characters long and contain both letters and digits.");
        }

        displayErrors(errors, errorContainerId);
    }

    // Validate ConfirmPassword field
    function validateConfirmPasswordField(field, errorContainerId, passwordField) {
        var value = field.value;
        var errors = [];

        // Confirm password validation
        if (value.trim() === "") {
            errors.push("Confirm password is required.");
        } else if (value !== passwordField.value) {
            errors.push("Confirm password should match the password.");
        }

        displayErrors(errors, errorContainerId);
    }

    // Display errors in the error container
    function displayErrors(errors, errorContainerId) {
        var errorContainer = document.getElementById(errorContainerId);
        errorContainer.textContent = errors.join(" ");
    }

    // Check if there are errors in any field
    function hasErrors() {
        return (
            document.getElementById("UsernameError").textContent !== "" ||
            document.getElementById("EmailError").textContent !== "" ||
            document.getElementById("PasswordError").textContent !== "" ||
            document.getElementById("ConfirmPasswordError").textContent !== ""
        );
    }
});
