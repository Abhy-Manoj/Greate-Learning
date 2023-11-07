
document.addEventListener("DOMContentLoaded", function () {
    var courseNameField = document.getElementById("CourseName");
    var descriptionField = document.getElementById("Description");
    var durationField = document.getElementById("Duration");
    var ImageUrlField = document.getElementById("ImageUrl");
    var VideoUrlField = document.getElementById("VideoUrl");

    courseNameField.onblur = function () {
        validateField(courseNameField);
    };

    descriptionField.onblur = function () {
        validateField(descriptionField);
    };

    durationField.onblur = function () {
        validateField(durationField);
    };

    ImageUrlField.onblur = function () {
        validateField(ImageUrlField);
    };

    VideoUrlField.onblur = function () {
        validateField(VideoUrlField);
    };

    function validateField(field) {
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
        } else if (field.id === "ImageUrl") {
            if (value.trim() === "") {
                errors.push("Image URL is required.");
            }
        }
        else if (field.id === "VideoUrl") {
            if (value.trim() === "") {
                errors.push("Video URL is required.");
            }
        }

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
});
