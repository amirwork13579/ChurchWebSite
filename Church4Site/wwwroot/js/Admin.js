

/*delete popup button*/
function UserTOpenPopup(id, actionUrl, customMessage = "Are you sure you want to continue?") {
    // 1. Set the ID
    document.getElementById("UserTPopupId").value = id;

    // 2. Set the Form Action (e.g., /Admin/DeleteEvent or /Admin/DeleteTestimony)
    document.getElementById("UserTPopupForm").action = actionUrl;

    // 3. Optional: Change the message text
    document.getElementById("UserTPopupMessage").innerText = customMessage;

    // 4. Show the popup
    document.getElementById("UserTPopup").style.display = "flex";
}

function UserTClosePopup() {
    document.getElementById("UserTPopup").style.display = "none";
}





/*teammember textbox display------------------------*/

    function schangeShow(section) {
        document.querySelectorAll('.schange-content').forEach(div => {
            div.style.display = 'none';
        });

    document.getElementById('schange-' + section).style.display = 'block';
}






/*email checker-------------------------*/



/*checks if email exists to create new staff memeber*/

// Wait for the page to load, then attach the listener
document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("teammemberform");

    if (form) {
        form.addEventListener("submit", async function (e) {
            e.preventDefault(); // 'e' is passed automatically here, no deprecation!

            const emailInput = document.getElementById("memberEmail");
            const popup = document.getElementById("emailPopup");

            if (!emailInput) return;

            const email = emailInput.value;

            try {
                const response = await fetch(`/Helper/GetUserEmailAsync?email=${encodeURIComponent(email)}`);
                const exists = await response.json();

                if (!exists) {
                    // Show our stylish CSS popup
                    console.log("Showing popup now...");
                    popup.classList.add("show");
                } else {
                    // Manually submit the form since we stopped it earlier
                    form.submit();
                }
            } catch (error) {
                console.error("Fetch error:", error);
            }
        });
    }
});




/* Closes email popup */
function emailErrorModalClose() {
    const popup = document.getElementById("emailPopup");
    popup.classList.remove("show");
}




/*device open image upload*/
//my js fun to open device files and upload the image
function openDeviceFile(element, srcid) {
    const photo = element.files[0];

    const imgshower = document.getElementById(srcid); 
        
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






/*IsApproved status and change html with no page refresh-----------------------*/


    // We define this function outside of any other blocks so the button can see it
    async function toggleApproved1(id) {
        console.log("Button clicked for ID:", id);

    // 1. Get the token safely
    const tokenElement = document.querySelector('input[name="__RequestVerificationToken"]');
    const token = tokenElement ? tokenElement.value : "";

    try {
            // 2. Fetch call with a "/" at the start
            const response = await fetch('/AdminFile/IsMessageApproved?id=' + id, {
        method: 'POST',
    headers: {
        'RequestVerificationToken': token
                }
            });

    if (response.ok) {
                const result = await response.json();
    if (result.success) {
                    // 3. Move the card UI
                    const card = document.getElementById("msg-card-" + id);
    if (card) {
                        const targetId = result.isApproved ? "approved-container" : "pending-container";
    document.getElementById(targetId).appendChild(card);
    // Toggle button text
    card.querySelector('button').innerText = result.isApproved ? "Unapprove" : "Approve";
                    }
                }
            } else {
        console.error("Server error status:", response.status);
            }
        } catch (err) {
        console.error("Critical JS Error:", err);
        }
    }






async function toggleApproved(messageId) {
    // GET THE BUTTON AND DISABLE IT IMMEDIATELY
    const btn = event.target;
    btn.disabled = true;
    btn.innerText = "Saving...";

    const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
    if (!tokenInput) {
        alert("Security token missing!");
        btn.disabled = false;
        return;
    }
    const token = tokenInput.value;

    try {
        const response = await fetch(`/AdminFile/IsMessageApproved?id=${messageId}`, {
            method: 'POST',
            headers: { 'RequestVerificationToken': token }
        });

        if (!response.ok) {
            alert("Server Error: " + response.status);
            btn.disabled = false; // Re-enable if it failed
            return;
        }

        const result = await response.json();

        if (result.success) {
            const card = document.getElementById(`msg-card-${messageId}`);
            const pendingBox = document.getElementById('pending-container');
            const approvedBox = document.getElementById('approved-container');

            if (result.isApproved) {
                approvedBox.appendChild(card);
                btn.innerText = "Unapprove";
            } else {
                pendingBox.appendChild(card);
                btn.innerText = "Approve Testimonie";
            }
        }
    } catch (error) {
        console.error("AJAX Error:", error);
    } finally {
        btn.disabled = false; // Always re-enable when done
    }
}
