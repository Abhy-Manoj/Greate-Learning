//Check for input from the user side
document.addEventListener("DOMContentLoaded", function () {
    var usernameField = document.getElementById("Username");
    var passwordField = document.getElementById("Password");
    var confirmPasswordField = document.getElementById("ConfirmPassword");
    var emailField = document.getElementById("Email");

    usernameField.onblur = function () {
        validateField(usernameField);
    };

    passwordField.onblur = function () {
        validateField(passwordField);
    };

    confirmPasswordField.onblur = function () {
        validateField(confirmPasswordField);
    };

    emailField.onblur = function () {
        validateField(emailField);
    };

    //Fields Validations
    function validateField(field) {
        var value = field.value;
        var errors = [];

        //trim the empty spaces in between
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
        } else if (field.id === "ConfirmPassword") {
            if (value.trim() === "") {
                errors.push("Confirm Password is required.");
            }
        } else if (field.id === "Email") {
            if (value.trim() === "") {
                errors.push("Admin Email is required.");
            } else if (!validateEmail(value)) {
                errors.push("Invalid Email Address.");
            }
        }

        // Validate Confirm Password with Password
        if (field.id === "ConfirmPassword") {
            var passwordValue = passwordField.value;
            if (value !== passwordValue) {
                errors.push("Confirm Password must match Password.");
            }
        }

        //Error validations
        var errorList = document.getElementById("validationErrors");
        errorList.innerHTML = "";

        if (errors.length > 0) {
            errors.forEach(function (error) {
                var li = document.createElement("li");
                li.textContent = error;
                errorList.appendChild(li);
            });
        }
    }

    //Email validation
    function validateEmail(email) {
        var emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
        return emailRegex.test(email);
    }
});
