document.addEventListener("DOMContentLoaded", function () {
    var psswdField = document.getElementById("pswd");
    var confirmPsswdField = document.getElementById("cnfpswd");
    var updateButton = document.getElementById("updtsbmt");

    updateButton.addEventListener("click", function (e) {
        clearValidationErrors();

        var pss = psswdField.value;
        var confirmPss = confirmPsswdField.value;

        if (!validatePasswordField(pss) || !validateConfirmPasswordField(pss, confirmPss)) {
            e.preventDefault();
        }
    });

    function validatePasswordField(pss) {
        var passwordPattern = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d!@@#$%^&*()-_+=?]{8,}$/;

        if (pss.trim() === "") {
            document.getElementById('pswdError').textContent = "Password is required.";
            return false;
        } else if (!passwordPattern.test(pss)) {
            document.getElementById('pswdError').textContent = "Password must be at least 8 characters long and contain both letters and digits.";
            return false;
        } else {
            document.getElementById('pswdError').textContent = "";
            return true;
        }
    }

    function validateConfirmPasswordField(pss, confirmPss) {
        if (confirmPss.trim() === "") {
            document.getElementById('cnfpswdError').textContent = "Confirm Password is required.";
            return false;
        } else if (pss !== confirmPss) {
            document.getElementById('cnfpswdError').textContent = "Password and Confirm Password do not match.";
            return false;
        } else {
            document.getElementById('cnfpswdError').textContent = "";
            return true;
        }
    }

    function clearValidationErrors() {
        document.getElementById('pswdError').textContent = "";
        document.getElementById('cnfpswdError').textContent = "";
    }
});
