// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.




/*hamburger menu js------------*/

document.addEventListener('DOMContentLoaded', () => {
    const trigger = document.querySelector('#portal-toggle');
    const wrapper = document.querySelector('.portal-wrapper');

    if (trigger && wrapper) {
        trigger.addEventListener('click', () => {
            // Toggles the hamburger icon state
            trigger.classList.toggle('is-open');
            // Toggles the visibility of the menu overlay
            wrapper.classList.toggle('is-visible');
        });

        // Close menu if a user clicks a link (helpful for one-page sites)
        wrapper.querySelectorAll('a').forEach(item => {
            item.addEventListener('click', () => {
                trigger.classList.remove('is-open');
                wrapper.classList.remove('is-visible');
            });
        });
    }
});


/*new hamburger nav*/

document.addEventListener('DOMContentLoaded', () => {
    const hamburger = document.getElementById('navbar-hamburger');
    const menu = document.getElementById('navbar-menu');

    // Selecting both regular links and the button
    const allLinks = document.querySelectorAll('.seethronavbar-link, .seethronavbar-btn');

    // Safety check to ensure elements exist
    if (hamburger && menu) {
        // Toggle mobile menu
        hamburger.addEventListener('click', (e) => {
            e.stopPropagation(); // Prevents click from bubbling
            hamburger.classList.toggle('is-active');
            menu.classList.toggle('is-active');
        });

        // Close menu when clicking any link
        allLinks.forEach(link => {
            link.addEventListener('click', () => {
                hamburger.classList.remove('is-active');
                menu.classList.remove('is-active');
            });
        });

        // Optional: Close menu if clicking outside the navbar
        document.addEventListener('click', (e) => {
            if (!menu.contains(e.target) && !hamburger.contains(e.target)) {
                hamburger.classList.remove('is-active');
                menu.classList.remove('is-active');
            }
        });
    }
});








/*email checker-------------------------*/

/*
document.querySelector(".eventpost-form").addEventListener("submit", async function (e) {
    e.preventDefault(); // stop normal submit

    const email = document.getElementById("emailInput").value;

    // Send email to server
    const response = await fetch("/YourController/CheckEmail", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "RequestVerificationToken": document.querySelector("input[name='__RequestVerificationToken']").value
        },
        body: JSON.stringify({ email: email })
    });

    const result = await response.json();

    if (!result.exists) {
        alert("The email you entered does not exist.");
        return; // stop here, do NOT submit form
    }

    // Email exists → submit form normally
    e.target.submit();
});
*/








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
                const response = await fetch('/Admin/SubmitEvent', {
                    method: 'POST',
                    body: formData
                });

                if (response.ok) {
                    alert("Event created successfully!");
                    window.location.href = "/Admin/AdminPage"; // Redirect on success
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

function clearImage(button) {
    const parentCard = button.closest('.upload-card');
    parentCard.querySelector('.file-input-hidden').value = "";
    parentCard.querySelector('.preview-container').classList.add('preview-hidden');
}





