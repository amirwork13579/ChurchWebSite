/*image file device open*/


function openFilePicker(element) {
    // 1. Find the hidden file input
    // If your input is inside the same container, use 'closest'
    const parent = element.closest('.upload-card');
    const fileInput = parent.querySelector('.file-input-hidden');

    // 2. Programmatically click it
    //opens user device files
    fileInput.click();
}



function handleFileSelected(input) {
    if (input.files && input.files[0]) {
        const reader = new FileReader();

        reader.onload = function (e) {
            // e.target.result is the base64 string of the image
            console.log("Image data ready!");
            // You would typically set an <img> src to e.target.result here
        };

        reader.readAsDataURL(input.files[0]);
    }
}



function openDeviceFile(element)
{   
    const photo = element.files[0];

    const parent = element.closest('.av');
    const imgshower = parent.querySelector("#image-select-pressent");

    const imgfinal = URL.createObjectURL(photo);
    imgshower.src = imgfinal;
}






/*device open image upload*/

document.addEventListener("DOMContentLoaded", function () {
    // 1. Find the form safely
    const eventForm = document.querySelector(".eventpost-form");

    if (eventForm) {
        eventForm.addEventListener("submit", async function (e) {
            e.preventDefault(); // Stop the standard form reload

            // 2. Locate the file input and other data
            const fileInput = eventForm.querySelector(".file-input-hidden");
            const titleInput = document.getElementById("event-title");
            // Add other inputs here as needed (e.g., Description, Date)

            // 3. Prepare the FormData object
            const formData = new FormData();

            // IMPORTANT: "imageFile" must match your C# parameter name exactly
            if (fileInput && fileInput.files[0]) {
                formData.append("imageFile", fileInput.files[0]);
            } else {
                alert("Please select an image first!");
                return;
            }

            // 4. Map the ViewModel data
            // We use 'event1.NewEvent.' to match your C# EventsDataViewModel
            if (titleInput) {
                formData.append("event1.NewEvent.Title", titleInput.value);
            }

            // You can loop through other inputs or append them manually:
            // formData.append("event1.NewEvent.Description", document.getElementById("desc").value);

            try {
                // 5. Send the request
                const response = await fetch('/AdminFile/SubmitEvent', {
                    method: 'POST',
                    body: formData
                });

                if (response.ok) {
                    alert("Event created successfully!");
                    window.location.href = "/AdminFile/AdminPage"; // Redirect on success
                } else {
                    const errorMsg = await response.text();
                    console.error("Server Error:", errorMsg);
                    alert("Error: " + errorMsg);
                }
            } catch (error) {
                console.error("Fetch Error:", error);
                alert("An error occurred while uploading.");
            }
        });
    }
});

/* --- Keep your previous helper functions for the UI --- */
function triggerClick(element) {
    const parentCard = element.closest('.upload-card');
    parentCard.querySelector('.file-input-hidden').click();
}

function handleFileChange(input) {
    const parentCard = input.closest('.upload-card');
    const previewContainer = parentCard.querySelector('.preview-container');
    const previewImage = parentCard.querySelector('.image-preview-element');
    const file = input.files[0];

    if (file) {
        const reader = new FileReader();
        reader.onload = (e) => {
            previewImage.src = e.target.result;
            previewContainer.classList.remove('preview-hidden');
        };
        reader.readAsDataURL(file);
    }
}


/*clears the selected image*/
function clearImage(button) {
    console.log("Remove button clicked");

    // 1. Find the container
    const parentCard = button.closest('.upload-card');

    if (!parentCard) {
        console.error("CRITICAL: Could not find .upload-card. Check your HTML nesting!");
        return;
    }

    // 2. Clear the file input
    const fileInput = parentCard.querySelector('.file-input-hidden');
    if (fileInput) {
        fileInput.value = "";
        console.log("File input cleared");
    }

    // 3. Reset the Image Preview
    const img = parentCard.querySelector('.image-preview-element');
    if (img) {
        img.src = "#";
    }


}