document.addEventListener("DOMContentLoaded", function () {
    var courseNameField = document.getElementById("CourseName");
    var descriptionField = document.getElementById("Description");
    var durationField = document.getElementById("Duration");
    var countField = document.getElementById("Count");
    var ImageUrlField = document.getElementById("ImageFile");
    var VideoUrlField = document.getElementById("VideoFile");

    //Course name
    courseNameField.onblur = function () {
        validateField(courseNameField, "courseNameError");
    };

    //Description
    descriptionField.onblur = function () {
        validateField(descriptionField, "descriptionError");
    };

    //Duration
    durationField.onblur = function () {
        validateField(durationField, "durationError");
    };

    //Count
    countField.onblur = function () {
        validateCountField(countField, "countError");
    };

    //Image
    ImageUrlField.onchange = function () {
        validateFileField(ImageUrlField, "imageFileError", ["image/*"]);
    };

    //Video
    VideoUrlField.onchange = function () {
        validateFileField(VideoUrlField, "videoFileError", ["video/*"]);
    };

    //Validate fields
    function validateField(field, errorContainerId) {
        var value = field.value;
        var errors = [];

        if (field.id === "CourseName") {
            if (value.trim() === "") {
                errors.push("Course Name is required.");
            }
        } else if (field.id === "Description") {
            if (value.trim() === "") {
                errors.push("Description is required.");
            }
        } else if (field.id === "Duration") {
            if (value.trim() === "") {
                errors.push("Duration is required.");
            }
        }

        var errorContainer = document.getElementById(errorContainerId);
        errorContainer.innerHTML = "";

        if (errors.length > 0) {
            errors.forEach(function (error) {
                var li = document.createElement("li");
                li.textContent = error;
                errorContainer.appendChild(li);
            });
        }
    }

    //Count validation
    function validateCountField(field, errorContainerId) {
        var value = field.value;
        var errors = [];

        if (field.id === "Count") {
            if (value.trim() === "") {
                errors.push("Enrollment count is required.");
            } else if (!/^\d+$/.test(value)) {
                errors.push("Enrollment count must be a valid number.");
            }
        }

        var errorContainer = document.getElementById(errorContainerId);
        errorContainer.innerHTML = "";

        if (errors.length > 0) {
            errors.forEach(function (error) {
                var li = document.createElement("li");
                li.textContent = error;
                errorContainer.appendChild(li);
            });
        }
    }

    //File validation
    function validateFileField(field, errorContainerId, allowedExtensions) {
        var file = field.files[0];
        var errors = [];

        if (!file) {
            errors.push("File is required.");
        } else {
            var isValidExtension = false;

            for (var i = 0; i < allowedExtensions.length; i++) {
                if (file.type.match(allowedExtensions[i])) {
                    isValidExtension = true;
                    break;
                }
            }

            if (!isValidExtension) {
                errors.push("Invalid file format. Please select an image or video file.");
            }
        }

        var errorContainer = document.getElementById(errorContainerId);
        errorContainer.innerHTML = "";

        if (errors.length > 0) {
            errors.forEach(function (error) {
                var li = document.createElement("li");
                li.textContent = error;
                errorContainer.appendChild(li);
            });
        }
    }
});
